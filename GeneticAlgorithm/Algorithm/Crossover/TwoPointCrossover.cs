using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Crossover
{
    public class TwoPointCrossover : Crossover
    {
        public TwoPointCrossover(double crossoverProbability, int populationSize)
            : base(crossoverProbability, populationSize)
        {
        }

        protected override void CrossChromosomes(Chromosome parent1, Chromosome parent2, int index)
        {
            var start = ThreadSafeRandom.NextInt(parent1.GenomeLength());
            var end = ThreadSafeRandom.NextInt(parent1.GenomeLength());

            var child1 = parent1.Clone();
            var child2 = parent2.Clone();

            NewPopulation[index] = child1;

            if (index + 1 < NewPopulation.Count)
                NewPopulation[index + 1] = child2;

            if (ThreadSafeRandom.NextDouble() > CrossoverProbability) return;

            if (start > end)
            {
                var tmp = start;
                start = end;
                end = tmp;
            }

            for (var i = start; i <= end; i++)
            {
                var gen = i / 32;
                var bit = i - gen * 32;


                if (bit == 0 && end / 32 != gen)
                {
                    var genome = child1.Genome[gen];
                    child1.Genome[gen] = child2.Genome[gen];
                    child2.Genome[gen] = genome;

                    i += 31;
                }
                else if (((child1.Genome[gen] >> bit & 1) ^ (child2.Genome[gen] >> bit & 1)) == 1)
                {
                    child1.Genome[gen] ^= (int) (1 << bit);
                    child2.Genome[gen] ^= (int) (1 << bit);
                }
            }
        }
    }
}