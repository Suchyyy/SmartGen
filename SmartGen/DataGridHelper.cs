using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using SmartGen.Model;

namespace SmartGen
{
    public static class DataGridHelper
    {
        public static readonly DependencyProperty TableDataProperty = DependencyProperty.RegisterAttached(
            "TableData",
            typeof(TableData),
            typeof(DataGridHelper),
            new PropertyMetadata(null, TableDataChanged)
        );

        private static void TableDataChanged(DependencyObject o, DependencyPropertyChangedEventArgs e)
        {
            var dataGrid = (DataGrid) o;
            var tableData = (TableData) e.NewValue;

            if (dataGrid == null || tableData == null) return;

            dataGrid.Columns.Clear();

            for (var i = 0; i < tableData.ColumnHeaders.Count; i++)
            {
                var tableDataColumnHeader = tableData.ColumnHeaders[i];
                var column = new DataGridTextColumn
                {
                    Binding = new Binding($"Cells[{i}]"),
                    Header = tableData.ColumnHeaders[i]
                };

                dataGrid.Columns.Add(column);
            }

            dataGrid.ItemsSource = tableData.Rows;
        }

        public static TableData GetTableData(DependencyObject o)
        {
            return (TableData) o.GetValue(TableDataProperty);
        }

        public static void SetTableData(DependencyObject o, TableData data)
        {
            o.SetValue(TableDataProperty, data);
        }
    }
}