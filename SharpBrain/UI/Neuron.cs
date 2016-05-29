using System;

using SharpBrain.Library;

namespace SharpBrain.UI
{
    public class Neuron : PanelObject
    {
        double mX;
        double mY;

        bool mIsHighlighted;
        Library.Neuron mReceptor;

        #region Property
        public int ActivationFunction
        {
            get { return (int)Helper.GetActivationFunctionInt(mReceptor.ActivationFunction); }
            set
            {
                mReceptor.ActivationFunction = Helper.GetActivationFunction((Activation)value);
                OnPropertyChanged("ActivationFunction");
            }
        }

        public double Bias
        {
            get { return mReceptor.Bias; }
            set
            {
                mReceptor.Bias = value;
                OnPropertyChanged("Bias");
            }
        }

        public override double X
        {
            get { return mX; }
            set
            {
                mX = (Math.Round(value / 50.0)) * 50;
                OnPropertyChanged("X");
            }
        }
        
        public override double Y
        {
            get { return mY; }
            set
            {
                mY = (Math.Round(value / 50.0)) * 50;
                OnPropertyChanged("Y");
            }
        }

        public Library.Neuron NeuronModel
        {
            get { return mReceptor; }
        }

        public bool IsHighlighted
        {
            get { return mIsHighlighted; }
            set
            {
                mIsHighlighted = value;
                OnPropertyChanged("IsHighlighted");
            }
        }
        #endregion

        static int _count = 0;
        public Neuron()
            :base("neuron-" + (++_count))
        {
            mReceptor = new Library.Neuron(Activation.TanH);
        }
    }
}
