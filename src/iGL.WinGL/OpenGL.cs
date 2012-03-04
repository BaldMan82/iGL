using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenTK.Graphics.ES20;
using OpenTK;

namespace iGL
{
    public class OpenGLTest
    {
        private int[] _bufferIds = new int[2];
        private int _shaderProgram;

        private Vector3[] _vertices;
        private UInt16[] _indices;

        public OpenGLTest()
        {
            /* load most simple shader */

            _shaderProgram = GL.CreateProgram();

            var vsSource =  @"attribute vec3 a_position;" +                         
                            @"varying vec4 v_positionalColor;" +
                            @"void main() { " +
                            @" gl_Position =  vec4(a_position.xyz, 1);" +                         
                            @" v_positionalColor = gl_Position;" +
                            @"}";

            var fsSource =  @"precision mediump float;" +
                            @"varying vec4 v_positionalColor;" +
                            @"void main() { " +
                            @"gl_FragColor = v_positionalColor;" +
                            @"}";
                       

            var vs = LoadShader(ShaderType.VertexShader, vsSource);
            GL.AttachShader(_shaderProgram, vs);
            
            var fs = LoadShader(ShaderType.FragmentShader, fsSource);
            GL.AttachShader(_shaderProgram, fs);

            GL.LinkProgram(_shaderProgram);
            var programLog = GL.GetProgramInfoLog(_shaderProgram);
            if (!programLog.Split(new char[] { '\n' }).All(s => string.IsNullOrEmpty(s) || s == "Vertex shader(s) linked, fragment shader(s) linked.")) throw new Exception(programLog);

            GL.UseProgram(_shaderProgram);

            /* create an indexed triangle */

            _vertices = new Vector3[3] 
            {
                new Vector3(-1, 1 , 0),
                new Vector3(1, 1, 0),
                new Vector3(1, -1, 0)
            };

            _indices = new UInt16[3] 
            {
                0, 1, 2
            };

            GL.GenBuffers(2, _bufferIds);
            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, new IntPtr(_vertices.Length * (Vector3.SizeInBytes)),
                       _vertices, BufferUsage.StaticDraw);          

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, new IntPtr(_indices.Length * sizeof(short)),
                          _indices.ToArray(), BufferUsage.StaticDraw);

            GL.ClearColor(0.2f, 0.2f, 0.2f, 255);
            GL.Enable(EnableCap.DepthTest);
        }

        private int LoadShader(ShaderType type, string source)
        {
            var shader = GL.CreateShader(type);
            GL.ShaderSource(shader, source);
            GL.CompileShader(shader);

            var shaderLog = GL.GetShaderInfoLog(shader);
            if (!string.IsNullOrEmpty(shaderLog) && !shaderLog.Contains("successfully compiled")) throw new Exception(shaderLog);

            return shader;
        }

        public void Render()
       {                        
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);           

            int vertexAttrib = GL.GetAttribLocation(_shaderProgram, "a_position");            

            GL.BindBuffer(BufferTarget.ArrayBuffer, _bufferIds[0]);
            GL.EnableVertexAttribArray(vertexAttrib);
            GL.VertexAttribPointer(vertexAttrib, 3, VertexAttribPointerType.Float, false, 0, 0);         

            GL.BindBuffer(BufferTarget.ElementArrayBuffer, _bufferIds[1]);

            GL.DrawElements(BeginMode.Triangles, _indices.Length, DrawElementsType.UnsignedShort, 0);
        }

    }
}
