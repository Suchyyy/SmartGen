using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Crossover
{
    public abstract class Crossover : GeneticOperator
    {
        private readonly double _crossoverProbability;

        protected Crossover(double crossoverProbability)
        {
            _crossoverProbability = crossoverProbability;
        }

        protected abstract void CrossChromosomes(Chromosome ch1, Chromosome ch2);

        public void CrossPopulation()
        {
            foreach (var ch1 in Population)
            {
                if (ThreadSafeRandom.NextDouble() < _crossoverProbability) continue;

                Chromosome ch2;

                do
                {
                    ch2 = Population[ThreadSafeRandom.NextInt(Population.Count)];
                } while (ch1.Equals(ch2));

                CrossChromosomes(ch1, ch2);
            }
        }
    }
}