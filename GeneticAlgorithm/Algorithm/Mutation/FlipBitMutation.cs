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
            var index = ThreadSafeRandom.NextInt(chromosome.GenomeLength());

            chromosome.Genome[index / 32] ^= (int) (1 << (index % 32));
        }
    }
}