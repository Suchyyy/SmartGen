using System.ComponentModel;
using NeuralNetwork.ActivationFunction;

namespace SmartGen.Types
{
    public enum ActivationFunctionType
    {
        [Description("Binary step")] BinaryStep,
        [Description("ReLU")] ReLU,
        [Description("Sigmoid")] Sigmoid,
        [Description("TanH")] TanH
    }

    public static class ActivationFunctionExtension
    {
        public static IActivationFunction GetFunction(ActivationFunctionType type)
        {
            switch (type)
            {
                case ActivationFunctionType.BinaryStep:
                    return new BinaryStepFunction();
                case ActivationFunctionType.ReLU:
                    return new ReLUFunction();
                case ActivationFunctionType.Sigmoid:
                    return new SigmoidFunction(1);
                case ActivationFunctionType.TanH:
                default:
                    return new TanHFunction();
            }
        }
    }
}