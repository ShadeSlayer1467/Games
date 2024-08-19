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
            Player = 0;
            Row = 0;
            Column = 0;
        }
    }
}
