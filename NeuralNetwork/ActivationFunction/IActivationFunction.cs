using System.Net.Security;

namespace NeuralNetwork.ActivationFunction
{
    public interface IActivationFunction
    {
        double GetValue(double x);
    }
}