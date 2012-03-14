using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public class LightObject : GameObject
    {
        public LightComponent LightComponent { get; private set; }

        public LightObject(ILight light)
        {
            LightComponent = new LightComponent(light);
            AddComponent(LightComponent);
        }
    }
}
