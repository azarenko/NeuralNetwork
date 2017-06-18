using NeuralNetwork;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Network net = new Network(2, 1, 1, 2, new FermiActivation().F);

            net.SetInput(new double[] { 1, 0 });
            double[] outPut = net.GetOutput();

            int i = outPut.Length;
        }
    }
}
