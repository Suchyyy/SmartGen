using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SmartGen.Model
{
    public class Data
    {
        private readonly Random _random = new Random(DateTime.Now.Millisecond);

        public List<List<double>> Attributes { get; } = new List<List<double>>();
        public List<double> ObjectClass { get; } = new List<double>();

        public Dictionary<DataType, Data> SplitData(int trainingRatio, int testRatio, int validationRatio)
        {
            var dictionary = new Dictionary<DataType, Data>();
            var trainingData = new Data();
            var testingData = new Data();
            var validationData = new Data();

            var trainingProb = (double) trainingRatio / (trainingRatio + testRatio + validationRatio);
            var testProb = (double) (trainingRatio + testRatio) / (trainingRatio + testRatio + validationRatio);

            for (var i = 0; i < Attributes.Count; i++)
            {
                var rand = _random.NextDouble();

                if (rand < trainingProb)
                {
                    trainingData.Attributes.Add(Attributes[i]);
                    trainingData.ObjectClass.Add(ObjectClass[i]);
                }
                else if (rand < testProb)
                {
                    testingData.Attributes.Add(Attributes[i]);
                    testingData.ObjectClass.Add(ObjectClass[i]);
                }
                else
                {
                    validationData.Attributes.Add(Attributes[i]);
                    validationData.ObjectClass.Add(ObjectClass[i]);
                }
            }

            dictionary.Add(DataType.Training, trainingData);
            dictionary.Add(DataType.Testing, testingData);
            dictionary.Add(DataType.Validating, validationData);

            return dictionary;
        }

        public Data RemoveLeastRelevantColumn(List<double> correlation, int resultColumnCount)
        {
            if (correlation.Count != Attributes.First().Count + 1)
            {
                throw new IndexOutOfRangeException("invalid data");
            }

            var data = new Data();

            var colWeights = new Dictionary<int, double>();

            for (var i = 0; i < Attributes.First().Count; i++)
            {
                colWeights.Add(i, Math.Abs(correlation[i]));
            }

            colWeights = colWeights.OrderByDescending(pair => Math.Abs(pair.Value)).ToDictionary();

            foreach (var attribute in Attributes)
            {
                var row = new List<double>();

                for (var i = 0; i < resultColumnCount; i++)
                {
                    var index = colWeights.ElementAt(i).Key;
                    row.Add(attribute[index]);
                }

                data.Attributes.Add(row);
            }

            foreach (var c in ObjectClass)
            {
                data.ObjectClass.Add(c);
            }

            return data;
        }
    }
}