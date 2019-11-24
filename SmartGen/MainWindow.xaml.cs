using System;
using System.Linq;
using GeneticAlgorithm.Algorithm.Crossover;
using GeneticAlgorithm.Algorithm.Mutation;
using GeneticAlgorithm.Algorithm.Selection;
using GeneticAlgorithm.Utils;
using MoreLinq;
using NeuralNetwork;
using NeuralNetwork.ActivationFunction;

namespace SmartGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        // #TODO some sample data for test
        class SampleData
        {
            public double X { get; set; }
            public double Y { get; set; }
            public int ObjectClass { get; set; }

            public SampleData()
            {
                X = ThreadSafeRandom.NextDouble();
                Y = ThreadSafeRandom.NextDouble();

                ObjectClass = 1;

                if (X * 10 - 20 > 0) ObjectClass += 1;
                if (Y * 10 - 20 > 0) ObjectClass += 2;
            }
        }

        public MainWindow()
        {
            InitializeComponent();
            
            TestAlgorithm();
        }


        // #TODO just for test
        public void TestAlgorithm()
        {
            var neuralNetwork = new NeuralNetwork.NeuralNetwork();
            var activationFunction = new SigmoidFunction(1);

            neuralNetwork.ActivationFunction = activationFunction;
            neuralNetwork.AddLayer(new Layer(2, 2));
            neuralNetwork.AddLayer(new Layer(4, 2));

            var genetic = new GeneticAlgorithm.GeneticAlgorithm(1000, 6)
            {
                Selection = new TournamentSelection(1000, 100),
                Crossover = new OnePointCrossover(0.65),
                Mutation = new FlipBitMutation(0.05)
            };

            var data = Enumerable.Repeat(new SampleData(), 100).ToList();

            for (var it = 0; it < 100; it++)
            {
                for (var i = 0; i < 1000; i++)
                {
                    neuralNetwork.SetWeights(genetic.Population[i]);

                    var res = neuralNetwork.GetResult(new[] {data[i].X, data[i].Y});
                    var c = res.IndexOf(res.Max()) + 1;

                    genetic.Population[i].Fitness += c == data[i].ObjectClass ? 1.0 : 0.0;
                }

                genetic.NextGeneration();
            }

            var best = genetic.Population.MaxBy(chromosome => chromosome.Fitness).First();
            neuralNetwork.SetWeights(best);

            Console.WriteLine(best.Fitness);
            
            foreach (var sampleData in data)
            {
               var outputs= neuralNetwork.GetResult(new []{sampleData.X, sampleData.Y});

               Console.WriteLine(string.Join(", ", outputs) + " " + sampleData.ObjectClass);
            }
        }
    }
}