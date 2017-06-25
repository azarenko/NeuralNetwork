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
            Network net = Network.Load("temp.net", new FermiActivation().F);

            net.SetInput(new double[] { 1, 0 });
            double[] outPut = net.GetOutput();
        }
    }
}
