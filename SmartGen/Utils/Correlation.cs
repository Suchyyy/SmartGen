using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SmartGen.Model;

namespace SmartGen.Utils
{
    public static class Correlation
    {
        public static List<double> GetCorrelation(Data data)
        {
            var attributes = data.Attributes.First().Count + 1;

            var sum = new double[attributes];
            var sqr = new double[attributes];
            var mul = new double[attributes];

            var tasks = new[]
            {
                Task.Run(() => CalculateSum(data, sum)),
                Task.Run(() => CalculateSquares(data, sqr)),
                Task.Run(() => CalculateMul(data, mul)),
            };

            Task.WaitAll(tasks);

            var count = data.Attributes.Count;
            var attrCount = data.Attributes.First().Count;

            var classCorr = new List<double>();

            for (var j = 0; j < mul.Length; j++)
            {
                var sum1 = sum[j];
                var sum2 = sum[attrCount];
                var sqr1 = sqr[j];
                var sqr2 = sqr[attrCount];
                var mul12 = mul[j];

                var cov = mul12 / count - sum1 * sum2 / count / count;
                var sig1 = Math.Sqrt(sqr1 / count - sum1 * sum1 / count / count);
                var sig2 = Math.Sqrt(sqr2 / count - sum2 * sum2 / count / count);

                classCorr.Add(cov / sig1 / sig2);
            }

            return classCorr;
        }

        private static void CalculateSum(Data data, double[] sum)
        {
            var attrCount = data.Attributes.First().Count;

            for (var i = 0; i < attrCount; i++)
            {
                sum[i] = data.Attributes.Sum(row => row[i]);
            }

            sum[attrCount] = data.ObjectClass.Sum();
        }

        private static void CalculateSquares(Data data, double[] sqr)
        {
            var attrCount = data.Attributes.First().Count;

            for (var i = 0; i < attrCount; i++)
            {
                sqr[i] = data.Attributes.Sum(row => Math.Pow(row[i], 2));
            }

            sqr[attrCount] = data.ObjectClass.Sum(row => Math.Pow(row, 2));
        }

        private static void CalculateMul(Data data, double[] mul)
        {
            var attrCount = data.Attributes.First().Count;

            for (var attr = 0; attr < attrCount; attr++)
            {
                for (var row = 0; row < data.Attributes.Count; row++)
                {
                    mul[attr] += data.Attributes[row][attr] * data.ObjectClass[row];
                }
            }
        }
    }
}