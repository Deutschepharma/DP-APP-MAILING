using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using System.IO;
using DP.Clases;


namespace DP.View
{
    /*
         * T = Por Enviar
         * E = Enviado
         * X = Error
         * N = No enviado
         * F = Correo Invalido
    */
    public partial class frmMailling : Form
    {
        SqlConnection cn = Conecta.getConexion();
        List<Cliente> listadoMail = new List<Cliente>();
        int j = 0;
        string response;
        string clave;
        string smtpCuenta;
        int pto;
        bool ssl;

        string QueryEnvio;
        public frmMailling()
        {
            InitializeComponent();
            limpiarPantalla();
        }
        

        private void limpiarPantalla()
        {
            lblEnviar.Text = "";
            txtArchivo.Text = "";
            txtAsunto.Text = "";
            txtCuerpo.Text = "";
            listadoMail.Clear();
            //llenarBox();
            
        }

        private void btnEnviar_Click(object sender, EventArgs e)
        {
             
                 
            if (cmbUsuario.Text!="" && cmbCuentaRemitente.Text!="")
            {

                SqlCommand cmd = new SqlCommand();
                SqlDataReader dr;
                cmd.Connection = cn;
                cmd.CommandText = QueryEnvio;
                cmd.CommandType = CommandType.Text;
                cn.Open();
                dr = cmd.ExecuteReader();

                
                while (dr.Read())
                {

                    Cliente cli = new Cliente();
                    switch (cmbUsuario.Text)
                    {
                        case "ISIS_P":
                            cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                            cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                            cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                            cli.email = dr.GetString(dr.GetOrdinal("email"));
                            listadoMail.Add(cli);
                            break;
                        case "DOUGLAS_P":
                            cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                            cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                            cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                            cli.email = dr.GetString(dr.GetOrdinal("email"));
                            listadoMail.Add(cli);
                            break;
                        case "JOANNA_P":
                            cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                            cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                            cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                            cli.email = dr.GetString(dr.GetOrdinal("email"));
                            listadoMail.Add(cli);
                            break;

                        default:
                            cli.email = dr.GetString(dr.GetOrdinal("mail"));
                            listadoMail.Add(cli);
                            break;
                    }

                    //if (cmbUsuario.Text == "ISIS_P")
                    //{
                    //    cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                    //    cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                    //    cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                    //    cli.email = dr.GetString(dr.GetOrdinal("email"));
                    //    listadoMail.Add(cli);
                    //}
                    //else if (cmbUsuario.Text == "DOUGLAS_P")
                    //{
                    //    cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                    //    cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                    //    cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                    //    cli.email = dr.GetString(dr.GetOrdinal("email"));
                    //    listadoMail.Add(cli);
                    //}
                    //else if (cmbUsuario.Text == "JOANNA_P")
                    //{
                    //    cli.nombre = dr.GetString(dr.GetOrdinal("nombre"));
                    //    cli.paterno = dr.GetString(dr.GetOrdinal("paterno"));
                    //    cli.materno = dr.GetString(dr.GetOrdinal("materno"));
                    //    cli.email = dr.GetString(dr.GetOrdinal("email"));
                    //    listadoMail.Add(cli);
                    //}
                    //else
                    //{
                    //    cli.email = dr.GetString(dr.GetOrdinal("mail"));
                    //    listadoMail.Add(cli);
                    //}

                }
                cn.Close();

                foreach (Cliente c in listadoMail)
                {
                    string para = c.nombre + " " + c.paterno + " " + c.materno;
                    string to = c.email;
                    string subjet = txtAsunto.Text.Trim();

                    string cuerpo = txtCuerpo.Text.Trim();
                    string ruta = @"" + txtArchivo.Text.Trim() + "";
                    Enviar send = new Enviar(para, to, subjet, cuerpo, ruta, response, clave, smtpCuenta, pto, ssl, cmbUsuario.Text);
                    Thread.Sleep(50000);
                    bar.PerformStep();
                }
                MessageBox.Show("Envio Completado.");
                limpiarPantalla();
            }
            else
            {
                MessageBox.Show("NO HA SELECCIONADO UNA BASE O CUENTA REMITENTE");
            }
            
        }

        private void btnAbrirArchivo_Click(object sender, EventArgs e)
        {

            OpenFileDialog open = new OpenFileDialog();
            open.Title = "Imagenes a Subir";
            if (open.ShowDialog() == DialogResult.OK)
            {
                txtArchivo.Text = open.FileName;
            }
            open.Dispose();

        }

        private void cmbUsuario_SelectedIndexChanged(object sender, EventArgs e)
        {
            QueryEnvio = "";   
            switch (cmbUsuario.Text)
            {
                case"ISIS":
                    string conta1 = "select count(*) 'No enviado' from email where estado='N' and usuario = '" + cmbUsuario.Text + "' ";
                    contarEnvio(conta1);
                    QueryEnvio = "select * from email where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";

                    break;
                case"DOUGLAS":
                    string conta2 = "select count(*) 'No enviado' from email where estado='N' and usuario = '" + cmbUsuario.Text + "' ";
                    contarEnvio(conta2);
                    QueryEnvio = "select * from email where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    break;
                case"ISIS_P":
                    string conta3 = "select count(*) 'No enviado' from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    contarEnvio(conta3);
                    QueryEnvio = "select * from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";

                    break;
                case "DOUGLAS_P":
                    string conta4 = "select count(*) 'No enviado' from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    contarEnvio(conta4);
                    QueryEnvio = "select * from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    break;
                case "JOANNA_P":
                    string conta5 = "select count(*) 'No enviado' from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    contarEnvio(conta5);
                    QueryEnvio = "select * from EmailPersonalizado where usuario = '" + cmbUsuario.Text.Trim() + "' and estado = 'N' ";
                    break;
                default:
                    break;
            }
        }

        private void contarEnvio(string recibeQuery)
        {
            lblEnviar.Text = "";
            SqlCommand comando = new SqlCommand();
            SqlDataReader dr;
            comando.Connection = cn;
            comando.CommandText = recibeQuery;
            comando.CommandType = CommandType.Text;
            cn.Open();
            dr = comando.ExecuteReader();
            int countEnviar = 0;
            if (dr.Read() == true)
            {
                countEnviar = Convert.ToInt32(dr["No enviado"].ToString());
            }
            lblEnviar.Text = countEnviar.ToString();
            cn.Close();
            bar.Minimum = 0;
            bar.Maximum = countEnviar;
            bar.Step = 1;
        }

        private void cmbCuentaRemitente_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (cmbCuentaRemitente.Text)
            {
                case "GOOGLE":
                    response="response.deutschepharma@gmail.com";
                    clave="Deutsche2016";
                    smtpCuenta = "smtp.gmail.com";
                    pto = 587;
                    ssl = true;
                    break;
                case "USYS":
                    response = "response@usystem.cl";
                    clave = "deu#2016";
                    smtpCuenta = "mail.usystem.cl";
                    pto = 25;
                    ssl = false;
                
                    break;
                case "USYS_P":
                    response = "estudios@deutschepharma.cl";
                    clave = "deu#2016";
                    smtpCuenta = "mail.deutschepharma.cl";
                    pto = 25;
                    ssl = false;
                  
                    break;
                default:

                    break;
            }
        }



     
            

        

    }
}
