using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;

namespace iGL.Engine.Textures
{
    public class Texture
    {
        public bool IsLoaded { get; private set; }
        private IGL _gl;

        internal Texture(IGL gl)
        {

        }
    }
}
