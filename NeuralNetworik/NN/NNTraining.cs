using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;

namespace NeuralNetworik.NN
{
    public class NNTraining
    {
        /// <summary>
        /// The greater the learning rate, the more the weight values are changed. Is usually decreased during the learning process.
        /// </summary>
        public double LearningRate { get; set; } = 0.2;

        /// <summary>
        /// A common modification to standard backpropagation training; at each step, weight adjustments 
        /// are based on a combination of the current weight adjustment (as found in standard  Backpropagation) 
        /// and the weight change from the previous step.
        /// </summary>
        public double Momentum { get; set; } = 0;

        public void TrainNetwork(ref NeuralNetwork network, List<TrainingEntry> trainingObjects, List<TrainingEntry> testObjects)
        {
            double ratio;
            int iteration = 0;
            testObjects = testObjects.Take(20).ToList();

            for (int i = 0; i < 2; ++i)
            {
                Stopwatch stopwatch = Stopwatch.StartNew();
                do
                {
                    Console.WriteLine($"Iteration {iteration++}");
                    int correct = 0;
                    // Training part
                    foreach (var trainingObject in trainingObjects)
                    {
                        NeuralNetwork.TrainNetwork(network, trainingObject, LearningRate, Momentum);
//                    NeuralNetwork.TrainNetwork(network, trainingObjects[0], LearningRate, Momentum);
//                    if (network.ComputeResponse(trainingObjects[0].Input) == trainingObjects[0].Response)
                        if (network.ComputeResponse(trainingObject.Input) == trainingObject.Response)
                        {
                            ++correct;
                        }
                    }
                    ratio = (double) correct / trainingObjects.Count * 100;

                    Console.WriteLine($"On training set: {ratio} %");

/*
                    // Evaluation part
                    correct = 0;
                    foreach (var testObject in testObjects)
                    {
                        // Check and sum
                        if (network.ComputeResponse(testObject.Input) == testObject.Response)
                        {
                            ++correct;
                        }
                    }

                    // Ratio
                    ratio = (double) correct / testObjects.Count * 100;

                    Console.WriteLine($"On test set: {ratio} %");
*/
                } while (iteration < 100);

                stopwatch.Stop();
                Console.WriteLine($"It took time {stopwatch.Elapsed} ms");

                NeuralNetwork.SaveTo(network, $"new_network_{i}.json");

                Console.WriteLine("Saved");
            }
        }
    }
}