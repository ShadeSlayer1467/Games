using System;
using System.Linq;
using AbstractGame;
using BasicGameInterface;
using GameEngine;
using GamePlatform.Utilities;

namespace _2048Game
{
    [GameName("2048 Game")]
    public class _2048Engine : ConsoleGame
    {
        private _2048Board boardModel;
        private int HighScore = 0;
        private readonly Random rand = RandomSingleton.Instance;

        public _2048Engine()
        {
            boardModel = new _2048Board();
        }
        public override void InitializeGame()
        {
            GameConsoleUI.ClearConsole();
            boardModel = new _2048Board();
            HighScore = 0;

            boardModel.GenerateNewNumbers(rand);
            PrintBoard();
            PrintValues();
        }
        public override void RunGame()
        {
            do PlayRound(); while (!IsGameOver());
            GameOver();
        }
        public override void CleanUp()
        {
            GameConsoleUI.SetConsoleCursorLine(COMMUNICATION_LINE_TOP + 1);
            GameConsoleUI.FlushKeyBuffer();
        }
        private void GameOver()
        {
            GameConsoleUI.ClearConsoleLineBuffer(COMMUNICATION_LINE_TOP);
            GameConsoleUI.WriteLine(GAME_OVER_MESSAGE + SPACE_TO_CONTINUE, COMMUNICATION_LINE_TOP);

            while (GameConsoleUI.ReadKeyChar(true) != ' ') ;
        }
        private void PlayRound()
        {
            MovePieces(GetMoveDirection());
            boardModel.GenerateNewNumbers(rand);
            PrintValues();
            UpdateHighScore();
        }
        private char GetMoveDirection()
        {
            char direction;

            GameConsoleUI.SetConsoleCursorLine(COMMUNICATION_LINE_TOP);
            GameConsoleUI.Write(ENGLISH_DIRECTIONS);
            while (true)
            {
                direction = GameConsoleUI.ReadKeyChar(false);
                if (QWERTY_DEFAULT_DIRECTION_KEYS.ToString().ToLower().Contains(direction)) break;
                else
                {
                    (int left, int top) = GameConsoleUI.GetConsoleCursorPosition();
                    GameConsoleUI.SetCursorPosition(left - 1, top);
                }
            }

            return direction;
        }
        private void MovePieces(char direction)
        {
            switch (char.ToUpper(direction))
            {
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.t:
                    boardModel.MoveUp();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.l:
                    boardModel.MoveLeft();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.d:
                    boardModel.MoveDown();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.r:
                    boardModel.MoveRight();
                    break;
            }
        }
        private void PrintHighScore()
        {
            SetCellColor(HighScore);
            GameConsoleUI.WriteLine(HIGH_SCORE_MESSAGE + HighScore, HIGH_SCORE_LINE_TOP);
            GameConsoleUI.ResetColor();

        }
        private void UpdateHighScore()
        {
            if (boardModel.Max > HighScore)
            {
                HighScore = boardModel.Max;
                PrintHighScore();
            }
        }
        private bool IsGameOver()
        {
            return (boardModel.IsFull() && !boardModel.CanMove());
        }
        private void PrintBoard()
        {
            GameConsoleUI.WriteLine("╔══════╦══════╦══════╦══════╗", 0, 0);

            for (int i = 0; i < 16; i++)
            {
                if (i % 4 == 0 && i != 0)
                {
                    GameConsoleUI.WriteLine("╠══════╬══════╬══════╬══════╣");
                }
                GameConsoleUI.Write("║      ");
                if ((i + 1) % 4 == 0)
                {
                    GameConsoleUI.WriteLine("║");
                }
            }
            GameConsoleUI.WriteLine("╚══════╩══════╩══════╩══════╝");
            PrintHighScore();
        }
        private void PrintValues()
        {
            GameConsoleUI.SetCursorPosition(0, 0);
            int index = 0;
            foreach (var item in boardModel.Board)
            {
                GameConsoleUI.SetCursorPosition(BOARD_CURSER_LOCATIONS[index].left, BOARD_CURSER_LOCATIONS[index].top);
                SetCellColor(item);
                string cellValue = item == 0 ? "." : item.ToString();
                GameConsoleUI.Write("{0,5} ", cellValue);
                GameConsoleUI.ResetColor();
                index++;
            }
        }
        private void SetCellColor(int value)
        {
            if (value == 0)
                GameConsoleUI.ForegroundColor = ConsoleColor.Gray;
            else if (value <= 4)
                GameConsoleUI.ForegroundColor = ConsoleColor.Cyan;
            else if (value <= 8)
                GameConsoleUI.ForegroundColor = ConsoleColor.Blue;
            else if (value <= 16)
                GameConsoleUI.ForegroundColor = ConsoleColor.Green;
            else if (value <= 32)
                GameConsoleUI.ForegroundColor = ConsoleColor.Magenta;
            else if (value <= 64)
                GameConsoleUI.ForegroundColor = ConsoleColor.Red;
            else if (value <= 128)
                GameConsoleUI.ForegroundColor = ConsoleColor.Yellow;
            else if (value <= 256)
                GameConsoleUI.ForegroundColor = ConsoleColor.DarkYellow;
            else if (value <= 512)
                GameConsoleUI.ForegroundColor = ConsoleColor.DarkRed;
            else if (value <= 1024)
                GameConsoleUI.ForegroundColor = ConsoleColor.DarkMagenta;
            else
                GameConsoleUI.ForegroundColor = ConsoleColor.DarkBlue;
        }



        private readonly (int left, int top)[] BOARD_CURSER_LOCATIONS = { (1,1), (8,1), (15,1), (22,1),
                                                                 (1,3), (8,3), (15,3), (22,3),
                                                                 (1,5), (8,5), (15,5), (22,5),
                                                                 (1,7), (8,7), (15,7), (22,7) };
        private const int COMMUNICATION_LINE_TOP = 10;
        private const int HIGH_SCORE_LINE_TOP = 9;
        private const string ENGLISH_DIRECTIONS = "Enter a direction (W, A, S, D): ";
        private const string HIGH_SCORE_MESSAGE = "High Score: ";
        private readonly (char t, char l, char d, char r) QWERTY_DEFAULT_DIRECTION_KEYS = ('W', 'A', 'S', 'D');
        private const string GAME_OVER_MESSAGE = "Game Over! ";
        private const string SPACE_TO_CONTINUE = "Press space to continue...";
    }
}