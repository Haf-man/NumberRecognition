using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetworik
{
    public static class Common
    {
        public static class Labels
        {
            private static readonly double[] Digit0 = {1, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            private static readonly double[] Digit1 = {0, 1, 0, 0, 0, 0, 0, 0, 0, 0};
            private static readonly double[] Digit2 = {0, 0, 1, 0, 0, 0, 0, 0, 0, 0};
            private static readonly double[] Digit3 = {0, 0, 0, 1, 0, 0, 0, 0, 0, 0};
            private static readonly double[] Digit4 = {0, 0, 0, 0, 1, 0, 0, 0, 0, 0};
            private static readonly double[] Digit5 = {0, 0, 0, 0, 0, 1, 0, 0, 0, 0};
            private static readonly double[] Digit6 = {0, 0, 0, 0, 0, 0, 1, 0, 0, 0};
            private static readonly double[] Digit7 = {0, 0, 0, 0, 0, 0, 0, 1, 0, 0};
            private static readonly double[] Digit8 = {0, 0, 0, 0, 0, 0, 0, 0, 1, 0};
            private static readonly double[] Digit9 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 1};

            public static Dictionary<int, double[]> LabelMapping = new Dictionary<int, double[]>()
            {
                {0, Digit0},
                {1, Digit1},
                {2, Digit2},
                {3, Digit3},
                {4, Digit4},
                {5, Digit5},
                {6, Digit6},
                {7, Digit7},
                {8, Digit8},
                {9, Digit9},
            };
        }

        public static readonly Size InputImageSize = new Size(32, 32);
    }
}