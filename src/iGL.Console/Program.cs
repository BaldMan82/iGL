using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;

namespace iGL.Console
{
    class Program
    {
        private static TestGame.TestGame game;

        static void Main(string[] args)
        {                    
            System.Console.WriteLine("Starting...");

            var gl = new WinGL();

            game = new TestGame.TestGame(gl);
            var gameWnd = new GameWindow(960, 640, new GraphicsMode(16, 16), "", GameWindowFlags.Default, DisplayDevice.Default,
                                  2, 0, GraphicsContextFlags.Embedded);

            gameWnd.Load += new EventHandler<EventArgs>(gameWnd_Load);
            gameWnd.RenderFrame += new EventHandler<FrameEventArgs>(gameWnd_RenderFrame);
            gameWnd.UpdateFrame += new EventHandler<FrameEventArgs>(gameWnd_UpdateFrame);
            gameWnd.Run();          

            System.Console.ReadLine();                      
        }

        static void gameWnd_UpdateFrame(object sender, FrameEventArgs e)
        {
            game.Tick((float)e.Time);
        }

        static void gameWnd_RenderFrame(object sender, FrameEventArgs e)
        {
            game.Render();
            ((GameWindow)sender).SwapBuffers();
        }

        static void gameWnd_Load(object sender, EventArgs e)
        {
            game.Load();
        }
    }
}
