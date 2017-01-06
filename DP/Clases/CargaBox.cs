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
using DP.View;

namespace DP.Clases
{
    class CargaBox
    {
        
        public DataSet CargarCombos()
        {
            DataSet ds = new DataSet();
            try
            {
                SqlConnection cn = Conecta.getConexion();
                SqlCommand Cmd = new SqlCommand("Sp_Llena_Box", cn);
                Cmd.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter da = new SqlDataAdapter(Cmd);
                da.Fill(ds);
            }

            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return ds;
        }
    }
}
