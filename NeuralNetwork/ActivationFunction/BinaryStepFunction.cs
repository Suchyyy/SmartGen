namespace NeuralNetwork.ActivationFunction
{
    public class BinaryStepFunction : IActivationFunction
    {
        public double GetValue(double x) => x < 0.0 ? 0.0 : 1.0;
    }
}