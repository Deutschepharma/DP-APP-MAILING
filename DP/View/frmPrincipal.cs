using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DP.View
{
    public partial class frmPrincipal : Form
    {
        public frmPrincipal()
        {
            InitializeComponent();
        }

        private void mailingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmMailling mail = new frmMailling();
            mail.MdiParent = this;
            mail.Show();
           
            
            
        }

        private void salirToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult resultado;
            resultado = MessageBox.Show("Realmente ¿desea salir de la aplicación?", "", MessageBoxButtons.YesNo);
            if (resultado == DialogResult.Yes)
            {
                System.Environment.Exit(1);
            }
        }

        private void cargasExcelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            frmCargasExcel CargaExcel = new frmCargasExcel();
            CargaExcel.MdiParent = this;
            CargaExcel.Show();
           
        }
    }
}
