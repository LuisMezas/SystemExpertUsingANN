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
using DataAccessLayer;

namespace SystemExpertANN
{
    public partial class MainForm : Form
    {
        private BecariosDBEntities db = new BecariosDBEntities();

        public ActivationNetwork _Network { get; set; }
        public ThresholdFunction _Function { get; set; }
        public BackPropagationLearning _Teacher { get; set; }
        public InfoModel _Model { get; set; }
        public double[] _Inputs { get; set; }
        public InfoModel formInputs = new InfoModel();
        public double[] _Output { get; set; }

        public Boolean needtoStop { get; set; }

        public MainForm()
        {
            InitializeComponent();
            _Function = new ThresholdFunction();
            _Network = new ActivationNetwork(_Function, 7, 2);
            ANNLearning_BackPropagation();
        }

        private void btnConsultarInfo_Click(object sender, EventArgs e)
        {
            var Form2 = new QueryForm();
            Form2.Show();
        }

        private void btnEnviarInfo_Click(object sender, EventArgs e)
        {
            ValidateInputs();
            try
            {
                PrepareFormInputs();
                _Inputs = Parse(formInputs);
                EvaluateInputsForANN();
                ResetObjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error." + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            var Query = db.Municipio.ToList();
            cbMunicipio.DataSource = Query;
            cbMunicipio.ValueMember = "Id_Municipio";
            cbMunicipio.DisplayMember = "NombreMunicipio";
            cbEdad.SelectedIndex = 0;
            // get learning error limit
        }

        private void txtNombre_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back || char.IsWhiteSpace(e.KeyChar));
        }

