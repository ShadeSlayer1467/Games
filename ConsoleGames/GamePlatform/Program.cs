using _2048Game;
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
            while (true)
            {
                _2048Engine game = new _2048Engine();
                game.StartAndRunGame();

                Console.WriteLine("Do you want to play again? (Y/N)");
                string response = Console.ReadLine();

                if (response.ToLower() != "y")
                {
                    break;
                }
            }

            Console.ReadKey();
        }
    }
}
