using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using iGL.Engine.Math;
using iGL.Engine.GL;

namespace iGL.Engine
{
    [Serializable]
    public class UIShader : ShaderProgram
    {
        public UIShader()
            : base(ProgramType.UI, new Shader(Shader.ShaderType.VS_UI), new Shader(Shader.ShaderType.FS_UI)) { }                                     		
    }
}
