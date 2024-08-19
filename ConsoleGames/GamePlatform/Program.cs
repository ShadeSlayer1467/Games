﻿using _2048Game;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.Title = "Game Platform";
            Console.CursorVisible = false;
            ConsoleEngine engine = new ConsoleEngine();
            engine.Run();
        }
    }
}
