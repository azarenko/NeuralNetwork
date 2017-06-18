using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class FermiActivation : IActivation
    {
        public double F(double NET)
        {
            return 1.0 / (1.0 + Math.Exp(-NET));
        }
    }
}
