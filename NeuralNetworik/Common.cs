using System.CodeDom;
using System.Collections.Generic;
using System.Drawing;

namespace NeuralNetworik
{
    public static class Common
    {
        public static class Labels
        {
            public static readonly int[] digit0 = {1, 0, 0, 0, 0, 0, 0, 0, 0, 0};
            public static readonly int[] digit1 = {0, 1, 0, 0, 0, 0, 0, 0, 0, 0};
            public static readonly int[] digit2 = {0, 0, 1, 0, 0, 0, 0, 0, 0, 0};
            public static readonly int[] digit3 = {0, 0, 0, 1, 0, 0, 0, 0, 0, 0};
            public static readonly int[] digit4 = {0, 0, 0, 0, 1, 0, 0, 0, 0, 0};
            public static readonly int[] digit5 = {0, 0, 0, 0, 0, 1, 0, 0, 0, 0};
            public static readonly int[] digit6 = {0, 0, 0, 0, 0, 0, 1, 0, 0, 0};
            public static readonly int[] digit7 = {0, 0, 0, 0, 0, 0, 0, 1, 0, 0};
            public static readonly int[] digit8 = {0, 0, 0, 0, 0, 0, 0, 0, 1, 0};
            public static readonly int[] digit9 = {0, 0, 0, 0, 0, 0, 0, 0, 0, 1};

            public static Dictionary<int, int[]> LabelMapping = new Dictionary<int, int[]>()
            {
                {0, digit0},
                {1, digit1},
                {2, digit2},
                {3, digit3},
                {4, digit4},
                {5, digit5},
                {6, digit6},
                {7, digit7},
                {8, digit8},
                {9, digit9},
            };
        }

        public static readonly Size InputImageSize = new Size(32, 32);
    }
}