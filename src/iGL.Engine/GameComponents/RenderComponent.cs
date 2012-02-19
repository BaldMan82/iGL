using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace iGL.Engine
{
    public abstract class RenderComponent : GameComponent
    {
        public RenderComponent(GameObject gameObject)
            : base(gameObject)
        {
            
        }

        public abstract void Render();
    }
}
