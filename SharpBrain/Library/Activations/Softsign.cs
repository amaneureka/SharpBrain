using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class Softsign : Function
    {
        public Softsign()
            : base("Softsign")
        { }

        public override double Compute(double aInput)
        {
            return (aInput / (1 + Math.Abs(aInput)));
        }

        public override double Derivate(double aInput)
        {
            double xD = 1 / (1 + Math.Abs(aInput));
            return (xD * xD);
        }
    }
}
