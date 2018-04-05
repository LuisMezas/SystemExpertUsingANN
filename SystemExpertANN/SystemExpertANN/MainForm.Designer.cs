namespace SystemExpertANN
{
    partial class MainForm
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnConsultarInfo = new System.Windows.Forms.Button();
            this.btnEnviarInfo = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.cbCuentaPROSPERA = new System.Windows.Forms.CheckBox();
            this.txtIngresoMensual = new System.Windows.Forms.TextBox();
            this.cbMunicipio = new System.Windows.Forms.ComboBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cbRegular = new System.Windows.Forms.CheckBox();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.nudPromedio = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.cbEdad = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.cbDiscapacidad = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.txtNombre = new System.Windows.Forms.TextBox();
            this.txtApellidoP = new System.Windows.Forms.TextBox();
            this.txtApellidoM = new System.Windows.Forms.TextBox();
            this.currentIterationBox = new System.Windows.Forms.TextBox();
            this.currentErrorBox = new System.Windows.Forms.TextBox();
            this.groupBox1.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPromedio)).BeginInit();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnConsultarInfo);
            this.groupBox1.Controls.Add(this.btnEnviarInfo);
            this.groupBox1.Controls.Add(this.groupBox3);
            this.groupBox1.Controls.Add(this.groupBox2);
            this.groupBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(488, 460);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Datos del aplicante";
            // 
            // btnConsultarInfo
            // 
            this.btnConsultarInfo.Location = new System.Drawing.Point(307, 413);
            this.btnConsultarInfo.Name = "btnConsultarInfo";
            this.btnConsultarInfo.Size = new System.Drawing.Size(172, 41);
            this.btnConsultarInfo.TabIndex = 8;
            this.btnConsultarInfo.Text = "Consultar información de los becarios";
            this.btnConsultarInfo.UseVisualStyleBackColor = true;
            this.btnConsultarInfo.Click += new System.EventHandler(this.btnConsultarInfo_Click);
            // 
            // btnEnviarInfo
            // 
            this.btnEnviarInfo.Enabled = false;
            this.btnEnviarInfo.Location = new System.Drawing.Point(6, 413);
            this.btnEnviarInfo.Name = "btnEnviarInfo";
            this.btnEnviarInfo.Size = new System.Drawing.Size(179, 41);
            this.btnEnviarInfo.TabIndex = 7;
            this.btnEnviarInfo.Text = "✓ Enviar información";
            this.btnEnviarInfo.UseVisualStyleBackColor = true;
            this.btnEnviarInfo.Click += new System.EventHandler(this.btnEnviarInfo_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.cbCuentaPROSPERA);
            this.groupBox3.Controls.Add(this.txtIngresoMensual);
            this.groupBox3.Controls.Add(this.cbMunicipio);
            this.groupBox3.Controls.Add(this.label8);
            this.groupBox3.Controls.Add(this.label7);
            this.groupBox3.Controls.Add(this.label5);
            this.groupBox3.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox3.Location = new System.Drawing.Point(6, 276);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(473, 131);
            this.groupBox3.TabIndex = 6;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Datos socioeconómicos";
            // 
            // cbCuentaPROSPERA
            // 
            this.cbCuentaPROSPERA.AutoSize = true;
            this.cbCuentaPROSPERA.Location = new System.Drawing.Point(209, 70);
            this.cbCuentaPROSPERA.Name = "cbCuentaPROSPERA";
            this.cbCuentaPROSPERA.Size = new System.Drawing.Size(15, 14);
            this.cbCuentaPROSPERA.TabIndex = 15;
            this.cbCuentaPROSPERA.UseVisualStyleBackColor = true;
            // 
            // txtIngresoMensual
            // 
            this.txtIngresoMensual.Location = new System.Drawing.Point(200, 35);
            this.txtIngresoMensual.Name = "txtIngresoMensual";
            this.txtIngresoMensual.Size = new System.Drawing.Size(260, 24);
            this.txtIngresoMensual.TabIndex = 12;
            this.txtIngresoMensual.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtIngresoMensual.TextChanged += new System.EventHandler(this.txtIngresoMensual_TextChanged);
            this.txtIngresoMensual.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtIngresoMensual_KeyPress);
            // 
            // cbMunicipio
            // 
            this.cbMunicipio.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbMunicipio.FormattingEnabled = true;
            this.cbMunicipio.Location = new System.Drawing.Point(200, 92);
            this.cbMunicipio.Name = "cbMunicipio";
            this.cbMunicipio.Size = new System.Drawing.Size(260, 26);
            this.cbMunicipio.TabIndex = 12;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(7, 95);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(149, 18);
            this.label8.TabIndex = 11;
            this.label8.Text = "Municipio donde vive:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(7, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(187, 18);
            this.label7.TabIndex = 10;
            this.label7.Text = "¿Cuenta con PROSPERA?";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(7, 38);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(172, 18);
            this.label5.TabIndex = 9;
            this.label5.Text = "Ingreso mensual familiar:";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.cbRegular);
            this.groupBox2.Controls.Add(this.label10);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.nudPromedio);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.cbEdad);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.cbDiscapacidad);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.txtNombre);
            this.groupBox2.Controls.Add(this.txtApellidoP);
            this.groupBox2.Controls.Add(this.txtApellidoM);
            this.groupBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.groupBox2.Location = new System.Drawing.Point(6, 19);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(475, 251);
            this.groupBox2.TabIndex = 5;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Datos generales";
            // 
            // cbRegular
            // 
            this.cbRegular.AutoSize = true;
            this.cbRegular.Location = new System.Drawing.Point(209, 189);
            this.cbRegular.Name = "cbRegular";
            this.cbRegular.Size = new System.Drawing.Size(15, 14);
            this.cbRegular.TabIndex = 14;
            this.cbRegular.UseVisualStyleBackColor = true;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(7, 185);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(144, 18);
            this.label10.TabIndex = 13;
            this.label10.Text = "¿Es alumno regular?";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 218);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(197, 18);
            this.label9.TabIndex = 12;
            this.label9.Text = "¿Tiene alguna discapacidad?";
            // 
            // nudPromedio
            // 
            this.nudPromedio.BackColor = System.Drawing.Color.White;
            this.nudPromedio.DecimalPlaces = 1;
            this.nudPromedio.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.nudPromedio.Location = new System.Drawing.Point(340, 150);
            this.nudPromedio.Maximum = new decimal(new int[] {
            10,
            0,
            0,
            0});
            this.nudPromedio.Name = "nudPromedio";
            this.nudPromedio.ReadOnly = true;
            this.nudPromedio.Size = new System.Drawing.Size(120, 24);
            this.nudPromedio.TabIndex = 11;
            this.nudPromedio.Value = new decimal(new int[] {
            60,
            0,
            0,
            65536});
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(7, 152);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(129, 18);
            this.label6.TabIndex = 10;
            this.label6.Text = "Promedio general:";
            // 
            // cbEdad
            // 
            this.cbEdad.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cbEdad.FormattingEnabled = true;
            this.cbEdad.Items.AddRange(new object[] {
            "26",
            "25",
            "24",
            "23",
            "22",
            "21",
            "20",
            "19",
            "18",
            "17",
            "16",
            "15",
            "14",
            "13",
            "12"});
            this.cbEdad.Location = new System.Drawing.Point(137, 115);
            this.cbEdad.Name = "cbEdad";
            this.cbEdad.Size = new System.Drawing.Size(323, 26);
            this.cbEdad.TabIndex = 9;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(7, 118);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(46, 18);
            this.label4.TabIndex = 8;
            this.label4.Text = "Edad:";
            // 
            // cbDiscapacidad
            // 
            this.cbDiscapacidad.AutoSize = true;
            this.cbDiscapacidad.Location = new System.Drawing.Point(209, 221);
            this.cbDiscapacidad.Name = "cbDiscapacidad";
            this.cbDiscapacidad.Size = new System.Drawing.Size(15, 14);
            this.cbDiscapacidad.TabIndex = 0;
            this.cbDiscapacidad.UseVisualStyleBackColor = true;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(7, 88);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(122, 18);
            this.label3.TabIndex = 7;
            this.label3.Text = "Apellido Materno:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 62);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 18);
            this.label2.TabIndex = 6;
            this.label2.Text = "Apellido Paterno:";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 35);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 18);
            this.label1.TabIndex = 5;
            this.label1.Text = "Nombre:";
            // 
            // txtNombre
            // 
            this.txtNombre.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtNombre.Location = new System.Drawing.Point(137, 32);
            this.txtNombre.Name = "txtNombre";
            this.txtNombre.Size = new System.Drawing.Size(323, 24);
            this.txtNombre.TabIndex = 1;
            this.txtNombre.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNombre.TextChanged += new System.EventHandler(this.txtNombre_TextChanged);
            this.txtNombre.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtNombre_KeyPress);
            // 
            // txtApellidoP
            // 
            this.txtApellidoP.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtApellidoP.Location = new System.Drawing.Point(137, 59);
            this.txtApellidoP.Name = "txtApellidoP";
            this.txtApellidoP.Size = new System.Drawing.Size(323, 24);
            this.txtApellidoP.TabIndex = 2;
            this.txtApellidoP.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtApellidoP.TextChanged += new System.EventHandler(this.txtApellidoP_TextChanged);
            this.txtApellidoP.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtApellidoP_KeyPress);
            // 
            // txtApellidoM
            // 
            this.txtApellidoM.CharacterCasing = System.Windows.Forms.CharacterCasing.Upper;
            this.txtApellidoM.Location = new System.Drawing.Point(137, 85);
            this.txtApellidoM.Name = "txtApellidoM";
            this.txtApellidoM.Size = new System.Drawing.Size(323, 24);
            this.txtApellidoM.TabIndex = 3;
            this.txtApellidoM.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtApellidoM.TextChanged += new System.EventHandler(this.txtApellidoM_TextChanged);
            this.txtApellidoM.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.txtApellidoM_KeyPress);
            // 
            // currentIterationBox
            // 
            this.currentIterationBox.Location = new System.Drawing.Point(12, 492);
            this.currentIterationBox.Name = "currentIterationBox";
            this.currentIterationBox.Size = new System.Drawing.Size(185, 20);
            this.currentIterationBox.TabIndex = 16;
            this.currentIterationBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // currentErrorBox
            // 
            this.currentErrorBox.Location = new System.Drawing.Point(306, 492);
            this.currentErrorBox.Name = "currentErrorBox";
            this.currentErrorBox.Size = new System.Drawing.Size(185, 20);
            this.currentErrorBox.TabIndex = 17;
            this.currentErrorBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(506, 538);
            this.Controls.Add(this.currentErrorBox);
            this.Controls.Add(this.currentIterationBox);
            this.Controls.Add(this.groupBox1);
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Sistema Experto";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudPromedio)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.TextBox txtApellidoM;
        private System.Windows.Forms.TextBox txtApellidoP;
        private System.Windows.Forms.TextBox txtNombre;
        private System.Windows.Forms.CheckBox cbDiscapacidad;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox cbEdad;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TextBox txtIngresoMensual;
        private System.Windows.Forms.ComboBox cbMunicipio;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Button btnConsultarInfo;
        private System.Windows.Forms.Button btnEnviarInfo;
        private System.Windows.Forms.NumericUpDown nudPromedio;
        private System.Windows.Forms.CheckBox cbCuentaPROSPERA;
        private System.Windows.Forms.CheckBox cbRegular;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox currentIterationBox;
        private System.Windows.Forms.TextBox currentErrorBox;
    }
}

