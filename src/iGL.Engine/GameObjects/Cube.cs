using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Cube : GameObject
    {
        public float Depth { get; set; }
        public float Height { get; set; }
        public float Width { get; set; }

        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        private static MeshComponent _staticMeshComponent;
        private static MeshRenderComponent _staticMeshRenderComponent;

        public Cube(float depth, float height, float width)
        {
            Depth = depth;
            Height = height;
            Width = width;

            this.Scale = new Vector3(Width, Height, Depth);

            if (_staticMeshRenderComponent == null)
            {             
                var halfWidth = 0.5f;
                var halfHeight = 0.5f;
                var halfDepth = 0.5f;                             

                _meshComponent = new MeshComponent(this);

                var vertices = new Vector3[36];
                // top (+z)
                vertices[0] = new Vector3(-halfWidth, -halfHeight, halfDepth);
                vertices[1] = new Vector3(halfWidth, -halfHeight, halfDepth);
                vertices[2] = new Vector3(-halfWidth, halfHeight, halfDepth);
                vertices[3] = new Vector3(-halfWidth, halfHeight, halfDepth);
                vertices[4] = new Vector3(halfWidth, -halfHeight, halfDepth);
                vertices[5] = new Vector3(halfWidth, halfHeight, halfDepth);

                // bottom (-z)                                              
                vertices[6] = new Vector3(-halfWidth, -halfHeight, -halfDepth);
                vertices[7] = new Vector3(-halfWidth, halfHeight, -halfDepth);
                vertices[8] = new Vector3(halfWidth, -halfHeight, -halfDepth);
                vertices[9] = new Vector3(halfWidth, -halfHeight, -halfDepth);
                vertices[10] = new Vector3(-halfWidth, halfHeight, -halfDepth);
                vertices[11] = new Vector3(halfWidth, halfHeight, -halfDepth);

                // right (+x)                                               
                vertices[12] = new Vector3(halfWidth, -halfHeight, -halfDepth);
                vertices[13] = new Vector3(halfWidth, halfHeight, -halfDepth);
                vertices[14] = new Vector3(halfWidth, -halfHeight, halfDepth);
                vertices[15] = new Vector3(halfWidth, -halfHeight, halfDepth);
                vertices[16] = new Vector3(halfWidth, halfHeight, -halfDepth);
                vertices[17] = new Vector3(halfWidth, halfHeight, halfDepth);

                // left (-x)                                                 
                vertices[18] = new Vector3(-halfWidth, -halfHeight, -halfDepth);
                vertices[19] = new Vector3(-halfWidth, -halfHeight, halfDepth);
                vertices[20] = new Vector3(-halfWidth, halfHeight, -halfDepth);
                vertices[21] = new Vector3(-halfWidth, halfHeight, -halfDepth);
                vertices[22] = new Vector3(-halfWidth, -halfHeight, halfDepth);
                vertices[23] = new Vector3(-halfWidth, halfHeight, halfDepth);

                // front (+y)                                                
                vertices[24] = new Vector3(-halfWidth, -halfHeight, -halfDepth);
                vertices[25] = new Vector3(halfWidth, -halfHeight, -halfDepth);
                vertices[26] = new Vector3(-halfWidth, -halfHeight, halfDepth);
                vertices[27] = new Vector3(-halfWidth, -halfHeight, halfDepth);
                vertices[28] = new Vector3(halfWidth, -halfHeight, -halfDepth);
                vertices[29] = new Vector3(halfWidth, -halfHeight, halfDepth);

                // back (-y)                                                 
                vertices[30] = new Vector3(-halfWidth, halfHeight, -halfDepth);
                vertices[31] = new Vector3(-halfWidth, halfHeight, halfDepth);
                vertices[32] = new Vector3(halfWidth, halfHeight, -halfDepth);
                vertices[33] = new Vector3(halfWidth, halfHeight, -halfDepth);
                vertices[34] = new Vector3(-halfWidth, halfHeight, halfDepth);
                vertices[35] = new Vector3(halfWidth, halfHeight, halfDepth);


                var indices = new short[] {
                 0, 1, 2, 
                 3, 4, 5,

                 6, 7, 8,
                 9,10,11,

                12,13,14,
                15,16,17,

                18,19,20,
                21,22,23,

                24,25,26,
                27,28,29,

                30,31,32,
                33,34,35
                };

                _meshComponent.Vertices = vertices;
                _meshComponent.Indices = indices;

                CalculateNormals();

                AddComponent(_meshComponent);

                _meshRenderComponent = new MeshRenderComponent(this);

                AddComponent(_meshRenderComponent);

                _staticMeshComponent = _meshComponent;
                _staticMeshRenderComponent = _meshRenderComponent;
            }
            else
            {
                /* reuse vertex buffers */
                _meshComponent = new MeshComponent(this);
                _meshComponent.Vertices = _staticMeshComponent.Vertices;
                _meshComponent.Normals = _staticMeshComponent.Normals;
                _meshComponent.Indices = _staticMeshComponent.Indices;

                _meshRenderComponent = _staticMeshRenderComponent.CloneForReuse(this);

                AddComponent(_meshComponent);
                AddComponent(_meshRenderComponent);
            }                        
        }

        private void CalculateNormals()
        {
            var indices = _meshComponent.Indices;
            var vertices = _meshComponent.Vertices;
            var normals = _meshComponent.Normals;

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

            _meshComponent.Normals = normals;
        }
    }
}
