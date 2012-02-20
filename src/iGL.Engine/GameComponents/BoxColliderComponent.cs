using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletXNA.BulletCollision;
using BulletXNA.LinearMath;

namespace iGL.Engine
{
    public class BoxColliderComponent : ColliderComponent
    {       
        public BoxColliderComponent(GameObject gameObject)
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

            Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in meshComponent.Vertices)
            {
                if (vertex.X < vMin.X) vMin.X = vertex.X;
                if (vertex.X > vMax.X) vMax.X = vertex.X;

                if (vertex.Y < vMin.Y) vMin.Y = vertex.Y;
                if (vertex.Y > vMax.Y) vMax.Y = vertex.Y;

                if (vertex.Z < vMin.Z) vMin.Z = vertex.Z;
                if (vertex.Z > vMax.Z) vMax.Z = vertex.Z;
            }

            CollisionShape = new BoxShape(vMax - vMin);
        }

        public override void Tick(float timeElapsed)
        {
            base.Tick(timeElapsed);
        }
    }
}
