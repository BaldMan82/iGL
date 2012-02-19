using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Resources;
using System.Reflection;
using System.IO;

namespace iGL.Engine
{
    public class Shader
    {
        public enum ShaderType
        {           
            VS_POINTLIGHT,           
            FS_POINTLIGHT
        }       

        public string Source { get; private set; }

        public ShaderType Type { get; private set; }

        public Shader(ShaderType shaderType)
        {
            Type = shaderType;

            LoadShader();
        }

        public void LoadShader()
        {
            var asm = this.GetType().Assembly;
            using (var textStreamReader = new StreamReader(asm.GetManifestResourceStream("iGL.Engine.Shaders." + Type.ToString() + ".c")))
            {
                Source = textStreamReader.ReadToEnd();                
            }
        }
    }
}
