using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Input;
using Microsoft.Win32;
using SmartGen.Mapper;
using SmartGen.Model;

namespace SmartGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        private static readonly Regex NumberRegex = new Regex("[^0-9]+");

        private TableData DataSet { get; set; }

        public MainWindow()
        {
            InitializeComponent();
        }

        #region Events

        #region TitleBar

        private void ButtonClose_OnClick(object sender, RoutedEventArgs e) => Application.Current.Shutdown();

        private void MinimizeButton_OnClick(object sender, RoutedEventArgs e) => WindowState = WindowState.Minimized;

        private void TitleBar_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left) DragMove();
        }

        #endregion

        private void ButtonTraining_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabTraining;

        private void ButtonNeuralNetwork_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabNeuralNetwork;

        private void ButtonGeneticAlgorithm_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabGeneticAlgorithm;

        #region DataSet

        private void ButtonDataSet_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabDataSet;

        private void ButtonLoadData_Click(object sender, RoutedEventArgs e)
        {
            var openFileDialog = new OpenFileDialog
            {
                Filter = "csv file (*.csv)|*.csv",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true) return;

            var filename = openFileDialog.FileName;
            var data = CsvMapper.ReadDataFromFile(filename,
                TextBoxColumnSeparator.Text.ToCharArray(),
                Convert.ToInt32(UpDownClassCount.Value),
                CheckBoxDecimalSeparator.IsChecked != null && CheckBoxDecimalSeparator.IsChecked.Value ? '.' : ',',
                Convert.ToInt32(UpDownLineSkip.Value));

            DataSet = new TableData(data);

            DataGridHelper.SetTableData(GridDataSet, DataSet);
        }

        #endregion

        private void TextBoxNumberFilter_KeyPress(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumberRegex.IsMatch(e.Text);
        }

        #endregion
    }
}