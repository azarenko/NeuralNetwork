using System;
using System.Collections.Generic;
using System.Text;

namespace NeuralNetwork
{
    public class Network
    {
        private int inputCount;
        private int outputCount;
        private int hidelayersCount;
        private int neuronsInLayerCount;

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

        public void Teach(double[] EtalonSet)
        {

        }

        public void Save(string fileName)
        {

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
