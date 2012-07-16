using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;
using Jitter.Dynamics.Constraints;
using Jitter.LinearMath;
using Jitter.Dynamics;

namespace iGL.Engine
{
    public class Physics2d : PhysicsBase
    {
        internal Jitter.World World { get; private set; }

        public Physics2d()
        {
            var collision = new CollisionSystemPersistentSAP();
            collision.UseTriangleMeshNormal = true;
            
            World = new Jitter.World(collision);
            World.AllowDeactivation = true;
            World.Gravity = new Jitter.LinearMath.JVector(0, -75.0f, 0);
            World.Events.PreStep += new Jitter.World.WorldStep(Events_PreStep);
        }

        void Events_PreStep(float timestep)
        {
            Func<GameObject, bool> hasGravity = g => ((RigidBodyComponent)g.Components.Single(c => c is RigidBodyComponent)).IsGravitySource;                       
            
            var staticBodies = World.RigidBodies.Where(rb => rb.IsStatic && hasGravity((GameObject)rb.Tag)).ToList();
            var dynamicBodies = World.RigidBodies.Where(rb => !rb.IsStatic && rb.IsActive).ToList();        
           
            foreach (var body in dynamicBodies)
            {
                var totalForce = new JVector();

                foreach (var staticBody in staticBodies)
                {
                    var G = (float)(6.67300 * System.Math.Pow(10, -1));
                    var r = staticBody.Position - body.Position;

                    var gravitySize = -G * (body.Mass * staticBody.Mass) / r.LengthSquared();

                    var force = (body.Position - staticBody.Position);
                    force.Normalize();

                    force = JVector.Multiply(force, gravitySize);

                    totalForce += force;
                }

                body.AddForce(totalForce);
            }
        }

        internal class Constraint2D : Constraint
        {         
            public Constraint2D(RigidBody body)
                : base(body, null)
            {               
            }

            public override void PrepareForIteration(float timestep) { }            

            public override void Iterate()
            {
                JVector bodyLinearVelocity = this.Body1.LinearVelocity;

                var lockedVelocity = new JVector(0,0,1);
                // how much of the "lockedAxis" vector is within the current linear velocity
                float lockedAxisMagnitude = JVector.Dot(lockedVelocity, bodyLinearVelocity);
                Body1.LinearVelocity = bodyLinearVelocity - (lockedAxisMagnitude * lockedVelocity);

                var lockedAngular1 = new JVector(0, 1, 0);
                float angularAxisMagnitude = JVector.Dot(lockedAngular1, Body1.AngularVelocity);
                Body1.AngularVelocity = Body1.AngularVelocity - (angularAxisMagnitude * lockedAngular1);

                lockedAngular1 = new JVector(1,0, 0);
                angularAxisMagnitude = JVector.Dot(lockedAngular1, Body1.AngularVelocity);
                Body1.AngularVelocity = Body1.AngularVelocity - (angularAxisMagnitude * lockedAngular1);              
            
            }
        }

        public override void Step(float timeStep)
        {
            World.Step(timeStep, false);
        }


        public override void AddBody(object body)
        {
            var jitterBody = body as RigidBody;
            World.AddBody(jitterBody);
            World.Events.DeactivatedBody += Events_DeactivatedBody;

            if (!jitterBody.IsStatic)
            {
                World.AddConstraint(new Constraint2D(jitterBody));
            }
        }

        public override void RemoveBody(object body)
        {
            var jitterBody = body as RigidBody;
            World.Events.DeactivatedBody -= Events_DeactivatedBody;
            World.RemoveBody(jitterBody);
        }

        public bool CheckAllSleeping()
        {
            return World.RigidBodies.All(r => r.IsStatic || !r.IsActive);
        }

        public void SleepAll()
        {
            foreach (var body in World.RigidBodies)
            {
                var rBody = body as RigidBody;
                if (!rBody.IsStatic) rBody.IsActive = false;
            }
        }

        void Events_DeactivatedBody(RigidBody obj)
        {
            ((GameObject)obj.Tag).OnSleepEvent(obj.Tag, new Events.SleepEvent());
        }


        public override object GetWorld()
        {
            return World;
        }


        public override event EventHandler<Events.CollisionEvent> CollisionEvent;

        public override void Dispose()
        {            
        }
    }
}
