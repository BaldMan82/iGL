using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;

namespace iGL.TestGame
{
    public class TestGame : Game
    {
        public TestGame()
        {
            TestScene scene = new TestScene();
            SetScene(scene);

            LoadScene();
        }
    }
}
