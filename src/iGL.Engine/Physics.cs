using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Collision;

namespace iGL.Engine
{
    internal class Physics
    {
        internal Jitter.World World { get; private set; }

        public Physics()
        {
            CollisionSystem collision = new CollisionSystemPersistentSAP();
            World = new Jitter.World(collision);
            World.AllowDeactivation = true;
            //World.Gravity = new Jitter.LinearMath.JVector(0, 0, 0);
        }
    }
}
