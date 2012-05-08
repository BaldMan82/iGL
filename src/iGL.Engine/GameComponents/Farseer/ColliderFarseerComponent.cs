using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using FarseerPhysics.Collision.Shapes;

namespace iGL.Engine
{
    [Serializable]
    public abstract class ColliderFarseerComponent : GameComponent
    {
        internal Shape CollisionShape { get; set; }

        public ColliderFarseerComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }

        public ColliderFarseerComponent() { }
       
        public override bool InternalLoad()
        {
            return true;
        }

        internal abstract void Reload();

        public override void Tick(float timeElapsed) { }        
    }
}
