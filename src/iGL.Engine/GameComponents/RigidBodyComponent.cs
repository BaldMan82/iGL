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
        private float _mass { get; set; }

        public float Mass
        {
            get
            {
                return _mass;
            }

            set
            {
                _mass = value;
                UpdateRigidBody();
            }
        }

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
            
            Vector3 inertia;
            ColliderComponent.CollisionShape.CalculateLocalInertia(_mass, out inertia);

            RigidBody = new RigidBody(_mass, motionState, ColliderComponent.CollisionShape, inertia);           

            RigidBody.SetWorldTransform(GameObject.Location.ToBullet());

            GameObject.Scene.Physics.World.AddRigidBody(RigidBody);
            
        }

        private void UpdateRigidBody()
        {
            if (!IsLoaded) return;

            Vector3 inertia;
            ColliderComponent.CollisionShape.CalculateLocalInertia(_mass, out inertia);

            RigidBody.SetMassProps(_mass, inertia);
        }

        public override void Tick(float timeElapsed)
        {
            /* update game object position */

            GameObject.Location = RigidBody.GetWorldTransform().ToOpenTK();
        }
    }
}
