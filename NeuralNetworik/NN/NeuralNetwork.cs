using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using NeuralNetworik.Utils;
using Newtonsoft.Json;

namespace NeuralNetworik.NN
{
    public class NeuralNetwork
    {
        [JsonProperty]
        private Node[][] _network;
        [JsonProperty]
        private readonly int[] _layers;
        [JsonProperty]
        private int _numberOfLayers;
        [JsonProperty]
        private int _inputLayerSize;

        public NeuralNetwork()
        {
            
        }

        /// <summary>
        /// Initialize of new NeuralNetwork object
        /// </summary>
        /// <param name="layers"></param>
        public NeuralNetwork(int[] layers)
        {
            _numberOfLayers = layers.Length - 1;
            _network = new Node[_numberOfLayers][];

            // Входной уровень не задаём, так как он будет неявно присутствовать
            for (int i = 0; i < _numberOfLayers; ++i)
            {
                _network[i] = new Node[layers[i + 1]];
                for (int j = 0; j < layers[i + 1]; ++j)
                {
                    // Каждый элемент слоя содержит столько весов, сколько было элементов
                    // на предыдущем слое 
                    _network[i][j] = new Node(layers[i]);
                }
            }

            _inputLayerSize = layers[0];
            _layers = new int[_numberOfLayers];
            for (int i = 0; i < _numberOfLayers; ++i)
            {
                _layers[i] = layers[i + 1];
            }
        }

        /// <summary>
        /// Returns predict
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public int ComputeResponse(double[] input)
        {
            unchecked
            {
                
            
            Node[] output = ComputeOutput(input);

#if DEBUG
            for (int i = 0; i < 32; ++i)
            {
                for (int j = 0; j < 32; ++j)
                {
                    Console.Write(input[i*32 + j]);
                }
                Console.WriteLine();
            }
            Console.WriteLine(new String('-', 50));
#endif

            // Find active output by finding maximum value
            int response = 0;
            double max = -1;
            for (int i = 0; i < output.Length; i++)
            {
#if DEBUG
                Console.WriteLine($"{i} : {output[i].Response}");
#endif
                if (output[i].Response > max)
                {
                    max = output[i].Response;
                    response = i;
                }
            }
#if DEBUG
            Console.WriteLine(response);
            Console.WriteLine(new String('=', 50));
#endif

            return response;
}
        }

        public double[] ComputeResonses(double[] input)
        {
            return ComputeOutput(input).Select(x => x.Response).ToArray();
        }

        public double ComputeBinaryResponse(double[] input)
        {
            Node output = ComputeOutput(input)[0];

            return output.Response;
        }

        /// <summary>
        /// Returns array of responses from last layer
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private Node[] ComputeOutput(double[] input)
        {
            unchecked
            {
                // Input layer
                int currentLayerLength = GetLengthOfLayer(0);
//            for (int j = 0; j < currentLayerLength; ++j)
//            {
//                _network[0][j].CalculateOutput(input);
//            }
                Parallel.For(0, currentLayerLength, (i) =>
                {
                    _network[0][i].CalculateOutput(input);
                });

                // Other layers
                for (int i = 1; i < _numberOfLayers; ++i)
                {
                    currentLayerLength = GetLengthOfLayer(i);
                    Parallel.For(0, currentLayerLength, j =>
                    {
                        _network[i][j].CalculateOutput(_network[i - 1]);
                    });
//                for (int j = 0; j < currentLayerLength; ++j)
//                {
//                    _network[i][j].CalculateOutput(_network[i - 1]);
//                }
                }

                return _network[_numberOfLayers - 1];
            }
        }

        public static void SaveTo(NeuralNetwork network, string path)
        {
            string json = JsonConvert.SerializeObject(network);
            File.WriteAllText(path, json);
        }

        public static NeuralNetwork ReadFrom(string path)
        {
            string json = File.ReadAllText(path);
            return JsonConvert.DeserializeObject<NeuralNetwork>(json);
        }

