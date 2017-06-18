using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class Neuron
    {
        private double[] x;
        private double[] w;
        private double y;
        private ActivationFunction F;
        private int inputCount;

        public Neuron(int inputCount, ActivationFunction F)
        {
            this.inputCount = inputCount;
            this.F = F;
            this.x = new double[inputCount];
            this.w = new double[inputCount];

            Random r = new Random();

            for(int i = 0; i < inputCount; i++)
            {
                this.w[i] = r.NextDouble();
            }
        }

        public double Exec()
        {
            double NET = 0;

            for(int i = 0; i < inputCount; i++)
            {
                NET += w[i] * x[i];
            }

            y = F(NET);

            return y;
        }

        public double Y
        {
            get { return y; }
        }

        public double[] X
        {
            get
            {
                return this.x;
            }
        }

        public double[] W
        {
            get
            {
                return this.w;
            }
        }
    }
}
