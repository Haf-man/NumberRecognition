using System;
using System.Data;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace NeuralNetworik
{
    public class NeuralNetwork
    {
        private int[][][] _network;
        private readonly int[] _layers;
        private int _numberOfLayers;
        private int _inputLayerSize;

        public NeuralNetwork(int[] layers)
        {
            _numberOfLayers = layers.Length - 1;
            _network = new int[_numberOfLayers][][];

            // Входной уровень не задаём, так как он будет неявно присутствовать
            for (int i = 0; i < _numberOfLayers; ++i)
            {
                _network[i] = new int[layers[i + 1]][];
                for (int j = 0; j < layers[i + 1]; ++j)
                {
                    // Каждый элемент слоя содержит столько весов, сколько было элементов
                    // на предыдущем слое
                    _network[i][j] = new int[layers[i]];
                }
            }

            _inputLayerSize = layers[0];
            _layers = new int[_numberOfLayers];
            for (int i = 0; i < _numberOfLayers; ++i)
            {
                _layers[i] = layers[i + 1];
            }
        }

        public int[] ComputeOutput(int[] input)
        {
            int[] tempPrev = (int[]) input.Clone();
            int[] temp = new int[_layers.Max()];

            for (int i = 0; i < _numberOfLayers; ++i)
            {
                for (int j = 0; j < _layers[i]; ++j)
                {
                    temp[j] = _network[i][j].Mult(tempPrev.Take(GetLengthOfLayer(i - 1)).ToArray());
                }
                tempPrev = temp;
            }

            return tempPrev;
        }

        public static void SaveTo(NeuralNetwork network, string path)
        {
            string json = JsonConvert.SerializeObject(network);
            File.WriteAllText(path, json);
        }

        public static NeuralNetwork ReadFrom(string path)
        {
            string json = File.ReadAllText(path);
            return (NeuralNetwork) JsonConvert.DeserializeObject(json);
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
    }
}