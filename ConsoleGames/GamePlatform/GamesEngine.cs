using _2048Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using TikTacToe;

namespace GamePlatform
{
    internal class GamesEngine
    {
        internal void Run()
        {
            while (true)
            {
                Console.Clear();
                ConsoleGame game = SelectGame();
                do
                {
                    game.InitializeGame();
                    game.RunGame();
                    game.CleanUp();
                } while (PlayAgainPrompt());
            }
        }
        private ConsoleGame SelectGame()
        {
            switch (SelectGameMenu())
            {
                case '1':
                    return new _2048Engine();
                case '2':
                    return new TikTacToeEngine();
                case '3':
                    return new _2048Engine(); //ConnectFourEngine();
                case '4':
                    Environment.Exit(0);
                    break;
            }
            return new _2048Engine();
        }
        private char SelectGameMenu()
        {
            string[] options = new string[] { "1", "2", "3", "4" };
            char response = ' ';
            Console.WriteLine(SELECT_GAME_MENU);
            Console.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            while (true)
            {
                response = Console.ReadKey().KeyChar;
                if (options.Contains(response.ToString()))
                {
                    break;
                }
                Console.SetCursorPosition(SELECT_GAME_MENU_C.left, SELECT_GAME_MENU_C.top);
                Console.WriteLine(SELECT_GAME_MENU);
                Console.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
                Console.WriteLine(INVALID_INPUT);
                Console.SetCursorPosition(SELECT_GAME_INPUT.left, SELECT_GAME_INPUT.top);
            }
            return response;
        }
        private bool PlayAgainPrompt()
        {
            while (Console.KeyAvailable) Console.ReadKey(true);
            Console.Write(PLAY_AGAIN_PROMPT);
            char inputResponse = Console.ReadKey(true).KeyChar;
            bool response = false;

            response = (inputResponse.ToString().ToLower() == PLAY_AGAIN_YES);
            ClearConsoleBuffer(Console.CursorTop);
            return response;
        }
        private void ClearConsoleBuffer(int top)
        {
            Console.SetCursorPosition(0, top);
            Console.Write(new String(' ', Console.BufferWidth));
        }

        private readonly (int left, int top) SELECT_GAME_MENU_C = (0, 0);
        private const string SELECT_GAME_MENU = "Select a game to play: \n1. 2048\n2. Tic Tac Toe\n3. Connect Four\n4. Exit";
        private readonly (int left, int top) SELECT_GAME_INPUT = (23,0);
        private const string INVALID_INPUT = " <-Invalid input. Please try again.";
        private const string PLAY_AGAIN_PROMPT = "Do you want to play again? (Y/N): ";
        private const string PLAY_AGAIN_YES = "y";
    }
}
