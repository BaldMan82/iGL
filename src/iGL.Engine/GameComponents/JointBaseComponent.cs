using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace iGL.Engine
{
    public abstract class JointBaseComponent : GameComponent
    {        
        public JointBaseComponent(XElement xmlElement) : base(xmlElement) { }
        public JointBaseComponent() { }
    }
}
