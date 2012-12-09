using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using iGL.Engine.GL;
using iGL.Engine.Events;
using System.Reflection;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Xml.Linq;
using iGL.Engine.GameObjects;
using System.Linq.Expressions;

namespace iGL.Engine
{   
    public class GameObject : Object, IXmlSerializable, IDisposable
    {
        public enum CreationModeEnum
        {
            Additional,
            Required,
            Runtime
        }

        public IGL GL { get { return Game.GL; } }

        public IEnumerable<GameObject> Children
        {
            get { return _children.AsEnumerable(); }
        }
        public IEnumerable<GameComponent> Components
        {
            get { return _components.AsEnumerable(); }
        }

        public IEnumerable<GameObject> AllChildren
        {
            get
            {
                var children = _children.SelectMany(c => c.AllChildren).ToList();
                children.AddRange(_children);

                return children;
            }
        }
        public CreationModeEnum CreationMode { get; set; }

        public bool Visible
        {
            get
            {
                if (Parent != null && !Parent.Visible) return false;
                return _visible;
            }
            set
            {
                _visible = value;
            }
        }
        public bool Enabled
        {
            get
            {
                if (Parent != null && !Parent.Enabled) return false;
                return _enabled;
            }
            set
            {
                _enabled = value;
            }
        }

        public bool Designer { get; set; }
        public string Name { get; set; }
        public bool ClearsZBuffer { get; set; }
        public bool DistanceSorting { get; set; }
        public bool AutoLoad { get; set; }
        public int RenderQueuePriority { get; set; }
        public bool IsDisposing { get; internal set; }

        public string Id { get; set; }
        public Scene Scene { get; internal set; }
        public GameObject Parent { get; internal set; }
        public bool IsLoaded { get; private set; }

        private bool _compositionChanged;
        protected RenderComponent _renderComponent;
        protected RigidBodyBaseComponent _rigidBodyComponent;
        protected GameObject[] _childrenArray = new GameObject[0];
        protected GameComponent[] _componentArray = new GameComponent[0];

        internal static Dictionary<Type, IEnumerable<RequiredComponent>> _typeComponentCache = new Dictionary<Type, IEnumerable<RequiredComponent>>();
        internal static Dictionary<Type, IEnumerable<RequiredChild>> _typeChildrenCache = new Dictionary<Type, IEnumerable<RequiredChild>>();
        internal static Dictionary<Type, IEnumerable<PropertyInfo>> _typePropertyCache = new Dictionary<Type, IEnumerable<PropertyInfo>>();

        public Vector3 Position
        {
            get
            {
                UpdateGetRigidBodyOrientation();

                return _position;
            }
            set
            {
				if (value == _position) return;

                _moveEvent.OldPosition = _position;
                _moveEvent.NewPosition = value;
               
                _position = value;
                UpdateTransform();

                OnMoveEvent(this, _moveEvent);
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                var transform = GetCompositeTransform(false);
                return Vector3.Transform(Position, transform);
            }
        }

        public Vector3 Scale
        {
            get
            {
                return _scale;
            }
            set
            {
                _scaleEvent.NewScale = value;
                _scaleEvent.OldScale = _scale;              

                _scale = value;
                UpdateTransform();

                OnScaleEvent(this, _scaleEvent);
            }
        }

        public Vector3 Rotation
        {
            get
            {
                UpdateGetRigidBodyOrientation();

                return _rotation;
            }
            set
            {
                _rotateEvent.NewRotation = value;
                _rotateEvent.OldRotation = _rotation;

                _rotation = value;
                UpdateTransform();

                OnRotateEvent(this, _rotateEvent);
            }
        }

        public Matrix4 Transform { get; internal set; }

        public GameObject Root
        {
            get
            {
                if (Parent != null) return Parent.Root;
                return this;
            }
        }

        private List<GameObject> _children;
        private List<GameComponent> _components;
        private bool _visible;
        private bool _enabled;
        
        internal bool _rigidPositionDirty = false;

        internal Vector3 _position;
        internal Vector3 _scale;
        internal Vector3 _rotation;

