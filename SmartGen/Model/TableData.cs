using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace SmartGen.Model
{
    public class TableData
    {
        public List<string> ColumnHeaders { get; } = new List<string>();

        public List<TableDataRow> Rows { get; } = new List<TableDataRow>();

        public TableData(Data data)
        {
            var format = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

            ColumnHeaders.Add("class");

            for (var i = 1; i <= data.Attributes.First().Count; i++)
            {
                ColumnHeaders.Add("attribute " + i);
            }

            for (var i = 0; i < data.Attributes.Count; i++)
            {
                var p = new List<double>(new[] {data.ObjectClass[i]});
                var row = new TableDataRow(p.Concat(data.Attributes[i])
                    .Select(d => d.ToString("N", format)).ToList());

                Rows.Add(row);
            }
        }

        public TableData(List<List<double>> predictions, Data data)
        {
            var format = new NumberFormatInfo
            {
                NumberDecimalSeparator = ".",
                NumberDecimalDigits = 2
            };

            ColumnHeaders.Add("prediction");
            ColumnHeaders.Add("class");

            for (var i = 1; i <= data.Attributes.First().Count; i++)
            {
                ColumnHeaders.Add("attribute " + i);
            }

            for (var i = 0; i < data.Attributes.Count; i++)
            {
                var p = new List<double>(new[] {predictions[i][0], data.ObjectClass[i]});
                var row = new TableDataRow(p.Concat(data.Attributes[i])
                    .Select(d => d.ToString("N", format)).ToList());

                Rows.Add(row);
            }
        }
    }
}