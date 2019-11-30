using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Crossover
{
    public abstract class Crossover : GeneticOperator
    {
        private readonly int _populationSize;
        protected readonly IList<Chromosome> NewPopulation;

        private readonly double _crossoverProbability;

        protected Crossover(double crossoverProbability, int populationSize)
        {
            _crossoverProbability = crossoverProbability;
            _populationSize = populationSize;
            NewPopulation = Enumerable.Repeat<Chromosome>(null, populationSize).ToList();
        }

        protected abstract void CrossChromosomes(Chromosome parent1, Chromosome parent2, int index);

        public void CrossPopulation()
        {
            for (var i = 0; i < _populationSize; i += 2)
            {
                if (ThreadSafeRandom.NextDouble() > _crossoverProbability) continue;

                var parent1 = Population[ThreadSafeRandom.NextInt(_populationSize)];

                Chromosome parent2;

                do
                {
                    parent2 = Population[ThreadSafeRandom.NextInt(_populationSize)];
                } while (parent1.Equals(parent2));

                CrossChromosomes(parent1, parent2, i);
            }
        }
    }
}