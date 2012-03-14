using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public class LightObject : GameObject
    {
        public LightComponent LightComponent { get; private set; }
        
        public LightObject() : this(new PointLight(){
             Ambient = new Vector4(1,1,1,1),
             Diffuse = new Vector4(1,1,1,1)
        })
        {
            var sphere = new Sphere(0.5f);
            sphere.Material.Ambient = new Vector4(1, 1, 0, 1);
            sphere.Designer = true;

            AddChild(sphere);
        }

        public LightObject(ILight light)
        {
            LightComponent = new LightComponent(light);
            AddComponent(LightComponent);
        }
    }
}
