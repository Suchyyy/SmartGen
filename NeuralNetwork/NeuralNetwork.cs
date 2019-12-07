using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.ActivationFunction;
using NeuralNetwork.Utils;

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
            var previousLayerResult = Layers[0].GetOutputs(inputs, ActivationFunction);

            for (var i = 1; i < Layers.Count; i++)
            {
                previousLayerResult = Layers[i].GetOutputs(previousLayerResult, ActivationFunction);
            }

            return previousLayerResult;
        }

        public int GetConnectionCount()
        {
            return Layers.AsParallel().Sum(layer => layer.Neurons.Sum(neuron => neuron.Weights.Count));
        }

        public void SetWeights(IEnumerable<int> genome, double minWeight, double maxWeight)
        {
            var weights = genome
                .Select(v => MathUtils.GetInNewRange(v, int.MinValue, int.MaxValue, minWeight, maxWeight))
                .ToList();

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