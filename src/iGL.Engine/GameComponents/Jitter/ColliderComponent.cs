using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;
using System.Runtime.Serialization;

namespace iGL.Engine
{
    [Serializable]
    public abstract class ColliderComponent : GameComponent
    {
        internal Shape CollisionShape { get; set; }

        public ColliderComponent(SerializationInfo info, StreamingContext context) : base(info, context) { }
        
        public ColliderComponent() { }
       
        public override bool InternalLoad()
        {
            return true;
        }

        internal abstract void Reload();

        public override void Tick(float timeElapsed) { }        
    }
}
