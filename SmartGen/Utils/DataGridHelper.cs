using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using SmartGen.Model;
using DataGridTextColumn = MaterialDesignThemes.Wpf.DataGridTextColumn;

namespace SmartGen.Utils
{
    public static class DataGridHelper
    {
        public static void SetTableData(DataGrid dataGrid, TableData tableData)
        {
            if (dataGrid == null || tableData == null) return;

            dataGrid.Columns.Clear();

            for (var i = 0; i < tableData.ColumnHeaders.Count; i++)
            {
                var tableDataColumnHeader = tableData.ColumnHeaders[i];
                var column = new DataGridTextColumn
                {
                    Binding = new Binding($"Cells[{i}]"),
                    Header = tableData.ColumnHeaders[i],
                };

                if (tableDataColumnHeader.Contains("class") || tableDataColumnHeader.Contains("prediction"))
                {
                    var baseStyle = (Style) Application.Current.Resources["MaterialDesignDataGridColumnHeader"];
                    var style = new Style(typeof(DataGridColumnHeader), baseStyle);
                    style.Setters.Add(new Setter(TextBlock.FontWeightProperty, FontWeights.Bold));
                    column.HeaderStyle = style;
                    column.FontWeight = FontWeights.Bold;
                }

                dataGrid.Columns.Add(column);
            }

            dataGrid.ItemsSource = tableData.Rows;
        }
    }
}