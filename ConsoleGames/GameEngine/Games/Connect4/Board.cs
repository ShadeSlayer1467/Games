using Connect4;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    internal class Board
    {
        public Slot[] slots = new Slot[42];
        public Slot this[int row, int col] => GetSlot(row, col);
        public Slot GetSlot(int row, int col)
        {
            if (row >= 0 && row < ROWS &&
                col >= 0 && col < COLUMNS)
                return slots[row * COLUMNS + col];
            else
                return new Slot() { Player = -1, Row = -1, Column = -1 };
        }
        public Slot[] Row(int row) => Enumerable.Range(0, COLUMNS).Select(col => this[row, col]).ToArray();
        public Slot[] Column(int col) => Enumerable.Range(0, ROWS).Select(row => this[row, col]).ToArray();

        public List<Slot[]> Diagonals()
        {
            List<Slot[]> diagonals = new List<Slot[]>();

            // Traverse diagonals starting from each column of the first row
            for (int column = 0; column < COLUMNS; column++)
            {
                List<Slot> descendingDiagonal = new List<Slot>();
                List<Slot> ascendingDiagonal = new List<Slot>();

                // build descending and ascending diagonals from the current column
                for (int row = 0, columnOffset = 0; row < ROWS && (column + columnOffset) < COLUMNS && (column - columnOffset) >= 0; row++, columnOffset++)
                {
                    if (column + columnOffset < COLUMNS)
                    {
                        Slot nextRight = this[row, column + columnOffset];
                        if (nextRight.IsValidSlot()) descendingDiagonal.Add(nextRight);
                    }

                    if (column - columnOffset >= 0)
                    {
                        Slot nextLeft = this[row, column - columnOffset];
                        if (nextLeft.IsValidSlot()) ascendingDiagonal.Add(nextLeft);
                    }
                }

                if (descendingDiagonal.Count >= WIN_CONDITION) diagonals.Add(descendingDiagonal.ToArray());
                if (ascendingDiagonal.Count >= WIN_CONDITION) diagonals.Add(ascendingDiagonal.ToArray());
            }

            // get all diagonals that pass through the first/last column
            for (int row = 1; row < ROWS; row++)
            {
                List<Slot> descendingDiagonal = new List<Slot>();
                List<Slot> ascendingDiagonal = new List<Slot>();

                for (int colOffset = 0; row + colOffset < ROWS && (COLUMNS - 1 - colOffset) >= 0; colOffset++)
                {
                    descendingDiagonal.Add(this[row + colOffset, COLUMNS - 1 - colOffset]);
                    ascendingDiagonal.Add(this[row + colOffset, colOffset]);
                }

                if (descendingDiagonal.Count >= WIN_CONDITION) diagonals.Add(descendingDiagonal.ToArray());
                if (ascendingDiagonal.Count >= WIN_CONDITION) diagonals.Add(ascendingDiagonal.ToArray());
            }

            return diagonals;
        }
        public Board()
        {
            for (int i = 0; i < 42; i++)
            {
                slots[i] = new Slot();
                slots[i].Row = i / COLUMNS;
                slots[i].Column = i % COLUMNS;
            }
        }
        internal bool PlacePiece(int column, int currentPlayer)
        {
            Slot bottom = null;
            for (int row = 0; row < ROWS; row++)
            {
                if (this[row, column].Player == 0)
                {
                    bottom = this[row, column];
                }
                else break;
            }
            if (bottom == null) return false;

            bottom.Player = currentPlayer;
            return true;
        }

        private const int COLUMNS = 7;
        private const int ROWS = 6;
        private const int WIN_CONDITION = 4;
        private const int DEFAULT_PLAYER = Slot.DEFAULT_PLAYER;

    }
}
