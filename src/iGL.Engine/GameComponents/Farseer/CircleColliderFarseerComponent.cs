using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;
using System.Xml.Linq;
using Microsoft.Xna.Framework;

namespace iGL.Engine
{
    [Serializable]
    public class CircleColliderFarseerComponent : ColliderFarseerComponent
    {        
        public CircleColliderFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public CircleColliderFarseerComponent() { }

        public float Radius { get; private set; }

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

            Vector2 vMin = new Vector2(float.MaxValue, float.MaxValue);
            Vector2 vMax = new Vector2(float.MinValue, float.MinValue);

            foreach (var vertex in meshComponent.Vertices)
            {
                if (vertex.X * GameObject.Scale.X < vMin.X) vMin.X = vertex.X * GameObject.Scale.X;
                if (vertex.X * GameObject.Scale.X > vMax.X) vMax.X = vertex.X * GameObject.Scale.X;

                if (vertex.Y * GameObject.Scale.Y < vMin.Y) vMin.Y = vertex.Y * GameObject.Scale.Y;
                if (vertex.Y * GameObject.Scale.Y > vMax.Y) vMax.Y = vertex.Y * GameObject.Scale.Y;
            }

            Radius = (vMax.X - vMin.X) / 2.0f;

            CollisionShape = new CircleShape(Radius, 15.0f);
            
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
