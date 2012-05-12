using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{

    [Serializable]
    [RequiredComponent(typeof(MeshComponent), Cube.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), Cube.MeshRenderComponentId)]
    public class Cube : GameObject
    {
        public Material Material
        {
            get
            {
                return _meshComponent.Material;
            }
        }

        public float Height
        {
            get { return _scale.Y; }
        }
        public float Width
        {
            get { return _scale.X; }
        }
        public float Depth
        {
            get { return _scale.Z; }
        }

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        private static MeshComponent _staticMeshComponent;
        private static MeshRenderComponent _staticMeshRenderComponent;

        private const string MeshComponentId = "b2dae056-2ff7-443f-aed1-0afd3db7b0be";
        private const string MeshRenderComponentId = "56af2307-be79-453a-a8ab-54bad0d21525";

        public Cube(XElement element) : base(element) { }

        public Cube() { }

        protected override void Init()
        {            
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
        }

        private void LoadCube()
        {
            if (_staticMeshRenderComponent == null)
            {
                var halfWidth = 0.5f;
                var halfHeight = 0.5f;
                var halfDepth = 0.5f;

                var vertices = new Vector3[36];
                var uv = new Vector2[36];

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

                for (int i = 0; i < 36; i += 6)
                {
                    uv[i] = new Vector2(0, 0);
                    uv[i + 1] = new Vector2(1, 0);
                    uv[i + 2] = new Vector2(0, 1);
                    uv[i + 3] = new Vector2(0, 1);
                    uv[i + 4] = new Vector2(1, 0);
                    uv[i + 5] = new Vector2(1, 1);
                }

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
                _meshComponent.UV = uv;

                _meshComponent.CalculateNormals();

                _staticMeshComponent = _meshComponent;
                _staticMeshRenderComponent = _meshRenderComponent;
            }
            else
            {
                /* reuse vertex buffers */
                _meshComponent.Vertices = _staticMeshComponent.Vertices;
                _meshComponent.Normals = _staticMeshComponent.Normals;
                _meshComponent.Indices = _staticMeshComponent.Indices;
                _meshComponent.UV = _staticMeshComponent.UV;

                _meshRenderComponent = _staticMeshRenderComponent.CloneForReuse();
            }
        }

        public override void Load()
        {
            LoadCube();

            base.Load();
        }
    }
}
