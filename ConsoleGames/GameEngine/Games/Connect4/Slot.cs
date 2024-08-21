using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Connect4
{
    internal class Slot
    {
        public int Row { get; set; }
        public int Column { get; set; }
        public int Player { get; set; }
        public Slot() 
        {
            Player = DEFAULT_PLAYER;
            Row = 0;
            Column = 0;
        }
        public bool IsValidSlot() => Player != INVALID_PLAYER;
        public bool IsPlayerOwned => Player != DEFAULT_PLAYER && Player != INVALID_PLAYER;

        public const int DEFAULT_PLAYER = 0;
        public const int INVALID_PLAYER = -1;
    }
}
