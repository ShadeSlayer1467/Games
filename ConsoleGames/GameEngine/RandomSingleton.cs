using System;

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
