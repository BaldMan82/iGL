using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class Gizmo : GameObject
    {
        private GameObject _yDirectionArrow;
        private GameObject _zDirectionArrow;
        private GameObject _xDirectionArrow;

        private GameObject _yDirection;
        private GameObject _zDirection;
        private GameObject _xDirection;

        public Gizmo()
        {
            float distance = 7.0f;

            _yDirectionArrow = new GameObject("yDirectionArrow");
            _yDirectionArrow.Scale = new Vector3(1, 3, 1);            

            _yDirectionArrow.Position = new Vector3(0, distance, 0);

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

            _yDirectionArrow.AddComponent(mesh);
            _yDirectionArrow.AddComponent(render);

            AddChild(_yDirectionArrow);

            _zDirectionArrow = new GameObject("zDirectionArrow");

            _zDirectionArrow.Rotation = new Vector3((float)(System.Math.PI / 2.0f), 0, 0);
            _zDirectionArrow.Scale = new Vector3(1, 3, 1);
            _zDirectionArrow.Position = new Vector3(0, 0, distance);

            var zMesh = new MeshComponent();

            zMesh.Vertices = mesh.Vertices;
            zMesh.Normals = mesh.Normals;
            zMesh.Indices = mesh.Indices;
            zMesh.Material.Ambient = new Vector4(1, 0, 0, 1);

            render = render.CloneForReuse();

            _zDirectionArrow.AddComponent(zMesh);
            _zDirectionArrow.AddComponent(render);

            AddChild(_zDirectionArrow);

            _xDirectionArrow = new GameObject("xDirectionArrow");

            _xDirectionArrow.Rotation = new Vector3(0, 0, -(float)(System.Math.PI / 2.0f));
            _xDirectionArrow.Scale = new Vector3(1, 3, 1);
            _xDirectionArrow.Position = new Vector3(distance, 0, 0);

            var xMesh = new MeshComponent();

            xMesh.Vertices = mesh.Vertices;
            xMesh.Normals = mesh.Normals;
            xMesh.Indices = mesh.Indices;
            xMesh.Material.Ambient = new Vector4(0, 1, 0, 1);

            render = render.CloneForReuse();

            _xDirectionArrow.AddComponent(xMesh);
            _xDirectionArrow.AddComponent(render);

            AddChild(_xDirectionArrow);

            _xDirection = new Cube(0.5f, 0.5f, distance) { Name = "xDirection" };
            _xDirection.Position = new Vector3((distance / 2.0f) - 0.25f, 0, 0);
            ((Cube)_xDirection).Material.Ambient = new Vector4(0, 1, 0, 1);

            AddChild(_xDirection);

            _yDirection = new Cube(0.5f, distance, 0.5f) { Name = "yDirection" };
            _yDirection.Position = new Vector3(0, (distance / 2.0f) - 0.25f, 0);
            ((Cube)_yDirection).Material.Ambient = new Vector4(0, 0, 1, 1);

            AddChild(_yDirection);

            _zDirection = new Cube(distance, 0.5f, 0.5f) { Name = "zDirection" };
            _zDirection.Position = new Vector3(0, 0, (distance / 2.0f) - 0.25f);
            ((Cube)_zDirection).Material.Ambient = new Vector4(1, 0, 0, 1);

            AddChild(_zDirection);

            Scale = new Vector3(0.2f, 0.2f, 0.2f);
        }
    }
}
