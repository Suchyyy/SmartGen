using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using LiveCharts;
using LiveCharts.Defaults;
using Microsoft.Win32;
using MoreLinq;
using NeuralNetwork.ActivationFunction;
using SmartGen.Mapper;
using SmartGen.Model;
using SmartGen.Properties;
using SmartGen.Types;
using SmartGen.Utils;

namespace SmartGen
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public Func<double, string> Formatter { get; set; }
        public ChartValues<ObservablePoint> AlgorithmErrorValues { get; set; }
        public ChartValues<ObservablePoint> AlgorithmValidationValues { get; set; }

        public ChartValues<ObservablePoint> ActivationFunctionChartValues { get; set; }

        public IActivationFunction Function { get; set; }

        public ObservableCollection<LayerModel> Layers { get; set; }
        public CsvMapper Mapper { get; set; }

        public Data Data { get; set; }
        private SmartGenAlgorithm _algorithm;
        private Task _algorithmTask;
        private long _milliseconds;

        public MainWindow()
        {
            Formatter = d => d.ToString("0.###", CultureInfo.InvariantCulture);
            _milliseconds = DateTimeOffset.Now.ToUnixTimeMilliseconds();

            AlgorithmErrorValues = new ChartValues<ObservablePoint>();
            AlgorithmValidationValues = new ChartValues<ObservablePoint>();

            ActivationFunctionChartValues = new ChartValues<ObservablePoint>();

            for (var i = -10.0; i < 10.0; i += 0.25)
            {
                ActivationFunctionChartValues.Add(new ObservablePoint(i, 0.0));
            }

            Function = new TanHFunction();

            Layers = new ObservableCollection<LayerModel>();

            InitializeComponent();
            InitializeDataSetSettings();
            InitializeNeuralNetworkSettings();
            InitializeGeneticAlgorithmSettings();

            DataContext = this;
        }

        private void HandleAlgorithmResult(Task task)
        {
            ButtonStop.Dispatcher?.Invoke(() => ButtonStop.IsEnabled = false);

            var trainedNetwork = _algorithm.GetTrainedNeuralNetwork();
            var testingData = _algorithm.DataSet[DataType.Testing];

            var predictions = testingData.Attributes
                .Select(testingDataAttribute => trainedNetwork.GetResult(testingDataAttribute).ToList())
                .ToList();

            RectangleDataGridTest.Dispatcher?.Invoke(() => RectangleDataGridTest.Visibility = Visibility.Visible);
            DataGridTest.Dispatcher?.Invoke(() =>
            {
                DataGridHelper.SetTableData(DataGridTest, new TableData(predictions, testingData));
                DataGridTest.Visibility = Visibility.Visible;
            });

            _algorithm = null;

            ButtonStart.Dispatcher?.Invoke(() =>
            {
                ButtonStart.IsChecked = false;
                ButtonStart.IsEnabled = true;
            });
        }

        #region Events

        private void ButtonStop_OnClick(object sender, RoutedEventArgs e)
        {
            if (_algorithm == null) return;

            ((Button) sender).IsEnabled = false;

            ButtonStart.Dispatcher?.Invoke(() =>
            {
                ButtonStart.IsChecked = false;
                ButtonStart.IsEnabled = false;
            });

            _algorithm.Stop();
        }

        private void ButtonStartAlgorithm_Click(object sender, RoutedEventArgs e)
        {
            if (_algorithm != null)
            {
                var s = ((ToggleButton) sender).IsChecked;
                _algorithm.IsPaused = !s ?? !_algorithm.IsPaused;
                return;
            }

            RectangleDataGridTest.Dispatcher?.Invoke(() => RectangleDataGridTest.Visibility = Visibility.Collapsed);
            DataGridTest.Dispatcher?.Invoke(() =>
            {
                DataGridTest.Visibility = Visibility.Collapsed;
            });

            AlgorithmErrorValues.Clear();
            AlgorithmValidationValues.Clear();

            var neuralNetwork = SmartGenAlgorithm.CreateNeuralNetwork();
            var geneticAlgorithm = SmartGenAlgorithm.CreateGeneticAlgorithm(neuralNetwork.GetConnectionCount());

            if (Settings.Default.InputLayerSize < Data.Attributes.First().Count)
            {
                var correlation = Correlation.GetCorrelation(Data);
                Data = Data.RemoveLeastRelevantColumn(correlation, Settings.Default.InputLayerSize);
            }

            var dataSets = Data.SplitData(Settings.Default.TrainingRatio,
                Settings.Default.TestRatio,
                Settings.Default.ValidationRatio);

            _algorithm = new SmartGenAlgorithm(geneticAlgorithm, neuralNetwork)
            {
                DataSet = dataSets,
                ErrorTolerance = Settings.Default.ErrorTolerance,
                MaxIterations = Settings.Default.MaxGenerationCount
            };

            _algorithm.IterationEvent += (iteration, error, validationError) =>
            {
                var current = DateTimeOffset.Now.ToUnixTimeMilliseconds();
                if (current - _milliseconds < 500) return;
                _milliseconds = current;

                AlgorithmErrorValues.Add(new ObservablePoint(iteration, error));
                AlgorithmValidationValues.Add(new ObservablePoint(iteration, validationError));

                if (AlgorithmErrorValues.Count <= 11) return;

                AlgorithmErrorValues.RemoveAt(0);
                AlgorithmValidationValues.RemoveAt(0);
            };

            _algorithmTask = Task.Run(() => _algorithm.Run());
            _algorithmTask.ContinueWith(HandleAlgorithmResult);
            ButtonStop.IsEnabled = true;
        }

        private void TabControl_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var s = (TabControl) sender;
            if (s.SelectedIndex != 0) return;

            Application.Current.Dispatcher?.Invoke(() => Chart.UpdateLayout());
        }

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
            if (!UpDownLineSkip.Value.HasValue || !UpDownClassCount.Value.HasValue) return;

            Mapper = new CsvMapper
            {
                Normalized = true,
                Separator = TextBoxColumnSeparator.Text[0],
                DecimalSeparator =
                    CheckBoxDecimalSeparator.IsChecked != null && CheckBoxDecimalSeparator.IsChecked.Value ? '.' : ',',
                SkipRows = (int) UpDownLineSkip.Value.Value
            };

            var openFileDialog = new OpenFileDialog
            {
                Filter = "csv file (*.csv)|*.csv",
                Multiselect = false
            };

            if (openFileDialog.ShowDialog() != true) return;

            var filename = openFileDialog.FileName;
            Data = Mapper.ReadDataFromFile(filename, Convert.ToInt32(UpDownClassCount.Value.Value));

            DataGridHelper.SetTableData(DataGridSet, new TableData(Data));

            ButtonStart.IsEnabled = true;
        }

        private void ButtonAddHiddenLayer_OnClick(object sender, RoutedEventArgs e)
        {
            Layers.Add(new LayerModel {LayerNo = Layers.Count + 1});
        }

        private void ButtonRemoveHiddenLayer_OnClick(object sender, RoutedEventArgs e)
        {
            Layers.Remove(Layers.Last());
        }

        private void ComboBoxActivationFunction_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Function = ActivationFunctionExtension.GetFunction(
                (ActivationFunctionType) ((ComboBox) sender).SelectedItem);

            if (UpDownBias.Value == null) return;

            var bias = UpDownBias.Value.Value;
            ActivationFunctionChartValues.ForEach(point => point.Y = Math.Round(Function.GetValue(point.X + bias), 3));
        }

        private void UpDownBias_OnValueChanged(object sender, RoutedPropertyChangedEventArgs<double?> e)
        {
            if (!UpDownBias.Value.HasValue) return;

            if (UpDownT?.Value != null && Function is SigmoidFunction)
                Function = new SigmoidFunction(UpDownT.Value.Value);

            var bias = UpDownBias.Value.Value;
            ActivationFunctionChartValues.ForEach(point => point.Y = Math.Round(Function.GetValue(point.X + bias), 3));
        }

        #endregion

        #region Settings

        private void InitializeDataSetSettings()
        {
            UpDownLineSkip.Value = Settings.Default.LinesToSkip;
            UpDownClassCount.Value = Settings.Default.ClassesCount;

            TextBoxColumnSeparator.Text = Settings.Default.ColumnSeparator.ToString();

            CheckBoxDecimalSeparator.IsChecked = Settings.Default.DotAsDecimalSeparator;

            UpDownTrainingSetRatio.Value = Settings.Default.TrainingRatio;
            UpDownTestSetRatio.Value = Settings.Default.TestRatio;
            UpDownValidationSetRatio.Value = Settings.Default.ValidationRatio;
        }


        private void ButtonSaveDataSetParams_OnClick(object sender, RoutedEventArgs e)
        {
            if (UpDownLineSkip.Value.HasValue)
                Settings.Default.LinesToSkip = (int) UpDownLineSkip.Value.Value;

            if (UpDownClassCount.Value.HasValue)
                Settings.Default.ClassesCount = (int) UpDownClassCount.Value.Value;

            Settings.Default.ColumnSeparator = TextBoxColumnSeparator.Text[0];

            if (CheckBoxDecimalSeparator.IsChecked.HasValue)
                Settings.Default.DotAsDecimalSeparator = CheckBoxDecimalSeparator.IsChecked.Value;

            if (UpDownTrainingSetRatio.Value.HasValue)
                Settings.Default.TrainingRatio = (int) UpDownTrainingSetRatio.Value.Value;

            if (UpDownTestSetRatio.Value.HasValue)
                Settings.Default.TestRatio = (int) UpDownTestSetRatio.Value.Value;

            if (UpDownValidationSetRatio.Value.HasValue)
                Settings.Default.ValidationRatio = (int) UpDownValidationSetRatio.Value.Value;

            Settings.Default.Save();
        }

        private void ButtonRestoreDataSetParams_OnClick(object sender, RoutedEventArgs e)
        {
            UpDownLineSkip.Value = Defaults.Default.LinesToSkip;
            UpDownClassCount.Value = Defaults.Default.ClassesCount;

            TextBoxColumnSeparator.Text = Defaults.Default.ColumnSeparator.ToString();

            CheckBoxDecimalSeparator.IsChecked = Defaults.Default.DotAsDecimalSeparator;

            UpDownTrainingSetRatio.Value = Defaults.Default.TrainingRatio;
            UpDownTestSetRatio.Value = Defaults.Default.TestRatio;
            UpDownValidationSetRatio.Value = Defaults.Default.ValidationRatio;
        }

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
            UpDownT.Value = Settings.Default.T;
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

            if (UpDownT.Value.HasValue)
                Settings.Default.T = UpDownT.Value.Value;

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
            UpDownT.Value = Defaults.Default.T;
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
    }
}