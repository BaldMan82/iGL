using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.Math;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    public abstract class RigidBodyBaseComponent : GameComponent
    {
        internal Matrix4 RigidBodyTransform { get; set; }
        
        public bool AutoReloadBody { get; set; }

        public RigidBodyBaseComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
        public RigidBodyBaseComponent() { }
        
    }
}
