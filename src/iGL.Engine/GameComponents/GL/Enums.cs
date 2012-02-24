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
        Triangles
    }

    public enum EnableCap
    {
        DepthTest
    }

    [Flags]
    public enum ClearBufferMask : int
    {
        DepthBufferBit = ((int)0x00000100),
        StencilBufferBit = ((int)0x00000400),
        ColorBufferBit = ((int)0x00004000),
    }
}
