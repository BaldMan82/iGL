using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;

namespace FarseerPhysics.Collision.Shapes
{
    public class MultiPolygonShape : Shape
    {
        public List<PolygonShape> Polygons { get; set; }
        public MultiPolygonShape(float density) : base(1.0f)
        {
            Polygons = new List<PolygonShape>();
        }

        public override int ChildCount
        {
            get { throw new NotImplementedException(); }
        }

        public override Shape Clone()
        {
            throw new NotImplementedException();
        }

        public override bool TestPoint(ref FarseerPhysics.Common.Transform transform, ref Microsoft.Xna.Framework.Vector2 point)
        {
            throw new NotImplementedException();
        }

        public override bool RayCast(out FarseerPhysics.Collision.RayCastOutput output, ref FarseerPhysics.Collision.RayCastInput input, ref FarseerPhysics.Common.Transform transform, int childIndex)
        {
            throw new NotImplementedException();
        }

        public override void ComputeAABB(out FarseerPhysics.Collision.AABB aabb, ref FarseerPhysics.Common.Transform transform, int childIndex)
        {
            throw new NotImplementedException();
        }

        public override void ComputeProperties()
        {
            throw new NotImplementedException();
        }

        public override float ComputeSubmergedArea(Microsoft.Xna.Framework.Vector2 normal, float offset, FarseerPhysics.Common.Transform xf, out Microsoft.Xna.Framework.Vector2 sc)
        {
            throw new NotImplementedException();
        }
    }
}
