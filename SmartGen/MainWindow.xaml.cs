using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Microsoft.Win32;
using SmartGen.Mapper;
using SmartGen.Model;
using SmartGen.Properties;
using SmartGen.Types;

namespace SmartGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public ObservableCollection<LayerModel> Layers { get; set; }

        private TableData DataSet { get; set; }

        public MainWindow()
        {
            Layers = new ObservableCollection<LayerModel>();

            InitializeComponent();
            InitializeNeuralNetworkSettings();
            InitializeGeneticAlgorithmSettings();
        }

        #region Settings

        private void InitializeNeuralNetworkSettings()
        {
            Layers.Clear();

            UpDownInputLayerSize.Value = Settings.Default.InputLayerSize;
            UpDownOutputLayerSize.Value = Settings.Default.OutputLayerSize;

            for (var i = 0; i < Settings.Default.HiddenLayers.Length; i++)
            {
                Layers.Add(new LayerModel {LayerNo = i + 1, Size = Settings.Default.HiddenLayers[i]});
            }

            RangeSliderWeight.LowerValue = Settings.Default.MinWeight;
            RangeSliderWeight.UpperValue = Settings.Default.MaxWeight;

            UpDownBias.Value = Settings.Default.Bias;
            ComboBoxActivationFunction.SelectedItem = Settings.Default.ActivationFunction;
        }

        private void ButtonSaveNetworkParams_OnClick(object sender, RoutedEventArgs e)
        {
            if (UpDownInputLayerSize.Value.HasValue)
                Settings.Default.InputLayerSize = (int) UpDownInputLayerSize.Value.Value;

            if (UpDownOutputLayerSize.Value.HasValue)
                Settings.Default.OutputLayerSize = (int) UpDownOutputLayerSize.Value.Value;

            Settings.Default.HiddenLayers = Layers.Select(layer => layer.Size).ToArray();

            Settings.Default.MinWeight = RangeSliderWeight.LowerValue;
            Settings.Default.MaxWeight = RangeSliderWeight.UpperValue;

            if (UpDownBias.Value.HasValue)
                Settings.Default.Bias = UpDownBias.Value.Value;

            Settings.Default.ActivationFunction = (ActivationFunctionType) ComboBoxActivationFunction.SelectedItem;

            Settings.Default.Save();
        }

        private void ButtonRestoreNetworkParams_OnClick(object sender, RoutedEventArgs e)
        {
            Layers.Clear();

            UpDownInputLayerSize.Value = Defaults.Default.InputLayerSize;
            UpDownOutputLayerSize.Value = Defaults.Default.OutputLayerSize;

            for (var i = 0; i < Defaults.Default.HiddenLayers.Length; i++)
            {
                Layers.Add(new LayerModel {LayerNo = i + 1, Size = Defaults.Default.HiddenLayers[i]});
            }

            RangeSliderWeight.LowerValue = Defaults.Default.MinWeight;
            RangeSliderWeight.UpperValue = Defaults.Default.MaxWeight;

            UpDownBias.Value = Defaults.Default.Bias;
            ComboBoxActivationFunction.SelectedItem = Defaults.Default.ActivationFunction;
        }

        private void InitializeGeneticAlgorithmSettings()
        {
            ComboBoxSelection.SelectedItem = Settings.Default.Selection;
            ComboBoxCrossover.SelectedItem = Settings.Default.Crossover;
            ComboBoxMutation.SelectedItem = Settings.Default.Mutation;

            UpDownSelectionSize.Value = Settings.Default.SelectionSize;
            UpDownCrossoverProbability.Value = Settings.Default.CrossoverProbability;
            UpDownMutationProbability.Value = Settings.Default.MutationProbability;

            UpDownMaxIterations.Value = Settings.Default.MaxGenerationCount;
            UpDownPopulationSize.Value = Settings.Default.PopulationSize;
            UpDownErrorTolerance.Value = Settings.Default.ErrorTolerance;
        }

        private void ButtonSaveGenParams_OnClick(object sender, RoutedEventArgs e)
        {
            Settings.Default.Selection = (SelectionType) ComboBoxSelection.SelectedItem;
            Settings.Default.Crossover = (CrossoverType) ComboBoxCrossover.SelectedItem;
            Settings.Default.Mutation = (MutationType) ComboBoxMutation.SelectedItem;

            if (UpDownSelectionSize.Value.HasValue)
                Settings.Default.SelectionSize = (int) UpDownSelectionSize.Value.Value;

            if (UpDownCrossoverProbability.Value.HasValue)
                Settings.Default.CrossoverProbability = UpDownCrossoverProbability.Value.Value;

            if (UpDownMutationProbability.Value.HasValue)
                Settings.Default.MutationProbability = UpDownMutationProbability.Value.Value;


            if (UpDownPopulationSize.Value.HasValue)
                Settings.Default.PopulationSize = (int) UpDownPopulationSize.Value.Value;

            if (UpDownMaxIterations.Value.HasValue)
                Settings.Default.MaxGenerationCount = (int) UpDownMaxIterations.Value.Value;

            if (UpDownErrorTolerance.Value.HasValue)
                Settings.Default.ErrorTolerance = UpDownErrorTolerance.Value.Value;

            Settings.Default.Save();
        }

        private void ButtonRestoreGenParams_OnClick(object sender, RoutedEventArgs e)
        {
            ComboBoxSelection.SelectedItem = Defaults.Default.Selection;
            ComboBoxCrossover.SelectedItem = Defaults.Default.Crossover;
            ComboBoxMutation.SelectedItem = Defaults.Default.Mutation;

            UpDownSelectionSize.Value = Defaults.Default.SelectionSize;
            UpDownCrossoverProbability.Value = Defaults.Default.CrossoverProbability;
            UpDownMutationProbability.Value = Defaults.Default.MutationProbability;

            UpDownMaxIterations.Value = Defaults.Default.MaxGenerationCount;
            UpDownPopulationSize.Value = Defaults.Default.PopulationSize;
            UpDownErrorTolerance.Value = Defaults.Default.ErrorTolerance;
        }

        #endregion

        #region Events

        private void ButtonTraining_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabTraining;

        private void ButtonNeuralNetwork_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabNeuralNetwork;

        private void ButtonGeneticAlgorithm_Click(object sender, RoutedEventArgs e) =>
            TabControl.SelectedItem = TabGeneticAlgorithm;

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

        private void ButtonAddHiddenLayer_OnClick(object sender, RoutedEventArgs e)
        {
            Layers.Add(new LayerModel {LayerNo = Layers.Count + 1});
        }

        private void ButtonRemoveHiddenLayer_OnClick(object sender, RoutedEventArgs e)
        {
            Layers.Remove(Layers.Last());
        }

        #endregion
    }
}