        private int GetLengthOfLayer(int layer)
        {
            // Input layer
            if (layer == -1)
            {
                return _inputLayerSize;
            }
            return _layers[layer];
        }

        public static void TrainNetwork(NeuralNetwork network, TrainingEntry trainingEntry, double learningRate,
            double momentum)
        {
            unchecked
            {
                // Compute output
                Node[] actualOutput = network.ComputeOutput(trainingEntry.Input);
                double[] desiredOutput = Common.Labels.LabelMapping[trainingEntry.Response];
//            double[] desiredOutput = new double[] {trainingEntry.Response};

                // Compute error
                // Output layer
                int outputLayer = network._numberOfLayers - 1;
                for (int j = 0; j < actualOutput.Length; ++j)
                {
                    network._network[outputLayer][j].Error = (desiredOutput[j] - actualOutput[j].Response) *
                                                             MathUtils.SigmoidDIfferential(actualOutput[j].Response);
//                network._network[outputLayer][j].Error = (desiredOutput[j] - actualOutput[j].Response) *
//                                  MathUtils.CalculateSigmoidDifferential(actualOutput[j].LocalOutput);
                }
                // Hidden layers
                for (int i = network._numberOfLayers - 2; i >= 0; --i)
                {
                    for (int j = 0, length = network.GetLengthOfLayer(i); j < length; ++j)
                    {
                        Node currentNode = network._network[i][j];

                        double summedError = 0;
//                    for (int k = 0, layerLength = network.GetLengthOfLayer(i + 1); k < layerLength; ++k)
//                    {
//                        Node node = network._network[i + 1][k];
//                        summedError += node[j] * node.Error;
//                    }
                        Parallel.For(0, network.GetLengthOfLayer(i + 1), k =>
                        {
                            Node node = network._network[i + 1][k];
                            summedError += node[j] * node.Error;
                        });

                        currentNode.Error = summedError * MathUtils.SigmoidDIfferential(currentNode.Response);
/*
                                                                                            double summedError = 0;
                                                                                            for (int k = 0, weightsLength = currentNode.Length; k < weightsLength; ++k)
                                                                                            {
                                                                                                summedError += network._network[i + 1][k].Error * currentNode[k];
                                                                                            }
                                                                        
                                                                                            currentNode.Error = summedError * MathUtils.SigmoidDIfferential(currentNode.Response);
                                                                        */
                    }
                }

//            momentum = 0;
                // Update weights
                for (int i = network._numberOfLayers - 1; i > 0; --i)
                {
                    for (int j = 0, length = network.GetLengthOfLayer(i); j < length; ++j)
                    {
                        Node currentNode = network._network[i][j];

//                    for (int k = 0, weightsLength = currentNode.Length; k < weightsLength; ++k)
//                    {
//                        currentNode.UpdateWeight(k, learningRate*currentNode.Error*network._network[i - 1][k].Response, momentum);
//                    }
                        Parallel.For(0, currentNode.Length,
                            k =>
                            {
                                currentNode.UpdateWeight(k,
                                    learningRate * currentNode.Error * network._network[i - 1][k].Response, momentum);
                            });

                        currentNode.UpdateBias(learningRate * currentNode.Error, momentum);
                    }
                }

                for (int j = 0, length = network.GetLengthOfLayer(0); j < length; ++j)
                {
                    Node currentNode = network._network[0][j];

//                for (int k = 0, weightsLength = currentNode.Length; k < weightsLength; ++k)
//                {
//                    currentNode.UpdateWeight(k, learningRate * currentNode.Error * trainingEntry.Input[j], momentum);
//                }
                    Parallel.For(0, currentNode.Length,
                        k =>
                        {
                            currentNode.UpdateWeight(k,
                                learningRate * currentNode.Error * trainingEntry.Input[j], momentum);
                        });


                    currentNode.UpdateBias(learningRate * currentNode.Error, momentum);
                }
            }
        }

    }
}