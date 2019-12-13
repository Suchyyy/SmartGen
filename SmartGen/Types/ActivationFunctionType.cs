using System.ComponentModel;

namespace SmartGen.Types
{
    public enum ActivationFunctionType
    {
        [Description("Binary step")] BinaryStep,
        [Description("ReLU")] ReLU,
        [Description("Sigmoid")] Sigmoid,
        [Description("TanH")] TanH
    }
}