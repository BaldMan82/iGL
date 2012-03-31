using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.Math;
using iGL.TestGame.GameObjects;

namespace iGL.TestGame
{
    public class TestScene : Scene
    {
    
        public TestScene()
            : base(new DesignPhysics())
        {
            this.OnTick += new EventHandler<Engine.Events.TickEvent>(TestScene_OnTick);
        }

        void TestScene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            
        }

        public override void Load()
        {           

        }
      
    }
}
