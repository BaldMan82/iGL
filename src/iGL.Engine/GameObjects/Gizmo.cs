using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Gizmo : GameObject
    {
        public GameObject YDirectionArrow { get; private set; }
        public GameObject ZDirectionArrow { get; private set; }
        public GameObject XDirectionArrow { get; private set; }
        public Sphere UniformSphere { get; private set; }

        private GameObject _yDirection;
        private GameObject _zDirection;
        private GameObject _xDirection;

        public Gizmo() : this(7.0f) { }
        
        public Gizmo(float arrowLength)
        {            
            YDirectionArrow = new GameObject("yDirectionArrow");
            YDirectionArrow.Scale = new Vector3(1, 3, 1);            

            YDirectionArrow.Position = new Vector3(0, arrowLength, 0);

            var mesh = new MeshComponent();

            var vertices = new Vector3[5];

            vertices[0] = new Vector3(-1, -0.5f, -1);
            vertices[1] = new Vector3(-1, -0.5f, 1);
            vertices[2] = new Vector3(0, 0.5f, 0);
            vertices[3] = new Vector3(1, -0.5f, 1);
            vertices[4] = new Vector3(1, -0.5f, -1);

            var indices = new short[18];
            int index = 0;

            indices[index++] = 0;
            indices[index++] = 1;
            indices[index++] = 2;

            indices[index++] = 1;
            indices[index++] = 3;
            indices[index++] = 2;

            indices[index++] = 3;
            indices[index++] = 4;
            indices[index++] = 2;

            indices[index++] = 4;
            indices[index++] = 0;
            indices[index++] = 2;

            indices[index++] = 3;
            indices[index++] = 1;
            indices[index++] = 0;

            indices[index++] = 0;
            indices[index++] = 4;
            indices[index++] = 3;


            mesh.Vertices = vertices;
            mesh.Indices = indices;
            mesh.Material.Ambient = new Vector4(0, 0, 1, 1);

            mesh.CalculateNormals();

            var render = new MeshRenderComponent();           

            YDirectionArrow.AddComponent(mesh);
            YDirectionArrow.AddComponent(render);

            AddChild(YDirectionArrow);

            ZDirectionArrow = new GameObject("zDirectionArrow");

            ZDirectionArrow.Rotation = new Vector3((float)(System.Math.PI / 2.0f), 0, 0);
            ZDirectionArrow.Scale = new Vector3(1, 3, 1);
            ZDirectionArrow.Position = new Vector3(0, 0, arrowLength);

            var zMesh = new MeshComponent();

            zMesh.Vertices = mesh.Vertices;
            zMesh.Normals = mesh.Normals;
            zMesh.Indices = mesh.Indices;
            zMesh.Material.Ambient = new Vector4(1, 0, 0, 1);

            render = render.CloneForReuse();

            ZDirectionArrow.AddComponent(zMesh);
            ZDirectionArrow.AddComponent(render);

            AddChild(ZDirectionArrow);

            XDirectionArrow = new GameObject("xDirectionArrow");

            XDirectionArrow.Rotation = new Vector3(0, 0, -(float)(System.Math.PI / 2.0f));
            XDirectionArrow.Scale = new Vector3(1, 3, 1);
            XDirectionArrow.Position = new Vector3(arrowLength, 0, 0);

            var xMesh = new MeshComponent();

            xMesh.Vertices = mesh.Vertices;
            xMesh.Normals = mesh.Normals;
            xMesh.Indices = mesh.Indices;
            xMesh.Material.Ambient = new Vector4(0, 1, 0, 1);

            render = render.CloneForReuse();

            XDirectionArrow.AddComponent(xMesh);
            XDirectionArrow.AddComponent(render);

            AddChild(XDirectionArrow);

            _xDirection = new Cube(0.5f, 0.5f, arrowLength) { Name = "xDirection" };
            _xDirection.Position = new Vector3((arrowLength / 2.0f) - 0.25f, 0, 0);
            ((Cube)_xDirection).Material.Ambient = new Vector4(0, 1, 0, 1);

            AddChild(_xDirection);

            _yDirection = new Cube(0.5f, arrowLength, 0.5f) { Name = "yDirection" };
            _yDirection.Position = new Vector3(0, (arrowLength / 2.0f) - 0.25f, 0);
            ((Cube)_yDirection).Material.Ambient = new Vector4(0, 0, 1, 1);

            AddChild(_yDirection);

            _zDirection = new Cube(arrowLength, 0.5f, 0.5f) { Name = "zDirection" };
            _zDirection.Position = new Vector3(0, 0, (arrowLength / 2.0f) - 0.25f);
            ((Cube)_zDirection).Material.Ambient = new Vector4(1, 0, 0, 1);

            AddChild(_zDirection);

            Scale = new Vector3(0.1f, 0.1f, 0.1f);

            RenderQueuePriority = -1;
            Designer = true;

            UniformSphere = new Sphere(2.0f);
            UniformSphere.Material.Ambient = new Vector4(1, 1, 1, 1);

            AddChild(UniformSphere);
        }

        public override void Render(Matrix4 parentTransform)
        {
            /* clear z-buffer, this gizmo is rendered last and above everything */
            GL.Clear(ClearBufferMask.DepthBufferBit);

            base.Render(parentTransform);
        }
    }
}
