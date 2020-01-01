using System;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Model
{
    public class Chromosome
    {
        public int[] Genome { get; }
        private readonly int _length;
        public double Fitness { get; set; }
        public double ValidationFitness { get; set; }

        public Chromosome(int length)
        {
            Fitness = 0;
            _length = length;
            Genome = new int[_length];

            for (var i = 0; i < _length; i++)
                Genome[i] = ThreadSafeRandom.NextInt(int.MinValue, int.MaxValue);
        }

        private Chromosome(int length, int[] genome)
        {
            Fitness = 0;
            ValidationFitness = 0;
            _length = length;
            Genome = new int[genome.Length];

            Buffer.BlockCopy(genome, 0, Genome, 0, length * sizeof(int));
        }

        public int GenomeLength()
        {
            return 32 * Genome.Length;
        }

        public Chromosome Clone()
        {
            return new Chromosome(_length, Genome);
        }
    }
}