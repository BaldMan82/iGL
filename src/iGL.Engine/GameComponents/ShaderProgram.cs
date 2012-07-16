using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class ShaderProgram : IDisposable
    {        
        private List<Shader> _vertexShaders;
        private List<Shader> _fragmentShaders;
        private Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();

        public IGL GL { get { return Game.GL; } }

        public int ProgramId { get; private set; }
        public int VertexShaderId { get; private set; }
        public int FragmentShaderId { get; private set; }

        public enum ProgramType
        {
            POINTLIGHT,
            FUR
        }

        public ProgramType Type { get; private set; }

        public ShaderProgram(ProgramType programType, Shader vertexShader, Shader fragmentShader) : 
            this(programType, new List<Shader>() { vertexShader }, new List<Shader>() { fragmentShader }) {}
        
        public ShaderProgram(ProgramType programType, List<Shader> vertexShaders, List<Shader> fragmentShaders)
        {
            _vertexShaders = vertexShaders;
            _fragmentShaders = fragmentShaders;

            Type = programType;

            ProgramId = -1;
        }

        public void Load()
        {
            string shaderLog;

            if (_vertexShaders == null) throw new NotSupportedException("Vertex shader required");
            if (_fragmentShaders == null) throw new NotSupportedException("Fragment shader required");

            /* create program */

            ProgramId = GL.CreateProgram();

            /* load vertex shader */

            var vertexProgramSource = string.Empty;
            
            foreach (var vertexShader in _vertexShaders)
            {
                vertexProgramSource += vertexShader.Source;
            }

            VertexShaderId = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(VertexShaderId, vertexProgramSource);

            GL.CompileShader(VertexShaderId);

            GL.GetShaderInfoLog(VertexShaderId, out shaderLog);

            if (!string.IsNullOrEmpty(shaderLog) && !shaderLog.Contains("successfully compiled")) throw new Exception(shaderLog);

            GL.AttachShader(ProgramId, VertexShaderId);

            /* load fragment shader */

            string fragmentShaderSource = string.Empty;

            foreach (var fragmentShader in _fragmentShaders)
            {
                fragmentShaderSource += fragmentShader.Source;
            }

            FragmentShaderId = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(FragmentShaderId, fragmentShaderSource);

            GL.CompileShader(FragmentShaderId);

            GL.GetShaderInfoLog(FragmentShaderId, out shaderLog);

            if (!string.IsNullOrEmpty(shaderLog) && !shaderLog.Contains("successfully compiled")) throw new Exception(shaderLog);

            GL.AttachShader(ProgramId, FragmentShaderId);

            GL.BindAttribLocation(ProgramId, 0, "a_position");
            GL.BindAttribLocation(ProgramId, 1, "a_normal");

            GL.LinkProgram(ProgramId);

            string programLog;
            GL.GetProgramInfoLog(ProgramId, out programLog);

            if (!programLog.Split(new char[] { '\n' }).All(s => string.IsNullOrEmpty(s) || s == "Vertex shader(s) linked, fragment shader(s) linked.")) throw new Exception(programLog);
        }

        public int GetVertexAttributeLocation()
        {
            return GL.GetAttribLocation(ProgramId, "a_position");
        }      

        public int GetNormalAttributeLocation()
        {
            return GL.GetAttribLocation(ProgramId, "a_normal");
        }

        public int GetUVAttributeLocation()
        {
            return GL.GetAttribLocation(ProgramId, "a_uv");
        }
      
        public void SetMaterial(Material material)
        {
            var loc = GetUniformLocation("u_material.ambient");
            GL.Uniform4(loc, material.Ambient);

            loc = GetUniformLocation("u_material.diffuse");
            GL.Uniform4(loc, material.Diffuse);

            loc = GetUniformLocation("u_material.specular");
            GL.Uniform4(loc, material.Specular);

            loc = GetUniformLocation("u_material.shininess");
            GL.Uniform1(loc, material.Shininess);
        }

        public void SetSamplerUnit(int unit)
        {
            var loc = GetUniformLocation("s_texture");
            GL.Uniform1(loc, unit);
        }

        public void SetNormalSamplerUnit(int unit)
        {
            var loc = GetUniformLocation("s_normalTexture");
            GL.Uniform1(loc, unit);
        }

        public void SetModelViewMatrix(Matrix4 modelView)
        {
            var loc = GetUniformLocation("u_modelViewMatrix");
            GL.UniformMatrix4(loc, false, modelView);
        }

        public void SetModelViewInverseMatrix(Matrix4 modelViewInverse)
        {
            var loc = GetUniformLocation("u_modelViewInverseMatrix");
            GL.UniformMatrix4(loc, false, modelViewInverse);
        }

        public void SetModelViewProjectionMatrix(Matrix4 modelViewProjection)
        {
            var loc = GetUniformLocation("u_modelViewProjectionMatrix");
            GL.UniformMatrix4(loc, false, modelViewProjection);
        }

        public void SetTransposeAdjointModelViewMatrix(Matrix4 modelViewTransposeInverse)
        {
            var loc = GetUniformLocation("u_transposeAdjointModelViewMatrix");
            GL.UniformMatrix4(loc, false, modelViewTransposeInverse);
        }

        public void SetHasTexture(bool hasTexture)
        {
            var loc = GetUniformLocation("u_hasTexture");
            GL.Uniform1(loc, hasTexture ? 1f : 0f);
        }

        public void SetHasNormalTexture(bool hasNormalTexture)
        {
            var loc = GetUniformLocation("u_hasNormalTexture");
            GL.Uniform1(loc, hasNormalTexture ? 1f : 0f);
        }

        public void SetAmbientColor(Vector4 color)
        {
            var loc = GetUniformLocation("u_globalAmbientColor");
            GL.Uniform4(loc, color);
        }

        public void SetEyePos(Vector4 eyePos)
        {
            var loc = GetUniformLocation("u_eyePos");
            GL.Uniform4(loc, eyePos);
        }

        protected int GetUniformLocation(string uniform)
        {
            int loc;
            if (_uniformLocations.TryGetValue(uniform,out loc))
            {
                return loc;
            }
            else
            {
                loc = GL.GetUniformLocation(this.ProgramId, uniform);
                _uniformLocations.Add(uniform, loc);
            }

            //if (loc == -1) throw new InvalidOperationException(uniform);

            return loc;
        }

        public void Use()
        {
            if (ProgramId == -1) throw new Exception("Invalid program");

            GL.UseProgram(ProgramId);
        }

        public void Dispose()
        {
            GL.DeleteShader(VertexShaderId);
            GL.DeleteShader(FragmentShaderId);
            GL.DeleteProgram(ProgramId);
        }
    }
}
