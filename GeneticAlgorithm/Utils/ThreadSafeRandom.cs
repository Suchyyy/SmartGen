using System;

namespace GeneticAlgorithm.Utils
{
    public static class ThreadSafeRandom
    {
        private static readonly Random Global = new Random(DateTime.Now.Millisecond);
        [ThreadStatic] private static Random _local;

        public static double NextDouble()
        {
            if (_local == null)
            {
                lock (Global)
                {
                    if (_local == null)
                    {
                        var seed = Global.Next();
                        _local = new Random(seed);
                    }
                }
            }

            return _local.NextDouble();
        }

        public static int NextInt(int min, int max)
        {
            if (_local == null)
            {
                lock (Global)
                {
                    if (_local == null)
                    {
                        var seed = Global.Next();
                        _local = new Random(seed);
                    }
                }
            }

            return _local.Next(min, max);
        }

        public static int NextInt(int max)
        {
            if (_local == null)
            {
                lock (Global)
                {
                    if (_local == null)
                    {
                        var seed = Global.Next();
                        _local = new Random(seed);
                    }
                }
            }

            return _local.Next(max);
        }
    }
}