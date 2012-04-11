using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine;
using iGL.Engine.GL;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;

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

            using (var textStreamReader = new StreamReader(this.GetType().Assembly.GetManifestResourceStream("iGL.TestGame.Resources.scene.igl")))
            {
                var sceneData = textStreamReader.ReadToEnd();
                LoadScene(sceneData);
            }          
        }
    }  
}
