using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;
using Jitter.Dynamics;

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
