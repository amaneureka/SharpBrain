using System;
using System.ComponentModel;

namespace SharpBrain.UI.Controls
{
    public partial class ErrorChart : INotifyPropertyChanged
    {
        RingBuffer<ErrorSample> _errorStat;

        public event PropertyChangedEventHandler PropertyChanged;

        public RingBuffer<ErrorSample> ErrorStat
        {
            get { return _errorStat; }
            set
            {
                _errorStat = value;
                OnPropertyChanged("ErrorStat");
            }
        }

        public ErrorChart()
        {
            InitializeComponent();
            ErrorStat = new RingBuffer<ErrorSample>(60);
            DataContext = this;
        }
                    
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