        private void txtApellidoP_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtApellidoM_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !(char.IsLetter(e.KeyChar) || e.KeyChar == (char)Keys.Back);
        }

        private void txtIngresoMensual_KeyPress(object sender, KeyPressEventArgs e)
        {
            // Verify that the pressed key isn't CTRL or any non-numeric digit
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar) && (e.KeyChar != '.'))
            {
                e.Handled = true;
            }

            // If you want, you can allow decimal (float) numbers
            if ((e.KeyChar == '.') && ((sender as TextBox).Text.IndexOf('.') > -1))
            {
                e.Handled = true;
            }
        }

        private void ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtNombre.Text)
                || string.IsNullOrWhiteSpace(txtApellidoP.Text)
                || string.IsNullOrWhiteSpace(txtApellidoM.Text)
                || string.IsNullOrWhiteSpace(cbEdad.Text)
                || string.IsNullOrWhiteSpace(txtIngresoMensual.Text)
                )
            {
                MessageBox.Show("Verifique que no hay datos en blanco.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        void ANNLearning_BackPropagation()
        {
            //initialize input and output values
                double[][] input = new double[14][] {
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 0, 0, 0},
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 1, 1, 1, 1, 1, 1},
                    new double[] {1, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 1, 1, 1},
                    new double[] {0, 0, 0, 0, 0, 0, 0}
            };

            double[][] output = new double[14][] {
                    new double[] {1},
                    new double[] {1},
                    new double[] {1},
                    new double[] {0},
                    new double[] {0},
                    new double[] {0},
                    new double[] {0},
                    new double[] {1},
                    new double[] {1},
                    new double[] {1},
                    new double[] {0},
                    new double[] {0},
                    new double[] {0},
                    new double[] {0}
            };
            //create neural network
           _Network = new ActivationNetwork(
               _Function,
               7, // seven inputs in the network
               7, // seven neurons in the first layer
               1); // one neuron in the second layer

            //create teacher
            _Teacher = new BackPropagationLearning(_Network);
            // loop
            _Teacher.LearningRate = 0.0001;
            _Teacher.Momentum = 0.01;
            //while (!needToStop)
            //{
            //    // run epoch of learning procedure
            //    double error = teacher.RunEpoch(input, output);
            //    // check error value to see if we need to stop
            //    // ...
            //}
            //while (!needtoStop)
            //{
            //    double error = _Teacher.RunEpoch(input, output);
            //    if (error <= 0.1)
            //        break;
            //}
            int iteration = 1;
            while(iteration < 10000)
            {
                double error = _Teacher.RunEpoch(input, output);
                iteration++;
            }
            //    double[][] input = new double[7][] {
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {-1, -1, -1, -1, 1, 1, 1},
            //        new double[] {-1, -1, -1, -1, 1, 1, 1},
            //        new double[] {-1, -1, -1, -1, -1, -1, 1},
            //        new double[] {-1, -1, -1, -1, -1, -1, -1}
            //};

            //    double[][] output = new double[7][] {
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //};
            //    _Network = new ActivationNetwork(new BipolarSigmoidFunction(2), 7, 7, 1);
            //    _Teacher = new BackPropagationLearning( _Network );
            //    _Teacher.LearningRate = 0.1;
            //    _Teacher.Momentum = 0;
            //    while(!needtoStop)
            //    {
            //        double error = _Teacher.RunEpoch(input, output);
            //        if (error <= 0.1)
            //            break;
            //    }
            try
                {
                _Network.Save("Network");
            }
            catch (Exception)
            {
                MessageBox.Show("No se pudo guardar la red neuronal", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }

        void EvaluateInputsForANN()
        {
                _Output = _Network.Compute(_Inputs);
            MessageBox.Show("Muchas gracias. +" + _Output.GetValue(0).ToString(), "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        void PrepareFormInputs()
        {
            try
            {
                formInputs.Nombre = txtNombre.Text;
                formInputs.ApellidoP = txtApellidoP.Text;
                formInputs.ApellidoM = txtApellidoM.Text;
                formInputs.Edad = int.Parse(cbEdad.Text);
                formInputs.Promedio = Convert.ToDouble(Math.Round(nudPromedio.Value, 1));
                formInputs.EsRegular = cbRegular.Checked;
                formInputs.Discapacidad = cbDiscapacidad.Checked;
                formInputs.IngresoMensualFamiliar = Convert.ToDouble(txtIngresoMensual.Text);
                formInputs.CuentaPROSPERA = cbCuentaPROSPERA.Checked;
                formInputs.Id_Municipio = int.Parse(cbMunicipio.SelectedValue.ToString());
            }
            catch (Exception)
            {
                MessageBox.Show("Por favor, verifique los datos.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            
        }

        double[] Parse(InfoModel model)
        {
            double[] Vector = new double[7]
            {
                0,0,0,0,0,0,0
            };
            if (model.Edad >= 18 && model.Edad <= 24)
            {
                Vector[0] = 1;
            }
            if (model.Promedio >= 8.0 && model.Promedio <= 10.0)
            {
                Vector[1] = 1;
            }
            if (model.EsRegular == true)
            {
                Vector[2] = 1;
            }
            if (model.IngresoMensualFamiliar <= 2686.14)
            {
                Vector[3] = 1;
            }
            if (model.CuentaPROSPERA == true)
            {
                Vector[4] = 1;
            }
            var Query = db.Municipio.Where(x => x.Id_Municipio == model.Id_Municipio).Where(x=>x.Prioridad == true).SingleOrDefault();
            if (Query != null)
            {
                Vector[5] = 1;
            }
            if (model.Discapacidad == true)
            {
                Vector[6] = 1;
            }
            return Vector;
        }

        void ResetObjects()
        {
            txtNombre.Text = "";
            txtApellidoP.Text = "";
            txtApellidoM.Text = "";
            cbEdad.Text = "";
            txtIngresoMensual.Text = "";
            nudPromedio.Value = 6;
            cbCuentaPROSPERA.Checked = false;
            cbDiscapacidad.Checked = false;
            cbRegular.Checked = false;
            cbEdad.SelectedIndex = 0;
            cbMunicipio.SelectedIndex = 0;
            for(int i = 0; i < _Inputs.Count(); i++)
            {
                _Inputs.SetValue(0, i);
            }
            btnEnviarInfo.Enabled = false;
        }

        private void txtNombre_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoP.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoM.Text)
                && !string.IsNullOrWhiteSpace(cbEdad.Text)
                && !string.IsNullOrWhiteSpace(txtIngresoMensual.Text)
                )
            {
                btnEnviarInfo.Enabled = true;
            }
        }

        private void txtApellidoP_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoP.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoM.Text)
                && !string.IsNullOrWhiteSpace(cbEdad.Text)
                && !string.IsNullOrWhiteSpace(txtIngresoMensual.Text)
                )
            {
                btnEnviarInfo.Enabled = true;
            }
        }

        private void txtApellidoM_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoP.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoM.Text)
                && !string.IsNullOrWhiteSpace(cbEdad.Text)
                && !string.IsNullOrWhiteSpace(txtIngresoMensual.Text)
                )
            {
                btnEnviarInfo.Enabled = true;
            }
        }

        private void txtIngresoMensual_TextChanged(object sender, EventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(txtNombre.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoP.Text)
                && !string.IsNullOrWhiteSpace(txtApellidoM.Text)
                && !string.IsNullOrWhiteSpace(cbEdad.Text)
                && !string.IsNullOrWhiteSpace(txtIngresoMensual.Text)
                )
            {
                btnEnviarInfo.Enabled = true;
            }
        }
    }

    public class InfoModel
    {
        public string Nombre { get; set; }
        public string ApellidoP { get; set; }
        public string ApellidoM { get; set; }
        public int Edad { get; set; }
        public double Promedio { get; set; }
        public double IngresoMensualFamiliar { get; set; }
        public Boolean CuentaPROSPERA { get; set; }
        public Boolean EsRegular { get; set; }
        public Boolean Discapacidad { get; set; }
        public int Id_Municipio { get; set; }
    }

    public class Vector
    {
        public double Edad { get; set; }
        public double Promedio { get; set; }
        public double IngresoMensualFamiliar { get; set; }
        public double CuentaProspera { get; set; }
        public double EsRegular { get; set; }
        public double Discapacidad { get; set; }
        public double Municipio { get; set; }
    }
}
