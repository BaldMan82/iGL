using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletXNA.BulletDynamics;
using BulletXNA;
using BulletXNA.LinearMath;

namespace iGL.Engine
{
    public class RigidBodyComponent : GameComponent
    {
        public ColliderComponent ColliderComponent { get; private set; }

        internal RigidBody RigidBody { get; private set; }

        public RigidBodyComponent(GameObject gameObject) : base(gameObject)
        {
            
        }

        public override void InternalLoad()
        {
            /* game object must have a collider component */
            ColliderComponent = GameObject.Components.FirstOrDefault(c => c is ColliderComponent) as ColliderComponent;

            if (ColliderComponent == null) throw new NotSupportedException("Rigid body must have a collider component");

            /* load the collider first */
            if (!ColliderComponent.IsLoaded) ColliderComponent.Load();

            var motionState = new DefaultMotionState(GameObject.Location.ToBullet(), Matrix.Identity);
            
            Vector3 intertia;
            ColliderComponent.CollisionShape.CalculateLocalInertia(1.0f, out intertia);

            RigidBody = new RigidBody(1.0f, motionState, ColliderComponent.CollisionShape, intertia);

            var m = Matrix.Identity;
            m.Translation = new Vector3(1, 2, 3);

            var m2 = m.ToOpenTK();

            GameObject.Scene.Physics.World.AddRigidBody(RigidBody);
            
        }

        public override void Tick(float timeElapsed)
        {
            /* update game object position */

            GameObject.Location = RigidBody.GetWorldTransform().ToOpenTK();
        }
    }
}
