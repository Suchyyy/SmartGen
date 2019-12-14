using System.ComponentModel;
using GeneticAlgorithm.Algorithm.Mutation;

namespace SmartGen.Types
{
    public enum MutationType
    {
        [Description("Flip bit")] FlipBit
    }

    public static class MutationExtension
    {
        public static Mutation GetMutation(MutationType type, double mutationProbability)
        {
            switch (type)
            {
                case MutationType.FlipBit:
                default:
                    return new FlipBitMutation(mutationProbability);
            }
        }
    }
}