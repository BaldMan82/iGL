using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Collision.Shapes;

namespace FarseerPhysics.Collision.Shapes
{
    public class MultiShape : Shape
    {
        public List<Shape> Shapes { get; set; }
        public MultiShape(float density) : base(1.0f)
        {
            Shapes = new List<Shape>();
        }

        public override int ChildCount
        {
            get { return Shapes.Count; }
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
            output = new RayCastOutput();
            return false;
        }

        public override void ComputeAABB(out FarseerPhysics.Collision.AABB aabb, ref FarseerPhysics.Common.Transform transform, int childIndex)
        {
            Shapes[childIndex].ComputeAABB(out aabb, ref transform, 0);
        }

        public override void ComputeProperties()
        {
           
        }

        public override float ComputeSubmergedArea(Microsoft.Xna.Framework.Vector2 normal, float offset, FarseerPhysics.Common.Transform xf, out Microsoft.Xna.Framework.Vector2 sc)
        {
            sc = new Microsoft.Xna.Framework.Vector2();
            return 0;
        }
    }
}
