using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using OpenGL = OpenTK.Graphics.OpenGL;
using OpenTK;

namespace iGL.Engine
{
    public class Texture : Resource
    {
        public int TextureId { get; private set; }

        public Texture()
        {

        }

        protected override bool InternalLoad()
        {
            try
            {
                var resourceAsm = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(asm => asm.GetManifestResourceNames().Contains(base.ResourceName));

                if (resourceAsm == null) throw new Exception(base.ResourceName + " not found.");

                using (var stream = resourceAsm.GetManifestResourceStream(base.ResourceName))
                {
                    var bytes = new byte[stream.Length - 8];

                    int height = 0, width = 0;
                    var intBytes = new byte[8];
                    stream.Read(intBytes, 0, 8);
                    width = BitConverter.ToInt32(intBytes, 0);
                    height = BitConverter.ToInt32(intBytes, 4);

                    stream.Read(bytes, 0, (int)bytes.Length);

                    TextureId = OpenGL.GL.GenTexture();

                    OpenGL.GL.BindTexture(OpenGL.TextureTarget.Texture2D, TextureId);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMinFilter, (int)OpenTK.Graphics.OpenGL.TextureMinFilter.Linear);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureMagFilter, (int)OpenTK.Graphics.OpenGL.TextureMagFilter.Linear);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapS, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);
                    OpenTK.Graphics.OpenGL.GL.TexParameter(OpenTK.Graphics.OpenGL.TextureTarget.Texture2D, OpenTK.Graphics.OpenGL.TextureParameterName.TextureWrapT, (int)OpenTK.Graphics.OpenGL.TextureWrapMode.ClampToEdge);                
                    unsafe
                    {
                        fixed (byte* p = bytes)
                        {
                            OpenGL.GL.TexImage2D(OpenGL.TextureTarget.Texture2D, 0, OpenGL.PixelInternalFormat.Rgba, width, height, 0, OpenGL.PixelFormat.Rgba, OpenGL.PixelType.UnsignedByte, (IntPtr)p);
                        }
                    }

                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}
