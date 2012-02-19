using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

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
                int v1 = Indices[i];
                int v2 = Indices[i + 1];
                int v3 = Indices[i + 2];

                Vector3 edge1 = Vertices[v2] - Vertices[v1];
                Vector3 edge2 = Vertices[v3] - Vertices[v1];

                var norm = Vector3.Cross(edge2, edge1);
                norm.Normalize();

                Normals[v1] += norm;
                Normals[v1].Normalize();

                Normals[v2] += norm;
                Normals[v2].Normalize();

                Normals[v3] += norm;
                Normals[v3].Normalize();                
            }

        }

        public override void InternalLoad()
        {

        }

        public override void Tick(double timeElapsed)
        {
           
        }
    }
}
