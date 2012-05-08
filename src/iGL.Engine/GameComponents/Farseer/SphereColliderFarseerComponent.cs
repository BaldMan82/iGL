using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;

namespace iGL.Engine
{
    [Serializable]
    public class SphereColliderFarseerComponent : ColliderFarseerComponent
    {        
        public SphereColliderFarseerComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public SphereColliderFarseerComponent() { }

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

            CollisionShape = new CircleShape(maxExtend, 1.0f);

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
