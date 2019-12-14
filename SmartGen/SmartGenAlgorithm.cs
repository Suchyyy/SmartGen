using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MoreLinq;
using NeuralNetwork;
using NeuralNetwork.ActivationFunction;
using SmartGen.Model;
using SmartGen.Properties;
using SmartGen.Types;

namespace SmartGen
{
    public class SmartGenAlgorithm
    {
        public delegate void IterationDelegate(int iteration, double minError, double avgError, double maxError);

        public event IterationDelegate IterationEvent = delegate { };

        private readonly GeneticAlgorithm.GeneticAlgorithm _geneticAlgorithm;
        private readonly NeuralNetwork.NeuralNetwork _neuralNetwork;

        public Dictionary<DataType, Data> DataSet { get; set; }

        public int MaxIterations { get; set; }
        public double ErrorTolerance { get; set; }

        public SmartGenAlgorithm(GeneticAlgorithm.GeneticAlgorithm geneticAlgorithm,
            NeuralNetwork.NeuralNetwork neuralNetwork)
        {
            _geneticAlgorithm = geneticAlgorithm;
            _neuralNetwork = neuralNetwork;
        }

        public void Run()
        {
            var keepLooping = true;
            var trainingData = DataSet[DataType.Training];
            var trainingDataCount = trainingData.Attributes.Count;
            
            for (var iteration = 0; iteration < MaxIterations && keepLooping; iteration++)
            {
                foreach (var chromosome in _geneticAlgorithm.Population)
                {
                    _neuralNetwork.SetWeights(chromosome.Genome);

                    Parallel.For(0, trainingDataCount, i =>
                    {
                        var res = _neuralNetwork.GetResult(trainingData.Attributes[i]);

                        lock (chromosome)
                        {
                            chromosome.Fitness += Math.Abs(res[0] - trainingData.ObjectClass[i][0]);
                        }
                    });
                }

                var avgError = 0.0;
                var minError = _geneticAlgorithm.Population.First().Fitness;
                var maxError = _geneticAlgorithm.Population.First().Fitness;

                foreach (var chromosome in _geneticAlgorithm.Population)
                {
                    var fitness = chromosome.Fitness;

                    avgError += fitness;
                    if (fitness > maxError) maxError = fitness;
                    if (fitness < minError) minError = fitness;
                    if (fitness < ErrorTolerance) keepLooping = false;
                }

                avgError /= _geneticAlgorithm.Population.Count;

                IterationEvent(iteration, minError, avgError, maxError);

                if (keepLooping && iteration < MaxIterations - 1) _geneticAlgorithm.NextGeneration();
            }
        }

        public static NeuralNetwork.NeuralNetwork CreateNeuralNetwork()
        {
            var neuralNetwork = new NeuralNetwork.NeuralNetwork
            {
                Bias = Settings.Default.Bias,
                MinWeight = Settings.Default.MinWeight,
                MaxWeight = Settings.Default.MaxWeight,
                ActivationFunction = ActivationFunctionExtension.GetFunction(Settings.Default.ActivationFunction)
            };

            if (neuralNetwork.ActivationFunction is SigmoidFunction)
            {
                neuralNetwork.ActivationFunction = new SigmoidFunction(Settings.Default.T);
            }

            if (Settings.Default.HiddenLayers.Length > 0)
            {
                var prevSize = Settings.Default.InputLayerSize;

                foreach (var hiddenLayer in Settings.Default.HiddenLayers)
                {
                    neuralNetwork.AddLayer(new Layer(hiddenLayer, prevSize));
                    prevSize = hiddenLayer;
                }

                neuralNetwork.AddLayer(new Layer(Settings.Default.OutputLayerSize, prevSize));
            }
            else
            {
                neuralNetwork.AddLayer(new Layer(Settings.Default.OutputLayerSize, Settings.Default.InputLayerSize));
            }

            return neuralNetwork;
        }

        public static GeneticAlgorithm.GeneticAlgorithm CreateGeneticAlgorithm(int genomeSize)
        {
            var populationSize = Settings.Default.PopulationSize;

            var geneticAlgorithm = new GeneticAlgorithm.GeneticAlgorithm(populationSize, genomeSize)
            {
                Selection = SelectionExtension.GetSelection(Settings.Default.Selection, populationSize,
                    Settings.Default.SelectionSize),
                Crossover = CrossoverExtension.GetCrossover(Settings.Default.Crossover, populationSize,
                    Settings.Default.CrossoverProbability),
                Mutation = MutationExtension.GetMutation(Settings.Default.Mutation,
                    Settings.Default.MutationProbability)
            };

            return geneticAlgorithm;
        }
    }
}