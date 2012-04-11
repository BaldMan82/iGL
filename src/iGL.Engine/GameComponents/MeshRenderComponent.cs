using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using System.Runtime.Serialization;

using OpenGL = OpenTK.Graphics.OpenGL;

namespace iGL.Engine
{
    [Serializable]
    public class MeshRenderComponent : RenderComponent
    {
        private int[] _bufferIds;
        private MeshComponent _meshComponent;
      
        public BeginMode BeginMode { get; set; }

        public MeshRenderComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public MeshRenderComponent() { }

        protected override void Init()
        {
            _bufferIds = new int[4];

            BeginMode = BeginMode.Triangles;
        }

        public override bool InternalLoad()
        {
            /* find mesh component and load it if needed */

            _meshComponent = GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (_meshComponent == null)
            {
                return false;
            }

            if (!_meshComponent.IsLoaded) _meshComponent.Load();

            /* create buffers to store vertex data */

            GL.GenBuffers(4, _bufferIds);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_meshComponent.Vertices.Length * (Vector3.SizeInBytes)),
                       _meshComponent.Vertices.ToArray(), BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_meshComponent.UV.Length * (Vector2.SizeInBytes)),
                     _meshComponent.UV.ToArray(), BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[2]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_meshComponent.Normals.Length * (Vector3.SizeInBytes)),
                     _meshComponent.Normals.ToArray(), BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[3]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(_meshComponent.Indices.Length * sizeof(short)),
                          _meshComponent.Indices.ToArray(), BufferUsage.StaticDraw);           

            return true;
        }

        public MeshRenderComponent CloneForReuse()
        {
            if (!IsLoaded) Load();

            var meshRenderComponent = new MeshRenderComponent();

            meshRenderComponent._bufferIds = _bufferIds;

            return meshRenderComponent;
        }

        public override void Render(Matrix4 transform)
        {
            var locationInverse = transform;

            locationInverse.Invert();
            locationInverse.Transpose();

            var shader = GameObject.Scene.ShaderProgram;

            shader.SetTransposeAdjointModelViewMatrix(locationInverse);
            shader.SetModelViewMatrix(transform);
            shader.SetMaterial(_meshComponent.Material);

            int vertexAttrib = shader.GetVertexAttributeLocation();
            int normalAttrib = shader.GetNormalAttributeLocation();
            int uvAttrib = shader.GetUVAttributeLocation();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.EnableVertexAttribArray(vertexAttrib);
            GL.VertexAttribPointer(vertexAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[1]);
            GL.EnableVertexAttribArray(uvAttrib);
            GL.VertexAttribPointer(uvAttrib, 2, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[2]);
            GL.EnableVertexAttribArray(normalAttrib);
            GL.VertexAttribPointer(normalAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[3]);

            if (_meshComponent.Texture != null)
            {
                OpenGL.GL.ActiveTexture(OpenGL.TextureUnit.Texture0);
                OpenGL.GL.BindTexture(OpenGL.TextureTarget.Texture2D, _meshComponent.Texture.TextureId);
                shader.SetSamplerUnit(0);
            }
            else
            {
                OpenGL.GL.BindTexture(OpenGL.TextureTarget.Texture2D, -1);
            }
           
            GL.DrawElements(BeginMode, _meshComponent.Indices.Length, DrawElementsType.UnsignedShort, 0);

        }

        public override void Tick(float timeElapsed)
        {

        }
    }
}
