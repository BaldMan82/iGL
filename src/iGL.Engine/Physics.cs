using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using BulletXNA.BulletDynamics;
using BulletXNA.BulletCollision;
using BulletXNA.LinearMath;

namespace iGL.Engine
{
    internal class Physics
    {
        internal DiscreteDynamicsWorld World { get; private set; }

        public Physics()
        {
            var collisionConf = new DefaultCollisionConfiguration();
            var dispatcher = new CollisionDispatcher(collisionConf);

            World = new DiscreteDynamicsWorld(dispatcher, new DbvtBroadphase(), null, collisionConf);
            var grav = new Vector3(0f, 1.0f, 0f);

            World.SetGravity(ref grav);                       
        }
    }
}
