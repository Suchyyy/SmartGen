using System.Windows;
using System.Windows.Controls;

namespace SmartGen.UserControls
{
    public partial class LayerControl : UserControl
    {
        public static readonly DependencyProperty LayerNoProperty = DependencyProperty.Register(
            "LayerNo", typeof(int), typeof(LayerControl), new FrameworkPropertyMetadata(0));

        public static readonly DependencyProperty SizeProperty = DependencyProperty.Register(
            "Size", typeof(int), typeof(LayerControl), new FrameworkPropertyMetadata(0));


        public int LayerNo
        {
            get => (int) GetValue(LayerNoProperty);
            set => SetValue(LayerNoProperty, value);
        }

        public int Size
        {
            get => (int) GetValue(SizeProperty);
            set => SetValue(SizeProperty, value);
        }

        public LayerControl()
        {
            InitializeComponent();
        }
    }
}