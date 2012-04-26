using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;

namespace iGL.Engine.Resources
{
    public class Texture : Resource
    {
        public int TextureId { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

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
                  
                    var intBytes = new byte[8];
                    stream.Read(intBytes, 0, 8);
                    Width = BitConverter.ToInt32(intBytes, 0);
                    Height = BitConverter.ToInt32(intBytes, 4);

                    stream.Read(bytes, 0, (int)bytes.Length);

                    TextureId = Game.GL.GenTexture();
                    GL.BindTexture(TextureTarget.Texture2D, TextureId);

                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                    GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);                  

                    unsafe
                    {
                        fixed (byte* p = bytes)
                        {
                            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, Width, Height, 0, PixelFormat.Rgba, PixelType.UnsignedByte, (IntPtr)p);                           
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
