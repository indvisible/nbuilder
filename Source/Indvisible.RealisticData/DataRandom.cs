using System;

namespace Indvisible.RealisticData
{
    public static class DataRandom
    {
        internal static Random Rand = new Random();

        public static void Seed(int seed)
        {
            Rand = new Random(seed);
        }
    }
}