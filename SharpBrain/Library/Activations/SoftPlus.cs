using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class SoftPlus : Function
    {
        public SoftPlus()
            : base("SoftPlus")
        { }

        public override double Compute(double aInput)
        {
            return Math.Log(1 + Math.Exp(aInput));
        }

        public override double Derivate(double aInput)
        {
            double eP = Math.Exp(aInput);
            return (eP / (1 + eP));
        }
    }
}
