using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletXNA.BulletCollision;

namespace iGL.Engine
{
    public abstract class ColliderComponent : GameComponent
    {
        public bool Enabled { get; set; }
        public RigidBodyComponent AttachedRigidBody { get; set; }

        internal CollisionShape CollisionShape { get; set; }

        public ColliderComponent(GameObject gameObject)
            : base(gameObject)
        {
            
        }

        public override void InternalLoad()
        {
            /* game object must have a rigid body component */
            //AttachedRigidBody = GameObject.Components.FirstOrDefault(c => c is RigidBodyComponent) as RigidBodyComponent;

            //if (AttachedRigidBody == null) throw new NotSupportedException("Collider must have a rigid body component");
        }

        public override void Tick(float timeElapsed)
        {
            
        }
    }
}
