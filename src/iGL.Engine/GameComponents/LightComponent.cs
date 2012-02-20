using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public class LightComponent : GameComponent
    {
        public ILight Light { get; private set; }

        public LightComponent(GameObject gameObject, ILight light) : base (gameObject)
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
