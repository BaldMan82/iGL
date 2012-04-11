using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Xml.Serialization;

namespace iGL.Engine
{
    [Serializable]
    public class Material 
    {
        public string Id { get; set; }

        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }  
        public float Shininess { get; set; }
        public string TextureName { get; set; }       
    }
}
