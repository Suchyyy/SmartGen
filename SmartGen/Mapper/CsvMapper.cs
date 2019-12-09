using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using SmartGen.Model;

namespace SmartGen.Mapper
{
    public static class CsvMapper
    {
        // ReSharper disable once ReturnTypeCanBeEnumerable.Global
        public static Data ReadDataFromFile(string path, char[] separator, IEnumerable<int> classIndexes,
            CultureInfo cultureInfo, int skipRow = 2, bool normalized = true)
        {
            var data = new Data();
            var sortedIndexes = classIndexes.OrderByDescending(v => v).ToArray();

            using (var file = new StreamReader(path))
            {
                for (var i = 0; i < skipRow; i++) file.ReadLine();

                string line;
                for (var i = 0; (line = file.ReadLine()) != null; i++)
                {
                    var parts = line.Split(separator);

                    if (parts.Length == 0) continue;

                    var row = parts.Select(part => Convert.ToDouble(part.Trim(), cultureInfo)).ToList();

                    data.ObjectClass.Add(new List<double>());

                    foreach (var index in sortedIndexes)
                    {
                        data.ObjectClass[i].Add(row[index]);
                        row.RemoveAt(index);
                    }

                    data.Attributes.Add(row);
                }
            }

            if (normalized) Normalize(data);

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

            // save min and max of the classes (may be useful)
            for (var i = attCount; i < data.ObjectClass.First().Count + attCount; i++)
            {
                data.MaxClassValues.Add(max[i]);
                data.MinClassValues.Add(min[i]);
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