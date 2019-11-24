using System.Collections.Generic;
using GeneticAlgorithm.Algorithm.Model;

namespace GeneticAlgorithm.Algorithm
{
    public abstract class GeneticOperator
    {
        internal IList<Chromosome> Population { get; set; }
    }
}