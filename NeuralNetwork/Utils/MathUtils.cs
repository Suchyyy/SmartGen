namespace NeuralNetwork.Utils
{
    public static class MathUtils
    {
        public static double GetInNewRange(double value, double oldMin, double oldMax, double newMin, double newMax)
        {
            return (value - oldMin) * (newMax - newMin) / (oldMax - oldMin) + newMin;
        }
    }
}