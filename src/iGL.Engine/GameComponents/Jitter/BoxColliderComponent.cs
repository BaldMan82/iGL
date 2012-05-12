using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using Jitter.LinearMath;
using System.Runtime.Serialization;
using System.Xml.Linq;


namespace iGL.Engine
{
    [Serializable]
    public class BoxColliderComponent : ColliderComponent
    {
        public override bool InternalLoad()
        {
            base.InternalLoad();
            return LoadCollider();
        }

        public BoxColliderComponent(XElement xmlElement) : base(xmlElement) { }

        public BoxColliderComponent() { }

        private bool LoadCollider()
        {

            /* find a mesh component to create box from */
            var meshComponent = this.GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (meshComponent == null)
            {
                return false;
            }

            if (!meshComponent.IsLoaded) meshComponent.Load();

            JVector vMin = new JVector(float.MaxValue, float.MaxValue, float.MaxValue);
            JVector vMax = new JVector(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in meshComponent.Vertices)
            {
                if (vertex.X * GameObject.Scale.X < vMin.X) vMin.X = vertex.X * GameObject.Scale.X;
                if (vertex.X * GameObject.Scale.X > vMax.X) vMax.X = vertex.X * GameObject.Scale.X;

                if (vertex.Y * GameObject.Scale.Y < vMin.Y) vMin.Y = vertex.Y * GameObject.Scale.Y;
                if (vertex.Y * GameObject.Scale.Y > vMax.Y) vMax.Y = vertex.Y * GameObject.Scale.Y;

                if (vertex.Z * GameObject.Scale.Z < vMin.Z) vMin.Z = vertex.Z * GameObject.Scale.Z;
                if (vertex.Z * GameObject.Scale.Z > vMax.Z) vMax.Z = vertex.Z * GameObject.Scale.Z;
            }

            CollisionShape = new BoxShape(vMax - vMin);
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
