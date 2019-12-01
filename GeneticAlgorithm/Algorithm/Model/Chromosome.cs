using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Model
{
    public class Chromosome
    {
        public BitVector32[] Genome { get; }
        public double Fitness { get; set; }

        public Chromosome(int length)
        {
            Fitness = 0;
            Genome = new BitVector32[length];

            for (var i = 0; i < length; i++)
                Genome[i] = new BitVector32(ThreadSafeRandom.NextInt(int.MinValue, int.MaxValue));
        }

        public IList<double> GetWeights()
        {
            return Genome.Select(v => v.Data / (double) int.MaxValue).ToList();
        }

        public Chromosome Clone()
        {
            var c = new Chromosome(Genome.Length) {Fitness = Fitness};
            for (var i = 0; i < Genome.Length; i++) c.Genome[i] = new BitVector32(Genome[i]);

            return c;
        }
    }
}