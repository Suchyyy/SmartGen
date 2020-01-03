using System;
using System.Linq;
using System.Threading.Tasks;

namespace GeneticAlgorithm.Algorithm.Selection
{
    public class RankSelection : Selection
    {
        private readonly int _sum;
        private readonly int _populationSize;

        public RankSelection(int populationSize) : base(populationSize)
        {
            _populationSize = populationSize;
            _sum = (1 + populationSize) * populationSize / 2;
        }

        public override void SelectPopulation()
        {
            var sortedPopulation = Population.OrderByDescending(chromosome => chromosome.Fitness).ToList();
            
            Parallel.For(0, _populationSize, i =>
            {
                var r = Utils.ThreadSafeRandom.NextDouble() * _sum;
                var index = (int) (0.5 * (Math.Sqrt(8 * r + 1) - 1));

                NewPopulation[i] = sortedPopulation[index];
            });

            Parallel.For(0, _populationSize, i => Population[i] = NewPopulation[i].Clone());
        }
    }
}