        internal event EventHandler<MouseButtonDownEvent> OnMouseButtonDownEvent;
        internal event EventHandler<MouseButtonUpEvent> OnMouseButtonUpEvent;
        internal event EventHandler<MouseInEvent> MouseInEvent;
        internal event EventHandler<MouseOutEvent> MouseOutEvent;
        internal event EventHandler<ComponentAddedEvent> ComponentAddedEvent;
        internal event EventHandler<ComponentRemovedEvent> ComponentRemovedEvent;
        internal event EventHandler<SleepEvent> SleepEvent;
        internal event EventHandler<AnimationSignalEvent> AnimationSignalEvent;
        internal event EventHandler<MoveEvent> MoveEvent;
        internal event EventHandler<ScaleEvent> ScaleEvent;
        internal event EventHandler<RotateEvent> RotateEvent;
        internal event EventHandler<ObjectCollisionEvent> ObjectCollisionEvent;
               
        private ComponentAddedEvent _componentAddedEvent = new ComponentAddedEvent();
        private ComponentRemovedEvent _componentRemovedEvent = new ComponentRemovedEvent();
        private SleepEvent _sleepEvent = new SleepEvent();
        private AnimationSignalEvent _animationSignalEvent = new AnimationSignalEvent();
        private MoveEvent _moveEvent = new MoveEvent();
        private ScaleEvent _scaleEvent = new ScaleEvent();
        private RotateEvent _rotateEvent = new RotateEvent();
        private ObjectCollisionEvent _objectCollisionEvent = new ObjectCollisionEvent();

        private XElement _xmlElement;

        private Dictionary<MethodInfo, object> _defaultValues = new Dictionary<MethodInfo, object>();       

        public GameObject(XElement xmlElement)
        {
            BaseInit();

           _xmlElement = xmlElement;

           /* set id property */
           Id = _xmlElement.Elements("Id").Single().Value;

            /* create the object tree */
            CreateStructureFromXml();

            /* default init */
            Init();

            /* load xml data (which overwrites default init props)*/
            InitFromXml();    
        }

        public GameObject() : this(string.Empty) { }

        public GameObject(string name)
        {
            BaseInit();

            Name = name;

            CreateDependentComponents();
            CreateDependentChildren();

            Init();

            _compositionChanged = true;
        }

        private void BaseInit()
        {          
            _components = new List<GameComponent>();
            _children = new List<GameObject>();

            Scale = new Vector3(1.0f, 1.0f, 1.0f);

            /* default props */
            Visible = true;
            Enabled = true;
            Designer = false;
            AutoLoad = true;

            Id = Guid.NewGuid().ToString();
        }

        internal IEnumerable<PropertyInfo> GetProperties()
        {
            /* cache properties */
            IEnumerable<PropertyInfo> props;
            if (!_typePropertyCache.TryGetValue(this.GetType(), out props))
            {
                props = this.GetType()
                              .GetProperties()
                              .Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));

                _typePropertyCache.Add(this.GetType(), props);
            }

