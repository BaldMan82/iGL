using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;

namespace iGL.Engine
{
    public abstract class RenderComponent : GameComponent
    {       
        public abstract void Render(Matrix4 transform);
    }
}
