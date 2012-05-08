using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public enum ShaderType
    {
        VertexShader,
        FragmentShader
    }

    public enum BufferUsage
    {
        StaticDraw
    }

    public enum BufferTarget
    {
        ArrayBuffer,
        ElementArrayBuffer
    }

    public enum DrawElementsType
    {
        UnsignedShort
    }

    public enum VertexAttribPointerType
    {
        Float
    }

    public enum BeginMode
    {
        Triangles,
        Lines
    }

    public enum EnableCap
    {
        DepthTest,
        CullFace,
        Texture2d,
        Blend
    }

    [Flags]
    public enum ClearBufferMask : int
    {
        DepthBufferBit = ((int)0x00000100),
        StencilBufferBit = ((int)0x00000400),
        ColorBufferBit = ((int)0x00004000),
    }

    public enum BlendingFactorSrc
    {
       One,
       SrcAlpha,
       OneMinusSrcAlpha
    }

    public enum BlendingFactorDest
    {        
        SrcColor,
        OneMinusSrcAlpha
    }

    public enum TextureTarget
    {
        Texture2D
    }

    public enum TextureParameterName
    {
        TextureMinFilter,
        TextureMagFilter,
        TextureWrapS,
        TextureWrapT
    }

    public enum TextureMinFilter
    {
        Nearest = 9728,
        Linear = 9729
    }

    public enum TextureMagFilter
    {
        Nearest = 9728,
        Linear = 9729
    }

    public enum TextureWrapMode
    {
        Clamp = 10496,
        Repeat = 10497,
        ClampToBorder = 33069,
        ClampToEdge = 33071,
        MirroredRepeat = 33648,
    }

    public enum PixelInternalFormat
    {
        Rgba = 6408
    }

    public enum PixelFormat
    {
        Rgba = 6408
    }

    public enum PixelType
    {
        UnsignedByte = 5121
    }

    public enum TextureUnit
    {
        Texture0,
        Texture1,
        Texture2,
        Texture3    
    }
}
