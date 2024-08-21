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
        public Slot[] slots;
        public int COLUMNS { get; private set; }
        public int ROWS { get; private set; }
        public Slot this[int row, int col] => GetSlot(row, col);
        public Board(int Rows = 6, int Columns = 7)
        {
            if (Columns <= 0 || Rows <= 0)
            {
                throw new ArgumentException("Columns and rows must be greater than zero.");
            }
            COLUMNS = Columns;
            ROWS = Rows;
            slots = new Slot[COLUMNS * ROWS];
            for (int i = 0; i < Columns * Rows; i++)
            {
                slots[i] = new Slot();
                slots[i].Row = i / COLUMNS;
                slots[i].Column = i % ROWS;
                slots[i].Player = Slot.DEFAULT_PLAYER;
            }
        }
        public Slot GetSlot(int row, int col)
        {
            if (row >= 0 && row < ROWS &&
                col >= 0 && col < COLUMNS)
                return slots[row * COLUMNS + col];
            else
                return Slot.INVALID_SLOT;
        }
        public Slot[] Row(int row) => Enumerable.Range(0, COLUMNS).Select(col => this[row, col]).ToArray();
        public Slot[] Column(int col) => Enumerable.Range(0, ROWS).Select(row => this[row, col]).ToArray();
        public (Slot[] asc, Slot[] desc) GetPieceDiagonal(Slot slot)
        {
            int row = slot.Row;
            int col = slot.Column;
            List<Slot> ascending = new List<Slot>();
            List<Slot> descending = new List<Slot>();

            for (int offset = 0; row + offset < ROWS; offset++)
            {
                Slot next = this[row + offset, col + offset];
                if (next.IsValid()) descending.Add(next);

                next = this[row + offset, col - offset];
                if (next.IsValid()) ascending.Add(next);
            }
            return (ascending.ToArray(), descending.ToArray());
        }

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
                        if (nextRight.IsValid()) descendingDiagonal.Add(nextRight);
                    }

                    if (column - columnOffset >= 0)
                    {
                        Slot nextLeft = this[row, column - columnOffset];
                        if (nextLeft.IsValid()) ascendingDiagonal.Add(nextLeft);
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
        internal bool TryPlacePiece(int column, int currentPlayer, out Slot piece)
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
            if (bottom == null)
            {
                piece = Slot.INVALID_SLOT; 
                return false;
            }

            bottom.Player = currentPlayer;
            piece = bottom;
            return true;
        }
        internal const int DEFAULT_PLAYER = Slot.DEFAULT_PLAYER;

        internal const int WIN_CONDITION = 4;
    }
}
