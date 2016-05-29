using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using SharpBrain.Library.Activations;

namespace SharpBrain.Library
{
    public enum Activation : int
    {
        Gaussian=0,
        Sigmoid=1,
        Sinusoid=2,
        SoftPlus=3,
        Softsign=4,
        TanH=5,
        NONE=-1
    };

    public class Helper
    {
        static readonly Gaussian mGaussian = new Gaussian();
        static readonly Sigmoid mSigmoid = new Sigmoid();
        static readonly Sinusoid mSinusoid = new Sinusoid();
        static readonly SoftPlus mSoftPlus = new SoftPlus();
        static readonly Softsign mSoftsign = new Softsign();
        static readonly TanH mTanH = new TanH();

        static List<string> mActivationFunctions;

        static readonly global::System.Random mRandom = new global::System.Random();
        
        public static double GetRandomDouble()
        {
            //'Coz, I don't want to waste time in intializing a new Random class and
            //allocating memory
            return (mRandom.NextDouble() * 0.2 - 1);
        }

        public static Function GetActivationFunction(Activation aActivationFunction)
        {
            switch(aActivationFunction)
            {
                case Activation.Gaussian:
                    return mGaussian;
                case Activation.Sigmoid:
                    return mSigmoid;
                case Activation.Sinusoid:
                    return mSinusoid;
                case Activation.SoftPlus:
                    return mSoftPlus;
                case Activation.Softsign:
                    return mSoftsign;
                case Activation.TanH:
                    return mTanH;
            }
            return mTanH;//fallback
        }

        public static Activation GetActivationFunctionInt(Function aActivationFunction)
        {
            if (aActivationFunction is Gaussian)
                return Activation.Gaussian;
            else if (aActivationFunction is Sigmoid)
                return Activation.Sigmoid;
            else if (aActivationFunction is Sinusoid)
                return Activation.Sinusoid;
            else if (aActivationFunction is SoftPlus)
                return Activation.SoftPlus;
            else if (aActivationFunction is Softsign)
                return Activation.Softsign;
            else if (aActivationFunction is TanH)
                return Activation.TanH;
            return Activation.TanH;//fallback
        }

        public static List<string> GetSupportedFunctions()
        {
            if (mActivationFunctions == null)
            {
                mActivationFunctions = new List<string>();
                mActivationFunctions.Add("Gaussian");
                mActivationFunctions.Add("Sigmoid");
                mActivationFunctions.Add("Sinusoid");
                mActivationFunctions.Add("SoftPlus");
                mActivationFunctions.Add("Softsign");
                mActivationFunctions.Add("TanH");
            }
            return mActivationFunctions;
        }
    }
}
