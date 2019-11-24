using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Algorithm.Model;

namespace GeneticAlgorithm.Algorithm.Selection
{
    public abstract class Selection : GeneticOperator
    {
        protected readonly IList<Chromosome> NewPopulation;

        protected Selection(int populationSize)
        {
            NewPopulation = Enumerable.Repeat<Chromosome>(null, populationSize).ToList();
        }

        public abstract void SelectPopulation();
    }
}