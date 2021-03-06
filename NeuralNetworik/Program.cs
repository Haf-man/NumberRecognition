﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Metadata.W3cXsd2001;
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
//            Tests.TestFileUtils();
//            Tests.TestTrainingXor();
//            Tests.TestTrainingDigits();
            Tests.TestNetwork();
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

            public static void TestTrainingXor()
            {
                NeuralNetwork network = new NeuralNetwork(new int[] {2, 2, 1}); //xor problem

                List<TrainingEntry> xorTrainingEntries = new List<TrainingEntry>
                {
                    new TrainingEntry() {Input = new double[] {0, 1}, Response = 1},
                    new TrainingEntry() {Input = new double[] {1, 0}, Response = 1},
                    new TrainingEntry() {Input = new double[] {1, 1}, Response = 0},
                    new TrainingEntry() {Input = new double[] {0, 0}, Response = 0},
                };

                NNTraining training = new NNTraining()
                {
                    LearningRate = 0.1,
                    Momentum = 0.000
                };

                training.TrainNetwork(ref network, xorTrainingEntries, xorTrainingEntries);
            }

            public static void TestTrainingDigits()
            {
                List<TrainingEntry> trainingSet = FileUtils.LoadDataSet(Properties.Resources.optdigits_tra);
                List<TrainingEntry> testSet = FileUtils.LoadDataSet(Properties.Resources.optdigits_tes);

                int inputSize = Common.InputImageSize.Height * Common.InputImageSize.Width;
                NeuralNetwork network = new NeuralNetwork(new int[] {inputSize, 256, 32, 10});

                NNTraining training = new NNTraining()
                {
                    LearningRate = 0.1,
                    Momentum = 0.0001
                };

                training.TrainNetwork(ref network, trainingSet, testSet);
            }

            public static void TestNetwork()
            {
                var network = NeuralNetwork.ReadFrom("new_network_1.json");
                var dataSet = FileUtils.LoadDataSet(Properties.Resources.optdigits_tra);

                int cnt = 0;
                Dictionary<int, int> rightCount = new Dictionary<int, int>();
                for (int i = 0; i < 10; i++)
                {
                    rightCount[i] = 0;
                }
                foreach (var trainingEntry in dataSet)
                {
                    int response = network.ComputeResponse(trainingEntry.Input);
                    Console.WriteLine($"actual {response} - desired {trainingEntry.Response}");
                    if (response == trainingEntry.Response)
                    {
                        cnt++;
                        rightCount[response] = rightCount[response]+1;
                    }
                }
                Console.WriteLine($"{cnt} / {dataSet.Count} == {100.0 * cnt / dataSet.Count}");
                foreach (var pair in rightCount)
                {
                    Console.WriteLine($"{pair.Key} : {pair.Value}");
                }
            }
        }
    }
}
