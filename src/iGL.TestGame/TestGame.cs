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
        public int TotalStarCount { get; set; }
        public int StarsCollected { get; set; }

        private int _level;        
        private bool _endingGame;
        private SlingshotBallFarseer2D _slingShotBall;
        private string _currentSceneXML;
        private UIScene _uiScene;

        public TestGame(IGL gl) : base(gl) 
        {
            _level = 1;
        }       

        public override void Load()
        {
            base.Load();

            LoadLevel();     

            /* load ui scene */

            _uiScene = new UIScene();
            SetUIScene(_uiScene);

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("iGL.TestGame.Resources.ui_level.igl")))
            {
                var sceneData = textStreamReader.ReadToEnd();               
                PopulateUIScene(sceneData);            
            }                    
        }

        public void NextLevel()
        {
            _level++;

            if (_level > 3) _level = 1;

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
              	resources = Scene.Resources.ToList();
				bufferCache = Scene.MeshBufferCache;

                Scene.Dispose(false);

            }

            _endingGame = false;
            
            var scene = new TestScene();
            scene.OnTick += scene_OnTick;
        
            SetScene(scene);

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream(string.Format("iGL.TestGame.Resources.level1_{0}.igl", _level))))
            {
                var sceneData = textStreamReader.ReadToEnd();
                _currentSceneXML = sceneData;

                PopulateScene(sceneData, resources, bufferCache);

                TotalStarCount = Scene.GameObjects.Where(g => g is Star).Count();
                StarsCollected = 0;
            
            }
         
            _slingShotBall = scene.GameObjects.First(g => g is SlingshotBallFarseer2D) as SlingshotBallFarseer2D;

           /* while (true)
            {
                var flare = new StarFlare();

                Scene.AddGameObject(flare);
              
                
                flare.PlayAnimation();

                Scene.DisposeGameObject(flare);

                //Scene.AddTimer(new Timer() { Action = () => Scene.DisposeGameObject(flare), Interval = TimeSpan.FromSeconds(0.2), Mode = Timer.TimerMode.Once });
                //Scene.Tick(0.01f);
            }*/

            w.Stop();
            Debug.WriteLine("Loadlevel:" + w.Elapsed.TotalMilliseconds);
        }      

        public void ReloadScene()
        {
            _endingGame = false;

            ReloadScene(_currentSceneXML);

            TotalStarCount = Scene.GameObjects.Where(g => g is Star).Count();
            StarsCollected = 0;
           
        }

        public void GameOver()
        {
            _uiScene.GameOver();
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
                NextLevel();                                                      
            }
        }
    }  
}
