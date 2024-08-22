using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameEngine
{
    internal static class RandomSingleton
    {
        private static readonly Random instance = new Random();
        public static Random Instance
        {
            get
            {
                return instance;
            }
        }
    }
}
