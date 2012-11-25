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
    [RequiredComponent(typeof(MeshComponent), Plane.MeshComponentId)]
    [RequiredComponent(typeof(MeshRenderComponent), Plane.MeshRenderComponentId)]
    public class Plane : GameObject
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

        private MeshComponent _meshComponent;
        private MeshRenderComponent _meshRenderComponent;

        private static MeshComponent _staticMeshComponent;
        private static MeshRenderComponent _staticMeshRenderComponent;

        private const string MeshComponentId = "e2dae056-2ff7-443f-aed1-0afd3db7b0bf";
        private const string MeshRenderComponentId = "36af2307-be79-453a-a8ab-54bad0d31525";

        public Plane(XElement element) : base(element) { }

        public Plane() { }

        protected override void Init()
        {
            _meshComponent = Components.Single(c => c.Id == MeshComponentId) as MeshComponent;
            _meshRenderComponent = Components.Single(c => c.Id == MeshRenderComponentId) as MeshRenderComponent;
        }

        private void LoadPlane()
        {
            if (_staticMeshRenderComponent == null)
            {
                var halfWidth = 0.5f;
                var halfHeight = 0.5f;             

                var vertices = new Vector3[6];
                var uv = new Vector2[6];               

                // front (+y)                                                
                vertices[0] = new Vector3(halfWidth, -halfHeight, 0);
                vertices[1] = new Vector3(-halfWidth, halfHeight, 0);
                vertices[2] = new Vector3(-halfWidth, -halfHeight, 0);
                vertices[3] = new Vector3(halfWidth, -halfHeight, 0);
                vertices[4] = new Vector3(halfWidth, halfHeight, 0);
                vertices[5] = new Vector3(-halfWidth, halfHeight, 0);

                uv[0] = new Vector2(1, 1);
                uv[1] = new Vector2(0, 0);
                uv[2] = new Vector2(0, 1);
                uv[3] = new Vector2(1, 1);
                uv[4] = new Vector2(1, 0);
                uv[5] = new Vector2(0, 0);
              
                var indices = new short[] {
                 0, 1, 2, 
                 3, 4, 5
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

                this.RemoveComponent(_meshRenderComponent);
                
                _meshRenderComponent = _staticMeshRenderComponent.CloneForReuse();

                this.AddComponent(_meshRenderComponent);
                
            }
        }

        public override void Load()
        {
            LoadPlane();

            base.Load();
        }

        public override void Dispose()
        {
            if (Scene.IsDisposing)
            {
                _staticMeshRenderComponent = null;
                _staticMeshComponent = null;
            }

            base.Dispose();
        }
    }
}
