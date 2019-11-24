using System;

namespace NeuralNetwork.ActivationFunction
{
    public class SigmoidFunction : IActivationFunction
    {
        private readonly double _t;

        public SigmoidFunction(double t)
        {
            _t = t;
        }


        public double GetValue(double x) => 1.0 / (1.0 + Math.Exp(-x * _t));
    }
}