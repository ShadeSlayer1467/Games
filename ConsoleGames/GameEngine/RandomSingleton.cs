using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Game
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
