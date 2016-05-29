using System;

using MahApps.Metro.Controls.Dialogs;

namespace SharpBrain.UI
{
    public class Connection : PanelObject
    {
        Neuron mStart;
        Neuron mEnd;

        #region Properties
        public Neuron Start
        {
            get { return mStart; }
            set
            {
                mStart = value;
                OnPropertyChanged("Start");
            }
        }

        public Neuron End
        {
            get { return mEnd; }
            set
            {
                mEnd = value;
                OnPropertyChanged("End");
            }
        }

        public override double X
        {
            get { return 0; }
            set { }
        }

        public override double Y
        {
            get { return 0; }
            set { }
        }
        #endregion

        static int _count = 0;
        public Connection()
            : base("Link " + (++_count))
        {

        }

        public void Glue()
        {
            mStart.NeuronModel.Connect(mEnd.NeuronModel);
        }
    }
}
