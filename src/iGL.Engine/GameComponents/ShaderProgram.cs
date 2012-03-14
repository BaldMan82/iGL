using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class ShaderProgram
    {        
        private List<Shader> _vertexShaders;
        private List<Shader> _fragmentShaders;
        private Dictionary<string, int> _uniformLocations = new Dictionary<string, int>();

        public IGL GL { get { return Game.GL; } }

        public int ProgramId { get; private set; }



        public ShaderProgram(Shader vertexShader, Shader fragmentShader) : 
            this(new List<Shader>() { vertexShader }, new List<Shader>() { fragmentShader }) {}
        
        public ShaderProgram(List<Shader> vertexShaders, List<Shader> fragmentShaders)
        {
            _vertexShaders = vertexShaders;
            _fragmentShaders = fragmentShaders;

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

            var vs = GL.CreateShader(ShaderType.VertexShader);

            GL.ShaderSource(vs, vertexProgramSource);

            GL.CompileShader(vs);

            GL.GetShaderInfoLog(vs, out shaderLog);

            if (!string.IsNullOrEmpty(shaderLog) && !shaderLog.Contains("successfully compiled")) throw new Exception(shaderLog);

            GL.AttachShader(ProgramId, vs);

            /* load fragment shader */

            string fragmentShaderSource = string.Empty;

            foreach (var fragmentShader in _fragmentShaders)
            {
                fragmentShaderSource += fragmentShader.Source;
            }

            var fs = GL.CreateShader(ShaderType.FragmentShader);

            GL.ShaderSource(fs, fragmentShaderSource);

            GL.CompileShader(fs);

            GL.GetShaderInfoLog(fs, out shaderLog);

            if (!string.IsNullOrEmpty(shaderLog) && !shaderLog.Contains("successfully compiled")) throw new Exception(shaderLog);

            GL.AttachShader(ProgramId, fs);

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

        public void SetModelViewMatrix(Matrix4 modelView)
        {
            var loc = GetUniformLocation("u_modelViewMatrix");
            GL.UniformMatrix4(loc, false, modelView);
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

        public void SetAmbientColor(Vector4 color)
        {
            var loc = GetUniformLocation("u_globalAmbientColor");

            GL.Uniform4(loc, color);
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
    }
}
