using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Model
{
    public class Chromosome : ICloneable
    {
        public BitVector32[] Genome { get; }
        public double Fitness { get; set; }

        public Chromosome(int length)
        {
            Fitness = 0;
            Genome = new BitVector32[length];

            for (var i = 0; i < length; i++) Genome[i] = new BitVector32(ThreadSafeRandom.NextInt(-1, 1));
        }

        public IList<double> GetWeights()
        {
            return Genome.Select(v => (v.Data - int.MinValue) / ((double) int.MaxValue - (double) int.MinValue))
                .ToList();
        }

        public object Clone() => MemberwiseClone();
    }
}