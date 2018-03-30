using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccessLayer;

namespace SystemExpertANN
{
    public partial class QueryForm : Form
    {
        private BecariosDBEntities db = new BecariosDBEntities();

        public QueryForm()
        {
            InitializeComponent();
        }

        private void QueryForm_Load(object sender, EventArgs e)
        {
            var Query = db.Becarios.Include(x => x.Municipio).OrderBy(x => x.Promedio).ToList();
            dgvConsulta.DataSource = Query;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
