using System;

namespace GeneticAlgorithm.Utils
{
    public static class ThreadSafeRandom
    {
        private static readonly Random Random = new Random(DateTime.Now.Millisecond);

        public static double NextDouble()
        {
            lock (Random)
            {
                return Random.NextDouble();
            }
        }

        public static int NextInt(int min, int max)
        {
            lock (Random)
            {
                return Random.Next(min, max);
            }
        }

        public static int NextInt(int max)
        {
            lock (Random)
            {
                return Random.Next(max);
            }
        }
    }
}