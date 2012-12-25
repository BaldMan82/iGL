using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using FarseerPhysics.Dynamics.Joints;

namespace iGL.Engine
{
    public abstract class JointBaseFarseerComponent : JointBaseComponent
    {
        public Joint Joint { get; protected set; }

        public JointBaseFarseerComponent(XElement xmlElement) : base(xmlElement) { }
        public JointBaseFarseerComponent() { }
       
    }
}