            return props;
        }

        private void CreateStructureFromXml()
        {            
            #region Create Components
            var componentElement = _xmlElement.Elements("Components").FirstOrDefault();
            List<GameComponent> components = new List<GameComponent>();

            if (componentElement != null)
            {
                components = componentElement.Elements().Select(c => XmlHelper.FromXml(c, Type.GetType(c.Name.ToString())) as GameComponent).ToList();
            }

            components.ForEach(c => AddComponent(c));

            /* set required components */
            IEnumerable<RequiredComponent> componentCache;
            if (!_typeComponentCache.TryGetValue(this.GetType(), out componentCache))
            {
                componentCache = this.GetType().GetCustomAttributes(typeof(RequiredComponent), true).Select(o => o as RequiredComponent);
                _typeComponentCache.Add(this.GetType(), componentCache);
            }

            foreach (var attr in componentCache)
            {
                var component = components.FirstOrDefault(c => c.Id == attr.Id);
                if (component != null)
                {
                    if (component.GetType() != attr.ComponentType) throw new InvalidOperationException();

                    component.CreationMode = GameComponent.CreationModeEnum.Required;
                }
            }

            #endregion

            #region Create children

            /* create children */
            var childrenElement = _xmlElement.Elements("Children").FirstOrDefault();
            List<GameObject> children = new List<GameObject>();

            if (childrenElement != null)
            {
                children = childrenElement.Elements().Select(c => XmlHelper.FromXml(c, Type.GetType(c.Name.ToString())) as GameObject).ToList();
            }

            children.ForEach(c => AddChild(c));

            /* set required property */

            IEnumerable<RequiredChild> childrenCache;
            if (!_typeChildrenCache.TryGetValue(this.GetType(), out childrenCache))
            {
                childrenCache = this.GetType().GetCustomAttributes(typeof(RequiredChild), true).Select(o => o as RequiredChild);
                _typeChildrenCache.Add(this.GetType(), childrenCache);
            }

            foreach (var childAttr in childrenCache)
            {
                var childObject = children.FirstOrDefault(c => c.Id == childAttr.Id);
                if (childObject != null)
                {
                    if (childObject.GetType() != childAttr.ChildType)
                    {
                        throw new InvalidOperationException();                        
                    }

                    childObject.CreationMode = CreationModeEnum.Required;
                }
            }

            #endregion

            /* load additional required objects */
            CreateDependentComponents();
            CreateDependentChildren();
        }

        internal void InitFromXml()
        {        
            if (_xmlElement == null) return;

            #region Load Properties           

            var props = GetProperties();

            _defaultValues = new Dictionary<MethodInfo, object>();

            foreach (var prop in props)
            {             
                var element = _xmlElement.Elements().FirstOrDefault(e => e.Name == prop.Name);
                var setter = prop.GetSetMethod();

                object value = element == null ? prop.GetValue(this, null) : XmlHelper.FromXml(element, prop.PropertyType);

                _defaultValues.Add(setter, value);                           
            }
            #endregion

            foreach (var kv in _defaultValues) kv.Key.Invoke(this, new object[] { kv.Value });
      
            /* init childs */
            _children.ForEach(c => c.InitFromXml());

            /* load additional components */
            _components.ForEach(c => c.InitFromXml());         
        }

        internal void ResetToInitValues()
        {
            foreach (var kv in _defaultValues) kv.Key.Invoke(this, new object[] { kv.Value });

            _children.ForEach(c => c.ResetToInitValues());
            _components.ForEach(c => c.ResetToInitValues());

            OverrideLoadedProperties();
        }

        private void CreateDependentComponents()
        {
            /* create required components */
            IEnumerable<RequiredComponent> componentCache;
            if (!_typeComponentCache.TryGetValue(this.GetType(), out componentCache))
            {
                componentCache = this.GetType().GetCustomAttributes(typeof(RequiredComponent), true).Select(o => o as RequiredComponent);
                _typeComponentCache.Add(this.GetType(), componentCache);
            }

            foreach (var attr in componentCache)
            {
                if (!_components.Any(c => c.Id == attr.Id))
                {
                    var component = Activator.CreateInstance(attr.ComponentType) as GameComponent;

                    component.Id = attr.Id;
                    component.CreationMode = GameComponent.CreationModeEnum.Required;

                    AddComponent(component);
                }
            }
        }

        private void CreateDependentChildren()
        {
            /* create required children */
            IEnumerable<RequiredChild> childrenCache;
            if (!_typeChildrenCache.TryGetValue(this.GetType(), out childrenCache))
            {
                childrenCache = this.GetType().GetCustomAttributes(typeof(RequiredChild), true).Select(o => o as RequiredChild);
                _typeChildrenCache.Add(this.GetType(), childrenCache);
            }

            foreach (var childAttr in childrenCache)
            {
                if (!_children.Any(c => c.Id == childAttr.Id))
                {
                    var child = Activator.CreateInstance(childAttr.ChildType) as GameObject;

                    child.Id = childAttr.Id;
                    child.CreationMode = GameObject.CreationModeEnum.Required;

                    AddChild(child);
                }
            }

        }       

        protected virtual void Init() { }

        public XElement ToXml(XElement element)
        {
            var props = this.GetType()
                              .GetProperties()
                              .Where(p => p.GetSetMethod() != null && !p.GetCustomAttributes(true).Any(attr => attr is XmlIgnoreAttribute));

            element.Add(props.Select(p => XmlHelper.ToXml(p.GetValue(this, null), p.Name)));

            element.Add(new XElement("Children", Children.Where(c => c.CreationMode != CreationModeEnum.Runtime).Select(c => XmlHelper.ToXml(c, name: "GameObject"))));
            element.Add(new XElement("Components", Components.Where(c => c.CreationMode != GameComponent.CreationModeEnum.Runtime).Select(c => XmlHelper.ToXml(c, name: "GameComponent"))));

            return element;
        }     

        public GameObject Clone()
        {
            var userComponents = this.Components.Where(c => c.CreationMode == GameComponent.CreationModeEnum.Additional).ToList();
            var obj = Activator.CreateInstance(this.GetType()) as GameObject;

            this.CopyPublicValues(obj);

            obj.Id = Guid.NewGuid().ToString();

            foreach (var internalComponent in this.Components.Where(c => c.CreationMode == GameComponent.CreationModeEnum.Required))
            {
                var cloneComponent = obj.Components.Single(c => c.Id == internalComponent.Id);
                internalComponent.CopyPublicValues(cloneComponent);
            }

            foreach (var userComponent in userComponents)
            {
                obj.AddComponent(userComponent.Clone());
            }

            return obj;
        }

        public void AddComponent(GameComponent component)
        {
            if (component.IsLoaded) throw new InvalidOperationException("Component cannot be loaded");

            component.GameObject = this;
            _components.Add(component);

            if (IsLoaded)
            {
                /* load all not yet loaded components (when dependencies were not met) */
                _components.ForEach(gc => { if (!gc.IsLoaded) gc.Load(); });
            }

            OnComponentAddedEvent(this, new ComponentAddedEvent() { Component = component });

            _compositionChanged = true;
        }

        public void RemoveComponent(GameComponent component)
        {
            _components.Remove(component);

            OnComponentRemovedEvent(this, new ComponentRemovedEvent() { Component = component });

            _compositionChanged = true;
        }

        public void AddChild(GameObject gameObject)
        {
            gameObject.Parent = this;

            _children.Add(gameObject);

            /* if this object is loaded, load the child */
            if (IsLoaded)
            {
                if (gameObject.IsLoaded) throw new NotSupportedException("Cannot have a loaded child");
                gameObject.Scene = this.Scene;

                gameObject.Load();
            }

            _compositionChanged = true;
        }

        public Matrix4 GetCompositeTransform(bool includeThisTransform = true)
        {
            Matrix4 parentMatrix = Matrix4.Identity;

            if (Parent != null)
            {
                parentMatrix = Parent.GetCompositeTransform();
            }
			else if (includeThisTransform){
				return Transform;
			}

            if (includeThisTransform)
            {
                return Transform * parentMatrix;
            }
            else
            {
                return parentMatrix;
            }
        }

        public virtual void Load()
        {
            if (IsLoaded) return;

            foreach (var child in _children)
            {
                if (child.IsLoaded) throw new NotSupportedException("Cannot have a loaded child");

                child.Scene = this.Scene;

                child.Load();
            }

            foreach (var component in _components)
            {
                if (!component.IsLoaded) component.Load();
            }

            IsLoaded = true;

            OverrideLoadedProperties();
        }

        public virtual void OverrideLoadedProperties()
        {
            _rigidPositionDirty = true;
        }

        public virtual void Render(bool overrideParentTransform = false)
        {            
            if (!Visible || !IsLoaded) return;

            UpdateCompositionCache();

            if (_renderComponent == null && _childrenArray.Length == 0) return;

            if (!this.IsLoaded) throw new InvalidOperationException("Game Object not loaded!");                   

            var compositeTransform = overrideParentTransform ? Transform : GetCompositeTransform();            

            Matrix4 thisTransform;

            if (_rigidBodyComponent != null && _rigidBodyComponent.IsLoaded)
            {
                /* when a gameobject has a rigidbody, always use this transform to render */
                /* gameobject transform has original positioning / orientation in relation to its parent tree */
                thisTransform = _rigidBodyComponent.RigidBodyTransform;
            }
            else
            {
                thisTransform = compositeTransform;
            }

            var modelviewProjection = thisTransform * Scene.CurrentCamera.ModelViewProjectionMatrix;           

            if (ClearsZBuffer)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
            }

            if (_renderComponent != null)
            {
                _renderComponent.Render(ref thisTransform, ref modelviewProjection);
            }         

        }

        private void UpdateCompositionCache()
        {
            if (!_compositionChanged) return;

            _renderComponent = Components.FirstOrDefault(c => c is RenderComponent && c.IsLoaded) as RenderComponent;
            _rigidBodyComponent = Components.FirstOrDefault(c => c is RigidBodyBaseComponent) as RigidBodyBaseComponent;
            _childrenArray = _children.ToArray();
            _componentArray = _components.ToArray();

            _compositionChanged = false;

        }

        public virtual void Tick(float timeElapsed)
        {
            if (!IsLoaded) return;

            UpdateCompositionCache();

            for (int i = 0; i < _componentArray.Length; i++)
            {
                if (_components[i].IsLoaded) _components[i].Tick(timeElapsed);
            }

            /* mark the position of the object as dirty if it is a rigidbody */
            _rigidPositionDirty = _rigidBodyComponent != null &&
                !(_rigidBodyComponent is RigidBodyFarseerComponent && ((RigidBodyFarseerComponent)_rigidBodyComponent).IsSensor && ((RigidBodyFarseerComponent)_rigidBodyComponent).IsStatic);

            
            if (_children.Count > 0 && _rigidBodyComponent != null)
            {
				var wasDirty = _rigidPositionDirty;
                UpdateGetRigidBodyOrientation();
				if (wasDirty) UpdateTransform();
            }

            for (int i = 0; i < _childrenArray.Length; i++) _childrenArray[i].Tick(timeElapsed);
            
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Format("[{0}] Unnamed", this.GetType().ToString());
            }

            return Name;
        }

        public GameObject RayTest(ref Vector3 origin, ref Vector3 direction, out Vector3 hitLocation)
        {
            hitLocation = new Vector3(0);           

            if (!Enabled) return null;

            var meshComponent = Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent != null) 
            {                
                if (meshComponent.RayTest(ref origin, ref direction, out hitLocation))
                {                    
                    return this;
                }
            }
           
            return null;
        }

        private void UpdateTransform()
        {
            var mPos = Matrix4.CreateTranslation(_position);
            var mRotationX = Matrix4.CreateRotationX(_rotation.X);
            var mRotationY = Matrix4.CreateRotationY(_rotation.Y);
            var mRotationZ = Matrix4.CreateRotationZ(_rotation.Z);

            var scale = Matrix4.Scale(Scale);

            Transform = scale * mRotationX * mRotationY * mRotationZ * mPos;

			if (_rigidBodyComponent != null) {
				var farseerBody = _rigidBodyComponent as RigidBodyFarseerComponent;
				if (farseerBody != null){
					farseerBody.UpdateTransform();
				}
			}
        }

        private void UpdateGetRigidBodyOrientation()
        {
            if (IsLoaded && _rigidPositionDirty)
            {
                if (_rigidBodyComponent != null && _rigidBodyComponent.IsLoaded)
                {
                    Vector3 position, rotation;

                    if (Parent != null)
                    {
                        var composite = this.Parent.GetCompositeTransform();
                        var rigidBodyTransform = _rigidBodyComponent.RigidBodyTransform;
                        composite.Invert();

                        var thisTransform = rigidBodyTransform * composite;
                        position = thisTransform.Translation();
                        thisTransform.EulerAngles(out rotation);
                    }
                    else
                    {
                        position = _rigidBodyComponent.RigidBodyTransform.Translation();
                        _rigidBodyComponent.RigidBodyTransform.EulerAngles(out rotation);
                    }

                    _rotation = rotation;
                    _position = position;

                    _rigidPositionDirty = false;
                }
            }
        }      

        #region Events

        public event EventHandler<MouseButtonDownEvent> OnMouseDown
        {
            add
            {
                OnMouseButtonDownEvent += value;
            }
            remove
            {
                OnMouseButtonDownEvent -= value;
            }
        }

        internal void OnMouseDownEvent(object sender, MouseButtonDownEvent e)
        {
            if (OnMouseButtonDownEvent != null) OnMouseButtonDownEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseDownEvent(sender, e);
        }

        public event EventHandler<MouseButtonUpEvent> OnMouseUp
        {
            add
            {
                OnMouseButtonUpEvent += value;
            }
            remove
            {
                OnMouseButtonUpEvent -= value;
            }
        }

        internal void OnMouseUpEvent(object sender, MouseButtonUpEvent e)
        {
            if (OnMouseButtonUpEvent != null) OnMouseButtonUpEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseUpEvent(sender, e);
        }

        public event EventHandler<MouseInEvent> OnMouseIn
        {
            add
            {
                MouseInEvent += value;
            }
            remove
            {
                MouseInEvent -= value;
            }
        }

        internal void OnMouseInEvent(object sender, MouseInEvent e)
        {
            if (MouseInEvent != null) MouseInEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseInEvent(sender, e);
        }

        public event EventHandler<MouseOutEvent> OnMouseOut
        {
            add
            {
                MouseOutEvent += value;
            }
            remove
            {
                MouseOutEvent -= value;
            }
        }

        internal void OnMouseOutEvent(object sender, MouseOutEvent e)
        {
            if (MouseOutEvent != null) MouseOutEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseOutEvent(sender, e);
        }

        public event EventHandler<ComponentAddedEvent> OnComponentAdded
        {
            add
            {
                ComponentAddedEvent += value;
            }
            remove
            {
                ComponentAddedEvent -= value;
            }
        }

        internal void OnComponentAddedEvent(object sender, ComponentAddedEvent e)
        {
            if (ComponentAddedEvent != null) ComponentAddedEvent(sender, e);

            if (Parent != null) Parent.OnComponentAddedEvent(sender, e);
        }

        public event EventHandler<ComponentRemovedEvent> OnComponentRemoved
        {
            add
            {
                ComponentRemovedEvent += value;
            }
            remove
            {
                ComponentRemovedEvent -= value;
            }
        }

        internal void OnComponentRemovedEvent(object sender, ComponentRemovedEvent e)
        {
            if (ComponentRemovedEvent != null) ComponentRemovedEvent(sender, e);

            if (Parent != null) Parent.OnComponentRemovedEvent(sender, e);
        }

        public event EventHandler<MoveEvent> OnMove
        {
            add { MoveEvent += value; }
            remove { MoveEvent -= value; }
        }

        internal void OnMoveEvent(object sender, MoveEvent e)
        {
            if (MoveEvent != null) MoveEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnMoveEvent(sender, e);
            }
        }

        public event EventHandler<ScaleEvent> OnScale
        {
            add { ScaleEvent += value; }
            remove { ScaleEvent -= value; }
        }

        internal void OnScaleEvent(object sender, ScaleEvent e)
        {
            if (ScaleEvent != null) ScaleEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnScaleEvent(sender, e);
            }
        }

        public event EventHandler<RotateEvent> OnRotate
        {
            add { RotateEvent += value; }
            remove { RotateEvent -= value; }
        }

        internal void OnRotateEvent(object sender, RotateEvent e)
        {
            if (RotateEvent != null) RotateEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnRotateEvent(sender, e);
            }
        }

        public event EventHandler<SleepEvent> OnSleep
        {
            add
            {
                SleepEvent += value;
            }
            remove
            {
                SleepEvent -= value;
            }
        }

        internal void OnSleepEvent(object sender, SleepEvent e)
        {
            if (SleepEvent != null) SleepEvent(sender, e);
        }

        public event EventHandler<AnimationSignalEvent> OnAnimationSignal
        {
            add
            {
                AnimationSignalEvent += value;
            }
            remove
            {
                AnimationSignalEvent -= value;
            }
        }

        internal void OnAnimationSignalEvent(object sender, AnimationSignalEvent e)
        {
            if (AnimationSignalEvent != null) AnimationSignalEvent(sender, e);
        }

        public event EventHandler<ObjectCollisionEvent> OnObjectCollision
        {
            add
            {
                ObjectCollisionEvent += value;
            }
            remove
            {
                ObjectCollisionEvent -= value;
            }
        }

        internal void OnObjectCollisionEvent(object sender, ObjectCollisionEvent e)
        {
            if (ObjectCollisionEvent != null) ObjectCollisionEvent(sender, e);
        }

        #endregion

        public virtual void Dispose()
        {
            foreach (var child in _childrenArray)
            {
                child.Dispose();
            }

            foreach (var component in _componentArray)
            {
                component.Dispose();
            }
        }
    }
}
