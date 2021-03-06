using System;
using iGL.Engine.GL;
using OpenTK.Graphics.ES20;
using System.Text;
using OpenTK;

namespace iGL.iPhone
{
	public class iOSGL : IGL
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
			int length = vertexProgramSource.Length;
            GL.ShaderSource(vs, 1, new string[1] { vertexProgramSource }, ref length);
        }

        public void CompileShader(int vs)
        {
            GL.CompileShader(vs);
        }

        public void GetShaderInfoLog(int vs, out string shaderLog)
        {			
			shaderLog = GL.GetShaderInfoLog(vs);			           
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
			programLog = GL.GetProgramInfoLog(programId);
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
			float[] v = new float[16] 
            {
                matrix.Row0.X, matrix.Row0.Y, matrix.Row0.Z, matrix.Row0.W,
                matrix.Row1.X, matrix.Row1.Y, matrix.Row1.Z, matrix.Row1.W,
                matrix.Row2.X, matrix.Row2.Y, matrix.Row2.Z, matrix.Row2.W,
                matrix.Row3.X, matrix.Row3.Y, matrix.Row3.Z, matrix.Row3.W
            };
			
            GL.UniformMatrix4(loc, 1, false, v);

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
            var target = ToBufferTarget(bufferTarget);

            GL.BindBuffer(target, p);
		
        }       

        public void BufferData(Engine.BufferTarget bufferTarget, IntPtr size, IntPtr data, Engine.BufferUsage bufferUsage) 
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

            GL.DrawElements(mode, p, drawType, offset);
        }

        public void VertexAttribPointer(int index, int size, Engine.VertexAttribPointerType vertexAttribPointerType, bool normalized, int stride, int offset)
        {
           var vertexPointerType = ToVertexAttribPointerType(vertexAttribPointerType);
			
           GL.VertexAttribPointer(index, size, vertexPointerType, normalized, stride, (IntPtr)offset);
        }

        public void Viewport(int x, int y, int width, int height)
        {
            GL.Viewport(x, y, width, height);
        }

        public void ClearColor(byte r, byte g, byte b, byte a)
        {
            GL.ClearColor(r / 255.0f, g / 255.0f, b / 255.0f, a / 255.0f);
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
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
        }
		
		public void ClearColor(float r, float g, float b, float a)
		{
			GL.ClearColor(r, g, b, a);	
		}
		
		public void Disable(Engine.EnableCap disableCap)
		{
			GL.Disable(ToEnableCap(disableCap));
		}
						
		public void BlendFunc(Engine.BlendingFactorSrc src, Engine.BlendingFactorDest dest)
        {
            var s = ToBlendingFactorSrc(src);
            var d = ToBlendingFactorDest(dest);

            GL.BlendFunc(s, d);           
        }

        public void TexParameter(Engine.TextureTarget textureTarget, Engine.TextureParameterName paramName, int value)
        {
            var target = ToTextureTarget(textureTarget);
            var param = ToTextureParameterName(paramName);

            GL.TexParameter(target, param, value);
        }

        public void BindTexture(Engine.TextureTarget textureTarget, int nTexture)
        {
            var target = ToTextureTarget(textureTarget);

            GL.BindTexture(target, nTexture);
        }

        public int GenTexture()
        {
            return GL.GenTexture();
        }

        public void TexImage2D(Engine.TextureTarget textureTarget, int level, Engine.PixelInternalFormat internalformat, int width, int height, int border, Engine.PixelFormat format, Engine.PixelType type, IntPtr pixels)
        {
            var target = ToTextureTarget(textureTarget);
            var internalPixelFormat = ToInternalPixelFormat(internalformat);
            var pixelFormat = ToPixelFormat(format);
            var pixelType = ToPixelType(type);

            GL.TexImage2D(target, level, internalPixelFormat, width, height, border, pixelFormat, pixelType, pixels);
        }

        public void ActiveTexture(Engine.TextureUnit textureUnit)
        {
            var unit = ToTextureUnit(textureUnit);
            GL.ActiveTexture(unit);
        }


        #region Enum conversions
		
		private static TextureUnit ToTextureUnit(Engine.TextureUnit textureUnit)
        {
            TextureUnit unit = TextureUnit.Texture0;
            switch (textureUnit)
            {
                case Engine.TextureUnit.Texture0:
                    unit = TextureUnit.Texture0;
                    break;
            }

            return unit;
        }

        private static PixelType ToPixelType(Engine.PixelType pixelType)
        {
            PixelType type = PixelType.UnsignedByte;
            switch (pixelType)
            {
                case Engine.PixelType.UnsignedByte:
                    type = PixelType.UnsignedByte;
                    break;
            }

            return type;
        }

        private static PixelFormat ToPixelFormat(Engine.PixelFormat pixelFormat)
        {
            PixelFormat format = PixelFormat.Alpha;
            switch (pixelFormat)
            {
                case Engine.PixelFormat.Rgba:
                    format = PixelFormat.Rgba;
                    break;
            }

            return format;
        }

        private static PixelInternalFormat ToInternalPixelFormat(Engine.PixelInternalFormat internalFormat)
        {
            PixelInternalFormat format = PixelInternalFormat.Alpha;
            switch (internalFormat)
            {
                case Engine.PixelInternalFormat.Rgba:
                    format = PixelInternalFormat.Rgba;
                    break;

            }

            return format;
        }

        private static TextureTarget ToTextureTarget(Engine.TextureTarget textureTarget)
        {
            TextureTarget target = TextureTarget.Texture2D;
            switch (textureTarget)
            {
                case Engine.TextureTarget.Texture2D:
                    target = TextureTarget.Texture2D;
                    break;
            }

            return target;
        }

        private static TextureParameterName ToTextureParameterName(Engine.TextureParameterName parameterName)
        {
            TextureParameterName name = TextureParameterName.TextureMagFilter;
            switch (parameterName)
            {
                case Engine.TextureParameterName.TextureMagFilter:
                    name = TextureParameterName.TextureMagFilter;
                    break;
                case Engine.TextureParameterName.TextureMinFilter:
                    name = TextureParameterName.TextureMinFilter;
                    break;
                case Engine.TextureParameterName.TextureWrapS:
                    name = TextureParameterName.TextureWrapS;
                    break;
                case Engine.TextureParameterName.TextureWrapT:
                    name = TextureParameterName.TextureWrapT;
                    break;
            }

            return name;
        }

		
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

        private static BufferUsage ToBufferUsage(Engine.BufferUsage bufferUsage)
        {
            BufferUsage usage = BufferUsage.DynamicDraw;

            switch (bufferUsage)
            {
                case Engine.BufferUsage.StaticDraw:
                    usage = BufferUsage.StaticDraw;
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
		
		private static BlendingFactorSrc ToBlendingFactorSrc(Engine.BlendingFactorSrc blendingSrc)
        {
            BlendingFactorSrc src = BlendingFactorSrc.One;

            switch (blendingSrc)
            {
                case Engine.BlendingFactorSrc.One:
                    src = BlendingFactorSrc.One;
                    break;
            }

            return src;
        }

        private static BlendingFactorDest ToBlendingFactorDest(Engine.BlendingFactorDest blendingDest)
        {
            BlendingFactorDest dest = BlendingFactorDest.SrcColor;

            switch (blendingDest)
            {
                case Engine.BlendingFactorDest.SrcColor:
                    dest = BlendingFactorDest.SrcColor;
                    break;
            }

            return dest;
        }

        #endregion

	}
}

