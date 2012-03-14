﻿using System;
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
        private Scene _scene;
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
        }

        public void MouseMove(int x, int y)
        {
            _scene.MouseMove(x, y);
        }

        public void MouseButton(MouseButton button, bool down, int x, int y)
        {
            _scene.UpdateMouseButton(button,down, x, y);
        }
      
        public void Render()
        {           
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
