using System;
using System.Threading;

namespace NeuralNetworik.Utils
{
    /// <summary>
    /// Provides ThreadSafe Random operations
    /// </summary>
    public static class ThreadSafeRandom
    {
        private static int _seed = Environment.TickCount;

        private static readonly ThreadLocal<Random> Local =
            new ThreadLocal<Random>(() => new Random(Interlocked.Increment(ref _seed)));

        public static int Next()
        {
            return Local.Value.Next();
        }

        /// <summary>
        /// Returns random number between 0 (inclusive) and 1 (exclusive)
        /// </summary>
        /// <returns></returns>
        public static double NextDouble()
        {
            return Local.Value.NextDouble();
        }

        public static int Next(int min, int max)
        {
            return Local.Value.Next(min, max);
        }

        public static Random GetLocal => Local.Value;
    }
}