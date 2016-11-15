using System.Linq;
using NeuralNetworik.Utils;
using Newtonsoft.Json;

namespace NeuralNetworik.NN
{
    public class Node
    {
        [JsonProperty]
        private double[] _weights;
        private double[] _previousDeltas;
        private double _gradient;

        private double _lastResponse;
        /// <summary>
        /// Returns weighted sum of inputs ater sigmoid function
        /// </summary>
        public double Response
        {
            get { return _lastResponse; }
        }

        public double Error { get; set; } = 0;

        private double _lastOutput;

        /// <summary>
        /// Returns weighted sum of inputs
        /// </summary>
        public double LocalOutput
        {
            get { return _lastOutput; }
        }

        public double this[int index] => _weights[index];

        public int Length => _weights.Length;

        private double _bias;
        private double _previousBiasDelta;

        public Node(int weightsAmount)
        {
            _weights = new double[weightsAmount];
            _previousDeltas = new double[weightsAmount];
            // inits
            for (int i = 0; i < _weights.Length; i++)
            {
                _weights[i] = ThreadSafeRandom.NextDouble() * 6 - 3;
            }
            _bias = ThreadSafeRandom.NextDouble() * 6 - 3;
        }

        public double CalculateOutput(double[] input)
        {
            double sum = input.Mult(_weights) + _bias;

            _lastOutput = sum;
            _lastResponse = MathUtils.CalculateSigmoid(sum);

            return _lastResponse;
        }

        public double CalculateOutput(Node[] input)
        {
            double[] doubleInput = input.Select(x => x.Response).ToArray();

            return CalculateOutput(doubleInput);
        }

        public void UpdateWeight(int index, double delta, double momentum)
        {
            double currentDelta = (1 - momentum) * delta + momentum * _previousDeltas[index];
            _weights[index] += currentDelta;
            _previousDeltas[index] = currentDelta;
        }

        public void UpdateBias(double delta, double momentum)
        {
            double currentDelta = (1 - momentum) * delta + _previousBiasDelta * momentum;;
            _bias += currentDelta;
            _previousBiasDelta = currentDelta;
        }
    }
}