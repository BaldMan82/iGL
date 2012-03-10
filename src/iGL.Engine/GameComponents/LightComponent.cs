﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public interface ILight { }

    public class PointLight : ILight
    {
        public Vector4 Ambient { get; set; }
        public Vector4 Diffuse { get; set; }
        public Vector4 Specular { get; set; }
    }   


    public class LightComponent : GameComponent
    {        
        public ILight Light { get; private set; }

        public LightComponent(GameObject gameObject, ILight light)
        {
            Light = light;
        }

        public override void InternalLoad()
        {
           
        }

        public override void Tick(float timeElapsed)
        {
            
        }
    }
}
