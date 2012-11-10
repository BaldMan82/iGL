using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Runtime.InteropServices;
using System.Xml.Linq;
using System.Diagnostics;

namespace iGL.Engine
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct GLVertex
    {
        [FieldOffset(0)]
        public short X;
        [FieldOffset(2)]
        public short Y;
        [FieldOffset(4)]
        public short Z;
        [FieldOffset(8)]
        public short NX;
        [FieldOffset(10)]
        public short NY;
        [FieldOffset(12)]
        public short NZ;
        [FieldOffset(16)]
        public short U;
        [FieldOffset(18)]
        public short V;
    }


    [Serializable]
    public class MeshRenderComponent : RenderComponent
    {
        private int[] _bufferIds;
        private bool _isClone;
        private float _shortFloatFactor;
        private Vector4 _eyePos = new Vector4(0, 0, 1, 1);

        private MeshRenderComponent _clonedReference;

        private MeshComponent _meshComponent;

        public BeginMode BeginMode { get; set; }

        public MeshRenderComponent(XElement xmlElement) : base(xmlElement) { }

        public MeshRenderComponent() { }

        protected override void Init()
        {
            _bufferIds = new int[2];

            BeginMode = BeginMode.Triangles;

			_shortFloatFactor = 1000;
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

			int[] cachedBuffers;
            if (!Game.InDesignMode && !string.IsNullOrEmpty(_meshComponent.MeshResourceName) &&
			    GameObject.Scene.MeshBufferCache.TryGetValue(_meshComponent.MeshResourceName, out cachedBuffers))
            {
                /* can reuse a rendercomponent + gl buffers */
				_bufferIds = cachedBuffers;
                _isClone = true;
            }
            else
            {

                /* create buffers to store vertex data */

                GL.GenBuffers(2, _bufferIds);

                GLVertex[] glVertices = new GLVertex[_meshComponent.Vertices.Length];

                bool hasUV = _meshComponent.UV.Length == _meshComponent.Vertices.Length;

                for (int i = 0; i < glVertices.Length; i++)
                {
                    glVertices[i].X = (short)(_meshComponent.Vertices[i].X * _shortFloatFactor);
                    glVertices[i].Y = (short)(_meshComponent.Vertices[i].Y * _shortFloatFactor);
                    glVertices[i].Z = (short)(_meshComponent.Vertices[i].Z * _shortFloatFactor);
                    glVertices[i].NX = (short)(_meshComponent.Normals[i].X * _shortFloatFactor);
                    glVertices[i].NY = (short)(_meshComponent.Normals[i].Y * _shortFloatFactor);
                    glVertices[i].NZ = (short)(_meshComponent.Normals[i].Z * _shortFloatFactor);

                    if (hasUV)
                    {                       
                        glVertices[i].U = (short)(_meshComponent.UV[i].X * _shortFloatFactor);
                        glVertices[i].V = (short)(_meshComponent.UV[i].Y * _shortFloatFactor);
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

                if (!Game.InDesignMode && !string.IsNullOrEmpty(_meshComponent.MeshResourceName))
                {
                    GameObject.Scene.MeshBufferCache.Add(_meshComponent.MeshResourceName, _bufferIds);
                }
            }

            return true;
        }

        public void Reload()
        {
            if (!Game.InDesignMode)
            {
                if (!string.IsNullOrEmpty(_meshComponent.MeshResourceName))
                    throw new NotSupportedException("Cannot reload object which has possible clones");
            }
            
            ReleaseBuffers();

            InternalLoad();
        }

        public void ReleaseBuffers()
        {
            /* only release buffers of original, not cloned object if scene is disposing */
			if (IsLoaded && (string.IsNullOrEmpty(_meshComponent.MeshResourceName) || GameObject.Scene.IsDisposingResources))
            {
                GL.DeleteBuffers(2, _bufferIds);
            }
        }

        public MeshRenderComponent CloneForReuse()
        {
            if (!IsLoaded) Load();

            var meshRenderComponent = new MeshRenderComponent();

            meshRenderComponent._bufferIds = _bufferIds;
            meshRenderComponent._isClone = true;

            return meshRenderComponent;
        }

        public override void Render(ref Matrix4 transform, ref Matrix4 modelView)
        {            
            if (_meshComponent.Material.ShaderProgram == ShaderProgram.ProgramType.POINTLIGHT)
            {
                ShaderProgram shader;

                if (GameObject.Designer)
                {
                    shader = GameObject.Scene.DesignShader;
                }
                else
                {
                    shader = GameObject.Scene.PointLightShader;
                }

                shader.Use();
                shader.SetModelViewProjectionMatrix(ref modelView);

                var locationInverse = transform;

                locationInverse.Invert();
                locationInverse.Transpose();

                shader.SetTransposeAdjointModelViewMatrix(ref locationInverse);
                shader.SetModelViewMatrix(ref transform);
                var material = _meshComponent.Material;
                shader.SetMaterial(ref material);
                shader.SetEyePos(ref _eyePos);

                var textureScale = new Vector2(_meshComponent.Material.TextureTilingX, _meshComponent.Material.TextureTilingY);
                shader.SetTextureScale(ref textureScale);

                shader.SetShortFloatFactor(_shortFloatFactor);
                GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
                
                GL.EnableVertexAttribArray(0);
                GL.EnableVertexAttribArray(1);
                GL.EnableVertexAttribArray(2);
                GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 0);
                GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 4 * sizeof(short));
                GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Short, false, 10 * sizeof(short), 8 * sizeof(short));

                if (_meshComponent.Texture != null)
                {                  
					GL.BindTexture(TextureTarget.Texture2D, _meshComponent.Texture.TextureId);

                    var wrapModeX = _meshComponent.Material.TextureRepeatX ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;
                    var wrapModeY = _meshComponent.Material.TextureRepeatY ? TextureWrapMode.Repeat : TextureWrapMode.Clamp;

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeX);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeY);
                 
                }
                else
                {
                    GL.BindTexture(TextureTarget.Texture2D, -1);      
                }

                if (_clonedReference != null)
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _clonedReference._bufferIds[1]);
                }
                else
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);
                }

                GL.DrawElements(BeginMode, _meshComponent.Indices.Length, DrawElementsType.UnsignedShort, 0);
            }
   
        }

        public override void Tick(float timeElapsed)
        {

        }

        public override void Dispose()
        {
            base.Dispose();

            ReleaseBuffers();
        }
    }
}
