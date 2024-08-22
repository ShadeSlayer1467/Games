using System;

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
