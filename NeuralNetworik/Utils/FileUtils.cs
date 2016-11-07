using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using NeuralNetworik.NN;

namespace NeuralNetworik.Utils
{
    public static class FileUtils
    {
        public static List<TrainingEntry> LoadDataSet(string dataSet)
        {
            List<TrainingEntry> training = new List<TrainingEntry>();

            //using (StringReader stringReader = new StringReader(dataSet))
            //{
            //    string line;
            //    while ((line = stringReader.ReadLine()?.Trim()) != null)
            //    {
            //        if (line == string.Empty)
            //        {
            //            continue;
            //        }

            //        int[] temp = new int[Common.InputImageSize.Height * Common.InputImageSize.Width];
            //        for (int i = 0, cnt = 0; i < Common.InputImageSize.Height; ++i)
            //        {
            //            foreach (char c in line)
            //            {
            //                temp[cnt++] = c == '0' ? 0 : 1;
            //            }
            //            line = stringReader.ReadLine()?.Trim();

            //            if (line == null)
            //            {
            //                return training;
            //            }
            //        }


            //        training.Add(new TrainingEntry() {Input = temp, Response = int.Parse(line)});
            //    }
            //}

            return training;
        }
    }
}