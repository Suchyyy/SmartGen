using System;
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
using SmartGen.Model;

namespace Test
{
    [TestFixture]
    public class ParkinsonDataTest
    {
        [Test]
        public void Test2([Values(500)] int iterations, [Values(100)] int columns)
        {
            var testDir = Path.GetDirectoryName(Path.GetDirectoryName(TestContext.CurrentContext.TestDirectory));
            var dataPath = Path.Combine(testDir ?? throw new NullReferenceException(), "..", "Data", "data.csv");

            var mapper = new CsvMapper();

            var data = mapper.ReadDataFromFile(dataPath, 1);
            var correlation = Correlation.GetCorrelation(data);
            data = data.RemoveLeastRelevantColumn(correlation, columns);

            var dataSet = data.SplitData(1, 2, 0);
            var trainingData = dataSet[DataType.Training];
            var testingData = dataSet[DataType.Testing];

            var neuralNetwork = new NeuralNetwork.NeuralNetwork { MinWeight = -5, MaxWeight = 5 };
            var activationFunction = new SigmoidFunction(-1);

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(10, columns));
            neuralNetwork.AddLayer(new Layer(1, 10));

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
                    neuralNetwork.SetWeights(chromosome.Genome);

                    Parallel.For(0, trainingData.Attributes.Count, i =>
                    {
                        var res = neuralNetwork.GetResult(trainingData.Attributes[i]);
                        lock (chromosome)
                        {
                            chromosome.Fitness += Math.Abs(res[0] - trainingData.ObjectClass[i][0]);
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
            neuralNetwork.SetWeights(best.Genome);

            var correct = testingData.Attributes
                .AsParallel()
                .Where((t, i) => Math.Abs(neuralNetwork.GetResult(t)[0] - testingData.ObjectClass[i][0]) < 0.3)
                .Count();

            Console.WriteLine(
                $@"Population: {100}, iterations: {iterations} - fitness: {best.Fitness:0.0} " +
                $@"| correctly recognized: {correct}/{testingData.Attributes.Count}");
        }
    }
}