using System;
using System.Linq;

namespace _2048Game
{
    internal class _2048Board
    {
        internal int Max { get { return board.Max(); } }
        internal int[] Board { get { return board; } }
        private int[] board = new int[BOARD_SIZE];

        internal void GenerateNewNumbers(Random rand)
        {
            int numOfSquaresToGenerate = (rand.Next(10) % 3 == 2) ? 2 : 1;
            for (int i = 0; i < numOfSquaresToGenerate; i++)
            {
                if (!board.Contains(0)) return;

                int index = rand.Next(0, 16);
                while (board[index] != 0)
                {
                    index = rand.Next(0, 16);
                }
                board[index] = rand.NextDouble() < 0.9 ? 2 : 4;
            }
        }
        internal void MoveLeft()
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
        internal void MoveRight()
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
        internal void MoveUp()
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
        internal void MoveDown()
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
        internal bool IsFull() => board.Contains(0) == false;
        internal bool CanMove()
        {
            for (int i = 0; i < BOARD_SIZE; i++)
            {
                if (i % COLUMN_COUNT != (COLUMN_COUNT - 1) && board[i] == board[i + 1])
                {
                    return true;
                }
                if (i < (ROW_COUNT - 1)* COLUMN_COUNT && board[i] == board[i + COLUMN_COUNT])
                {
                    return true;
                }
            }
            return false;
        }
        internal void Reset()
        {
            board = new int[BOARD_SIZE];
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

        private const int BOARD_SIZE = 16;
        private const int ROW_COUNT = 4;
        private const int COLUMN_COUNT = 4;
    }
}
