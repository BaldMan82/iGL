using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Jitter.Dynamics;
using iGL.Engine.Events;

namespace iGL.Engine
{
    public interface IPhysics
    {
        void Step(float timeStep);
        void AddBody(object body);
        void RemoveBody(object body);

        object GetWorld();

        event EventHandler<CollisionEvent> CollisionEvent;
    }

    public abstract class PhysicsBase : IPhysics, IDisposable
    {

        public abstract void Step(float timeStep);


        public abstract void AddBody(object body);


        public abstract void RemoveBody(object body);

        public abstract object GetWorld();


        public abstract event EventHandler<CollisionEvent> CollisionEvent;

        public abstract void Dispose();
        
    }
}
