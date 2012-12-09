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
    public class PointLightShader : ShaderProgram
    {
        public PointLightShader()
            : base(ProgramType.POINTLIGHT, new Shader(Shader.ShaderType.VS_POINTLIGHT), new Shader(Shader.ShaderType.FS_POINTLIGHT)) { }                               
      
		public void SetLight(ILight light)
		{
			if (!(light is PointLight)) throw new NotSupportedException("Only pointlights supported in this shader");
			
			var baseLight = light as PointLight;
			
			var loc = GetUniformLocation("u_light.position");
			GL.Uniform4(loc, baseLight.WorldPosition);
			
			loc = GetUniformLocation("u_light.ambient");
			GL.Uniform4(loc, baseLight.Ambient);
			
			loc = GetUniformLocation("u_light.diffuse");
			GL.Uniform4(loc, baseLight.Diffuse);
			
			loc = GetUniformLocation("u_light.specular");
			GL.Uniform4(loc, baseLight.Specular);
		}

        public void ClearLight()
        {
            var loc = GetUniformLocation("u_light.position");
            GL.Uniform4(loc, Vector4.Zero);

            loc = GetUniformLocation("u_light.ambient");
            GL.Uniform4(loc, Vector4.Zero);

            loc = GetUniformLocation("u_light.diffuse");
            GL.Uniform4(loc, Vector4.Zero);

            loc = GetUniformLocation("u_light.specular");
            GL.Uniform4(loc, Vector4.Zero);
        }
    }
}
