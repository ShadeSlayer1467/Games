﻿using _2048Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GamePlatform
{
    internal class Program
    {
        static void Main(string[] args)
        {
            GamesEngine engine = new GamesEngine();
            engine.Run();
        }
    }
}
