using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    public abstract class RenderComponent : GameComponent
    {       
        public RenderComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public RenderComponent() { }

        public abstract void Render(Matrix4 transform);
    }
}
