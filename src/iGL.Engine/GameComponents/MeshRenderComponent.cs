using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;

namespace iGL.Engine
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct GLVertex
    {
        [FieldOffset(0)]
        public float X;
        [FieldOffset(4)]
        public float Y;
        [FieldOffset(8)]
        public float Z;
        [FieldOffset(12)]
        public float NX;
        [FieldOffset(16)]
        public float NY;
        [FieldOffset(20)]
        public float NZ;
        [FieldOffset(24)]
        public float U;
        [FieldOffset(28)]
        public float V;
    }


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
            _bufferIds = new int[2];

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

            GL.GenBuffers(2, _bufferIds);

            GLVertex[] glVertices = new GLVertex[_meshComponent.Vertices.Length];

            bool hasUV = _meshComponent.UV.Length == _meshComponent.Vertices.Length;

            for (int i = 0; i < glVertices.Length; i++)
            {
                glVertices[i].X = _meshComponent.Vertices[i].X;
                glVertices[i].Y = _meshComponent.Vertices[i].Y;
                glVertices[i].Z = _meshComponent.Vertices[i].Z;
                glVertices[i].NX = _meshComponent.Normals[i].X;
                glVertices[i].NY = _meshComponent.Normals[i].Y;
                glVertices[i].NZ = _meshComponent.Normals[i].Z;
                
                if (hasUV)
                {
                    glVertices[i].U = _meshComponent.UV[i].X * _meshComponent.Material.TextureTilingX;
                    glVertices[i].V = _meshComponent.UV[i].Y * _meshComponent.Material.TextureTilingY;
                }
            }

            unsafe
            {
                fixed (GLVertex* data = glVertices)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(glVertices.Length * sizeof(GLVertex)),
                               (IntPtr)data, BufferUsage.StaticDraw);
                }

                fixed (short* data = _meshComponent.Indices.ToArray())
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(_meshComponent.Indices.Length * sizeof(short)),
                          (IntPtr)data, BufferUsage.StaticDraw);
                }
            }

            return true;
        }

        public void Reload()
        {
            ReleaseBuffers();
            InternalLoad();
        }

        public void ReleaseBuffers()
        {
            if (IsLoaded)
            {
                GL.DeleteBuffers(2, _bufferIds);
            }
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

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Float, false, 8 * sizeof(float), 3 * sizeof(float));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Float, false, 8 * sizeof(float), 6 * sizeof(float));

            if (_meshComponent.Texture != null)
            {
                GL.ActiveTexture(TextureUnit.Texture0);
                GL.BindTexture(TextureTarget.Texture2D, _meshComponent.Texture.TextureId);

                var wrapModeX = _meshComponent.Material.TextureRepeatX ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
                var wrapModeY = _meshComponent.Material.TextureRepeatY ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeX);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeY);
                
                shader.SetSamplerUnit(0);
                shader.SetHasTexture(true);
            }
            else
            {
                GL.BindTexture(TextureTarget.Texture2D, -1);
                shader.SetHasTexture(false);
            }

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);
            GL.DrawElements(BeginMode, _meshComponent.Indices.Length, DrawElementsType.UnsignedShort, 0);

        }

        public override void Tick(float timeElapsed)
        {

        }
    }
}
