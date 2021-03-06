﻿using System;
using System.Collections.Generic;
using System.Linq;
using NeuralNetwork.ActivationFunction;
using NeuralNetwork.Utils;

namespace NeuralNetwork
{
    public class NeuralNetwork
    {
        public IList<Layer> Layers { get; }
        public IActivationFunction ActivationFunction { get; set; }
        public double Bias { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }

        public NeuralNetwork()
        {
            Layers = new List<Layer>();
        }


        public void AddLayer(Layer layer) => Layers.Add(layer);

        public IList<double> GetResult(IList<double> inputs)
        {
            var previousLayerResult = Layers[0].GetOutputs(inputs, ActivationFunction, Bias);

            for (var i = 1; i < Layers.Count; i++)
            {
                previousLayerResult = Layers[i].GetOutputs(previousLayerResult, ActivationFunction, Bias);
            }

            return previousLayerResult;
        }

        public int GetConnectionCount()
        {
            return Layers.Sum(layer => layer.InputSize * layer.OutputSize);
        }

        public void SetWeights(IEnumerable<int> genome)
        {
            var weights = genome
                .Select(v => MathUtils.GetInNewRange(v, int.MinValue, int.MaxValue, MinWeight, MaxWeight))
                .ToArray();

            var offset = 0;

            foreach (var layer in Layers)
            {
                var size = layer.InputSize * layer.OutputSize * sizeof(double);

                Buffer.BlockCopy(weights, offset, layer.WeightMatrix, 0, size);

                offset += size;
            }
        }
    }
}