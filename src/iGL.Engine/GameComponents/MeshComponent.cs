using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using iGL.Engine.Math;

namespace iGL.Engine
{
    public class MeshComponent : GameComponent
    {
        public Vector3[] Vertices { get; set; }
        public Vector3[] Normals { get; set; }  

        public short[] Indices { get; set; }

        public Material Material { get; set; }

        public MeshComponent(GameObject gameObject)
            : base(gameObject)
        {
            Material = new Material();
        }

        public void CalculateNormals()
        {
            if (Indices.Length % 3 != 0)
                throw new NotSupportedException("Invalid indices count for normal calc");

            Normals = new Vector3[Vertices.Length];                      

            for (int i = 0; i < Indices.Length; i += 3)
            {
                Vector3[] v = new Vector3[3] { Vertices[Indices[i]], Vertices[Indices[i + 1]], Vertices[Indices[i + 2]] };

                var norm = Vector3.Cross(v[1] - v[0], v[2] - v[0]);

                for (int j = 0; j < 3; j++)
                {
                    Vector3 a = v[(j + 1) % 3] - v[j];
                    Vector3 b = v[(j + 2) % 3] - v[j];

                    float weight = (float)System.Math.Acos(Vector3.Dot(a, b) / (a.Length * b.Length));
                    Normals[Indices[i + j]] += weight * norm;
                }              
            }

            for (int i = 0; i < Normals.Length; i++)
            {
                Normals[i].Normalize();               
            }            

        }

        public override void InternalLoad()
        {

        }

        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
