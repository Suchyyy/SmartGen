using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SmartGen.Model;

namespace SmartGen.Mapper
{
    public class CsvMapper
    {
        public char Separator { get; set; } = ',';
        public char DecimalSeparator { get; set; } = '.';
        public int SkipRows { get; set; } = 2;
        public bool Normalized { get; set; } = true;

        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public Data ReadDataFromFile(string path, int classCount)
        {
            var data = new Data();
            var format = new NumberFormatInfo {NumberDecimalSeparator = DecimalSeparator.ToString()};

            using (var file = new StreamReader(path))
            {
                for (var i = 0; i < SkipRows; i++) file.ReadLine();

                string line;
                for (var i = 0; (line = file.ReadLine()) != null; i++)
                {
                    var parts = line.Split(Separator);

                    if (parts.Length == 0) continue;

                    var row = parts.Select(part => Convert.ToDouble(part.Trim(), format)).ToList();

                    data.ObjectClass.Add(new List<double>());

                    for (var j = 0; j < classCount; j++)
                    {
                        data.ObjectClass[i].Add(row.Last());
                        row.RemoveAt(row.Count - 1);
                    }

                    data.Attributes.Add(row);
                }
            }

            if (Normalized) Normalize(data);

            return data;
        }

        private static void Normalize(Data data)
        {
            var max = new double[data.Attributes.First().Count + data.ObjectClass.First().Count];
            var min = new double[data.Attributes.First().Count + data.ObjectClass.First().Count];

            var attCount = data.Attributes.First().Count;

            // find min and max for each attribute and class
            for (var i = 0; i < data.Attributes.Count; i++)
            {
                for (var j = 0; j < attCount; j++)
                {
                    var value = data.Attributes[i][j];

                    if (value > max[j]) max[j] = value;
                    if (value < min[j]) min[j] = value;
                }

                for (var j = 0; j < data.ObjectClass[i].Count; j++)
                {
                    var value = data.ObjectClass[i][j];

                    if (value > max[j + attCount]) max[j + attCount] = value;
                    if (value < min[j + attCount]) min[j + attCount] = value;
                }
            }

            // normalize them
            for (var i = 0; i < data.Attributes.Count; i++)
            {
                for (var j = 0; j < attCount; j++)
                {
                    data.Attributes[i][j] = (data.Attributes[i][j] - min[j]) / (max[j] - min[j]);
                }

                for (var j = 0; j < data.ObjectClass[i].Count; j++)
                {
                    data.ObjectClass[i][j] = (data.ObjectClass[i][j] - min[j + attCount]) /
                                             (max[j + attCount] - min[j + attCount]);
                }
            }
        }
    }
}