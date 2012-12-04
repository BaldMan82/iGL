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
using System.Diagnostics;

namespace iGL.TestGame
{
    public class TestGame : Game
    {
        private int _level;
        private int _starCount;
        private bool _endingGame;
        private SlingshotBallFarseer2D _slingShotBall;
        private string _currentSceneXML;

        public TestGame(IGL gl) : base(gl) 
        {
            _level = 1;
        }       

        public override void Load()
        {
            base.Load();

            LoadLevel();     

            /* load ui scene */

            var uiScene = new UIScene();
            SetUIScene(uiScene);

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("iGL.TestGame.Resources.ui_level.igl")))
            {
                var sceneData = textStreamReader.ReadToEnd();
                _currentSceneXML = sceneData;

                PopulateUIScene(sceneData);            
            }                    
        }

        public void NextLevel()
        {
            _level++;

            if (_level > 2) _level = 1;

            LoadLevel();
        }

        public void LoadLevel()
        {
            Stopwatch w = new Stopwatch();
            w.Start();

			var resources = new List<Resource>();
			var bufferCache = new Dictionary<string, int[]>();

            if (Scene != null)
            {
                Scene.OnDisposeObject -= TestScene_OnDisposeObject;
				resources = Scene.Resources.ToList();
				bufferCache = Scene.MeshBufferCache;

                Scene.Dispose(false);

            }

            _endingGame = false;
            
            var scene = new TestScene();
            scene.OnTick += scene_OnTick;
            scene.OnDisposeObject += TestScene_OnDisposeObject;
          
            SetScene(scene);

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(string.Format("iGL.TestGame.Resources.level1_{0}.igl", _level))))
            {
                var sceneData = textStreamReader.ReadToEnd();
                _currentSceneXML = sceneData;

                PopulateScene(sceneData, resources, bufferCache);

                _starCount = Scene.GameObjects.Where(g => g is Star).Count();
            }

            _slingShotBall = scene.GameObjects.First(g => g is SlingshotBallFarseer2D) as SlingshotBallFarseer2D;

            //while (true)
            //{
            //    var flare = new StarFlare();

            //    Scene.AddGameObject(flare);

            //    flare.PlayAnimation();

            //    Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(flare), Interval = TimeSpan.FromSeconds(0.2), Mode = Timer.TimerMode.Once });
            //    Scene.Tick(0.01f);
            //}

            w.Stop();
            Debug.WriteLine("Loadlevel:" + w.Elapsed.TotalMilliseconds);
        }

        public void ReloadScene()
        {
            _endingGame = false;

            ReloadScene(_currentSceneXML);
            _starCount = Scene.GameObjects.Where(g => g is Star).Count();
          
        }

        public void EndGame()
        {
            _endingGame = true;
        }

        void scene_OnTick(object sender, Engine.Events.TickEvent e)
        {
            if (Game.InDesignMode) return;

            if (_endingGame)
            {               
                //if (_slingShotBall._rigidBodyComponent.Sleeping)
                {
                    NextLevel();                                       
                }
            }
        }

        void TestScene_OnDisposeObject(object sender, Engine.Events.DisposeObjectEvent e)
        {
            //if (e.GameObject is Star)
            //{
            //    _starCount--;

            //    if (_starCount == 0)
            //    {
            //        _endingGame = true;                   
            //    }
            //}
        }
    }  
}
