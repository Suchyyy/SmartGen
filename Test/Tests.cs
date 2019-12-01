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
    internal class SampleData
    {
        public int ObjectClass { get; }

        public double[] Data { get; }

        public SampleData()
        {
            var x = ThreadSafeRandom.NextDouble() * 2.0 - 1.0;
            var y = ThreadSafeRandom.NextDouble() * 2.0 - 1.0;

            if (x >= 0.0) x += 0.3;
            if (x >= 1.0) x = 1.0;

            if (y >= 0.0) y += 0.3;
            if (y >= 1.0) y = 1.0;

            if (x <= 0.0) x -= 0.3;
            if (x <= -1.0) x = -1.0;

            if (y <= 0.0) y -= 0.3;
            if (y <= -1.0) y = -1.0;

            ObjectClass = x * y > 0.0 ? 1 : -1;

            Data = new[] {x, y};
        }
    }

    [TestFixture]
    public class Tests
    {
        [Test]
        public void Test1([Values(100, 200)] int populationSize,
            [Values(10, 50, 250, 1000)] int iterations,
            [Values(30)] int dataSize)
        {
            var neuralNetwork = new NeuralNetwork.NeuralNetwork();
            var activationFunction = new TanHFunction();

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(4, 2));
            neuralNetwork.AddLayer(new Layer(1, 4));

            var genetic = new GeneticAlgorithm.GeneticAlgorithm(populationSize, neuralNetwork.GetConnectionCount())
            {
                Selection = new TournamentSelection(populationSize, populationSize / 10),
                Crossover = new TwoPointCrossover(0.65, populationSize),
                Mutation = new FlipBitMutation(0.05)
            };

            var data = new List<SampleData>();
            for (var i = 0; i < dataSize; i++) data.Add(new SampleData());

            for (var it = 0; it < iterations; it++)
            {
                foreach (var chromosome in genetic.Population)
                {
                    neuralNetwork.SetWeights(chromosome);

                    data.ForEach(sampleData =>
                    {
                        var res = neuralNetwork.GetResult(sampleData.Data);
                        var fit = 0.3 / Math.Abs(res[0] - sampleData.ObjectClass);

                        if (fit > 1.0) fit = 1.0;

                        chromosome.Fitness += fit;
                    });
                }

                if (it < iterations - 1)
                {
                    genetic.NextGeneration();
                }
            }

            var best = genetic.Population.MaxBy(chromosome => chromosome.Fitness).First();
            neuralNetwork.SetWeights(best);

            var correct = data.Count(d => Math.Abs(neuralNetwork.GetResult(d.Data)[0] - d.ObjectClass) < 0.3);

            Console.WriteLine(
                $"Population: {populationSize}, iterations: {iterations} - fitness: {best.Fitness:0.0}/{dataSize} " +
                $"| correctly recognized: {correct}");

            data.ForEach(d =>
            {
                var results = neuralNetwork.GetResult(d.Data)[0];
                Console.WriteLine($"[{d.Data[0]: 0.00;-0.00} {d.Data[1]: 0.00;-0.00}] - class: {d.ObjectClass: 0;-0} " +
                                  $"| recognized: {results: 0.00;-0.00} " +
                                  $"| is correct: {Math.Abs(results - d.ObjectClass) < 0.3}");
            });
        }
    }
}