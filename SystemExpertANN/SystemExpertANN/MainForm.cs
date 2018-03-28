using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using AForge.Neuro;
using System.Linq.Expressions;
using AForge.Neuro.Learning;
using System.Threading;

namespace SystemExpertANN
{
    public partial class MainForm : Form
    {
        public ActivationNetwork _Network { get; set; }
        public ThresholdFunction _Function { get; set; }
        private double learningRate = 0.1;
        private bool saveStatisticsToFiles = false;

        private Thread workerThread = null;
        private volatile bool needToStop = false;

        public MainForm()
        {
            InitializeComponent();
            _Function = new ThresholdFunction();
            _Network = new ActivationNetwork(_Function, 7, 2);
            //ANNLearning_BackPropagation();
        }

//        void ANNLearning_BackPropagation()
//        {
//            // initialize input and output values
//            double[][] input = new double[4][] {
//    new double[] {0, 0}, new double[] {0, 1},
//    new double[] {1, 0}, new double[] {1, 1}
//};
//            var inputs = new List<double[][]>();

//            double[][] output = new double[4][] {
//    new double[] {0}, new double[] {1},
//    new double[] {1}, new double[] {0}
//};
//            // create neural network
//            ActivationNetwork network = new ActivationNetwork(
//                new ThresholdFunction(),
//                7, // seven inputs in the network
//                7, // seven neurons in the first layer
//                2); // two neurons in the second layer
//            // create teacher
//            BackPropagationLearning teacher = new BackPropagationLearning(network);
//            // loop
//            while (!needToStop)
//            {
//                // run epoch of learning procedure
//                double error = teacher.RunEpoch(input, output);
//                // check error value to see if we need to stop
//                // ...
//            }

//        }
    }
}
