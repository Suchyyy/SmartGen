using System.ComponentModel;

namespace SmartGen.Model
{
    public class LayerModel : INotifyPropertyChanged
    {
        public int LayerNo { get; set; }
        public int Size { get; set; } = 10;
        public event PropertyChangedEventHandler PropertyChanged;

        protected void NotifyPropertyChanged(string info)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(info));
        }
    }
}