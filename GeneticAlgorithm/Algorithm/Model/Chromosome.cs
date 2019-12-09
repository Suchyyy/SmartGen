using System;
using GeneticAlgorithm.Utils;

namespace GeneticAlgorithm.Algorithm.Model
{
    public class Chromosome
    {
        public short[] Genome { get; }
        private readonly int _length;
        public double Fitness { get; set; }

        public Chromosome(int length)
        {
            Fitness = 0;
            _length = length;
            Genome = new short[_length];

            for (var i = 0; i < _length; i++)
                Genome[i] = ThreadSafeRandom.NextShort(short.MinValue, short.MaxValue);
        }

        private Chromosome(int length, short[] genome)
        {
            Fitness = 0;
            _length = length;
            Genome = new short[genome.Length];

            Buffer.BlockCopy(genome, 0, Genome, 0, length * sizeof(short));
        }

        public int GenomeLength()
        {
            return sizeof(short) * 8 * Genome.Length;
        }

        public Chromosome Clone()
        {
            return new Chromosome(_length, Genome);
        }
    }
}