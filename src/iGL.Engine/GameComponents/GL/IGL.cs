using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.InteropServices;

namespace iGL.Engine.GL
{
    public interface IGL
    {
        int CreateProgram();

        int CreateShader(ShaderType shaderType);

        void ShaderSource(int vs, string vertexProgramSource);

        void CompileShader(int vs);

        void GetShaderInfoLog(int vs, out string shaderLog);

        void AttachShader(int programId, int vs);

        void BindAttribLocation(int programId, int index, string name);

        void LinkProgram(int programId);

        void GetProgramInfoLog(int programId, out string programLog);

        int GetAttribLocation(int programId, string p);

        void Uniform4(int loc, Math.Vector4 vector4);

        void Uniform1(int loc, float p);

        void UniformMatrix4(int loc, bool transpose, Matrix4 matrix);    

        int GetUniformLocation(int p, string uniform);

        void GenBuffers(int p, int[] bufferIds);

        void BindBuffer(BufferTarget bufferTarget, int name);

        void BufferData<T>(BufferTarget bufferTarget, IntPtr size, [InAttribute, OutAttribute] T[] data, BufferUsage bufferUsage) where T : struct;

        void EnableVertexAttribArray(int vertexAttrib);

        void DrawElements(BeginMode beginMode, int p, DrawElementsType drawElementsType, int offset);

        void VertexAttribPointer(int index, int size, VertexAttribPointerType vertexAttribPointerType, bool normalized, int stride, int offset);

        void Viewport(int x, int y, int width, int height);

        void ClearColor(float r, float g, float b, float a);

        void Enable(EnableCap enableCap);

        void Disable(EnableCap disableCap);

        void UseProgram(int programId);

        void Clear(ClearBufferMask mask);
    }
}
