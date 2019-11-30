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
            var start = ThreadSafeRandom.NextInt(parent1.Genome.Length * 32);
            var end = ThreadSafeRandom.NextInt(parent1.Genome.Length * 32);

            var child1 = parent1.Clone();
            var child2 = parent2.Clone();

            NewPopulation[index] = child1;
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


                if (bit == 0 && (end / 32) != gen)
                {
                    var genome = child1.Genome[gen];
                    child1.Genome[gen] = child2.Genome[gen];
                    child2.Genome[gen] = genome;

                    i += 31;
                }
                else
                {
                    var mask = 1 << bit;

                    child1.Genome[gen][mask] = parent2.Genome[gen][mask];
                    child2.Genome[gen][mask] = parent1.Genome[gen][mask];
                }
            }
        }
    }
}