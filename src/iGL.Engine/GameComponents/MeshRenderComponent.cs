using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

namespace iGL.Engine
{
    public class MeshRenderComponent : RenderComponent
    {
        private int[] _bufferIds;
        private MeshComponent _meshComponent;

        public MeshRenderComponent(GameObject gameObject)
            : base(gameObject)
        {           
            _bufferIds = new int[3];
        }

        public override void InternalLoad()
        {
            /* find mesh component and load it if needed */

            _meshComponent = GameObject.Components.FirstOrDefault(c => c is MeshComponent) as MeshComponent;
            if (_meshComponent == null) throw new InvalidOperationException("No mesh component to render");

            if (!_meshComponent.IsLoaded) _meshComponent.Load();

            /* create buffers to store vertex data */

            GL.GenBuffers(4, _bufferIds);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_meshComponent.Vertices.Length * (Vector3.SizeInBytes)),
                          _meshComponent.Vertices.ToArray(), BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_meshComponent.Normals.Length * (Vector3.SizeInBytes)),
                          _meshComponent.Vertices.ToArray(), BufferUsage.StaticDraw);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[2]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(_meshComponent.Indices.Length * sizeof(short)),
                          _meshComponent.Indices.ToArray(), BufferUsage.StaticDraw);
                    
        }

        public override void Render()
        {            
            var locationInverse = GameObject.Location;         

            locationInverse.Invert();
            locationInverse.Transpose();

            var shader = GameObject.Scene.ShaderProgram;
               
            shader.SetTransposeAdjointModelViewMatrix(locationInverse);
            shader.SetModelViewMatrix(GameObject.Location);
            shader.SetMaterial(_meshComponent.Material);

            int vertexAttrib = shader.GetVertexAttributeLocation();         
            int normalAttrib = shader.GetNormalAttributeLocation();

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.EnableVertexAttribArray(vertexAttrib);
           
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[1]);
            GL.EnableVertexAttribArray(normalAttrib);

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[2]);

            GL.VertexAttribPointer(vertexAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);        
            GL.VertexAttribPointer(normalAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);

            GL.DrawElements(BeginMode.Triangles, _meshComponent.Indices.Length, DrawElementsType.UnsignedShort, 0);
        }

        public override void Tick(float timeElapsed)
        {
           
        }
    }
}
