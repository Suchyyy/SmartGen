using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using GeneticAlgorithm.Algorithm.Crossover;
using GeneticAlgorithm.Algorithm.Mutation;
using GeneticAlgorithm.Algorithm.Selection;
using MoreLinq;
using NeuralNetwork;
using NeuralNetwork.ActivationFunction;
using NUnit.Framework;
using SmartGen.Mapper;
using SmartGen.MathUtils;

namespace Test
{
    [TestFixture]
    public class ParkinsonDataTest
    {
        [Test]
        public void Test2([Values(100)] int iterations, [Values(100)] int columns)
        {
            var testDir = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var dataPath = Path.Combine(testDir ?? throw new NullReferenceException(), "..", "Data", "data.csv");

            var data = CsvMapper.ReadDataFromFile(dataPath, new[] {','}, new[] {754}, CultureInfo.InvariantCulture);
            var correlation = Correlation.GetCorrelation(data);
            data = data.RemoveLeastRelevantColumn(correlation, columns);
            
            var neuralNetwork = new NeuralNetwork.NeuralNetwork();
            var activationFunction = new SigmoidFunction(-1);

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(columns / 2, columns));
            neuralNetwork.AddLayer(new Layer(1, columns / 2));

            var genetic = new GeneticAlgorithm.GeneticAlgorithm(100, neuralNetwork.GetConnectionCount())
            {
                Selection = new TournamentSelection(100, 10),
                Crossover = new TwoPointCrossover(0.65, 100),
                Mutation = new FlipBitMutation(0.05)
            };

            var keepLooping = true;
            for (var it = 0; it < iterations && keepLooping; it++)
            {
                foreach (var chromosome in genetic.Population)
                {
                    neuralNetwork.SetWeights(chromosome.Genome, -5, 5);

                    Parallel.For(0, 100, i =>
                    {
                        var res = neuralNetwork.GetResult(data.Attributes[i]);
                        lock (chromosome)
                        {
                            chromosome.Fitness += Math.Abs(res[0] - data.ObjectClass[i][0]);
                        }
                    });
                }

                keepLooping = !genetic.Population.Any(chromosome => chromosome.Fitness < 5.0);

                Console.WriteLine($@"iteration: {it} | " +
                                  $@"best: {genetic.Population.MinBy(chromosome => chromosome.Fitness).First().Fitness}");

                if (keepLooping && it < iterations - 1)
                {
                    genetic.NextGeneration();
                }
            }

            var best = genetic.Population.MinBy(chromosome => chromosome.Fitness).First();
            neuralNetwork.SetWeights(best.Genome, -5, 5);

            var correct = data.Attributes
                .AsParallel()
                .Where((t, i) => Math.Abs(neuralNetwork.GetResult(t)[0] - data.ObjectClass[i][0]) < 0.3)
                .Count();

            Console.WriteLine(
                $@"Population: {100}, iterations: {iterations} - fitness: {best.Fitness:0.0} " +
                $@"| correctly recognized: {correct}/{data.Attributes.Count}");
        }

        [Test]
        [Ignore("d")]
        public void Test([Values(10)] int iterations)
        {
            var testDir = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var dataPath = Path.Combine(testDir ?? throw new NullReferenceException(), "..", "Data", "data.csv");

            var data = CsvMapper.ReadDataFromFile(dataPath, new[] {','}, new[] {754}, CultureInfo.InvariantCulture);

            var neuralNetwork = new NeuralNetwork.NeuralNetwork();
            var activationFunction = new SigmoidFunction(-1);

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(data.Attributes.First().Count / 2, data.Attributes.First().Count));
            neuralNetwork.AddLayer(new Layer(data.Attributes.First().Count / 2, data.Attributes.First().Count / 2));
            neuralNetwork.AddLayer(new Layer(1, data.Attributes.First().Count / 2));

            var genetic = new GeneticAlgorithm.GeneticAlgorithm(100, neuralNetwork.GetConnectionCount())
            {
                Selection = new TournamentSelection(100, 10),
                Crossover = new TwoPointCrossover(0.65, 100),
                Mutation = new FlipBitMutation(0.05)
            };

            var keepLooping = true;
            for (var it = 0; it < iterations && keepLooping; it++)
            {
                foreach (var chromosome in genetic.Population)
                {
                    neuralNetwork.SetWeights(chromosome.Genome, -5, 5);

                    Parallel.For(0, 100, i =>
                    {
                        var res = neuralNetwork.GetResult(data.Attributes[i]);
                        lock (chromosome)
                        {
                            chromosome.Fitness += Math.Abs(res[0] - data.ObjectClass[i][0]);
                        }
                    });
                }

                keepLooping = !genetic.Population.Any(chromosome => chromosome.Fitness < 5.0);

                Console.WriteLine($@"iteration: {it} | " +
                                  $@"best: {genetic.Population.MinBy(chromosome => chromosome.Fitness).First().Fitness}");

                if (keepLooping && it < iterations - 1)
                {
                    genetic.NextGeneration();
                }
            }

            var best = genetic.Population.MinBy(chromosome => chromosome.Fitness).First();
            neuralNetwork.SetWeights(best.Genome, -5, 5);

            var correct = data.Attributes
                .AsParallel()
                .Where((t, i) => Math.Abs(neuralNetwork.GetResult(t)[0] - data.ObjectClass[i][0]) < 0.3)
                .Count();

            Console.WriteLine(
                $@"Population: {100}, iterations: {iterations} - fitness: {best.Fitness:0.0} " +
                $@"| correctly recognized: {correct}/{data.Attributes.Count}");
        }
    }
}