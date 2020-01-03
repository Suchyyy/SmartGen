using System.Threading.Tasks;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Utils;
using MoreLinq;

namespace GeneticAlgorithm.Algorithm.Selection
{
    public class TournamentSelection : Selection
    {
        private readonly int _populationSize;
        private readonly int _tournamentSize;

        public TournamentSelection(int populationSize, int tournamentSize) : base(populationSize)
        {
            _populationSize = populationSize;
            _tournamentSize = tournamentSize;
        }

        public override void SelectPopulation()
        {
            Parallel.For(0, _populationSize, i =>
            {
                var tournament = new Chromosome[_tournamentSize];

                for (var j = 0; j < _tournamentSize; j++)
                {
                    tournament[j] = Population[ThreadSafeRandom.NextInt(Population.Count)];
                }

                NewPopulation[i] = tournament.MinBy(ch => ch.Fitness).First();
            });

            Parallel.For(0, _populationSize, i => Population[i] = NewPopulation[i].Clone());
        }
    }
}