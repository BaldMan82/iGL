using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

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

        public Cube(float depth, float height, float width)
        {
            Depth = depth;
            Height = height;
            Width = width;

            var halfWidth = Width / 2.0f;
            var halfHeight = Height / 2.0f;
            var halfDepth = Depth / 2.0f;

            _meshComponent = new MeshComponent(this);
                      
            /* create cube vertices */
            var vertices = new Vector3[8];
            vertices[0] = new Vector3(-halfWidth, -halfHeight, halfDepth);
            vertices[1] = new Vector3(halfWidth, -halfHeight, halfDepth);
            vertices[2] = new Vector3(halfWidth, halfHeight, halfDepth);
            vertices[3] = new Vector3(-halfWidth, halfHeight, halfDepth);
            vertices[4] = new Vector3(-halfWidth, -halfHeight, -halfDepth);
            vertices[5] = new Vector3(halfWidth, -halfHeight, -halfDepth);
            vertices[6] = new Vector3(halfWidth, halfHeight, -halfDepth);
            vertices[7] = new Vector3(-halfWidth, halfHeight, -halfDepth);

            /* create indices */
            var indices = new short[]{
             // front face
                0, 1, 2, 2, 3, 0,
                // top face
                3, 2, 6, 6, 7, 3,
                // back face
                7, 6, 5, 5, 4, 7,
                // left face
                4, 0, 3, 3, 7, 4,
                // bottom face
                0, 1, 5, 5, 4, 0,
                // right face
                1, 5, 6, 6, 2, 1 };

            _meshComponent.Vertices = vertices;
            _meshComponent.Indices = indices;
            
            _meshComponent.CalculateNormals();

             AddComponent(_meshComponent);

            _meshRenderComponent = new MeshRenderComponent(this);           

            AddComponent(_meshRenderComponent);            
        }       
    }
}
