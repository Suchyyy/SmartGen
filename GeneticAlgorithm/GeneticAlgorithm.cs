using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Crossover;
using GeneticAlgorithm.Algorithm.Model;
using GeneticAlgorithm.Algorithm.Mutation;
using GeneticAlgorithm.Algorithm.Selection;

namespace GeneticAlgorithm
{
    public class GeneticAlgorithm
    {
        private Selection _selection;
        private Crossover _crossover;
        private Mutation _mutation;
        public IList<Chromosome> Population { get; }

        public Selection Selection
        {
            get => _selection;
            set
            {
                _selection = value;
                _selection.Population = Population;
            }
        }

        public Crossover Crossover
        {
            get => _crossover;
            set
            {
                _crossover = value;
                _crossover.Population = Population;
            }
        }

        public Mutation Mutation
        {
            get => _mutation;
            set
            {
                _mutation = value;
                _mutation.Population = Population;
            }
        }

        public GeneticAlgorithm(int populationSize, int genomeSize)
        {
            Population= new List<Chromosome>();
            
            for(var i=0;i<populationSize;i++) Population.Add(new Chromosome(populationSize));
        }

        public void NextGeneration()
        {
            _selection.SelectPopulation();
            _crossover.CrossPopulation();
            _mutation.MutatePopulation();
        }
    }
}