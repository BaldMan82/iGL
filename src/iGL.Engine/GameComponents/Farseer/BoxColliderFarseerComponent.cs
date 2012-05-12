using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common;
using FarseerPhysics.Collision.Shapes;
using System.Xml.Linq;


namespace iGL.Engine
{
    [Serializable]
    public class BoxColliderFarseerComponent : ColliderFarseerComponent
    {
        public override bool InternalLoad()
        {
            base.InternalLoad();
            return LoadCollider();
        }

        public BoxColliderFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public BoxColliderFarseerComponent() : base() { }

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

            var vertices = PolygonTools.CreateRectangle((vMax.X - vMin.X) / 2.0f, (vMax.Y - vMin.Y) / 2);

            CollisionShape = new PolygonShape(vertices, 10.0f);

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
