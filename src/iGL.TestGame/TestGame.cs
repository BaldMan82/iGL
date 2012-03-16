using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.GL;

namespace iGL.TestGame
{
    public class TestGame : Game
    {
        public TestGame(IGL gl) : base(gl) { }       

        public override void Load()
        {
            base.Load();

            //CastleScene scene = new CastleScene();
            //CubeScene scene = new CubeScene();
            //TestScene scene = new TestScene();
            SpaceScene scene = new SpaceScene();

            SetScene(scene);

            LoadScene();
        }
    }
}
