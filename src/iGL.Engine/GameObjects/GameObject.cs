using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using iGL.Engine.GL;
using iGL.Engine.Events;
using System.Reflection;
using Newtonsoft.Json;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    public class GameObject : Object, ISerializable
    {
        public IGL GL { get { return Game.GL; } }

        public IEnumerable<GameObject> Children
        {
            get { return _children.AsEnumerable(); }
        }
        public IEnumerable<GameComponent> Components
        {
            get { return _components.AsEnumerable(); }
        }

        public int RenderQueuePriority { get; set; }
        public bool Visible { get; set; }
        public bool Enabled { get; set; }
        public bool Designer { get; set; }
        public string Name { get; set; }

        public string Id { get; set; }

        public Scene Scene { get; internal set; }

        public GameObject Parent { get; internal set; }

        public bool IsLoaded { get; private set; }

        public Vector3 Position
        {
            get
            {
                UpdateGetRigidBodyOrientation();

                return _position;
            }
            set
            {
                var moveEvent = new MoveEvent()
                {
                    OldPosition = _position,
                    NewPosition = value
                };

                _position = value;
                UpdateTransform();
                OnMoveEvent(this, moveEvent);
            }
        }

        public Vector3 WorldPosition
        {
            get
            {
                var transform = GetCompositeTransform();
                return Vector3.Transform(new Vector3(), transform);
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
                var scaleEvent = new ScaleEvent()
                {
                    NewScale = value,
                    OldScale = _scale
                };

                _scale = value;
                UpdateTransform();
                OnScaleEvent(this, scaleEvent);
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
                var rotationEvent = new RotateEvent()
                {
                    NewRotation = value,
                    OldRotation = _rotation
                };

                _rotation = value;
                UpdateTransform();
                OnRotateEvent(this, rotationEvent);
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

        private List<GameObject> _children { get; set; }
        private List<GameComponent> _components { get; set; }

        internal Vector3 _position { get; set; }
        internal Vector3 _scale { get; set; }
        internal Vector3 _rotation { get; set; }

        internal event EventHandler<MouseButtonDownEvent> _mouseButtonDownEvent;
        internal event EventHandler<MouseButtonUpEvent> _mouseButtonUpEvent;
        internal event EventHandler<MouseInEvent> _mouseInEvent;
        internal event EventHandler<MouseOutEvent> _mouseOutEvent;
        internal event EventHandler<ComponentAddedEvent> _componentAddedEvent;
        internal event EventHandler<ComponentRemovedEvent> _componentRemovedEvent;
        internal event EventHandler<SleepEvent> _sleepEvent;

        internal event EventHandler<MoveEvent> _moveEvent;
        internal event EventHandler<ScaleEvent> _scaleEvent;
        internal event EventHandler<RotateEvent> _rotateEvent;

        public GameObject(SerializationInfo info, StreamingContext context)
            : this()
        {
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                prop.SetValue(this, info.GetValue(prop.Name, prop.PropertyType), null);
            }

            /* create required components */

            var attributes = this.GetType().GetCustomAttributes(typeof(RequiredComponent), true).Select(o => o as RequiredComponent);
            foreach (var attr in attributes)
            {
                var component = info.GetValue(attr.Id, attr.ComponentType) as GameComponent;
                var loadedComponent = _components.Single(c => c.Id == component.Id);

                component.CopyPublicValues(loadedComponent);
            }

            /* load addition components */
            var componentList = info.GetValue("componentList", typeof(List<string>)) as List<string>;
            foreach (var componentId in componentList.Where(id => !_components.Any(c2 => c2.Id == id)))
            {
                var component = info.GetValue(componentId, typeof(GameComponent)) as GameComponent;
                AddComponent(component);
            }
        }

        public GameObject()
            : this(string.Empty)
        {

        }

        public GameObject(string name)
        {
            _components = new List<GameComponent>();
            _children = new List<GameObject>();

            Scale = new Vector3(1.0f, 1.0f, 1.0f);

            Name = name;

            /* default props */
            Visible = true;
            Enabled = true;
            Designer = false;

            Id = Guid.NewGuid().ToString();

            /* create required components */
            var attributes = this.GetType().GetCustomAttributes(typeof(RequiredComponent), true).Select(o => o as RequiredComponent);
            foreach (var attr in attributes)
            {
                if (!_components.Any(c => c.Id == attr.Id))
                {
                    var component = Activator.CreateInstance(attr.ComponentType) as GameComponent;
                    
                    component.Id = attr.Id;
                    component.CreationMode = GameComponent.CreationModeEnum.Internal;

                    AddComponent(component);
                }
            }

            Init();
        }

        protected virtual void Init() { }

        public GameObject Clone()
        {
            var userComponents = this.Components.Where(c => c.CreationMode == GameComponent.CreationModeEnum.Additional).ToList();
            var obj = Activator.CreateInstance(this.GetType()) as GameObject;

            this.CopyPublicValues(obj);

            obj.Id = Guid.NewGuid().ToString();

            foreach (var internalComponent in this.Components.Where(c => c.CreationMode == GameComponent.CreationModeEnum.Internal))
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
                /* load all not yet loaded components (when dependencies where not met) */
                _components.ForEach(gc => { if (!gc.IsLoaded) gc.Load(); });
            }

            OnComponentAddedEvent(this, new ComponentAddedEvent() { Component = component });
        }

        public void RemoveComponent(GameComponent component)
        {
            _components.Remove(component);

            OnComponentRemovedEvent(this, new ComponentRemovedEvent() { Component = component });
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
        }

        public Matrix4 GetCompositeTransform()
        {
            Matrix4 parentMatrix = Matrix4.Identity;

            if (Parent != null)
            {
                parentMatrix = Parent.GetCompositeTransform();
            }

            return Transform * parentMatrix;
        }

        public virtual void Load()
        {
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

        }

        public virtual void Render(Matrix4 parentTransform)
        {
            if (!Visible) return;

            if (!this.IsLoaded) throw new InvalidOperationException("Game Object not loaded!");

            var rigidBody = Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

            /* render designer objects in white ambient color */
            var sceneColor = Scene.AmbientColor;
            Scene.AmbientColor = new Vector4(1, 1, 1, 1);

            Matrix4 thisTransform;

            if (rigidBody != null && rigidBody.IsLoaded)
            {
                /* when a gameobject has a rigidbody, always use this transform to render */
                /* gameobject transform has original positioning / orientation in relation to its parent tree */
                thisTransform = rigidBody.RigidBodyTransform;
            }
            else
            {
                thisTransform = Transform * parentTransform;
            }

            var modelviewProjection = thisTransform * Scene.CurrentCamera.ModelViewProjectionMatrix;
            Scene.ShaderProgram.SetModelViewProjectionMatrix(modelviewProjection);

            foreach (var child in _children)
            {
                child.Render(thisTransform);
            }

            var renderComponents = Components.Where(c => c is RenderComponent)
                                             .Select(c => c as RenderComponent);

            foreach (var renderComponent in renderComponents)
            {
                renderComponent.Render(thisTransform);
            }

            Scene.AmbientColor = sceneColor;

        }

        private void UpdateTransform()
        {
            var mPos = Matrix4.CreateTranslation(_position);
            var mRotationX = Matrix4.CreateRotationX(_rotation.X);
            var mRotationY = Matrix4.CreateRotationY(_rotation.Y);
            var mRotationZ = Matrix4.CreateRotationZ(_rotation.Z);

            var scale = Matrix4.Scale(Scale);

            Transform = scale * mRotationX * mRotationY * mRotationZ * mPos * Matrix4.Identity;
        }

        public void Tick(float timeElapsed)
        {
            if (!Enabled) return;

            foreach (var child in _children)
            {
                child.Tick(timeElapsed);
            }

            _components.ForEach(gc => { if (gc.IsLoaded) gc.Tick(timeElapsed); });
        }

        public override string ToString()
        {
            return Name;
        }

        public GameObject RayTest(Vector3 origin, Vector3 direction)
        {
            var deviation = this.Position - origin;
            deviation.Normalize();
            var normdir = direction;
            normdir.Normalize();

            if (!Enabled) return null;

            var meshComponent = Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent != null && meshComponent.RayTest(origin, direction)) return this;

            foreach (var child in _children)
            {
                var rayResult = child.RayTest(origin, direction);
                if (rayResult != null) return rayResult;
            }

            return null;
        }

        private void UpdateGetRigidBodyOrientation()
        {
            if (IsLoaded)
            {
                var rigidBody = this.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

                if (rigidBody != null && rigidBody.IsLoaded)
                {
                    Vector3 position, rotation;

                    if (Parent != null)
                    {
                        var composite = this.Parent.GetCompositeTransform();
                        var rigidBodyTransform = rigidBody.RigidBodyTransform;
                        composite.Invert();

                        var thisTransform = rigidBodyTransform * composite;
                        position = thisTransform.Translation();
                        thisTransform.EulerAngles(out rotation);
                    }
                    else
                    {
                        position = rigidBody.RigidBodyTransform.Translation();
                        rigidBody.RigidBodyTransform.EulerAngles(out rotation);
                    }

                    _rotation = rotation;
                    _position = position;
                }
            }
        }

        public virtual void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                info.AddValue(prop.Name, prop.GetValue(this, null));
            }

            /* save components */
            var componentList = new List<string>();
            foreach (var component in Components)
            {
                info.AddValue(component.Id, component);
                componentList.Add(component.Id);
            }

            info.AddValue("componentList", componentList);
        }

        #region Events

        public event EventHandler<MouseButtonDownEvent> OnMouseDown
        {
            add
            {
                _mouseButtonDownEvent += value;
            }
            remove
            {
                _mouseButtonDownEvent -= value;
            }
        }

        internal void OnMouseDownEvent(object sender, MouseButtonDownEvent e)
        {
            if (_mouseButtonDownEvent != null) _mouseButtonDownEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseDownEvent(sender, e);
        }

        public event EventHandler<MouseButtonUpEvent> OnMouseUp
        {
            add
            {
                _mouseButtonUpEvent += value;
            }
            remove
            {
                _mouseButtonUpEvent -= value;
            }
        }

        internal void OnMouseUpEvent(object sender, MouseButtonUpEvent e)
        {
            if (_mouseButtonUpEvent != null) _mouseButtonUpEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseUpEvent(sender, e);
        }

        public event EventHandler<MouseInEvent> OnMouseIn
        {
            add
            {
                _mouseInEvent += value;
            }
            remove
            {
                _mouseInEvent -= value;
            }
        }

        internal void OnMouseInEvent(object sender, MouseInEvent e)
        {
            if (_mouseInEvent != null) _mouseInEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseInEvent(sender, e);
        }

        public event EventHandler<MouseOutEvent> OnMouseOut
        {
            add
            {
                _mouseOutEvent += value;
            }
            remove
            {
                _mouseOutEvent -= value;
            }
        }

        internal void OnMouseOutEvent(object sender, MouseOutEvent e)
        {
            if (_mouseOutEvent != null) _mouseOutEvent(sender, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseOutEvent(sender, e);
        }

        public event EventHandler<ComponentAddedEvent> OnComponentAdded
        {
            add
            {
                _componentAddedEvent += value;
            }
            remove
            {
                _componentAddedEvent -= value;
            }
        }

        internal void OnComponentAddedEvent(object sender, ComponentAddedEvent e)
        {
            if (_componentAddedEvent != null) _componentAddedEvent(sender, e);

            if (Parent != null) Parent.OnComponentAddedEvent(sender, e);
        }

        public event EventHandler<ComponentRemovedEvent> OnComponentRemoved
        {
            add
            {
                _componentRemovedEvent += value;
            }
            remove
            {
                _componentRemovedEvent -= value;
            }
        }

        internal void OnComponentRemovedEvent(object sender, ComponentRemovedEvent e)
        {
            if (_componentRemovedEvent != null) _componentRemovedEvent(sender, e);

            if (Parent != null) Parent.OnComponentRemovedEvent(sender, e);
        }

        public event EventHandler<MoveEvent> OnMove
        {
            add { _moveEvent += value; }
            remove { _moveEvent -= value; }
        }

        internal void OnMoveEvent(object sender, MoveEvent e)
        {
            if (_moveEvent != null) _moveEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnMoveEvent(sender, e);
            }
        }

        public event EventHandler<ScaleEvent> OnScale
        {
            add { _scaleEvent += value; }
            remove { _scaleEvent -= value; }
        }

        internal void OnScaleEvent(object sender, ScaleEvent e)
        {
            if (_scaleEvent != null) _scaleEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnScaleEvent(sender, e);
            }
        }

        public event EventHandler<RotateEvent> OnRotate
        {
            add { _rotateEvent += value; }
            remove { _rotateEvent -= value; }
        }

        internal void OnRotateEvent(object sender, RotateEvent e)
        {
            if (_rotateEvent != null) _rotateEvent(sender, e);

            foreach (var child in Children)
            {
                child.OnRotateEvent(sender, e);
            }
        }

        public event EventHandler<SleepEvent> OnSleep
        {
            add
            {
                _sleepEvent += value;
            }
            remove
            {
                _sleepEvent -= value;
            }
        }

        internal void OnSleepEvent(object sender, SleepEvent e)
        {
            if (_sleepEvent != null) _sleepEvent(sender, e);
        }

        #endregion        

        #region Helper Methods

        public void Sleep()
        {
            var rigidBody = _components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;
            if (rigidBody == null) return;

            rigidBody.RigidBody.IsActive = false;
        }

        #endregion
    }
}
