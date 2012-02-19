using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace iGL.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            
            Thread renderThread = new Thread(RenderThread);

            System.Console.WriteLine("Starting...");
            renderThread.Start();

            System.Console.ReadLine();

            renderThread.Join();
        }

        static void RenderThread()
        {
            TestGame.TestGame game = new TestGame.TestGame();

            game.Start();

            while (true)
            {               
                Thread.Sleep(100);
            }
        }
    }
}
