using System.ComponentModel;
using GeneticAlgorithm.Algorithm.Crossover;

namespace SmartGen.Types
{
    public enum CrossoverType
    {
        [Description("One point")] OnePoint,

        [Description("Two point")] TwoPoint
    }

    public static class CrossoverExtension
    {
        public static Crossover GetCrossover(CrossoverType type, int populationSize, double crossoverProbability)
        {
            switch (type)
            {
                case CrossoverType.OnePoint:
                    return new OnePointCrossover(crossoverProbability, populationSize);
                case CrossoverType.TwoPoint:
                default:
                    return new TwoPointCrossover(crossoverProbability, populationSize);
            }
        }
    }
}