using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FarseerPhysics.Dynamics;
using Microsoft.Xna.Framework;

namespace iGL.Engine
{
    public class DesignFarseerPhysics : IPhysics
    {
        private World _world;

        public DesignFarseerPhysics()
        {
            _world = new World(new Vector2(0, -9.81f));

        }
        public void Step(float timeStep)
        {
            _world.Step(timeStep);
        }

        public void AddBody(object body)
        {
           
        }

        public void RemoveBody(object body)
        {
            
        }

        public object GetWorld()
        {
            return _world;
        }
    }
}
