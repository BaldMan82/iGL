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
            System.Console.WriteLine("Starting...");
  
            TestGame.TestGame game = new TestGame.TestGame();
            game.Start();

            System.Console.ReadLine();                      
        }
    }
}
