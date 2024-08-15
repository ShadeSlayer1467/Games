using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AbstractGame
{
    public abstract class ConsoleGame
    {
        public abstract void InitializeGame();
        public abstract void RunGame();
        public abstract void CleanUp();
    }
}
