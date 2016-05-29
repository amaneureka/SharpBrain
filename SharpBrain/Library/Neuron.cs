using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpBrain.Library.Activations;

namespace SharpBrain.Library
{
    public class Neuron
    {
        /// <summary>
        /// Get neuron id of this neuron
        /// </summary>
        public readonly uint NeuronID;

        /// <summary>
        /// Function which optimizes the output of neuron
        /// </summary>
        public Function ActivationFunction;

        /// <summary>
        /// Bias to the weight sum
        /// </summary>
        public double Bias = -1;

        /// <summary>
        /// speed of learning process should be low but not very low
        /// </summary>
        public double LearningRate = 0.3;

        /// <summary>
        /// Output at preceptron
        /// </summary>
        public double Output
        {
            get;
            private set;
        }

        /// <summary>
        /// Each Parent Neuron of this Neuron class must be associated
        /// with a weight, which is used to decide
        /// whether this neuron should fire or not
        /// </summary>
        Dictionary<Neuron, double> mWeights;

        /// <summary>
        /// It will store values at the receptor of the neuron
        /// </summary>
        Dictionary<Neuron, double> mReceptors;

        /// <summary>
        /// Holds the propagation error of each child neuron
        /// </summary>
        Dictionary<Neuron, double> mPropagationError;

        /// <summary>
        /// injected error for output neuron
        /// </summary>
        double InjectedError;
        
        /// <summary>
        /// this is used by Input neurons only
        /// </summary>
        double InjectedValue;

        /// <summary>
        /// ∑(xw)
        /// </summary>
        double WeightedSum;

        /// <summary>
        /// parent -> this (directed edge)
        /// </summary>
        List<Neuron> mParentNeurons;

        /// <summary>
        /// this -> child (directed edge)
        /// </summary>
        List<Neuron> mChildNeurons;

        public int ChildrenCount
        { get { return mChildNeurons.Count; } }

        public int ParentCount
        { get { return mParentNeurons.Count; } }

        static uint mNeuronCounter = 0;

        public Neuron(Activation aActivationFunction)
            : this(Helper.GetActivationFunction(aActivationFunction))
        { }

        public Neuron(Function aActivationFunction)
        {
            ActivationFunction = aActivationFunction;

            InjectedValue = 0.0;
            InjectedError = 0.0;
            NeuronID = ++mNeuronCounter;

            mWeights = new Dictionary<Neuron, double>();
            mReceptors = new Dictionary<Neuron, double>();
            mPropagationError = new Dictionary<Neuron, double>();

            mChildNeurons = new List<Neuron>();
            mParentNeurons = new List<Neuron>();
        }

        protected void AddParent(Neuron aParent)
        {
            mParentNeurons.Add(aParent);

            /*
             * Once we add a parent Neuron to a neuron
             * It should create a Random weight
             * for this and continue it's work
             */
            double currentWeight = Helper.GetRandomDouble();
            mWeights.Add(aParent, currentWeight);
        }

        protected void RemoveParent(Neuron aParent)
        {
            mParentNeurons.Remove(aParent);
            mWeights.Remove(aParent);
            mReceptors.Remove(aParent);
        }

        /// <summary>
        /// Create a directed edge between this -> aChildren
        /// </summary>
        /// <param name="aChildren"></param>
        public void Connect(Neuron aChildren)
        {
            mChildNeurons.Add(aChildren);
            aChildren.AddParent(this);
        }

        /// <summary>
        /// Deleted directed edge between this -> aChildren
        /// </summary>
        /// <param name="aChildren"></param>
        public void Disconnect(Neuron aChildren)
        {
            mChildNeurons.Remove(aChildren);
            aChildren.RemoveParent(this);
        }
        
        public void Feed(Neuron aParent, double aValue)
        {
            if (aParent == null)
            {
                //Injected Value -- Input Neuron
                InjectedValue = aValue;
                Refresh(false);
                return;
            }

            //TODO: Check if it is parent or not

            if (mReceptors.ContainsKey(aParent))
                mReceptors.Clear();

            mReceptors.Add(aParent, aValue);
            if (mReceptors.Count == mParentNeurons.Count)
                Refresh(false);
        }

        public void Propagate(Neuron aChild, double aError)
        {
            if (aChild == null)
            {
                InjectedError = aError;
                Refresh(true);
                return;
            }

            //if we feeded again then neuron my baby please forget your past and concentrate on present
            //kuch na rakha purani baato ko yaad karkar
            //nayi jindgi jiyo :D
            //i know it's very difficult to forget past but live in present my baby neuron :D
            if (mPropagationError.ContainsKey(aChild))
                mPropagationError.Clear();

            mPropagationError.Add(aChild, aError);
            if (mPropagationError.Count == mChildNeurons.Count)
                Refresh(true);
        }

        private void Refresh(bool IsBackPropogating)
        {
            if (IsBackPropogating)
            {
                double propagationError = InjectedError;
                foreach (var child in mPropagationError)
                    propagationError += child.Value;

                //  f'(∑xw) * (∑xδ) * η
                double output = ActivationFunction.Derivate(WeightedSum) * propagationError * LearningRate;
                
                //update bias
                Bias = Bias + output * 1.0;

                //gradient descent
                //update weights
                var xNewWeights = new Dictionary<Neuron, double>();
                foreach (var xNeuron in mWeights.Keys)
                {
                    double weight = mWeights[xNeuron];
                    //w = w + η * f'(∑xw) * ∑xδ * x
                    weight = weight + output * mReceptors[xNeuron];
                    //update
                    xNewWeights.Add(xNeuron, weight);
                }
                
                //propagate this error to all parents
                foreach (var xParent in mParentNeurons)
                    //propagate f'(∑xw) * ∑xδ * w
                    xParent.Propagate(this, output * mWeights[xParent]);

                mWeights = xNewWeights;
            }
            else
            {
                //  f(∑xw)
                double xOutput = Bias + InjectedValue;
                foreach (var xNeuron in mWeights)
                    xOutput += mReceptors[xNeuron.Key] * xNeuron.Value;

                WeightedSum = xOutput;
                xOutput = ActivationFunction.Compute(xOutput);

                //reset injected value
                InjectedValue = 0.0;

                //Update Value
                Output = xOutput;

                //propogate this output to all childrens
                foreach (var xChildren in mChildNeurons)
                    xChildren.Feed(this, xOutput);
            }
        }
    }
}
