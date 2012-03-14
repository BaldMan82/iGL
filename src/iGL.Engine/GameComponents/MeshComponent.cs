using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iGL.Engine.Math;
using Jitter.LinearMath;

namespace iGL.Engine
{
    public class MeshComponent : GameComponent
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }  

        public short[] Indices { get; set; }

        public Material Material { get; set; }

        private JBBox _boundingBox;        

        public MeshComponent()
        {
            Material = new Material();
        }       

        public override void InternalLoad()
        {
            Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            foreach (var vertex in Vertices)
            {
                if (vertex.X < vMin.X) vMin.X = vertex.X;
                if (vertex.X > vMax.X) vMax.X = vertex.X;

                if (vertex.Y < vMin.Y) vMin.Y = vertex.Y;
                if (vertex.Y > vMax.Y) vMax.Y = vertex.Y;

                if (vertex.Z < vMin.Z) vMin.Z = vertex.Z;
                if (vertex.Z > vMax.Z) vMax.Z = vertex.Z;
            }

            _boundingBox.Max = vMax.ToJitter();
            _boundingBox.Min = vMin.ToJitter();
        }

        public bool RayTest(Vector3 origin, Vector3 direction)
        {          
            var transform = GameObject.GetCompositeTransform();

            transform.Invert();
            
            /* transform ray to object space */

            var o = Vector3.Transform(origin, transform);
            var d = Vector3.Transform(direction, transform);                 

            return _boundingBox.RayIntersect(o.ToJitter(), d.ToJitter());            
        }


        public override void Tick(float timeElapsed)
        {
           
        }

        public void CalculateNormals()
        {
            var indices = Indices;
            var vertices = Vertices;
            var normals = Normals;

            if (indices.Length % 3 != 0)
                throw new NotSupportedException("Invalid indices count for normal calc");

            normals = new Vector3[vertices.Length];

            for (int i = 0; i < indices.Length; i += 3)
            {
                Vector3[] v = new Vector3[3] { vertices[indices[i]], vertices[indices[i + 1]], vertices[indices[i + 2]] };

                var norm = Vector3.Cross(v[1] - v[0], v[2] - v[0]);

                for (int j = 0; j < 3; j++)
                {
                    Vector3 a = v[(j + 1) % 3] - v[j];
                    Vector3 b = v[(j + 2) % 3] - v[j];

                    float weight = (float)System.Math.Acos(Vector3.Dot(a, b) / (a.Length * b.Length));
                    normals[indices[i + j]] += weight * norm;
                }
            }

            for (int i = 0; i < normals.Length; i++)
            {
                normals[i].Normalize();
            }

            Normals = normals;
        }
    }
}
