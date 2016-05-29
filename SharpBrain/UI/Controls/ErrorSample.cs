using System;

namespace SharpBrain.UI.Controls
{
    public class ErrorSample
    {
        public int Iteration { get; set; }
        public double Error { get; set; }

        static int _iterationCount = 0;
        public static ErrorSample Generate(double aError)
        {
            return new ErrorSample
            {
                Iteration = _iterationCount++,
                Error = aError
            };
        }
    }
}
