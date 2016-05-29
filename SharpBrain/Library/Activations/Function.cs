using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpBrain.Library.Activations
{
    /// <summary>
    /// Gives defination to a Mathematical Function y = f(x)
    /// where x, y are of double type
    /// </summary>
    public abstract class Function
    {
        string mName;
        
        public Function(string aName)
        {
            mName = aName;
        }

        /// <summary>
        /// Return function(aInput)
        /// </summary>
        /// <param name="aInput"></param>
        /// <returns></returns>
        public abstract double Compute(double aInput);

        /// <summary>
        /// Return d(function(x))/dx at x = aInput
        /// </summary>
        /// <param name="aInput"></param>
        /// <returns></returns>
        public abstract double Derivate(double aInput);
    }
}
