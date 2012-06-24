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

        void Uniform1(int loc, int p);

        void UniformMatrix4(int loc, bool transpose, Matrix4 matrix);    

        int GetUniformLocation(int p, string uniform);

        void GenBuffers(int p, int[] bufferIds);

        void DeleteBuffers(int p, int[] bufferIds);

        void BindBuffer(BufferTarget bufferTarget, int name);

        void BufferData(BufferTarget bufferTarget, IntPtr size, IntPtr data, BufferUsage bufferUsage);

        void EnableVertexAttribArray(int vertexAttrib);

        void DrawElements(BeginMode beginMode, int p, DrawElementsType drawElementsType, int offset);

        void VertexAttribPointer(int index, int size, VertexAttribPointerType vertexAttribPointerType, bool normalized, int stride, int offset);

        void Viewport(int x, int y, int width, int height);

        void ClearColor(float r, float g, float b, float a);

        void Enable(EnableCap enableCap);

        void Disable(EnableCap disableCap);

        void UseProgram(int programId);

        void Clear(ClearBufferMask mask);

        void BlendFunc(BlendingFactorSrc src, BlendingFactorDest dest);

        void TexParameter(TextureTarget textureTarget, TextureParameterName paramName, int value);

        void BindTexture(TextureTarget textureTarget, int nTexture);

        int GenTexture();

        void TexImage2D(TextureTarget textureTarget, int level, PixelInternalFormat internalformat, int width, int height, int border, PixelFormat format, PixelType type, IntPtr pixels);

        void ActiveTexture(TextureUnit textureUnit);
    }
}
