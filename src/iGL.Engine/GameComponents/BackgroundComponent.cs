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
    [Serializable]
    public class BackgroundComponent : RenderComponent
    {
        private int[] _bufferIds;
        private bool _isClone;
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private short[] _indices;
        private BackgroundShader _shader;

        public BeginMode BeginMode { get; set; }

        public BackgroundComponent(XElement xmlElement) : base(xmlElement) { }

        public BackgroundComponent() { }

        protected override void Init()
        {
            _bufferIds = new int[2];

            BeginMode = BeginMode.Triangles;
        }

        public override bool InternalLoad()
        {
            _shader = new BackgroundShader();
            _shader.Load();

            _vertices = new Vector3[4] {
                new Vector3(-1,1,0),
                new Vector3(1,-1,0),
                new Vector3(-1,-1,0),
                new Vector3(1,1,0)
            };

            _normals = new Vector3[4] {
                new Vector3(0,0,-1),
                new Vector3(0,0,-1),
                new Vector3(0,0,-1),
                new Vector3(0,0,-1)
            };

            _indices = new short[6] {
                0,1,2,0,3,1
            };

            /* create buffers to store vertex data */

            GL.GenBuffers(2, _bufferIds);

            GLVertex[] glVertices = new GLVertex[_vertices.Length];

            for (int i = 0; i < glVertices.Length; i++)
            {
                glVertices[i].X = (short)(_vertices[i].X);
                glVertices[i].Y = (short)(_vertices[i].Y);
                glVertices[i].Z = (short)(_vertices[i].Z);
                glVertices[i].NX = (short)(_normals[i].X);
                glVertices[i].NY = (short)(_normals[i].Y);
                glVertices[i].NZ = (short)(_normals[i].Z);
            }

            unsafe
            {
                fixed (GLVertex* data = glVertices)
                {
                    GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
                    GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(glVertices.Length * sizeof(GLVertex)),
                               (IntPtr)data, BufferUsage.StaticDraw);
                }

                fixed (short* data = _indices.ToArray())
                {
                    GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);
                    GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(_indices.Length * sizeof(short)),
                          (IntPtr)data, BufferUsage.StaticDraw);
                }
            }


            return true;
        }

        public void ReleaseBuffers()
        {
            _shader.Dispose();
            GL.DeleteBuffers(2, _bufferIds);
        }

        public override void Render(ref Matrix4 transform, ref Matrix4 modelView)
        {
            _shader.Use();
            _shader.SetLight(GameObject.Scene.CurrentLight.Light, new Vector4(GameObject.Scene.CurrentLight.GameObject.WorldPosition));

            _shader.SetModelViewProjectionMatrix(modelView);

            var locationInverse = transform;

            locationInverse.Invert();
            locationInverse.Transpose();

            _shader.SetTransposeAdjointModelViewMatrix(locationInverse);
            _shader.SetModelViewMatrix(transform);
            _shader.SetEyePos(new Vector4(0, 0, 1, 1));

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 4 * sizeof(short));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Short, false, 10 * sizeof(short), 8 * sizeof(short));
            GL.BindTexture(TextureTarget.Texture2D, -1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);

            GL.DrawElements(BeginMode, _indices.Length, DrawElementsType.UnsignedShort, 0);

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
