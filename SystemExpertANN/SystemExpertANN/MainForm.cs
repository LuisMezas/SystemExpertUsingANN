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
using System.IO;
using System.Collections;
using System.Globalization;

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

        private Thread workerThread = null;
        private volatile bool needToStop = false;

        private double learningRate = 0.1;
        private double momentum = 0.0;
        private double sigmoidAlphaValue = 2.0;

        private Boolean ExisteBecario = false;

        public MainForm()
        {
            InitializeComponent();

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
                FlagExisteBecario();
                if (ExisteBecario == true)
                {
                    MessageBox.Show("Ya existe un registro de este becario."
                        , "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                EvaluateInputsForANN();
                InsertBecario();
                ResetObjects();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ocurrió un error." + ex, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            ANNLearning_BackPropagation();
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

        void InsertBecario()
        {
            Becarios model = new Becarios();
            if (_Output[0] <= 0)
            {
                model.Nombre = formInputs.Nombre;
                model.ApellidoPaterno = formInputs.ApellidoP;
                model.ApellidoMaterno = formInputs.ApellidoM;
                model.Edad = formInputs.Edad;
                model.EsBecado = false;
                model.EsBecadoProspera = formInputs.CuentaPROSPERA;
                model.EsRegular = formInputs.EsRegular;
                model.FechaEvaluacionbeca = DateTime.Now.Date;
                model.Discapacidad = formInputs.Discapacidad;
                model.Id_Municipio = formInputs.Id_Municipio;
                model.IngresoMensual = formInputs.IngresoMensualFamiliar;
                model.Promedio = formInputs.Promedio;
            }
            else
            {
                model.Nombre = formInputs.Nombre;
                model.ApellidoPaterno = formInputs.ApellidoP;
                model.ApellidoMaterno = formInputs.ApellidoM;
                model.Edad = formInputs.Edad;
                model.EsBecado = true;
                model.EsBecadoProspera = formInputs.CuentaPROSPERA;
                model.EsRegular = formInputs.EsRegular;
                model.FechaEvaluacionbeca = DateTime.Now.Date;
                model.Discapacidad = formInputs.Discapacidad;
                model.Id_Municipio = formInputs.Id_Municipio;
                model.IngresoMensual = formInputs.IngresoMensualFamiliar;
                model.Promedio = formInputs.Promedio;
            }
            db.Becarios.Add(model);
            db.SaveChanges();
        }

        void FlagExisteBecario()
        {
            var query = db.Becarios.Where(x => x.Nombre.ToUpper() == formInputs.Nombre.ToUpper())
                .Where(x => x.ApellidoPaterno.ToUpper() == formInputs.ApellidoP.ToUpper())
                .Where(x => x.ApellidoMaterno.ToUpper() == formInputs.ApellidoM.ToUpper()).FirstOrDefault();
            if (query == null)
            {
                ExisteBecario = false;
            }
            else
            {
                ExisteBecario = true;
            }
        }

        void ValidateInputs()
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

            //double[][] input = new double[30][] {
            //    //INPUTS for OUTPUT 1
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {-1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, -1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {-1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, -1, 1, 1, 1, 1},
            //        new double[] {-1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {1, 1, -1, 1, 1, 1, 1},
            //        new double[] {1, 1, 1, 1, 1, 1, 1},
            //        new double[] {-1, 1, 1, 1, 1, 1, 1},
            //        //INPUTS for OUTPUT -1
            //        new double[] {-1, 1, 1, 1, -1, -1, -1},
            //        new double[] {1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, -1, -1, -1, -1, -1},
            //        new double[] {1, -1, 1, -1, -1, -1, -1},
            //        new double[] {-1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, -1, -1, -1, -1, -1},
            //        new double[] {-1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, -1, -1, -1, -1, -1},
            //        new double[] {-1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, 1, -1, -1, -1, -1},
            //        new double[] {1, -1, -1, -1, -1, -1, -1},
            //        new double[] {-1, -1, -1, -1, -1, -1, -1},
            //        new double[] {-1, -1, -1, -1, -1, -1, -1}
            //};

            //double[][] output = new double[30][] {
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1},
            //        new double[] {-1}
            //};

            double[][] input = new double[289][]
            {
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,-1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,-1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,-1,1,1,1,1},
                new double[] {1,1,-1,1,1,1,1},
                new double[] {-1,1,-1,1,1,-1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,1,1,1,1,1,1},
                new double[] {1,1,1,1,1,1,1},
                new double[] {-1,-1,-1,1,1,1,1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,-1,-1,-1,-1,-1,-1},
                new double[] {-1,1,1,-1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {1,-1,-1,-1,-1,1,-1},
                new double[] {1,-1,1,-1,1,-1,-1},
                new double[] {-1,-1,1,1,-1,-1,-1}
            };

            double[][] output = new double[289][] {
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},
                new double[] {1},

                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1},
                new double[] {-1}
            };

            _Network = new ActivationNetwork(new BipolarSigmoidFunction(sigmoidAlphaValue), 7, 2, 1);

            _Teacher = new BackPropagationLearning(_Network);
            _Teacher.LearningRate = learningRate;
            _Teacher.Momentum = momentum;

            int iteration = 1;
            StreamWriter errorsFile = null;
            try
            {
                errorsFile = File.CreateText("Errors.csv");
                // errors list
                ArrayList errorsList = new ArrayList();
                while (!needToStop)
                {

                    // run epoch of learning procedure
                    double error = _Teacher.RunEpoch(input, output);
                    errorsList.Add(error);

                    // save current error
                    if (errorsFile != null)
                    {
                        errorsFile.WriteLine(error);
                    }

                    // show current iteration & error
                    currentIterationBox.Text = iteration.ToString();
                    currentErrorBox.Text = error.ToString();
                    iteration++;

                    // check if we need to stop
                    if (iteration > 5000)
                        break;
                }
                double[,] errors = new double[errorsList.Count, 2];

                for (int i = 0, n = errorsList.Count; i < n; i++)
                {
                    errors[i, 0] = i;
                    errors[i, 1] = (double)errorsList[i];
                }

            }
            catch (IOException)
            {
                MessageBox.Show("Failed writing file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                // close files
                if (errorsFile != null)
                    errorsFile.Close();
            }
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
            if (_Output[0] <= 0)
            {
                MessageBox.Show("No es apto para una beca.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("El aplicante es apto para una beca.", "Ok", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
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
                formInputs.IngresoMensualFamiliar =  (float)Convert.ToDouble(txtIngresoMensual.Text, CultureInfo.InvariantCulture);
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
                -1,-1,-1,-1,-1,-1,-1
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
            var Query = db.Municipio.Where(x => x.Id_Municipio == model.Id_Municipio).Where(x => x.Prioridad == true).SingleOrDefault();
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
            for (int i = 0; i < _Inputs.Count(); i++)
            {
                _Inputs.SetValue(0, i);
            }
            btnEnviarInfo.Enabled = false;
            formInputs.ApellidoM = "";
            formInputs.ApellidoP = "";
            formInputs.CuentaPROSPERA = false;
            formInputs.Discapacidad = false;
            formInputs.Edad = 26;
            formInputs.EsRegular = false;
            formInputs.IngresoMensualFamiliar = 0;
            formInputs.Nombre = "";
            formInputs.Promedio = 6.0;
            formInputs.Id_Municipio = 1;
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

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // check if worker thread is running
            if ((workerThread != null) && (workerThread.IsAlive))
            {
                needToStop = true;
                while (!workerThread.Join(100))
                    Application.DoEvents();
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
        public float IngresoMensualFamiliar { get; set; }
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
