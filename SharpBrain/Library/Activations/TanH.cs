using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class TanH : Function
    {
        public TanH()
            : base("TanH")
        { }

        public override double Compute(double aInput)
        {
            return Math.Tanh(aInput);
        }

        public override double Derivate(double aInput)
        {
            double y = Compute(aInput);
            return 1 - Math.Pow(y, 2);
        }
    }
}
