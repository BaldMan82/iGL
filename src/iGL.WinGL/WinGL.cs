using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using OpenTK.Graphics.OpenGL;
using OpenTK;

namespace iGL
{
    public class WinGL : IGL
    {
        public int CreateProgram()
        {
            return GL.CreateProgram();
        }

        public int CreateShader(Engine.ShaderType shaderType)
        {
            ShaderType glShaderType = ShaderType.VertexShader;

            switch (shaderType)
            {
                case Engine.ShaderType.FragmentShader:
                    glShaderType = ShaderType.FragmentShader;
                    break;
                case Engine.ShaderType.VertexShader:
                    glShaderType = ShaderType.VertexShader;
                    break;
            }

            return GL.CreateShader(glShaderType);
        }

        public void ShaderSource(int vs, string vertexProgramSource)
        {
            GL.ShaderSource(vs, vertexProgramSource);
        }

        public void CompileShader(int vs)
        {
            GL.CompileShader(vs);
        }

        public void GetShaderInfoLog(int vs, out string shaderLog)
        {
            GL.GetShaderInfoLog(vs, out shaderLog);
        }

        public void AttachShader(int programId, int vs)
        {
            GL.AttachShader(programId, vs);
        }

        public void BindAttribLocation(int programId, int index, string name)
        {
            GL.BindAttribLocation(programId, index, name);
        }

        public void LinkProgram(int programId)
        {
            GL.LinkProgram(programId);
        }

        public void GetProgramInfoLog(int programId, out string programLog)
        {
            GL.GetProgramInfoLog(programId, out programLog);
        }

        public int GetAttribLocation(int programId, string name)
        {
            return GL.GetAttribLocation(programId, name);
        }

        public void Uniform4(int loc, Engine.Math.Vector4 vector4)
        {
            GL.Uniform4(loc, vector4.X, vector4.Y, vector4.Z, vector4.W);
        }

        public void Uniform1(int loc, float p)
        {
            GL.Uniform1(loc, p);
        }

        public void UniformMatrix4(int loc, bool transpose, Engine.Math.Matrix4 matrix)
        {
            Matrix4 m = new Matrix4(matrix.M11, matrix.M12, matrix.M13, matrix.M14,
                                    matrix.M21, matrix.M22, matrix.M23, matrix.M24,
                                    matrix.M31, matrix.M32, matrix.M33, matrix.M34,
                                    matrix.M41, matrix.M42, matrix.M43, matrix.M44);


            GL.UniformMatrix4(loc, false, ref m);
        }

        public int GetUniformLocation(int p, string uniform)
        {
            return GL.GetUniformLocation(p, uniform);
        }

        public void GenBuffers(int p, int[] bufferIds)
        {
            GL.GenBuffers(p, bufferIds);
        }

        public void BindBuffer(Engine.BufferTarget bufferTarget, int p)
        {
            BufferTarget target = ToBufferTarget(bufferTarget);

            GL.BindBuffer(target, p);
        }

        public void BufferData<T>(Engine.BufferTarget bufferTarget, IntPtr size, T[] data, Engine.BufferUsage bufferUsage) where T : struct
        {
            var target = ToBufferTarget(bufferTarget);
            var usage = ToBufferUsage(bufferUsage);

            GL.BufferData(target, size, data, usage);
        }

        public void EnableVertexAttribArray(int vertexAttrib)
        {
            GL.EnableVertexAttribArray(vertexAttrib);
        }

        public void DrawElements(Engine.BeginMode beginMode, int p, Engine.DrawElementsType drawElementsType, int offset)
        {
            var drawType = ToDrawElemensType(drawElementsType);
            var mode = ToBeginMode(beginMode);

            GL.DrawElements(mode, p, drawType, (IntPtr)offset);
        }

        public void VertexAttribPointer(int index, int size, Engine.VertexAttribPointerType vertexAttribPointerType, bool normalized, int stride, int offset)
        {
            var vertexPointerType = ToVertexAttribPointerType(vertexAttribPointerType);
            GL.VertexAttribPointer(index, size, vertexPointerType, normalized, stride, offset);
        }

        public void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public void ClearColor(float r, float g, float b, float a)
        {
            GL.ClearColor(r, g, b, a);
        }

        public void Enable(Engine.EnableCap enableCap)
        {
            var cap = ToEnableCap(enableCap);
            GL.Enable(cap);
            
        }

        public void UseProgram(int programId)
        {
            GL.UseProgram(programId);
        }


        public void Clear(Engine.ClearBufferMask mask)
        {
            GL.Clear((ClearBufferMask)mask);
        }

        #region Enum conversions

        private static BufferTarget ToBufferTarget(Engine.BufferTarget bufferTarget)
        {
            BufferTarget target = BufferTarget.ArrayBuffer;

            switch (bufferTarget)
            {
                case Engine.BufferTarget.ArrayBuffer:
                    target = BufferTarget.ArrayBuffer;
                    break;
                case Engine.BufferTarget.ElementArrayBuffer:
                    target = BufferTarget.ElementArrayBuffer;
                    break;
            }
            return target;
        }

        private static BufferUsageHint ToBufferUsage(Engine.BufferUsage bufferUsage)
        {
            BufferUsageHint usage = BufferUsageHint.StaticDraw;

            switch (bufferUsage)
            {
                case Engine.BufferUsage.StaticDraw:
                    usage = BufferUsageHint.StaticDraw;
                    break;
            }

            return usage;
        }

        private static DrawElementsType ToDrawElemensType(Engine.DrawElementsType drawElementsType)
        {
            DrawElementsType drawType = DrawElementsType.UnsignedByte;

            switch (drawElementsType)
            {
                case Engine.DrawElementsType.UnsignedShort:
                    drawType = DrawElementsType.UnsignedShort;
                    break;
            }

            return drawType;
        }

        private static BeginMode ToBeginMode(Engine.BeginMode beginMode)
        {
            BeginMode mode = BeginMode.LineLoop;

            switch (beginMode)
            {
                case Engine.BeginMode.Triangles:
                    mode = BeginMode.Triangles;
                    break;
            }

            return mode;
        }

        private static VertexAttribPointerType ToVertexAttribPointerType(Engine.VertexAttribPointerType pointerType)
        {
            VertexAttribPointerType type = VertexAttribPointerType.Byte;

            switch (pointerType)
            {
                case Engine.VertexAttribPointerType.Float:
                    type = VertexAttribPointerType.Float;
                    break;
            }

            return type;
        }

        private static EnableCap ToEnableCap(Engine.EnableCap enableCap)
        {
            EnableCap cap = EnableCap.Blend;

            switch (enableCap)
            {
                case Engine.EnableCap.DepthTest:
                    cap = EnableCap.DepthTest;
                    break;
            }

            return cap;
        }

        #endregion
    }
}
