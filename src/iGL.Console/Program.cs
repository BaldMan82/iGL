﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using OpenTK;
using OpenTK.Graphics;
using System.Diagnostics;
using iGL.Engine.Events;
using iGL.Engine;

namespace iGL.Console
{
    class Program
    {
        private static TestGame.TestGame game;
        private static GameWindow gameWnd;
        private static Stopwatch tickWatch;
        private static Stopwatch renderWatch;
        private static float stepInterval = 1.0f / 80.0f;

        static void Main(string[] args)
        {
            System.Console.WriteLine("Starting...");

            var gl = new WinGL();

            game = new TestGame.TestGame(gl);
            gameWnd = new GameWindow(960, 640, new GraphicsMode(16, 16), "", GameWindowFlags.Fullscreen, DisplayDevice.Default,
                                  2, 0, GraphicsContextFlags.Default);

            tickWatch = new Stopwatch();
            renderWatch = new Stopwatch();

            tickWatch.Start();
            renderWatch.Start();

            gameWnd.Load += new EventHandler<EventArgs>(gameWnd_Load);
            gameWnd.RenderFrame += new EventHandler<FrameEventArgs>(gameWnd_RenderFrame);
            gameWnd.UpdateFrame += new EventHandler<FrameEventArgs>(gameWnd_UpdateFrame);
            gameWnd.Resize += new EventHandler<EventArgs>(gameWnd_Resize);
            gameWnd.Mouse.Move += new EventHandler<OpenTK.Input.MouseMoveEventArgs>(Mouse_Move);
            gameWnd.Mouse.ButtonDown += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonDown);
            gameWnd.Mouse.ButtonUp += new EventHandler<OpenTK.Input.MouseButtonEventArgs>(Mouse_ButtonUp);
            gameWnd.Mouse.WheelChanged += new EventHandler<OpenTK.Input.MouseWheelEventArgs>(Mouse_WheelChanged);
            gameWnd.Run();

            System.Console.ReadLine();
        }

        static void Mouse_WheelChanged(object sender, OpenTK.Input.MouseWheelEventArgs e)
        {
            game.MouseZoom(e.Delta);
        }

        static void Mouse_ButtonUp(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                game.MouseButton(MouseButton.Button1, e.IsPressed, e.X, e.Y);
            }

            if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                game.MouseButton(MouseButton.Button2, e.IsPressed, e.X, e.Y);
            }
        }

        static void Mouse_ButtonDown(object sender, OpenTK.Input.MouseButtonEventArgs e)
        {
            if (e.Button == OpenTK.Input.MouseButton.Left)
            {
                game.MouseButton(MouseButton.Button1, e.IsPressed, e.X, e.Y);
            }

            if (e.Button == OpenTK.Input.MouseButton.Right)
            {
                game.MouseButton(MouseButton.Button2, e.IsPressed, e.X, e.Y);
            }
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
           

        }

        static void gameWnd_RenderFrame(object sender, FrameEventArgs e)
        {
            bool runningBehind = false;
            if (tickWatch.ElapsedMilliseconds >= (stepInterval * 0.9f)*1000.0f)
            {
                //System.Console.WriteLine(tickWatch.ElapsedMilliseconds);

                if (tickWatch.ElapsedMilliseconds > stepInterval * 1000.0f)
                {                    
                    runningBehind = true;
                }

                game.Tick(stepInterval);

                tickWatch.Restart();


            }

            if (runningBehind)
            {
                renderWatch.Restart();
                return;
            }       

            if (renderWatch.Elapsed.TotalSeconds > 1d / 60d)
            {
                renderWatch.Restart();

                game.Render();
                ((GameWindow)sender).SwapBuffers();
            }
        }

        static void gameWnd_Load(object sender, EventArgs e)
        {
            game.Load();
        }
    }
}
