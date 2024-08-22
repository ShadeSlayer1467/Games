using System;
using System.Collections.Generic;
using System.Linq;

namespace Connect4
{
    internal class Connect4Board
    {
        internal Slot[] slots;
        internal int COLUMNS { get; private set; }
        internal int ROWS { get; private set; }
        internal Slot this[int row, int col] => GetSlot(row, col);
        internal Connect4Board(int Rows = 6, int Columns = 7)
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
                slots[i] = new Slot
                {
                    Row = i / COLUMNS,
                    Column = i % ROWS,
                    Player = Slot.DEFAULT_PLAYER
                };
            }
        }
        internal Slot GetSlot(int row, int col)
        {
            if (row >= 0 && row < ROWS &&
                col >= 0 && col < COLUMNS)
                return slots[row * COLUMNS + col];
            else
                return Slot.INVALID_SLOT;
        }
        internal Slot[] Row(int row) => Enumerable.Range(0, COLUMNS).Select(col => this[row, col]).ToArray();
        internal Slot[] Column(int col) => Enumerable.Range(0, ROWS).Select(row => this[row, col]).ToArray();
        internal (Slot[] asc, Slot[] desc) GetPieceDiagonal(Slot slot)
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

        internal List<Slot[]> Diagonals()
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
        internal bool InARow(Slot lastPlacedSlot, out int winner)
        {
            winner = Slot.DEFAULT_PLAYER;
            var (asc, desc) = GetPieceDiagonal(lastPlacedSlot);

            return ContainsWinner(Row(lastPlacedSlot.Row), out winner) || 
                ContainsWinner(Column(lastPlacedSlot.Column), out winner) || 
                ContainsWinner(asc, out winner) || 
                ContainsWinner(desc, out winner) || 
                (slots.Where(s => s.Player == Slot.DEFAULT_PLAYER).Count() == 0);
        }
        private bool ContainsWinner(Slot[] slots, out int winner)
        {
            winner = Slot.DEFAULT_PLAYER;
            if (slots.Length < Connect4Board.WIN_CONDITION) return false;
            int inARow = 1;
            for (int i = 0; i < slots.Length - 1; i++)
            {
                if (!slots[i].IsOpenSlot && slots[i].Player == slots[i + 1].Player)
                {
                    inARow++;
                    if (inARow == Connect4Board.WIN_CONDITION)
                    {
                        winner = slots[i].Player;
                        return true;
                    }
                }
                else inARow = 1;
            }
            return false;
        }


        internal const int DEFAULT_PLAYER = Slot.DEFAULT_PLAYER;

        internal const int WIN_CONDITION = 4;
    }
}
