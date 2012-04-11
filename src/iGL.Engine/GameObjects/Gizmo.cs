using iGL.Engine.Math;
using System;
using System.Xml.Linq;

namespace iGL.Engine
{
    [Serializable]
    public class Gizmo : GameObject
    {
        public GameObject YDirectionArrow { get; private set; }
        public GameObject ZDirectionArrow { get; private set; }
        public GameObject XDirectionArrow { get; private set; }
        public Sphere UniformSphere { get; private set; }

        private GameObject _yDirection;
        private GameObject _zDirection;
        private GameObject _xDirection;

        public float ArrowLength { get; set; }        
        public bool ShowUniformSphere { get; set; }       

        public Gizmo(XElement element) : base(element) { }

        public Gizmo() { }

        protected override void Init()
        {
            ShowUniformSphere = true;
            YDirectionArrow = new GameObject("yDirectionArrow");
            ZDirectionArrow = new GameObject("zDirectionArrow");
            XDirectionArrow = new GameObject("xDirectionArrow");

            _xDirection = new Cube() { Name = "xDirection" };
            _yDirection = new Cube() { Name = "yDirection" };
            _zDirection = new Cube() { Name = "zDirection" };

            UniformSphere = new Sphere() { Scale = new Vector3(2.0f) };

            ArrowLength = 7.0f;

            RenderQueuePriority = -1;
        }

        private void LoadGizmo()
        {
            YDirectionArrow.Scale = new Vector3(1, 3, 1);

            YDirectionArrow.Position = new Vector3(0, ArrowLength, 0);

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
            mesh.UV = new Vector2[vertices.Length];

            mesh.Material.Ambient = new Vector4(0, 0, 1, 1);

            mesh.CalculateNormals();

            var render = new MeshRenderComponent();

            YDirectionArrow.AddComponent(mesh);
            YDirectionArrow.AddComponent(render);

            AddChild(YDirectionArrow);

            ZDirectionArrow.Rotation = new Vector3((float)(System.Math.PI / 2.0f), 0, 0);
            ZDirectionArrow.Scale = new Vector3(1, 3, 1);
            ZDirectionArrow.Position = new Vector3(0, 0, ArrowLength);

            var zMesh = new MeshComponent();

            zMesh.Vertices = mesh.Vertices;
            zMesh.Normals = mesh.Normals;
            zMesh.Indices = mesh.Indices;
            zMesh.UV = mesh.UV;
            zMesh.Material.Ambient = new Vector4(1, 0, 0, 1);

            render = render.CloneForReuse();

            ZDirectionArrow.AddComponent(zMesh);
            ZDirectionArrow.AddComponent(render);

            AddChild(ZDirectionArrow);

            XDirectionArrow.Rotation = new Vector3(0, 0, -(float)(System.Math.PI / 2.0f));
            XDirectionArrow.Scale = new Vector3(1, 3, 1);
            XDirectionArrow.Position = new Vector3(ArrowLength, 0, 0);

            var xMesh = new MeshComponent();

            xMesh.Vertices = mesh.Vertices;
            xMesh.Normals = mesh.Normals;
            xMesh.Indices = mesh.Indices;
            xMesh.UV = mesh.UV;
            xMesh.Material.Ambient = new Vector4(0, 1, 0, 1);

            render = render.CloneForReuse();

            XDirectionArrow.AddComponent(xMesh);
            XDirectionArrow.AddComponent(render);

            AddChild(XDirectionArrow);

            _xDirection.Scale = new Vector3(ArrowLength, 0.5f, 0.5f);
            _xDirection.Position = new Vector3((ArrowLength / 2.0f) - 0.25f, 0, 0);
            ((Cube)_xDirection).Material.Ambient = new Vector4(0, 1, 0, 1);

            AddChild(_xDirection);

            _yDirection.Scale = new Vector3(0.5f, ArrowLength, 0.5f);
            _yDirection.Position = new Vector3(0, (ArrowLength / 2.0f) - 0.25f, 0);
            ((Cube)_yDirection).Material.Ambient = new Vector4(0, 0, 1, 1);

            AddChild(_yDirection);

            _zDirection.Scale = new Vector3(0.5f, 0.5f, ArrowLength);
            _zDirection.Position = new Vector3(0, 0, (ArrowLength / 2.0f) - 0.25f);
            ((Cube)_zDirection).Material.Ambient = new Vector4(1, 0, 0, 1);

            AddChild(_zDirection);

            Scale = new Vector3(0.1f, 0.1f, 0.1f);
       
            Designer = true;

            UniformSphere.Material.Ambient = new Vector4(1, 1, 1, 1);
            UniformSphere.Scale = new Vector3(3);
            UniformSphere.Visible = ShowUniformSphere;
            UniformSphere.Enabled = ShowUniformSphere;

            AddChild(UniformSphere);
        }

        public override void Render(Matrix4 parentTransform)
        {
            /* clear z-buffer, this gizmo is rendered last and above everything */
            GL.Clear(ClearBufferMask.DepthBufferBit);

            base.Render(parentTransform);
        }

        public override void Load()
        {
            base.Load();

            LoadGizmo();
        }
    }
}
