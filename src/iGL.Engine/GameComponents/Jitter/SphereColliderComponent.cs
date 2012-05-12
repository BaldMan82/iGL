using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    public class SphereColliderComponent : ColliderComponent
    {        
        public SphereColliderComponent(XElement xmlElement) : base(xmlElement) { }

        public SphereColliderComponent() { }

        public override bool InternalLoad()
        {
            base.InternalLoad();

            return LoadCollider();
        }

        private bool LoadCollider()
        {
            /* find a mesh component to create box from */
            var meshComponent = this.GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent == null)
            {
                return false;
            }

            if (!meshComponent.IsLoaded) meshComponent.Load();

            float maxExtend = float.MinValue;

            foreach (var vertex in meshComponent.Vertices)
            {
                if (vertex.X * GameObject.Scale.X > maxExtend) maxExtend = vertex.X * GameObject.Scale.X;
                if (vertex.Y * GameObject.Scale.Y > maxExtend) maxExtend = vertex.Y * GameObject.Scale.Y;
                if (vertex.Z * GameObject.Scale.Z > maxExtend) maxExtend = vertex.Z * GameObject.Scale.Z;
            }

            float max = maxExtend;

            CollisionShape = new SphereShape(maxExtend);
            CollisionShape.Tag = GameObject;

            return true;
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }

        internal override void Reload()
        {
            if (!LoadCollider()) throw new InvalidOperationException();
        }
    }
}
