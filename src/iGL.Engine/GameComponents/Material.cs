using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;

namespace iGL.Engine
{   
    public class Material
    {
        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }  
        public float Shininess { get; set; }
    }
}
