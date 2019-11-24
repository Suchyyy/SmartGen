using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Mutation
{
    public abstract class Mutation : GeneticOperator
    {
        private readonly double _mutationProbability;

        protected Mutation(double mutationProbability)
        {
            _mutationProbability = mutationProbability;
        }

        protected abstract void Mutate(Chromosome chromosome);

        public void MutatePopulation()
        {
            foreach (var chromosome in Population)
            {
                if (ThreadSafeRandom.NextDouble() < _mutationProbability)
                {
                    Mutate(chromosome);
                }
            }
        }
    }
}