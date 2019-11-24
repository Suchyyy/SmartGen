using System.Collections;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Algorithm.Model;
using NeuralNetwork.ActivationFunction;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public IList<Layer> Layers { get; }
        public IActivationFunction ActivationFunction { get; set; }

        public NeuralNetwork()
        {
            Layers = new List<Layer>();
        }


        public void AddLayer(Layer layer) => Layers.Add(layer);

        public IList<double> GetResult(IEnumerable<double> inputs)
        {
            Layers[0].CalculateOutputs(inputs, ActivationFunction);

            for (var i = 1; i < Layers.Count; i++)
            {
                Layers[i].CalculateOutputs(Layers[i - 1].Outputs, ActivationFunction);
            }

            return Layers.Last().Outputs;
        }

        public void SetWeights(Chromosome chromosome)
        {
            var weights = chromosome.GetWeights();

            var index = 0;
            foreach (var layer in Layers)
            {
                foreach (var neuron in layer.Neurons)
                {
                    for (var i = 0; i < neuron.Weights.Count; i++)
                    {
                        neuron.Weights[i] = weights[index++];
                    }
                }
            }
        }
    }
}