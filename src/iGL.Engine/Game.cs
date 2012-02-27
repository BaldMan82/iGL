using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;

namespace iGL.Engine
{
    public abstract class Game
    {
        private Scene _scene;
        
        public static IGL GL { get ; private set; }

        public Game(IGL gl)
        {          
            GL = gl;
        }        

        public void Resize(int width, int height)
        {
            GL.Viewport(0, 0, width, height);
        }

        public virtual void Load()
        {
            GL.ClearColor(200, 200, 200, 255);
            GL.Enable(EnableCap.DepthTest);
        }
      
        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _scene.Render();           
        }

        public void Tick(float timeElapsed)
        {
            _scene.Tick(timeElapsed);        
        }

        public void SetScene(Scene scene)
        {
            scene.Game = this;
            _scene = scene;
        }

        public void LoadScene()
        {           
            _scene.Load();
        }

    }
}
