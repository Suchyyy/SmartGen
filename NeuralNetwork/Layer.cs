using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using NeuralNetwork.ActivationFunction;

namespace NeuralNetwork
{
    public class Layer
    {
        public IList<Neuron> Neurons { get; }
        public IList<double> Outputs { get; }

        public Layer(int size, Layer previousLayer)
        {
            var prevSize = previousLayer.Neurons.Count;

            Neurons = Enumerable.Repeat(new Neuron(prevSize), size).ToList();
            Outputs = Enumerable.Repeat(0.0, size).ToList();
        }

        public Layer(int size, int inputs)
        {
            Neurons = Enumerable.Repeat(new Neuron(inputs), size).ToList();
        }

        public void CalculateOutputs(IEnumerable<double> inputs, IActivationFunction activationFunction)
        {
            Parallel.ForEach(Neurons,
                (neuron, state, index) =>
                {
                    Outputs[(int) index] = activationFunction.GetValue(neuron.GetOutput(inputs));
                });
        }
    }
}