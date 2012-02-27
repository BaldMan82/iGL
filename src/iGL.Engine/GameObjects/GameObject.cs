using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using iGL.Engine.GL;

namespace iGL.Engine
{
    public class GameObject
    {
        internal Vector3 _position { get; set; }
        internal Vector3 _scale { get; set; }
        internal Vector3 _rotation { get; set; }

        public IGL GL { get { return Game.GL; } }

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
                UpdateRigidBody();
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

        public Scene Scene { get; internal set; }

        public bool IsLoaded { get; private set; }

        private List<GameComponent> _components { get; set; }       

        public IEnumerable<GameComponent> Components 
        {
            get { return _components.AsEnumerable(); }
        }

        public GameObject()
        {
            Scale = new Vector3(1.0f, 1.0f, 1.0f);
            _components = new List<GameComponent>();
        }

        public void AddComponent(GameComponent component)
        {           
            _components.Add(component);
        }      

        public void Load()
        {
            foreach(var component in _components)
            {
                if (!component.IsLoaded) component.Load();
            }

            IsLoaded = true;
        }

        public void Render()
        {           
            var renderComponents = Components.Where(c => c is RenderComponent)
                                             .Select(c => c as RenderComponent);

            foreach (var renderComponent in renderComponents)
            {
                renderComponent.Render();
            }
            
        }

        private void UpdateRigidBody()
        {
            if (!this.IsLoaded) return;


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
            _components.ForEach(gc => gc.Tick(timeElapsed));
        }
    }
}
