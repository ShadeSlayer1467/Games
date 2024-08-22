using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using AbstractGame;
using BasicGameInterface;
using GamePlatform.Utilities;

namespace _2048Game
{
    [GameName("2048 Game")]
    public class _2048Engine : ConsoleGame
    {
        public _2048Engine()
        {
            board = new int[16];
            for (int i = 0; i < 16; i++)
            {
                board[i] = 0;
            }
        }
        public override void InitializeGame()
        {
            GameConsoleUI.ClearConsole();
            GameConsoleUI.Title = "2048 Game";
            board = new int[BOARD_SIZE];
            HighScore = 0;

            GenerateNewNumbers();
            PrintBoard();
            PrintValues();
        }
        public override void RunGame()
        {
            while (true)
            {
                PlayRound();
                if (IsGameOver()) break;
            }
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
            
            while (GameConsoleUI.ReadKeyChar(true) != ' ');
        }
        private void PlayRound()
        {
            MovePieces(GetMoveDirection());
            GenerateNewNumbers();
            PrintValues();
            UpdateHighScore();
        }
        private char GetMoveDirection()
        {
            char direction;

            GameConsoleUI.WriteLine(ENGLISH_DIRECTIONS, COMMUNICATION_LINE_TOP);
            while (true)
            {
                direction = GameConsoleUI.ReadKeyChar(false);
                if (QWERTY_DEFAULT_DIRECTION_KEYS.ToString().ToLower().Contains(direction)) break;
                else
                {
                    (int left, int top) c = GameConsoleUI.GetConsoleCursorPosition();
                    GameConsoleUI.SetCursorPosition(c.left - 1, c.top);
                }
            }

            return direction;
        }
        private void MovePieces(char direction)
        {
            switch (char.ToUpper(direction))
            {
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.t:
                    MoveUp();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.l:
                    MoveLeft();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.d:
                    MoveDown();
                    break;
                case var t when t == QWERTY_DEFAULT_DIRECTION_KEYS.r:
                    MoveRight();
                    break;
            }
        }
        private void MoveLeft()
        {
            int[] ints = new int[COLUMN_COUNT];
            int index = 0;
            for (int i = 0; i < BOARD_SIZE; i += COLUMN_COUNT)
            {
                for (int j = i; j < i + COLUMN_COUNT; j++)
                {
                    ints[index] = board[j];
                    index++;
                }
                ComputeLine(ints, out ints);
                index = 0;
                for (int j = i; j < i + COLUMN_COUNT; j++)
                {
                    board[j] = ints[index];
                    index++;
                }
                index = 0;
            }
        }
        private void MoveRight()
        {
            int[] ints = new int[COLUMN_COUNT];
            int index = 0;
            for (int i = COLUMN_COUNT - 1; i < BOARD_SIZE; i += COLUMN_COUNT)
            {
                for (int j = i; j > i - COLUMN_COUNT; j--)
                {
                    ints[index] = board[j];
                    index++;
                }
                ComputeLine(ints, out ints);
                index = 0;
                for (int j = i; j > i - COLUMN_COUNT; j--)
                {
                    board[j] = ints[index];
                    index++;
                }
                index = 0;
            }
        }
        private void MoveUp()
        {
            int[] ints = new int[ROW_COUNT];
            int index = 0;
            for (int i = 0; i < COLUMN_COUNT; i++)
            {
                for (int j = i; j < BOARD_SIZE; j += COLUMN_COUNT)
                {
                    ints[index] = board[j];
                    index++;
                }
                ComputeLine(ints, out ints);
                index = 0;
                for (int j = i; j < BOARD_SIZE; j += COLUMN_COUNT)
                {
                    board[j] = ints[index];
                    index++;
                }
                index = 0;
            }

        }
        private void MoveDown()
        {
            int[] ints = new int[ROW_COUNT];
            int index = 0;
            for (int i = BOARD_SIZE - 1; i >= BOARD_SIZE - COLUMN_COUNT; i--)
            {
                for (int j = i; j >= 0; j -= COLUMN_COUNT)
                {
                    ints[index] = board[j];
                    index++;
                }
                ComputeLine(ints, out ints);
                index = 0;
                for (int j = i; j >= 0; j -= COLUMN_COUNT)
                {
                    board[j] = ints[index];
                    index++;
                }
                index = 0;
            }
        }
        private void ComputeLine(int[] inputLineCells, out int[] lineCell)
        {
            for (int i = 0; i < inputLineCells.Length - 1; i++)
            {
                for (int j = i + 1; j < inputLineCells.Length; j++)
                {
                    if (inputLineCells[i] == inputLineCells[j])
                    {
                        inputLineCells[i] *= 2;
                        inputLineCells[j] = 0;
                        break;
                    }
                    if (inputLineCells[j] != 0)
                    {
                        break;
                    }
                }
            }
            for (int i = 0; i < inputLineCells.Length - 1; i++)
            {
                if (inputLineCells[i] != 0) continue;
                for (int j = i + 1; j < inputLineCells.Length; j++)
                {
                    if (inputLineCells[j] == 0) continue;
                    if (inputLineCells[i] == 0)
                    {
                        inputLineCells[i] = inputLineCells[j];
                        inputLineCells[j] = 0;
                        break;
                    }
                }
            }
            lineCell = inputLineCells;
        }
        private void PrintHighScore()
        {
            SetCellColor(HighScore);
            GameConsoleUI.WriteLine(HIGH_SCORE_MESSAGE + HighScore, HIGH_SCORE_LINE_TOP);
            GameConsoleUI.ResetColor();

        }
        private void UpdateHighScore()
        {
            if (board.Max() > HighScore)
            {
                HighScore = board.Max();
                PrintHighScore();
            }
        }
        private void GenerateNewNumbers()
        {

            int numOfSquaresToGenerate = (Random.Next(10) % 3 == 2) ? 2 : 1;
            for (int i = 0; i < numOfSquaresToGenerate; i++)
            {
                if (!board.Contains(0)) return;

                int index = Random.Next(0, 16);
                while (board[index] != 0)
                {
                    index = Random.Next(0, 16);
                }
                board[index] = Random.NextDouble() < 0.9 ? 2 : 4;
            }
        }
        private bool IsGameOver()
        {
            if (board.Contains(0))
            {
                return false;
            }
            for (int i = 0; i < 16; i++)
            {
                if (i % 4 != 3 && board[i] == board[i + 1])
                {
                    return false;
                }
                if (i < 12 && board[i] == board[i + 4])
                {
                    return false;
                }
            }
            return true;
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
            GameConsoleUI.SetCursorPosition(0,0);
            int index = 0;
            foreach (var item in board)
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
            // Change the color based on the cell value
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
        private void DebugPrintBoard()
        {
            PrintBoard();
            PrintValues();
        }
        private void ClearConsoleBuffer(int top)
        {
            GameConsoleUI.ClearConsoleLineBuffer(top);
        }

        private int[] board;
        private int HighScore = 0;
        private Random Random = RandomSingleton.Instance;



        private const int BOARD_SIZE = 16;
        private const int ROW_COUNT = 4;
        private const int COLUMN_COUNT = 4;
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
