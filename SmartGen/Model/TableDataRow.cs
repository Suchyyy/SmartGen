using System.Collections.Generic;

namespace SmartGen.Model
{
    public class TableDataRow
    {
        public List<string> Cells { get; }

        public TableDataRow(List<string> cells)
        {
            Cells = cells;
        }
    }
}