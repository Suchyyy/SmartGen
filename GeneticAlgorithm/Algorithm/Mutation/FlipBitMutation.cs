using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Mutation
{
    public class FlipBitMutation : Mutation
    {
        public FlipBitMutation(double mutationProbability) : base(mutationProbability)
        {
        }

        protected override void Mutate(Chromosome chromosome)
        {
            var index = ThreadSafeRandom.NextInt(chromosome.Genome.Length * 32);

            chromosome.Genome[index / 32][1 << (index % 32)] = !chromosome.Genome[index / 32][1 << (index % 32)];
        }
    }
}