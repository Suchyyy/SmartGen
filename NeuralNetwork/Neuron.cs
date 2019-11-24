using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Utils;

namespace NeuralNetwork
{
    public class Neuron
    {
        public IList<double> Weights { get; }

        public Neuron(int connectionsCount)
        {
            Weights = Enumerable.Repeat(0.0, connectionsCount).ToList();
        }

        public double GetOutput(IEnumerable<double> inputs)
        {
            return inputs.Select((input, index) => Weights[index] * input).Sum();
        }
    }
}