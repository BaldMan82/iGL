﻿using System;
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

namespace iGL.Engine
{
    public class GameObject : Object, IXmlSerializable
    {
        public enum CreationModeEnum
        {
            Additional,
            Internal
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
        public CreationModeEnum CreationMode { get; internal set; }

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
        public int RenderQueuePriority { get; set; }

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

        private List<GameObject> _children;
        private List<GameComponent> _components;
        private bool _visible;
        private bool _enabled;
        
        private bool _rigidPositionDirty = false;

        internal Vector3 _position;
        internal Vector3 _scale;
        internal Vector3 _rotation;

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

        public GameObject(XElement xmlElement)
            : this()
        {
            /* from here on, Init() has been executed, all componets & children should exist */

            #region Load Properties
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null).ToList();

            foreach (var prop in props)
            {
                var element = xmlElement.Elements().FirstOrDefault(e => e.Name == prop.Name);

                if (element != null)
                {
                    prop.SetValue(this, XmlHelper.FromXml(element, prop.PropertyType), null);
                }
            } 
            #endregion

            #region Load Components
            var componentElement = xmlElement.Elements("Components").FirstOrDefault();
            List<GameComponent> components = new List<GameComponent>();

            if (componentElement != null)
            {
                components = componentElement.Elements().Select(c => XmlHelper.FromXml(c, Type.GetType(c.Name.ToString())) as GameComponent).ToList();
            }

            /* create required components */
            var attributes = this.GetType().GetCustomAttributes(typeof(RequiredComponent), true).Select(o => o as RequiredComponent);
            foreach (var attr in attributes)
            {
                var component = components.FirstOrDefault(c => c.Id == attr.Id);
                if (component != null)
                {
                    if (component.GetType() != attr.ComponentType) throw new InvalidOperationException();

                    var loadedComponent = _components.Single(c => c.Id == component.Id);
                    component.CopyPublicValues(loadedComponent);
                }
            }

            /* load additional components */
            foreach (var component in components.Where(c => !_components.Any(c2 => c2.Id == c.Id)))
            {
                AddComponent(component);
            }           
            #endregion

            #region Load Children
            var childrenElement = xmlElement.Elements("Children").FirstOrDefault();
            List<GameObject> children = new List<GameObject>();

            if (childrenElement != null)
            {
                children = childrenElement.Elements().Select(c => XmlHelper.FromXml(c, Type.GetType(c.Name.ToString())) as GameObject).ToList();
            }

            var childAttributes = this.GetType().GetCustomAttributes(typeof(RequiredChild), true).Select(o => o as RequiredChild);
            foreach (var childAttr in childAttributes)
            {
                var childObject = children.FirstOrDefault(c => c.Id == childAttr.Id);
                if (childObject != null)
                {
                    if (childObject.GetType() != childAttr.ChildType) throw new InvalidOperationException();

                    var loadedChild = _children.Single(c => c.Id == childAttr.Id);
                    childObject.CopyPublicValues(loadedChild);
                }
            }

            /* load additional children */
            foreach (var child in children.Where(c => !_children.Any(c2 => c2.Id == c.Id)))
            {
                AddChild(child);
            }      

            #endregion
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

            /* create required children */
            var childAttributes = this.GetType().GetCustomAttributes(typeof(RequiredChild), true).Select(o => o as RequiredChild);
            foreach (var childAttr in childAttributes)
            {
                if (!_children.Any(c => c.Id == childAttr.Id))
                {
                    var child = Activator.CreateInstance(childAttr.ChildType) as GameObject;

                    child.Id = childAttr.Id;
                    child.CreationMode = GameObject.CreationModeEnum.Internal;

                    AddChild(child);
                }
            }

            Init();
        }

        protected virtual void Init() { }

        public XElement ToXml(XElement element)
        {
            var props = this.GetType().GetProperties().Where(p => p.GetSetMethod() != null);
            element.Add(props.Select(p => XmlHelper.ToXml(p.GetValue(this, null), p.Name)));

            element.Add(new XElement("Children", Children.Select(c => XmlHelper.ToXml(c, name: "GameObject"))));
            element.Add(new XElement("Components", Components.Select(c => XmlHelper.ToXml(c, name: "GameComponent"))));

            return element;
        }     

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

        public Matrix4 GetCompositeTransform(bool includeThisTransform = true)
        {
            Matrix4 parentMatrix = Matrix4.Identity;

            if (Parent != null)
            {
                parentMatrix = Parent.GetCompositeTransform();
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

        public virtual void Render(bool overrideParentTransform = false)
        {
            if (!Visible) return;

            if (!this.IsLoaded) throw new InvalidOperationException("Game Object not loaded!");
          
            /* render designer objects in white ambient color */
            var sceneColor = Scene.AmbientColor;
            if (this.Designer) Scene.AmbientColor = new Vector4(1, 1, 1, 1);

            var rigidBody = Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

            if (_children.Count > 0 && rigidBody != null)
            {
                UpdateGetRigidBodyOrientation();
                UpdateTransform();                
            }

            var compositeTransform = overrideParentTransform ? Transform : GetCompositeTransform();        

            var renderComponents = Components.Where(c => c is RenderComponent)
                                             .Select(c => c as RenderComponent);

            Matrix4 thisTransform;            

            if (rigidBody != null && rigidBody.IsLoaded)
            {
                /* when a gameobject has a rigidbody, always use this transform to render */
                /* gameobject transform has original positioning / orientation in relation to its parent tree */
                thisTransform = rigidBody.RigidBodyTransform;
            }
            else
            {
                thisTransform = compositeTransform;
            }

            var modelviewProjection = thisTransform * Scene.CurrentCamera.ModelViewProjectionMatrix;
            Scene.ShaderProgram.SetModelViewProjectionMatrix(modelviewProjection);

            if (ClearsZBuffer)
            {
                GL.Clear(ClearBufferMask.DepthBufferBit);
            }

            foreach (var renderComponent in renderComponents)
            {               
                renderComponent.Render(thisTransform);
            }

            Scene.AmbientColor = sceneColor;

        }        

        public virtual void Tick(float timeElapsed)
        {
            if (!Enabled) return;

            foreach (var child in _children)
            {
                child.Tick(timeElapsed);
            }

            _components.ForEach(gc => { if (gc.IsLoaded) gc.Tick(timeElapsed); });

            _rigidPositionDirty = true;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(Name))
            {
                return string.Format("[{0}] Unnamed", this.GetType().ToString());
            }

            return Name;
        }

        public GameObject RayTest(Vector3 origin, Vector3 direction, out Vector3 hitLocation)
        {
            hitLocation = new Vector3(0);           

            if (!Enabled) return null;

            var meshComponent = Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent != null) 
            {                
                if (meshComponent.RayTest(origin, direction, out hitLocation))
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

            Transform = scale * mRotationX * mRotationY * mRotationZ * mPos * Matrix4.Identity;
        }

        private void UpdateGetRigidBodyOrientation()
        {
            if (IsLoaded && _rigidPositionDirty)
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

                    _rigidPositionDirty = false;
                }
            }
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
