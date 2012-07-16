using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace iGL.Engine
{
    public class DesignFarseerPhysics : PhysicsBase
    {
        private World _world;

        public DesignFarseerPhysics()
        {
            _world = new World(new Vector2(0, -9.81f));

        }
        public override void Step(float timeStep)
        {
            _world.Step(timeStep);
        }

        public override void AddBody(object body)
        {
           
        }

        public override void RemoveBody(object body)
        {
            
        }

        public override object GetWorld()
        {
            return _world;
        }


        public override event EventHandler<Events.CollisionEvent> CollisionEvent;

        public override void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
