using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using Jitter.Collision.Shapes;
using Jitter.Collision;
using Jitter.LinearMath;
using System.Xml.Linq;
using FarseerPhysics.Common.Decomposition;
using FarseerPhysics.Common;
using Microsoft.Xna.Framework;
using FarseerPhysics.Common.ConvexHull;
using FarseerPhysics.Collision.Shapes;

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
            var maxZ = meshComponent.Vertices.Max(v => v.Z);
            var scale = this.GameObject.Scale;

            var multiPoly = new MultiPolygonShape(1.0f);

            for (int i = 0; i < meshComponent.Indices.Length; i += 3)
            {
                var v1 = meshComponent.Vertices[meshComponent.Indices[i]];
                var v2 = meshComponent.Vertices[meshComponent.Indices[i + 1]];
                var v3 = meshComponent.Vertices[meshComponent.Indices[i + 2]];

                if (EqualWithTolerance(v1.Z, maxZ, 0.01f) &&
                    EqualWithTolerance(v2.Z, maxZ, 0.01f) && 
                    EqualWithTolerance(v3.Z, maxZ, 0.01f))
                {
                    try
                    {
                        var p1 = new Vector2(v1.X*GameObject.Scale.X, v1.Y*GameObject.Scale.Y);
                        var p2 = new Vector2(v2.X*GameObject.Scale.X, v2.Y*GameObject.Scale.Y);
                        var p3 = new Vector2(v3.X*GameObject.Scale.X, v3.Y*GameObject.Scale.Y);

                        if (EqualWithTolerance(p1, p2, 0.01f) || EqualWithTolerance(p1, p3, 0.01f) || EqualWithTolerance(p2, p3, 0.01f)) continue;

                        var poly = new PolygonShape(new Vertices() { p1, p2, p3 }, 1.0f);

                        multiPoly.Polygons.Add(poly);
                    }
                    catch
                    {
                        int a = 0;
                    }
                }
            }

            CollisionShape = multiPoly;

            return true;
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
