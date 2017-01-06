using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data;


namespace DP.Clases
{
    public class Enviar
    {
        SqlConnection cn = Conecta.getConexion();

        public string para { get; set; }
        public string to { get; set; }
        public string subjet {get;set;}
        public string cuerpo { get; set; }
        public string ruta { get; set; }
        public string response { get; set; }
        public string clave { get; set; }
        public string smtpCuenta { get; set; }
        public int pto { get; set; }
        public bool ssl { get; set; }

        public string usuario { get; set; }

        //public ComboBox cmbUsuario { get; set; }
        //public ComboBox cmbBase { get; set; }

        public Enviar(string para, string to, string subjet, string cuerpo, string ruta, string response, string clave, string smtpCuenta, int pto, bool ssl, string usuario)
        {
            this.para = para;
            this.to=to;
            this.subjet = subjet;
            this.cuerpo = cuerpo;
            this.ruta = ruta;
            this.response = response;
            this.clave = clave;
            this.smtpCuenta = smtpCuenta;
            this.pto = pto;
            this.ssl = ssl;
            this.usuario = usuario;
            Envio();
        }

        private void Envio()
        {
            string log;
            string html;
            try
            {
                MailMessage mensaje = new MailMessage(response, to);

                mensaje.Subject = subjet;
                string Titulo = @"<h4>" +"Estimado(a): "+ para + "</h4>";
                string Cuerpo = "<p>" + cuerpo + "</p>";
                string logo = @"<img src=""cid:logo""/><br />";
                if (para.Length > 2)
                {
                    html = Titulo + Cuerpo + logo;
                }
                else
                {
                    html = Cuerpo + logo;
                }

                AlternateView htmlView = AlternateView.CreateAlternateViewFromString(html, Encoding.UTF8, MediaTypeNames.Text.Html);

                if (ruta != "")
                {
                    LinkedResource img = new LinkedResource(ruta);
                    img.ContentId = "logo";
                    htmlView.LinkedResources.Add(img);
                }
                mensaje.AlternateViews.Add(htmlView);
                SmtpClient smtp = new SmtpClient(smtpCuenta, pto);
                smtp.EnableSsl = ssl;
                smtp.Credentials = new NetworkCredential(response, clave);
                try
                {
                    smtp.Send(mensaje);
                    switch (usuario)
                    {
                        case "ISIS_P":
                            CambiaEstado(to, "E", 1);
                            break;
                        case "DOUGLAS_P":
                            CambiaEstado(to, "E", 1);
                            break;
                        case "JOANNA_P":
                            CambiaEstado(to, "E", 1);
                            break;
                        default:
                            CambiaEstado(to, "E", 0);
                            break;
                    }
                    //if (usuario == "ISIS_P")
                    //{
                    //    CambiaEstado(to, "E", 1);
                    //}
                    //else if (usuario == "DOUGLAS_P")
                    //{
                    //    CambiaEstado(to, "E", 1);
                    //}
                    //else
                    //{
                    //    CambiaEstado(to, "E", 0);
                    //}
                    
                }
                catch (Exception e)
                {
                    //aqui caen los errores de tipo ascii
                    switch (usuario)
                    {
                        case "ISIS_P":
                            CambiaEstado(to, "X", 1);
                            InsertaError(to, e.ToString(), "X");
                            break;
                        case "DOUGLAS_P":
                            CambiaEstado(to, "X", 1);
                            InsertaError(to, e.ToString(), "X");
                            break;
                        case "JOANNA_P":
                            CambiaEstado(to, "X", 1);
                            InsertaError(to, e.ToString(), "X");
                            break;
                        default:
                            CambiaEstado(to, "X", 0);
                            InsertaError(to, e.ToString(), "X");
                            break;
                    }
                    //if (usuario == "ISIS_P")
                    //{
                    //    CambiaEstado(to, "X", 1);
                    //    InsertaError(to, e.ToString(), "X");
                    //}
                    //else if (usuario == "DOUGLAS_P")
                    //{
                    //    CambiaEstado(to, "X", 1);
                    //    InsertaError(to, e.ToString(), "X");
                    //}
                    //else
                    //{
                    //    CambiaEstado(to, "X", 0);
                    //    InsertaError(to, e.ToString(), "X");

                    //}
                }

            }
            catch (Exception ex)
            {
                switch (usuario)
                {
                    case "ISIS_P":
                        CambiaEstado(to, "F", 1);
                        InsertaError(to, ex.ToString(), "F");
                        break;
                    case "DOUGLAS_P":
                        CambiaEstado(to, "F", 1);
                        InsertaError(to, ex.ToString(), "F");
                        break;
                    case "JOANNA_P":
                        CambiaEstado(to, "F", 1);
                        InsertaError(to, ex.ToString(), "F");
                        break;
                    default:
                        CambiaEstado(to, "F", 0);
                        InsertaError(to, ex.ToString(), "F");
                        break;
                }
                //if (usuario == "ISIS_P")
                //{
                //    CambiaEstado(to, "F", 1);
                //    InsertaError(to, ex.ToString(), "F");
                //}
                //else if (usuario == "DOUGLAS_P")
                //{
                //    CambiaEstado(to, "F", 1);
                //    InsertaError(to, ex.ToString(), "F");
                //}
                //else
                //{
                //    CambiaEstado(to, "F", 0);
                //    InsertaError(to, ex.ToString(), "F");
                //}
            }

        }
        private void InsertaError(string to, string error, string tipo)
        {
            string log;
            log = "insert into errores values ('" + to + "',GETDATE(), '" + error + "','" + tipo + "')";
            SqlCommand cmd = new SqlCommand(log, cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
        }
        private void CambiaEstado(string to, string flag, int tipo)
        {
            string Consulta;
            switch (tipo)
            {
                case 1:
                    Consulta = "update EmailPersonalizado set estado='" + flag + "' where email='" + to + "' ";
                    break;
                default:
                    Consulta = @"update email set estado='" + flag + "' where mail='" + to + "'";
                    break;
            }
            //if (tipo==1)
            //{
            //    Consulta = "update EmailPersonalizado set estado='" + flag + "' where email='" + to + "' ";
            //}
            //else
            //{
            //    Consulta = @"update email set estado='" + flag + "' where mail='" + to + "'";
            //}

            SqlCommand cmd = new SqlCommand(Consulta,cn);

            cn.Open();
            cmd.ExecuteNonQuery();
            cn.Close();
            

        }



    }

}
