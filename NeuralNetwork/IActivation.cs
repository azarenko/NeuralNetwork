using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public delegate double ActivationFunction(double NET);

    interface IActivation
    {
        double F(double NET);
    }
}
