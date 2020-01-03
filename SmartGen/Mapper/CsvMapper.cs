using System;
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
                while ((line = file.ReadLine()) != null)
                {
                    var parts = line.Split(Separator);

                    if (parts.Length == 0) continue;

                    var row = parts.Select(part => Convert.ToDouble(part.Trim(), format)).ToList();

                    for (var j = 0; j < classCount; j++)
                    {
                        data.ObjectClass.Add(row.Last());
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
            var max = new double[data.Attributes.First().Count + 1];
            var min = new double[data.Attributes.First().Count + 1];

            var attCount = data.Attributes.First().Count;

            // find min and max for each attribute
            for (var i = 0; i < data.Attributes.Count; i++)
            {
                double value;

                for (var j = 0; j < attCount; j++)
                {
                    value = data.Attributes[i][j];

                    if (value > max[j]) max[j] = value;
                    if (value < min[j]) min[j] = value;
                }

                value = data.ObjectClass[i];

                if (value > max[attCount]) max[attCount] = value;
                if (value < min[attCount]) min[attCount] = value;
            }

            // normalize them
            for (var i = 0; i < data.Attributes.Count; i++)
            {
                for (var j = 0; j < attCount; j++)
                {
                    data.Attributes[i][j] = (data.Attributes[i][j] - min[j]) / (max[j] - min[j]);
                }

                data.ObjectClass[i] = (data.ObjectClass[i] - min[attCount]) / (max[attCount] - min[attCount]);
            }
        }
    }
}