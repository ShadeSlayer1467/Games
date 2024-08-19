using Connect4;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    internal class Board
    {
        public Slot[] slots = new Slot[42];
        public Slot this[int row, int col] => (row * COLUMNS + col < COLUMNS * ROWS) ? slots[row * COLUMNS + col] : new Slot() { Player = -1, Row = -1, Column = -1 };
        public Board()
        {
            for (int i = 0; i < 42; i++)
            {
                slots[i] = new Slot();
                slots[i].Row = i / 7;
                slots[i].Column = i % 7;
                slots[i].Player = 0;
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
    }
}
