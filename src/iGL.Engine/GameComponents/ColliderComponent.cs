using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision.Shapes;

namespace iGL.Engine
{
    public abstract class ColliderComponent : GameComponent
    {
        public bool Enabled { get; set; }
        public RigidBodyComponent AttachedRigidBody { get; set; }

        internal Shape CollisionShape { get; set; }

        public ColliderComponent() { }
       
        public override bool InternalLoad()
        {
            /* game object must have a rigid body component */
            //AttachedRigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

            //if (AttachedRigidBody == null) throw new NotSupportedException("Collider must have a rigid body component");

            return true;
        }

        internal abstract void Reload();

        public override void Tick(float timeElapsed) { }        
    }
}
