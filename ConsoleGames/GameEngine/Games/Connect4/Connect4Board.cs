using System;
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
                    Column = i % COLUMNS,
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
            if (lastPlacedSlot == null || !lastPlacedSlot.IsValid() || lastPlacedSlot.IsOpenSlot)
            {
                return IsFull();
            }

            if (HasConnectedLine(lastPlacedSlot, 0, 1) ||
                HasConnectedLine(lastPlacedSlot, 1, 0) ||
                HasConnectedLine(lastPlacedSlot, 1, 1) ||
                HasConnectedLine(lastPlacedSlot, 1, -1))
            {
                winner = lastPlacedSlot.Player;
                return true;
            }

            return IsFull();
        }
        private bool HasConnectedLine(Slot slot, int rowDelta, int columnDelta)
        {
            int matchingSlots = CountMatchingSlots(slot, rowDelta, columnDelta) +
                                CountMatchingSlots(slot, -rowDelta, -columnDelta) - 1;
            return matchingSlots >= WIN_CONDITION;
        }
        private int CountMatchingSlots(Slot slot, int rowDelta, int columnDelta)
        {
            int count = 0;

            for (int row = slot.Row, column = slot.Column;
                 row >= 0 && row < ROWS && column >= 0 && column < COLUMNS;
                 row += rowDelta, column += columnDelta)
            {
                Slot nextSlot = this[row, column];
                if (!nextSlot.IsValid() || nextSlot.Player != slot.Player) break;
                count++;
            }

            return count;
        }
        private bool IsFull()
        {
            return !slots.Any(s => s.Player == Slot.DEFAULT_PLAYER);
        }
        internal const int WIN_CONDITION = 4;
    }
}
