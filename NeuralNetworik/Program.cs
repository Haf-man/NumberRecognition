using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NeuralNetworik.NN;
using NeuralNetworik.Utils;

namespace NeuralNetworik
{
    class Program
    {
        static void Main(string[] args)
        {
            Tests.TestFileUtils();
        }

        static class Tests
        {
            public static void TestFileUtils()
            {
                List<TrainingEntry> temp = FileUtils.LoadDataSet(Properties.Resources.optdigits_tes);
                Console.WriteLine(temp.Count == 550);
                temp = FileUtils.LoadDataSet(Properties.Resources.optdigits_tra);
                Console.WriteLine(temp.Count == 1382);
            }
        }
    }
}
