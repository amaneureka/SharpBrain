using System.ComponentModel;

namespace SharpBrain.UI
{
    public abstract class PanelObject : INotifyPropertyChanged
    {
        string mName;
        string mKey;
        bool mIsFloating;
        
        public string Name
        {
            get { return mName; }
            set
            {
                mName = value;
                OnPropertyChanged("Name");
            }
        }

        public string Key
        {
            get { return mKey; }
            set
            {
                mKey = value.Replace(' ', '_');
                OnPropertyChanged("Key");
            }
        }
        
        public bool IsFloating
        {
            get { return mIsFloating; }
            set
            {
                mIsFloating = value;
                OnPropertyChanged("IsFloating");
            }
        }

        public abstract double X { get; set; }

        public abstract double Y { get; set; }


        public PanelObject(string aName)
        {
            Name = aName;
            Key = aName;
            IsFloating = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
