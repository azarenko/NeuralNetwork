using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NeuralNetwork
{
    public class Network
    {
        private int inputCount;
        private int outputCount;
        private int hidelayersCount;
        private int neuronsInLayerCount;
        private double error;

        private List<Neuron> ALayer = new List<Neuron>();
        private List<List<Neuron>> HideLayers = new List<List<Neuron>>();
        private List<Neuron> OutPutLayer = new List<Neuron>();

        public Network(int InputCount, int OutPutCount, int HideLayersCount, int NeuronsInLayerCount, ActivationFunction F)
        {
            this.inputCount = InputCount;
            this.outputCount = OutPutCount;
            this.hidelayersCount = HideLayersCount;
            this.neuronsInLayerCount = NeuronsInLayerCount;

            /// Fill A layer
            for (int i = 0; i < neuronsInLayerCount; i++)
            {
                ALayer.Add(new Neuron(inputCount, F));
            }

            /// Fill Hiden layers
            for (int i = 0; i < hidelayersCount; i++)
            {
                HideLayers.Add(new List<Neuron>());

                for (int j = 0; j < neuronsInLayerCount; j++)
                {
                    HideLayers[i].Add(new Neuron(neuronsInLayerCount, F));
                }
            }

            /// Fill A layer
            for (int i = 0; i < outputCount; i++)
            {
                OutPutLayer.Add(new Neuron(neuronsInLayerCount, F));
            }
        }

        public void SetInput(double[] InputVector)
        {
            for (int i = 0; i < neuronsInLayerCount; i++)
            {
                for (int j = 0; j < neuronsInLayerCount; j++)
                {
                    ALayer[i].X[j] = InputVector[j];
                }
            }
        }

        private void Exec()
        {
            int i, j, k;

            // Exec A layer
            for (i = 0; i < neuronsInLayerCount; i++)
            {
                this.ALayer[i].Exec();
            }

            // Fill first hiden layer
            for (i = 0; i < neuronsInLayerCount; i++)
            {
                for (j = 0; j < neuronsInLayerCount; j++)
                {
                    this.HideLayers[0][i].X[j] = this.ALayer[j].Y;
                }
            }

            // Exec first hide layer
            for (i = 0; i < neuronsInLayerCount; i++)
            {
                this.HideLayers[0][i].Exec();
            }

            // Combine hide layer
            for (i = 1; i < hidelayersCount; i++)
            {
                for (j = 0; j < neuronsInLayerCount; j++)
                {
                    for (k = 0; k < neuronsInLayerCount; k++)
                    {
                        this.HideLayers[i][j].X[k] = this.HideLayers[i - 1][k].Y;
                    }

                    this.HideLayers[i][j].Exec();
                }
            }

            // Combine out layer
            for (j = 0; j < this.outputCount; j++)
            {
                for (k = 0; k < neuronsInLayerCount; k++)
                {
                    this.OutPutLayer[j].X[k] = this.HideLayers[hidelayersCount - 1][k].Y;
                }

                this.OutPutLayer[j].Exec();
            }
        }

        private void TeachIteration(double[] DataSet, double[] Result)
        {

        }

        public double Teach(double[][] DataSet, double[][] Results, double permissibleError)
        {
            do
            {
                this.error = 0.0;

                for(int i = 0; i < DataSet.Length; i++)
                {
                    TeachIteration(DataSet[i], Results[i]);

                    SetInput(DataSet[i]);
                    double[] outPut = GetOutput();

                    for(int j = 0; j < Results[i].Length; j++)
                    {
                        this.error += outPut[j] / Results[i][j];
                    }
                }

                this.error /= (Results.Length * Results[0].Length);
            }
            while (this.error > permissibleError);

            return this.error;
        }

        public void Save(string fileName)
        {
            using (StreamWriter sw = new StreamWriter(fileName, false))
            {
                sw.WriteLine(string.Format("inputCount={0}", inputCount));
                sw.WriteLine(string.Format("outputCount={0}", outputCount));
                sw.WriteLine(string.Format("hidelayersCount={0}", hidelayersCount));
                sw.WriteLine(string.Format("neuronsInLayerCount={0}", neuronsInLayerCount));
                sw.WriteLine(string.Format("error={0}", error));

                for (int i = 0; i < this.neuronsInLayerCount; i++)
                {
                    for (int j = 0; j < this.neuronsInLayerCount; j++)
                    {
                        sw.WriteLine(string.Format("ALayer[{0}].W[{1}]={2}", i, j, this.ALayer[i].W[j]));
                    }
                }

                for (int i = 0; i < this.hidelayersCount; i++)
                {
                    for (int j = 0; j < this.neuronsInLayerCount; j++)
                    {
                        for (int k = 0; k < this.neuronsInLayerCount; k++)
                        {
                            sw.WriteLine(string.Format("HideLayers[{0}][{1}].W[{2}]={3}", i, j, k, this.HideLayers[i][j].W[k]));
                        }
                    }
                }

                // Combine out layer
                for (int j = 0; j < this.outputCount; j++)
                {
                    for (int k = 0; k < this.neuronsInLayerCount; k++)
                    {
                        sw.WriteLine(string.Format("OutPutLayer[{0}].W[{1}]={2}", j, k, this.OutPutLayer[j].W[k]));
                    }
                }
            }
        }

        private static string ParseValue(string line)
        {
            Regex re = new Regex("^.*=(.*)$");
            Match m = re.Match(line);
            return m.Groups[1].Value;
        }

        public static Network Load(string fileName, ActivationFunction F)
        {
            Network net = null;

            using (StreamReader sr = new StreamReader(fileName))
            {
                string line = sr.ReadLine();
                int inputCount = int.Parse(ParseValue(line));

                line = sr.ReadLine();
                int outputCount = int.Parse(ParseValue(line));

                line = sr.ReadLine();
                int hidelayersCount = int.Parse(ParseValue(line));

                line = sr.ReadLine();
                int neuronsInLayerCount = int.Parse(ParseValue(line));

                line = sr.ReadLine();
                double error = double.Parse(ParseValue(line));

                net = new Network(inputCount, outputCount, hidelayersCount, neuronsInLayerCount, F);

                for (int i = 0; i < neuronsInLayerCount; i++)
                {
                    for (int j = 0; j < neuronsInLayerCount; j++)
                    {
                        line = sr.ReadLine();
                        double value = double.Parse(ParseValue(line));
                        net.ALayer[i].W[j] = value;
                    }
                }

                for (int i = 0; i < hidelayersCount; i++)
                {
                    for (int j = 0; j < neuronsInLayerCount; j++)
                    {
                        for (int k = 0; k < neuronsInLayerCount; k++)
                        {
                            line = sr.ReadLine();
                            double value = double.Parse(ParseValue(line));
                            net.HideLayers[i][j].W[k] = value;
                        }
                    }
                }

                // Combine out layer
                for (int j = 0; j < outputCount; j++)
                {
                    for (int k = 0; k < neuronsInLayerCount; k++)
                    {
                        line = sr.ReadLine();
                        double value = double.Parse(ParseValue(line));
                        net.OutPutLayer[j].W[k] = value;
                    }
                }
            }

            return net;
        }

        public double Error
        {
            get { return this.error; }
        }

        public double[] GetOutput()
        {
            this.Exec();

            double[] output = new double[outputCount];

            for (int i = 0; i < outputCount; i++)
            {
                output[i] = OutPutLayer[i].Y;
            }

            return output;
        }
    }
}
