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
        public double[] Bias { get; }

        public Layer(int size, Layer previousLayer)
        {
            var prevSize = previousLayer.WeightMatrix.GetLength(0);

            InputSize = prevSize;
            OutputSize = size;
            WeightMatrix = new double[OutputSize, InputSize];
            Bias = Enumerable.Repeat(0.0, OutputSize).ToArray();
        }

        public Layer(int size, int inputs)
        {
            InputSize = inputs;
            OutputSize = size;
            WeightMatrix = new double[OutputSize, InputSize];
            Bias = Enumerable.Repeat(0.0, OutputSize).ToArray();
        }

        public IList<double> GetOutputs(IList<double> inputs, IActivationFunction activationFunction)
        {
            if(inputs.Count != InputSize)
                throw new ArgumentOutOfRangeException(nameof(inputs), "Inputs size should be the same as layer input size.");
            
            var output = new double[OutputSize];

            for (var row = 0; row < OutputSize; row++)
            {
                var value = Bias[row];

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