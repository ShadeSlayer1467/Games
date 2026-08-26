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
            if (!board.Contains(0)) return;

            int index = rand.Next(0, BOARD_SIZE);
            while (board[index] != 0)
            {
                index = rand.Next(0, BOARD_SIZE);
            }

            board[index] = rand.NextDouble() < 0.9 ? 2 : 4;
        }
        internal bool MoveLeft()
        {
            int[] previousBoard = board.ToArray();
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
            return !board.SequenceEqual(previousBoard);
        }
        internal bool MoveRight()
        {
            int[] previousBoard = board.ToArray();
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
            return !board.SequenceEqual(previousBoard);
        }
        internal bool MoveUp()
        {
            int[] previousBoard = board.ToArray();
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

            return !board.SequenceEqual(previousBoard);
        }
        internal bool MoveDown()
        {
            int[] previousBoard = board.ToArray();
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
            return !board.SequenceEqual(previousBoard);
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
            int[] result = new int[inputLineCells.Length];
            int targetIndex = 0;

            for (int i = 0; i < inputLineCells.Length; i++)
            {
                if (inputLineCells[i] == 0) continue;

                int value = inputLineCells[i];
                for (int j = i + 1; j < inputLineCells.Length; j++)
                {
                    if (inputLineCells[j] == 0) continue;

                    if (value == inputLineCells[j])
                    {
                        value *= 2;
                        i = j;
                    }
                    break;
                }

                result[targetIndex++] = value;
            }

            lineCell = result;
        }

        private const int BOARD_SIZE = 16;
        private const int ROW_COUNT = 4;
        private const int COLUMN_COUNT = 4;
    }
}
