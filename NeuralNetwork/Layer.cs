using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.ActivationFunction;

namespace NeuralNetwork
{
    public class Layer
    {
        public int InputSize { get; }
        public int OutputSize { get; }
        public double[,] WeightMatrix { get; }

        public Layer(int size, Layer previousLayer)
        {
            var prevSize = previousLayer.WeightMatrix.GetLength(0);

            InputSize = prevSize;
            OutputSize = size;
            WeightMatrix = new double[OutputSize, InputSize];
        }

        public Layer(int size, int inputs)
        {
            InputSize = inputs;
            OutputSize = size;
            WeightMatrix = new double[OutputSize, InputSize];
        }

        public IList<double> GetOutputs(IList<double> inputs, IActivationFunction activationFunction, double bias)
        {
            if (inputs.Count != InputSize)
                throw new ArgumentOutOfRangeException(nameof(inputs),
                    "Inputs size should be the same as layer input size.");

            var output = new double[OutputSize];

            for (var row = 0; row < OutputSize; row++)
            {
                var value = bias;

                for (var col = 0; col < InputSize; col++)
                {
                    value += WeightMatrix[row, col] * inputs[col];
                }

                output[row] = activationFunction.GetValue(value);
            }

            return output;
        }
    }
}