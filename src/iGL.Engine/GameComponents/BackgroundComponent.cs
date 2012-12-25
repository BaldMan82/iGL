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
using iGL.Engine.Resources;

namespace iGL.Engine
{
    [Serializable]
    public class BackgroundComponent : RenderComponent
    {
        private int[] _bufferIds;
     
        private Vector3[] _vertices;
        private Vector3[] _normals;
        private short[] _indices;
        private BackgroundShader _shader;
        private float _distance;
        private Texture _bgTexture;
        private Vector2 _centerPoint;

        public Vector2 MinBounds { get; private set; }
        public Vector2 MaxBounds { get; private set; }

        public BeginMode BeginMode { get; set; }

        public BackgroundComponent(XElement xmlElement) : base(xmlElement) { }

        public BackgroundComponent() { }

        protected override void Init()
        {
            _bufferIds = new int[2];

            BeginMode = BeginMode.Triangles;

            _distance = -5.0f;
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
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1),
                new Vector3(0,0,1)
            };

            _indices = new short[6] {
                2,1,0,1,3,0
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

            GameObject.Position = new Vector3(0, 0, _distance);

            GameObject.Scene.OnLoaded += (a, b) =>
            {
                /* calculate level bounding box */

                var objs = GameObject.Scene.GameObjects.SelectMany(g => g.AllChildren).ToList();
                var meshComponents = objs.Select(o => o.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent).Where(c => c != null);
                
                Vector3 vMin = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
                Vector3 vMax = new Vector3(float.MinValue, float.MinValue, float.MinValue);

                foreach (var meshComponent in meshComponents)
                {
                    var meshMin = meshComponent.MinBox;
                    var meshMax = meshComponent.MaxBox;

                    var transform = meshComponent.GameObject.GetCompositeTransform();
                    meshMin = Vector3.Transform(meshMin, transform);
                    meshMax = Vector3.Transform(meshMax, transform);

                    if (meshMin.X < vMin.X) vMin.X = meshMin.X;
                    if (meshMin.X > vMax.X) vMax.X = meshMin.X;

                    if (meshMin.Y < vMin.Y) vMin.Y = meshMin.Y;
                    if (meshMin.Y > vMax.Y) vMax.Y = meshMin.Y;

                    if (meshMin.Z < vMin.Z) vMin.Z = meshMin.Z;
                    if (meshMin.Z > vMax.Z) vMax.Z = meshMin.Z;

                    if (meshMax.X < vMin.X) vMin.X = meshMax.X;
                    if (meshMax.X > vMax.X) vMax.X = meshMax.X;
                                                     
                    if (meshMax.Y < vMin.Y) vMin.Y = meshMax.Y;
                    if (meshMax.Y > vMax.Y) vMax.Y = meshMax.Y;      
                                              
                    if (meshMax.Z < vMin.Z) vMin.Z = meshMax.Z;
                    if (meshMax.Z > vMax.Z) vMax.Z = meshMax.Z;
                }

                float margin = 30.0f;
                MinBounds = new Vector2(vMin.X - margin, vMin.Y - margin);
                MaxBounds = new Vector2(vMax.X + margin, vMax.Y + margin);

                if (GameObject.Scene.CurrentCamera is PerspectiveCameraComponent)
                {
                    var cam = GameObject.Scene.CurrentCamera as PerspectiveCameraComponent;
                    var sin = System.Math.Sin(cam.FieldOfViewRadians);
                    var l = (float)(sin * (System.Math.Abs(_distance) + (cam.GameObject.Position.Z))) - 5;
					GameObject.Scale = new Vector3(l*cam.AspectRatio, l,1);

                    cam.GameObject.OnMove += (c, d) =>
                    {                     
                        GameObject.Position = new Vector3(cam.GameObject.Position.X, cam.GameObject.Position.Y, _distance);
                    };
                }
            };

            this.GameObject.RenderQueuePriority = int.MaxValue;

            _bgTexture = GameObject.Scene.Resources.FirstOrDefault(r => r.Name == "bg" && r is Texture) as Texture;

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

            var ambientColor = GameObject.Scene.AmbientColor;
            _shader.SetAmbientColor(ref ambientColor);

            _shader.SetModelViewProjectionMatrix(ref modelView);
            _shader.SetModelViewMatrix(ref transform);

            var locationInverse = transform;

            locationInverse.M41 = 0;
            locationInverse.M42 = 0;
            locationInverse.M43 = 0;

            var mi = Matrix4.Identity;

            _shader.SetTransposeAdjointModelViewMatrix(ref mi);
                   
            var eyePos = new Vector4(0, 0, 1, 1);
            _shader.SetEyePos(ref eyePos);
            
            _shader.SetMinBounds(MinBounds);
            _shader.SetMaxBounds(MaxBounds);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);

            GL.EnableVertexAttribArray(0);
            GL.EnableVertexAttribArray(1);
            GL.EnableVertexAttribArray(2);
            GL.VertexAttribPointer(0, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 0);
            GL.VertexAttribPointer(1, 3, VertexAttribPointerType.Short, false, 10 * sizeof(short), 4 * sizeof(short));
            GL.VertexAttribPointer(2, 2, VertexAttribPointerType.Short, false, 10 * sizeof(short), 8 * sizeof(short));
            GL.BindTexture(TextureTarget.Texture2D, -1);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);

            if (_bgTexture != null)
            {
                GL.BindTexture(TextureTarget.Texture2D, _bgTexture.TextureId);

                var wrapModeX = TextureWrapMode.Clamp;
                var wrapModeY = TextureWrapMode.Clamp;

                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)wrapModeX);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)wrapModeY);

            }

            var center = new Vector2(GameObject.Position.X, GameObject.Position.Y);
            _shader.SetCenterPoint(ref center);
          
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
