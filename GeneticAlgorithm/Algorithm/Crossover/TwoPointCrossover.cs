using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Crossover
{
    public class TwoPointCrossover : Crossover
    {
        public TwoPointCrossover(double crossoverProbability) : base(crossoverProbability)
        {
        }

        protected override void CrossChromosomes(Chromosome ch1, Chromosome ch2)
        {
            var start = ThreadSafeRandom.NextInt(ch1.Genome.Length * 32);
            var end = ThreadSafeRandom.NextInt(ch1.Genome.Length * 32);

            if (start > end)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }

            for (var i = start; i < end; i++)
            {
                var gen = i / 32;
                var bit = i - gen * 32;
                var value1 = ch1.Genome[gen][bit];
                var value2 = ch2.Genome[gen][bit];

                ch1.Genome[gen][bit] = value2;
                ch2.Genome[gen][bit] = value1;
            }
        }
    }
}