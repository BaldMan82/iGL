﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using iGL.Engine.Math;
using iGL.Engine.GL;

namespace iGL.Engine
{
    [Serializable]
    public class BackgroundShader : ShaderProgram
    {
        public BackgroundShader()
            : base(ProgramType.BACKGROUND, new Shader(Shader.ShaderType.VS_BACKGROUND), new Shader(Shader.ShaderType.FS_BACKGROUND)) { }

        public void SetLight(ILight light, Vector4 position)
        {
            if (!(light is PointLight)) throw new NotSupportedException("Only pointlights supported in this shader");

            var baseLight = light as PointLight;

            var loc = GetUniformLocation("u_light.position");
            GL.Uniform4(loc, position);

            loc = GetUniformLocation("u_light.ambient");
            GL.Uniform4(loc, baseLight.Ambient);

            loc = GetUniformLocation("u_light.diffuse");
            GL.Uniform4(loc, baseLight.Diffuse);

            loc = GetUniformLocation("u_light.specular");
            GL.Uniform4(loc, baseLight.Specular);
        }

        public void SetCenterPoint(ref Vector2 point)
        {
            var loc = GetUniformLocation("u_centerPoint");
            GL.Uniform2(loc, point);
        }

        public void SetMinBounds(Vector2 minBounds)
        {
            var loc = GetUniformLocation("u_minBounds");
            GL.Uniform2(loc, minBounds);
        }

        public void SetMaxBounds(Vector2 maxBounds)
        {
            var loc = GetUniformLocation("u_maxBounds");
            GL.Uniform2(loc, maxBounds);
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
