using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{
    public abstract class RenderComponent : GameComponent
    {
        public RenderComponent(XElement xmlElement) : base(xmlElement) { }

        public RenderComponent() { }

        public abstract void Render(Matrix4 transform);
    }
}
