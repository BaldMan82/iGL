using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;
using Jitter.Dynamics;
using Jitter.LinearMath;

namespace iGL.Engine
{
    public class Physics : IPhysics
    {
        internal Jitter.World World { get; private set; }
       
        public Physics()
        {
            CollisionSystem collision = new CollisionSystemPersistentSAP();
            World = new Jitter.World(collision);
            World.AllowDeactivation = true;
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
        

        public void Step(float timeStep)
        {
            World.Step(timeStep, false);
        }

        public void AddBody(Jitter.Dynamics.RigidBody body)
        {
            World.AddBody(body);
        }


        public void RemoveBody(RigidBody body)
        {
            World.RemoveBody(body);
        }
    }
}
