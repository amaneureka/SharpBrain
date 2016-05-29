using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class Sigmoid : Function
    {
        public Sigmoid()
            : base("Sigmoid")
        { }
        
        public override double Compute(double aInput)
        {
            double eN = 1 / Math.Exp(aInput);
            return (1 /(1 + eN));
        }

        public override double Derivate(double aInput)
        {
            double y = Compute(aInput);
            return (y * (1 - y));
        }
    }
}
