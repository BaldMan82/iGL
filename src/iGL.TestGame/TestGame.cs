﻿using System;
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

            var scene = new TestScene();

            SetScene(scene);

            LoadScene();

            this.SaveSceneToJson();
        }
    }
}
