using System;

namespace NeuralNetworik.Utils
{
    public static class MathUtils
    {
        public static double CalculateSigmoid(double input)
        {
            return 1d / (1 + Math.Pow(Math.E, -input));
        }

        public static double CalculateSigmoidDifferential(double input)
        {
            double sigmoid = CalculateSigmoid(input);
            return sigmoid * (1 - sigmoid);
        }

        /// <summary>
        /// Is's assumes thath input it's already a value from sigmoid function
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static double SigmoidDIfferential(double input)
        {
            return input * (1 - input);
        }
    }
}