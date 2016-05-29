using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class Sinusoid : Function
    {
        public Sinusoid()
            : base("Sinusoid")
        { }

        public override double Compute(double aInput)
        {
            return Math.Sin(aInput);
        }

        public override double Derivate(double aInput)
        {
            return Math.Cos(aInput);
        }
    }
}
