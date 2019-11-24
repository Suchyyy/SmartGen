using System.Collections.Generic;
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

            Neurons = new List<Neuron>();
            Outputs = new List<double>();
            
            for (var i = 0; i < size; i++)
            {
                Neurons.Add(new Neuron(prevSize));
                Outputs.Add(0);
            }
        }

        public Layer(int size, int inputs)
        {
            Neurons = new List<Neuron>();
            Outputs = new List<double>();
            
            for (var i = 0; i < size; i++)
            {
                Neurons.Add(new Neuron(inputs));
                Outputs.Add(0);
            }
        }

        public void CalculateOutputs(IEnumerable<double> inputs, IActivationFunction activationFunction)
        {
            for (var i = 0; i < Outputs.Count; i++)
            {
                Outputs[i] = activationFunction.GetValue(Neurons[i].GetOutput(inputs));
            }
        }
    }
}