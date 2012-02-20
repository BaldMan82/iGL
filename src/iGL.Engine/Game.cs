using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.ES20;

namespace iGL.Engine
{
    public abstract class Game
    {
        private Scene _scene;
        private GameWindow _wnd;     

        public Game()
        {
            _wnd = new GameWindow(960, 640, new GraphicsMode(16, 16), "", GameWindowFlags.Default, DisplayDevice.Default,
                                  2, 0, GraphicsContextFlags.Embedded);

            _wnd.Load += new EventHandler<EventArgs>(_wnd_Load);
            _wnd.Resize += new EventHandler<EventArgs>(_wnd_Resize);
            _wnd.UpdateFrame += new EventHandler<FrameEventArgs>(_wnd_UpdateFrame);
            _wnd.RenderFrame += new EventHandler<FrameEventArgs>(_wnd_RenderFrame);
        }        

        void _wnd_Resize(object sender, EventArgs e)
        {
            GL.Viewport(0, 0, _wnd.Width, _wnd.Height);
        }

        void _wnd_Load(object sender, EventArgs e)
        {
            Color4 color = Color4.White;
            GL.ClearColor(color.R, color.G, color.B, color.A);
            GL.Enable(EnableCap.DepthTest);
        }

        public void Start()
        {
            _wnd.Run(60);

        }

        void _wnd_RenderFrame(object sender, FrameEventArgs e)
        {
            this.Render();
        }

        void _wnd_UpdateFrame(object sender, FrameEventArgs e)
        {
            this.Tick((float)e.Time);
        }

        public void Render()
        {
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);
            
            _scene.Render();

            _wnd.SwapBuffers();
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
