using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using iGL.Engine.GL;
using iGL.Engine.Math;
using iGL.Engine.Events;

namespace iGL.Engine
{
    public abstract class Game
    {
        public Scene Scene { get; private set; }
        public Size WindowSize { get; private set; }        
        public static IGL GL { get; private set; }
     
        public Game(IGL gl)
        {
            GL = gl;
        }

        public void Resize(int width, int height)
        {
            WindowSize = new Size(width, height);
            GL.Viewport(0, 0, width, height);
        }

        public virtual void Load()
        {          
            GL.Enable(EnableCap.DepthTest);
            GL.Enable(EnableCap.CullFace);
        }

        public void MouseMove(int x, int y)
        {
            Scene.MouseMove(x, y);           
        }

        public void MouseButton(MouseButton button, bool down, int x, int y)
        {
            Scene.UpdateMouseButton(button,down, x, y);
        }
      
        public void Render()
        {           
            Scene.Render();
        }

        public void Tick(float timeElapsed)
        {
            Scene.Tick(timeElapsed);
        }

        public void SetScene(Scene scene)
        {
            scene.Game = this;
            Scene = scene;
        }

        public void LoadScene()
        {
            Scene.Load();
        }

        public void SaveScene()
        {

        }

    }
}
