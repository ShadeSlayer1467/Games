using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace _2048Game
{
    public class _2048Engine
    {
        private Random Random = RandomSingleton.Instance;
        public _2048Engine()
        {
            board = new int[16];
            for (int i = 0; i < 16; i++)
            {
                board[i] = 0;
            }
        }
        public void StartAndRunGame()
        {
            GenerateNewNumbers();
            int[] board1 = new int[16] { 0,0,0,4,
                                         0,0,0,4,
                                         0,0,0,2,
                                         0,0,0,2};
            int[] board2 = new int[16] { 0,0,0,2,
                                         0,0,2,2,
                                         0,2,2,2,
                                         2,2,2,2};
            int[] board3 = new int[16] { 2,0,2,0,
                                         0,0,0,0,
                                         0,0,0,0,
                                         2,0,0,2};
            int[] board4 = new int[16] { 2,0,0,2,
                                         0,0,0,0,
                                         0,0,0,0,
                                         2,0,0,2};
            int[] board5 = new int[16] { 8,4,0,0,
                                         0,0,0,0,
                                         0,0,0,0,
                                         2,0,0,2};
            //board = board5;
            PrintBoard();
            while (!IsGameOver()) 
                PlayRound();

            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new String(' ', Console.BufferWidth));
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.WriteLine("Game Over!");
            return;
        }
        private void PlayRound()
        {
            MovePieces(GetMoveDirection());
            GenerateNewNumbers();
            PrintBoard();
        }
        private char GetMoveDirection()
        {
            char direction = ' ';

            Console.Write("Enter a direction (W, A, S, D): ");
            while (true)
            {
                direction = Console.ReadKey().KeyChar;
                if (direction == 'w' || direction == 'a' || direction == 's' || direction == 'd')
                {
                    Console.WriteLine();
                    break;
                }
                Console.SetCursorPosition(Console.CursorLeft - 1, Console.CursorTop);
            }

            return direction;
        }
        private void MovePieces(char direction)
        {
            switch (direction)
            {
                case 'w':
                    MoveUp();
                    break;
                case 'a':
                    MoveLeft();
                    break;
                case 's':
                    MoveDown();
                    break;
                case 'd':
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
            Console.SetCursorPosition(0, 0);
            Console.WriteLine("╔══════╦══════╦══════╦══════╗");

            for (int i = 0; i < 16; i++)
            {
                if (i % 4 == 0 && i != 0)
                {
                    Console.WriteLine("╠══════╬══════╬══════╬══════╣");
                }
                Console.Write("║");

                SetCellColor(board[i]);
                string cellValue = board[i] == 0 ? "." : board[i].ToString();
                Console.Write("{0,5} ", cellValue);

                Console.ResetColor();

                if ((i + 1) % 4 == 0)
                {
                    Console.WriteLine("║");
                }
            }

            Console.WriteLine("╚══════╩══════╩══════╩══════╝");
        }

        private void SetCellColor(int value)
        {
            // Change the color based on the cell value
            if (value == 0)
                Console.ForegroundColor = ConsoleColor.Gray;
            else if (value <= 4)
                Console.ForegroundColor = ConsoleColor.Cyan;
            else if (value <= 8)
                Console.ForegroundColor = ConsoleColor.Blue;
            else if (value <= 16)
                Console.ForegroundColor = ConsoleColor.Green;
            else if (value <= 32)
                Console.ForegroundColor = ConsoleColor.Magenta;
            else if (value <= 64)
                Console.ForegroundColor = ConsoleColor.Red;
            else if (value <= 128)
                Console.ForegroundColor = ConsoleColor.Yellow;
            else if (value <= 256)
                Console.ForegroundColor = ConsoleColor.DarkYellow;
            else if (value <= 512)
                Console.ForegroundColor = ConsoleColor.DarkRed;
            else if (value <= 1024)
                Console.ForegroundColor = ConsoleColor.DarkMagenta;
            else
                Console.ForegroundColor = ConsoleColor.DarkBlue;
        }


        private void DebugPrintBoard()
        {
            for (int i = 0; i < 16; i++)
            {
                if (board[i] == 0)
                {
                    Console.Write("    .");
                }
                else
                {
                    Console.Write("{0,5}", board[i]);
                }
                if ((i + 1) % 4 == 0)
                {
                    Console.WriteLine();
                }
            }
            Console.WriteLine("-------------------------");
        }

        private int[] board;
        private const int BOARD_SIZE = 16;
        private const int ROW_COUNT = 4;
        private const int COLUMN_COUNT = 4;
    }
}
