using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class StepActivation : IActivation
    {
        private double O;

        public StepActivation(double O)
        {
            this.O = O;
        }

        public double F(double NET)
        {
            return NET < O ? 0 : 1;
        }
    }
}
