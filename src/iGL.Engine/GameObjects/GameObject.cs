﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using iGL.Engine.GL;
using iGL.Engine.Events;
using System.Reflection;

namespace iGL.Engine
{
    public class GameObject : Object
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

        public Guid Id { get; private set; }

        public Scene Scene { get; internal set; }
        public GameObject Parent { get; internal set; }
        public bool IsLoaded { get; private set; }

        public Vector3 Position
        {
            get
            {
                return _position;
            }
            set
            {
                _position = value;
                UpdateTransform();
                UpdateRigidBody();
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
                _scale = value;
                UpdateTransform();
                UpdateRigidBody(true);
            }
        }
        public Vector3 Rotation
        {
            get
            {
                return _rotation;
            }
            set
            {
                _rotation = value;
                UpdateTransform();
                UpdateRigidBody();
            }
        }
        public Matrix4 Transform { get; internal set; }

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

        public GameObject()
            : this(string.Empty)
        {

        }

        public GameObject(string name)
        {
            Scale = new Vector3(1.0f, 1.0f, 1.0f);

            _components = new List<GameComponent>();
            _children = new List<GameObject>();

            Name = name;

            /* default props */
            Visible = true;
            Enabled = true;
            Designer = false;

            Id = Guid.NewGuid();
        }

        public GameObject Clone()
        {
            var userComponents = this.Components.Where(c => c.CreationMode == GameComponent.CreationModeEnum.Additional).ToList();
            var obj = Activator.CreateInstance(this.GetType()) as GameObject;

            this.CopyPublicValues(obj);
       
            obj.Id = this.Id;

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

            OnComponentAddedEvent(new ComponentAddedEvent() { Component = component });
        }

        public void RemoveComponent(GameComponent component)
        {
            _components.Remove(component);

            OnComponentRemovedEvent(new ComponentRemovedEvent() { Component = component });
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
            bool hasRigidBody = Components.Any(c => c is RigidBodyComponent);

            if (hasRigidBody) return Transform;

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
            bool hasRigidBody = Components.Any(c => c is RigidBodyComponent);

            /* render designer objects in white ambient color */
            var sceneColor = Scene.AmbientColor;
            Scene.AmbientColor = new Vector4(1, 1, 1, 1);

            Matrix4 thisTransform;

            if (hasRigidBody)
            {
                thisTransform = Transform;
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

        private void UpdateRigidBody(bool rescale = false)
        {
            if (!this.IsLoaded) return;

            var rigidBodyComponent = this.Components.FirstOrDefault(c => c is RigidBodyComponent && c.IsLoaded) as RigidBodyComponent;

            if (rigidBodyComponent != null )
            {
                if (rescale)
                {
                    rigidBodyComponent.Reload();
                }
                else
                {
                    rigidBodyComponent.RigidBody.Position = this.Position.ToJitter();
                }
            }
        }

        private void UpdateTransform()
        {
            var mPos = Matrix4.CreateTranslation(Position);
            var mRotationX = Matrix4.CreateRotationX(Rotation.X);
            var mRotationY = Matrix4.CreateRotationY(Rotation.Y);
            var mRotationZ = Matrix4.CreateRotationZ(Rotation.Z);

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

        internal void OnMouseDownEvent(MouseButtonDownEvent e)
        {
            if (_mouseButtonDownEvent != null) _mouseButtonDownEvent(this, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseDownEvent(e);
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

        internal void OnMouseUpEvent(MouseButtonUpEvent e)
        {
            if (_mouseButtonUpEvent != null) _mouseButtonUpEvent(this, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseUpEvent(e);
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

        internal void OnMouseInEvent(MouseInEvent e)
        {
            if (_mouseInEvent != null) _mouseInEvent(this, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseInEvent(e);
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

        internal void OnMouseOutEvent(MouseOutEvent e)
        {
            if (_mouseOutEvent != null) _mouseOutEvent(this, e);

            /* bubble up */
            if (Parent != null) Parent.OnMouseOutEvent(e);
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

        internal void OnComponentAddedEvent(ComponentAddedEvent e)
        {
            if (_componentAddedEvent != null) _componentAddedEvent(this, e);

            if (Parent != null) Parent.OnComponentAddedEvent(e);
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

        internal void OnComponentRemovedEvent(ComponentRemovedEvent e)
        {
            if (_componentRemovedEvent != null) _componentRemovedEvent(this, e);

            if (Parent != null) Parent.OnComponentRemovedEvent(e);
        }

        #endregion
    }
}
