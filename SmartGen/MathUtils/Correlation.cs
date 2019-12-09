using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartGen.Model;

namespace SmartGen.MathUtils
{
    public static class Correlation
    {
        public static List<List<double>> GetCorrelation(Data data)
        {
            var attributes = data.Attributes.First().Count + data.ObjectClass.First().Count;

            var correlation = new List<List<double>>();
            var sum = new double[attributes];
            var sqr = new double[attributes];
            var mul = new double[data.ObjectClass.First().Count, attributes];

            var tasks = new[]
            {
                Task.Run(() => CalculateSum(data, sum)),
                Task.Run(() => CalculateSquares(data, sqr)),
                Task.Run(() => CalculateMul(data, mul)),
            };

            Task.WaitAll(tasks);

            var count = data.Attributes.Count;
            var attrCount = data.Attributes.First().Count;

            for (var i = 0; i < mul.GetLength(0); i++)
            {
                var classCorr = new List<double>();

                for (var j = 0; j < mul.GetLength(1); j++)
                {
                    var sum1 = sum[j];
                    var sum2 = sum[i + attrCount];
                    var sqr1 = sqr[j];
                    var sqr2 = sqr[i + attrCount];
                    var mul12 = mul[i, j];

                    var cov = mul12 / count - sum1 * sum2 / count / count;
                    var sig1 = Math.Sqrt(sqr1 / count - sum1 * sum1 / count / count);
                    var sig2 = Math.Sqrt(sqr2 / count - sum2 * sum2 / count / count);

                    classCorr.Add(cov / sig1 / sig2);
                }

                correlation.Add(classCorr);
            }
            
            return correlation;
        }

        private static void CalculateSum(Data data, double[] sum)
        {
            var attrCount = data.Attributes.First().Count;

            for (var i = 0; i < attrCount; i++)
            {
                sum[i] = data.Attributes.Sum(row => row[i]);
            }

            for (var i = 0; i < data.ObjectClass.First().Count; i++)
            {
                sum[i + attrCount] = data.ObjectClass.Sum(row => row[i]);
            }
        }

        private static void CalculateSquares(Data data, double[] sqr)
        {
            var attrCount = data.Attributes.First().Count;

            for (var i = 0; i < attrCount; i++)
            {
                sqr[i] = data.Attributes.Sum(row => Math.Pow(row[i], 2));
            }

            for (var i = 0; i < data.ObjectClass.First().Count; i++)
            {
                sqr[i + attrCount] = data.ObjectClass.Sum(row => Math.Pow(row[i], 2));
            }
        }

        private static void CalculateMul(Data data, double[,] mul)
        {
            var attrCount = data.Attributes.First().Count;

            for (var c = 0; c < data.ObjectClass.First().Count; c++)
            {
                for (var attr = 0; attr < attrCount; attr++)
                {
                    for (var row = 0; row < data.Attributes.Count; row++)
                    {
                        mul[c, attr] += data.Attributes[row][attr] * data.ObjectClass[row][c];
                    }
                }

                for (var attr = 0; attr < data.ObjectClass.First().Count; attr++)
                {
                    mul[c, attr + attrCount] = data.ObjectClass.Sum(row => row[attr] * row[c]);
                }
            }
        }
    }
}