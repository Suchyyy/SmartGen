namespace NeuralNetwork.ActivationFunction
{
    public class ReLUFunction : IActivationFunction
    {
        public double GetValue(double x) => x > 0.0 ? x : 0.0;
    }
}