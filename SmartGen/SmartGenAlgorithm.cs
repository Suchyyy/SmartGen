using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
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
        public delegate void IterationDelegate(int iteration, double trainError, double validationError);

        public event IterationDelegate IterationEvent = delegate { };

        private readonly GeneticAlgorithm.GeneticAlgorithm _geneticAlgorithm;
        private readonly NeuralNetwork.NeuralNetwork _neuralNetwork;

        private bool _keepLooping = true;

        public Dictionary<DataType, Data> DataSet { get; set; }
        public bool IsPaused { get; set; }

        public int MaxIterations { get; set; }
        public double ErrorTolerance { get; set; }
        

        public SmartGenAlgorithm(GeneticAlgorithm.GeneticAlgorithm geneticAlgorithm,
            NeuralNetwork.NeuralNetwork neuralNetwork)
        {
            _geneticAlgorithm = geneticAlgorithm;
            _neuralNetwork = neuralNetwork;
            IsPaused = false;
        }

        public NeuralNetwork.NeuralNetwork GetTrainedNeuralNetwork()
        {
            _neuralNetwork.SetWeights(_geneticAlgorithm.Population.MinBy(chromosome => chromosome.Fitness)
                .First().Genome);
            return _neuralNetwork;
        }

        public void Stop()
        {
            _keepLooping = false;
        }

        public void Run()
        {
            var trainingData = DataSet[DataType.Training];
            var validationData = DataSet[DataType.Validating];
            var trainingDataCount = trainingData.Attributes.Count;
            var validationDataCount = validationData.Attributes.Count;

            for (var iteration = 0; iteration < MaxIterations && _keepLooping; iteration++)
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

                    Parallel.For(0, validationDataCount, i =>
                    {
                        var res = _neuralNetwork.GetResult(validationData.Attributes[i]);

                        lock (chromosome)
                        {
                            chromosome.ValidationFitness += Math.Abs(res[0] - validationData.ObjectClass[i][0]);
                        }
                    });

                    chromosome.Fitness /= trainingDataCount;
                    chromosome.ValidationFitness /= validationDataCount;
                }

                var err = _geneticAlgorithm.Population.Min(chromosome => chromosome.Fitness);
                var valErr = _geneticAlgorithm.Population.Min(chromosome => chromosome.ValidationFitness);

                _keepLooping = err > ErrorTolerance;

                IterationEvent(iteration, err, valErr);

                if (_keepLooping && iteration < MaxIterations - 1) _geneticAlgorithm.NextGeneration();

                while (IsPaused)
                {
                    Thread.Sleep(100);
                }
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