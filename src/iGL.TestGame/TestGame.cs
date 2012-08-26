using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.GL;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using iGL.TestGame.GameObjects;

namespace iGL.TestGame
{
    public class TestGame : Game
    {
        private int _level;
        private int _starCount;

        public TestGame(IGL gl) : base(gl) 
        {
            _level = 1;
        }       

        public override void Load()
        {
            base.Load();

            LoadLevel();     
        }

        public void NextLevel()
        {
            _level++;

            if (_level > 7) _level = 1;

            LoadLevel();
        }

        public void LoadLevel()
        {
            if (Scene != null)
            {
                Scene.OnDisposeObject -= TestScene_OnDisposeObject;
                Scene.Dispose();
            }

            var scene = new TestScene();
            scene.OnDisposeObject += TestScene_OnDisposeObject;

            SetScene(scene);

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(string.Format("iGL.TestGame.Resources.level1_{0}.igl", _level))))
            {
                var sceneData = textStreamReader.ReadToEnd();
                PopulateScene(sceneData);

                _starCount = Scene.GameObjects.Where(g => g is Star).Count();
            }  
        }

        void TestScene_OnDisposeObject(object sender, Engine.Events.DisposeObjectEvent e)
        {
            if (e.GameObject is Star)
            {
                _starCount--;

                if (_starCount == 0)
                {
                    Scene.AddTimer(new Timer() { Action = () => NextLevel(), Interval = TimeSpan.FromSeconds(1), Mode = Timer.TimerMode.Once });             
                }
            }
        }
    }  
}
