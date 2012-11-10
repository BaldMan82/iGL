using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Xml.Linq;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Collision.Shapes;
using iGL.Engine.Math;

namespace iGL.Engine.GameComponents
{
    [Serializable]
    public class MeshColliderFarseerComponent : ColliderFarseerComponent
    {
        public MeshColliderFarseerComponent(XElement xmlElement) : base(xmlElement) { }

        public MeshColliderFarseerComponent() { }

        public override bool InternalLoad()
        {
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
            if (!meshComponent.IsLoaded) return false;

            if (meshComponent.Vertices.Length == 0) return false;

            var maxZ = meshComponent.Vertices.Max(v => v.Z);
            var scale = this.GameObject.Scale;

            var multiShape = new MultiShape(1.0f);
           
            List<Vector2[]> points = new List<Vector2[]>();

            for (int i = 0; i < meshComponent.Indices.Length; i += 3)
            {
                List<Vector2> edge = new List<Vector2>();

                var edge1Point = GetXYPointOfEdge(meshComponent.Vertices[meshComponent.Indices[i]], meshComponent.Vertices[meshComponent.Indices[i + 1]]);
                var edge2Point = GetXYPointOfEdge(meshComponent.Vertices[meshComponent.Indices[i + 1]], meshComponent.Vertices[meshComponent.Indices[i + 2]]);
                var edge3Point = GetXYPointOfEdge(meshComponent.Vertices[meshComponent.Indices[i + 2]], meshComponent.Vertices[meshComponent.Indices[i]]);

                if (edge1Point != null) edge.Add(edge1Point.Value);
                if (edge2Point != null) edge.Add(edge2Point.Value);
                if (edge3Point != null) edge.Add(edge3Point.Value);

                if (edge.Count == 2)
                {                    
                    points.Add(edge.ToArray());
                }

            }

            Vector2 center = Vector2.Zero;

            for (int i = 0; i < points.Count; i++)
            {
                points[i][0].X *= GameObject.Scale.X;
                points[i][0].Y *= GameObject.Scale.Y;

                points[i][1].X *= GameObject.Scale.X;
                points[i][1].Y *= GameObject.Scale.Y;

                center += points[i][0] + points[i][1];
            }

            center /= (points.Count * 2);

            for (int i = 0; i < points.Count; i++)
            {
                var edge = new EdgeShape(points[i][0].ToXNA(), points[i][1].ToXNA());
                multiShape.Shapes.Add(edge);
            }

            CollisionShape = multiShape;

            return true;
        }
   
        private Vector2? GetXYPointOfEdge(Vector3 p0, Vector3 p1)
        {
            Vector3 N = new Vector3(0, 0, 1);
            Vector3 X = new Vector3(0, 0, 0);

            float t = Vector3.Dot(N, p0) / Vector3.Dot(N, p0 - p1);

            if (t >= 0 && t <= 1)
            {
                /* intersection on plane */
                var point = p0 + t * (p1 - p0);
                return new Vector2(point.X, point.Y);
            }

            return null;
        }

        private bool EqualWithTolerance(float value1, float value2, float tolerance)
        {
            return System.Math.Abs(value1 - value2) < tolerance;
        }

        private bool EqualWithTolerance(Vector2 v1, Vector2 v2, float tolerance)
        {
            return EqualWithTolerance(v1.X, v2.X, tolerance) && EqualWithTolerance(v1.Y, v2.Y, tolerance);
        }

        private List<Vector2> ConvexHull(List<Vector2> points)
        {
            if (points.Count < 3)
            {
                throw new ArgumentException("At least 3 points reqired", "points");
            }

            List<Vector2> hull = new List<Vector2>();

            // get leftmost point
            Vector2 vPointOnHull = points.Where(p => p.X == points.Min(min => min.X)).First();

            Vector2 vEndpoint;
            do
            {
                hull.Add(vPointOnHull);
                vEndpoint = points[0];

                for (int i = 1; i < points.Count; i++)
                {
                    if ((vPointOnHull == vEndpoint)
                        || (Orientation(vPointOnHull, vEndpoint, points[i]) == -1))
                    {
                        vEndpoint = points[i];
                    }
                }

                vPointOnHull = vEndpoint;

            }
            while (vEndpoint != hull[0]);

            return hull;
        }

        private int Orientation(Vector2 p1, Vector2 p2, Vector2 p)
        {
            // Determinant
            float Orin = (p2.X - p1.X) * (p.Y - p1.Y) - (p.X - p1.X) * (p2.Y - p1.Y);

            if (Orin > 0)
                return -1; //          (* Orientaion is to the left-hand side  *)
            if (Orin < 0)
                return 1; // (* Orientaion is to the right-hand side *)

            return 0; //  (* Orientaion is neutral aka collinear  *)
        }

        internal override void Reload()
        {
            if (!LoadCollider()) throw new InvalidOperationException();
        }
    }
}
