using System;

namespace NeuralNetwork.ActivationFunction
{
    public class TanHFunction : IActivationFunction
    {
        public double GetValue(double x)
        {
            var eX = Math.Exp(x);
            var emX = Math.Exp(-x);

            return (eX - emX) / (eX + emX);
        }
    }
}