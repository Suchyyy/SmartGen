using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;
using MoreLinq;

namespace GeneticAlgorithm.Algorithm.Selection
{
    public class TournamentSelection : Selection
    {
        private readonly int _tournamentSize;

        public TournamentSelection(int populationSize,int tournamentSize) : base(populationSize)
        {
            _tournamentSize = tournamentSize;
        }

        public override void SelectPopulation()
        {
            var tournament = new Chromosome[_tournamentSize];

            for (var i = 0; i < Population.Count; i++)
            {
                var chromosome = Population[i];
                for (var j = 0; j < _tournamentSize; j++)
                {
                    tournament[j] = Population[ThreadSafeRandom.NextInt(Population.Count)];
                }

                NewPopulation[i] = tournament.MaxBy(chromosome1 => chromosome.Fitness).First();
            }
        }
    }
}