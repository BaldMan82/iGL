using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Dynamics;

namespace iGL.Engine
{
    public interface IPhysics
    {
        void Step(float timeStep);
        void AddBody(RigidBody body);
        void RemoveBody(RigidBody body);
    }
}
