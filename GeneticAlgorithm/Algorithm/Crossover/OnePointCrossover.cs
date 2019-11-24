using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Crossover
{
    public class OnePointCrossover : Crossover
    {
        public OnePointCrossover(double crossoverProbability) : base(crossoverProbability)
        {
        }

        protected override void CrossChromosomes(Chromosome ch1, Chromosome ch2)
        {
            var index = ThreadSafeRandom.NextInt(ch1.Genome.Length * 32);

            for (int i = 0, gen = 0; i < index; i++, gen += i / 32)
            {
                var bit = i - gen * 32;

                var value1 = ch1.Genome[gen][bit];
                var value2 = ch2.Genome[gen][bit];

                ch1.Genome[gen][bit] = value2;
                ch2.Genome[gen][bit] = value1;
            }
        }
    }
}