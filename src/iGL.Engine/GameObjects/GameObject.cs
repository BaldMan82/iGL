using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace iGL.Engine
{
    public class GameObject
    {
        public Vector3 Position { get; set; }
        public Vector3 Scale { get; set; }
        public Vector3 Rotation { get; set; }

        public Matrix4 Location { get; private set; }

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

        public void Tick(double timeElapsed)
        {
            var mPos = Matrix4.Translation(Position);
            var mRotationX = Matrix4.Rotate(new Vector3(1.0f, 0.0f, 0.0f), Rotation.X);
            var mRotationY = Matrix4.Rotate(new Vector3(0.0f, 1.0f, 0.0f), Rotation.Y);
            var mRotationZ = Matrix4.Rotate(new Vector3(0.0f, 0.0f, 1.0f), Rotation.Z);
            var scale = Matrix4.Scale(Scale);            

            Location =  scale * mRotationX * mRotationY * mRotationZ * mPos * Matrix4.Identity;

            _components.ForEach(gc => gc.Tick(timeElapsed));
        }
    }
}
