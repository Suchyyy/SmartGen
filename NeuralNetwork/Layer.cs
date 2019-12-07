using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.ActivationFunction;

namespace NeuralNetwork
{
    public class Layer
    {
        public IList<Neuron> Neurons { get; }

        public Layer(int size, Layer previousLayer)
        {
            var prevSize = previousLayer.Neurons.Count;

            Neurons = new List<Neuron>();

            for (var i = 0; i < size; i++)
            {
                Neurons.Add(new Neuron(prevSize));
            }
        }

        public Layer(int size, int inputs)
        {
            Neurons = new List<Neuron>();

            for (var i = 0; i < size; i++)
            {
                Neurons.Add(new Neuron(inputs));
            }
        }

        public IList<double> GetOutputs(IEnumerable<double> inputs, IActivationFunction activationFunction)
        {
            return Neurons.Select(neuron => activationFunction.GetValue(neuron.GetOutput(inputs))).ToList();
        }
    }
}