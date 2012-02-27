using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletXNA.BulletCollision;
using BulletXNA.LinearMath;

namespace iGL.Engine
{
    public class SphereColliderComponent : ColliderComponent
    {
        public SphereColliderComponent(GameObject gameObject)
            : base(gameObject)
        {
            
        }

        public override void InternalLoad()
        {
            base.InternalLoad();

            /* find a mesh component to create box from */
            var meshComponent = this.GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent == null) return;

            if (!meshComponent.IsLoaded) meshComponent.Load();

            float maxExtend = float.MinValue;

            foreach (var vertex in meshComponent.Vertices)
            {
                if (vertex.X > maxExtend) maxExtend = vertex.X;
                if (vertex.Y > maxExtend) maxExtend = vertex.Y;
                if (vertex.Z > maxExtend) maxExtend = vertex.Z;
            }

            float max = maxExtend;

            CollisionShape = new SphereShape(maxExtend);
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }
    }
}
