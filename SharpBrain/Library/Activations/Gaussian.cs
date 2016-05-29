using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    public class Gaussian : Function
    {
        public Gaussian()
            : base("Gaussian")
        { }

        public override double Compute(double aInput)
        {
            return (1 / Math.Exp(Math.Pow(aInput, 2)));
        }

        public override double Derivate(double aInput)
        {
            double y = Compute(aInput);
            return (-2 * aInput * y);
        }
    }
}
