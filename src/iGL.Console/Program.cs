﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using System.Diagnostics;

namespace iGL.Console
{
    class Program
    {
        private static TestGame.TestGame game;
        private static GameWindow gameWnd;

        static void Main(string[] args)
        {                    
            System.Console.WriteLine("Starting...");

            var gl = new WinGL();

            game = new TestGame.TestGame(gl);
            gameWnd = new GameWindow(960, 640, new GraphicsMode(16, 16), "", GameWindowFlags.Default, DisplayDevice.Default,
                                  2, 0, GraphicsContextFlags.Default);

            gameWnd.Load += new EventHandler<EventArgs>(gameWnd_Load);
            gameWnd.RenderFrame += new EventHandler<FrameEventArgs>(gameWnd_RenderFrame);
            gameWnd.UpdateFrame += new EventHandler<FrameEventArgs>(gameWnd_UpdateFrame);
            gameWnd.Resize += new EventHandler<EventArgs>(gameWnd_Resize);
            gameWnd.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
            gameWnd.Run();       

            System.Console.ReadLine();                      
        }

        static void Mouse_Move(object sender, OpenTK.Input.MouseMoveEventArgs e)
        {            
            game.MouseMove(e.X, e.Y);
        }

        static void gameWnd_Resize(object sender, EventArgs e)
        {
            var size = ((GameWindow)sender).ClientSize;
            game.Resize(size.Width, size.Height);
        }

        static void gameWnd_UpdateFrame(object sender, FrameEventArgs e)
        {           
            game.Tick((float)e.Time);                       
        }

        static void gameWnd_RenderFrame(object sender, FrameEventArgs e)
        {
            var now = DateTime.UtcNow;

            game.Render();
            ((GameWindow)sender).SwapBuffers();

            var renderTime = DateTime.UtcNow - now;

            if (renderTime.Milliseconds > 0)
            {
                int fps = 1000 / (int)renderTime.Milliseconds;

                System.Console.WriteLine("FPS: " + fps.ToString());
            }
        }

        static void gameWnd_Load(object sender, EventArgs e)
        {
            game.Load();
        }
    }
}
