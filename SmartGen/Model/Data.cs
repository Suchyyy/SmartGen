using System;
using System.Collections.Generic;
using System.Linq;
using MoreLinq;

namespace SmartGen.Model
{
    public class Data
    {
        public List<List<double>> Attributes { get; } = new List<List<double>>();
        public List<List<double>> ObjectClass { get; } = new List<List<double>>();

        public List<double> MinClassValues { get; private set; } = new List<double>();
        public List<double> MaxClassValues { get; private set; } = new List<double>();

        public Data RemoveLeastRelevantColumn(List<List<double>> correlation, int resultColumnCount)
        {
            if (correlation.Count != ObjectClass.First().Count ||
                correlation.First().Count != Attributes.First().Count + ObjectClass.First().Count)
            {
                throw new IndexOutOfRangeException("invalid data");
            }

            var data = new Data();

            var colWeights = new Dictionary<int, double>();

            for (var i = 0; i < Attributes.First().Count; i++)
            {
                colWeights.Add(i, correlation.Sum(row => row[i]));
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

            foreach (var classes in ObjectClass)
            {
                data.ObjectClass.Add(new List<double>(classes));
            }

            data.MaxClassValues = new List<double>(MaxClassValues);
            data.MinClassValues = new List<double>(MinClassValues);

            return data;
        }
    }
}