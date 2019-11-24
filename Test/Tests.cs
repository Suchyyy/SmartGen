using System;
using System.Collections.Generic;
using System.Linq;
using GeneticAlgorithm.Algorithm.Crossover;
using GeneticAlgorithm.Algorithm.Mutation;
using GeneticAlgorithm.Algorithm.Selection;
using GeneticAlgorithm.Utils;
using MoreLinq.Extensions;
using NeuralNetwork;
using NeuralNetwork.ActivationFunction;
using NUnit.Framework;

namespace Test
{
    class SampleData
    {
        public double X { get; }
        public double Y { get; }
        public int ObjectClass { get; }

        public SampleData()
        {
            X = ThreadSafeRandom.NextDouble();
            Y = ThreadSafeRandom.NextDouble();

            ObjectClass = 1;

            if (X * 20.0 - 10 > 0 && X * 20.0 - 10 < 1) X += 0.1;
            if (X * 20.0 - 10 < 0 && X * 20.0 - 10 > -1) X -= 0.1;

            if (Y * 20.0 - 10 > 0 && Y * 20.0 - 10 < 1) Y += 0.1;
            if (Y * 20.0 - 10 < 0 && Y * 20.0 - 10 > -1) Y -= 0.1;

            if (X * 20.0 - 10.0 > 0.0) ObjectClass += 1;
            if (Y * 20.0 - 10.0 > 0.0) ObjectClass += 2;
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1()
        {
            var neuralNetwork = new NeuralNetwork.NeuralNetwork();
            var activationFunction = new SigmoidFunction(1);

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(2, 2));
            neuralNetwork.AddLayer(new Layer(4, 2));

            var genetic = new GeneticAlgorithm.GeneticAlgorithm(1000, neuralNetwork.GetConnectionCount())
            {
                Selection = new TournamentSelection(1000, 10),
                Crossover = new OnePointCrossover(0.65),
                Mutation = new FlipBitMutation(0.05)
            };

            var data = new List<SampleData>();
            for (var i = 0; i < 100; i++) data.Add(new SampleData());

            for (var it = 0; it < 30; it++)
            {
                for (var i = 0; i < 1000; i++)
                {
                    neuralNetwork.SetWeights(genetic.Population[i]);

                    data.ForEach(sampleData =>
                    {
                        var res = neuralNetwork.GetResult(new[] {sampleData.X, sampleData.Y});
                        var c = res.IndexOf(res.Max()) + 1;

                        genetic.Population[i].Fitness += c == sampleData.ObjectClass ? 1.0 : 0.0;
                    });
                }

                if (it != 29)
                {
                    genetic.NextGeneration();
                }
            }

            var best = genetic.Population.MaxBy(chromosome => chromosome.Fitness).First();

            Console.WriteLine(best.Fitness + "/100");
        }
    }
}