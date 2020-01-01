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
                NumberDecimalDigits = 3
            };

            for (var i = 1; i <= data.ObjectClass.First().Count; i++)
            {
                ColumnHeaders.Add("class " + i);
            }

            for (var i = 1; i <= data.Attributes.First().Count; i++)
            {
                ColumnHeaders.Add("attribute " + i);
            }

            for (var i = 0; i < data.Attributes.Count; i++)
            {
                var row = new TableDataRow(data.ObjectClass[i].Concat(data.Attributes[i])
                    .Select(d => d.ToString("N", format)).ToList());

                Rows.Add(row);
            }
        }
    }
}