using System;

namespace NeuralNetworik.Utils
{
    public static class Extensions
    {
        public static int Mult(this int[] lhs, int[] rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentException("Both operands must be not null");
            }
            if (lhs.Length != rhs.Length)
            {
                throw new ArgumentException("Both operands must have equal size");
            }

            int sum = 0;
            for (int i = 0; i < lhs.Length; ++i)
            {
                sum += lhs[i] * rhs[i];
            }

            return sum;
        }

        public static double Mult(this double[] lhs, double[] rhs)
        {
            if (lhs == null || rhs == null)
            {
                throw new ArgumentException("Both operands must be not null");
            }
            if (lhs.Length != rhs.Length)
            {
                throw new ArgumentException("Both operands must have equal size");
            }

            double sum = 0;
            for (int i = 0; i < lhs.Length; ++i)
            {
                sum += lhs[i] * rhs[i];
            }

            return sum;
        }
    }


}