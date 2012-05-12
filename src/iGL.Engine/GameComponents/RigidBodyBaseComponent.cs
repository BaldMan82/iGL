using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;
using System.Xml.Linq;

namespace iGL.Engine
{
    public abstract class RigidBodyBaseComponent : GameComponent
    {
        internal Matrix4 RigidBodyTransform { get; set; }
        
        public bool AutoReloadBody { get; set; }

        public RigidBodyBaseComponent(XElement xmlElement) : base(xmlElement) { }
        public RigidBodyBaseComponent() { }
        
    }
}
