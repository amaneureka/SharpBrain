using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpBrain.Library.Activations;

namespace SharpBrain.Library
{
    public class MLP
    {
        public int InputPorts
        {
            get;
            private set;
        }

        public int OutputPorts
        {
            get;
            private set;
        }

        List<Neuron[]> mNetwork;

        public MLP(params int[] aLayers)
        {
            InputPorts = aLayers[0];
            OutputPorts = aLayers[aLayers.Length - 1];

            Setup(aLayers);
        }

        private void Setup(int[] aLayers)
        {
            var xLayers = aLayers;
            
            //Initialize
            mNetwork = new List<Neuron[]>(xLayers.Length);

            var activationFunction = new Sigmoid();

            foreach (var xCount in xLayers)
            {
                var neurons = new Neuron[xCount];
                for (int i = 0; i < xCount; i++)
                    neurons[i] = new Neuron(activationFunction);
                mNetwork.Add(neurons);
            }

            //Create fully connected network
            Neuron[] neuronLayer = null;
            //MLP Input - > Hidden -> MLP Output
            for (int i = 0; i < xLayers.Length - 1; i++)
            {
                neuronLayer = mNetwork[i];
                var NextLayer = mNetwork[i + 1];
                foreach (var neuron in neuronLayer)
                {
                    foreach (var connectedNeuron in NextLayer)
                        neuron.Connect(connectedNeuron);
                }
            }
        }

        public double[] Activate(double[] aInputs)
        {
            var InputLayer = mNetwork[0];
            for (int i = 0; i < InputPorts; i++)
                InputLayer[i].Feed(null, aInputs[i]);
            
            var result = new double[OutputPorts];
            var mOutputs = mNetwork[mNetwork.Count - 1];
            for (int i = 0; i < result.Length; i++)
                result[i] = mOutputs[i].Output;

            return result;
        }

        public double BackPropagate(double[] aOutputs)
        {
            double error = 0;
            var mOutputs = mNetwork[mNetwork.Count - 1];
            for (int i = 0; i < OutputPorts; i++)
            {
                double value = aOutputs[i] - mOutputs[i].Output;
                error += Math.Abs(value);
                mOutputs[i].Propagate(null, value);
            }
            
            return error;
        }
    }
}
