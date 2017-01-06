using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DP
{
    class Conecta
    {
        public static SqlConnection getConexion()
        {
            try
            {
                string archivoExiste = "C:\\sistemas\\conecta.ini";
                string leer;
                FileStream fs_inv = new FileStream(archivoExiste, FileMode.Open);
                StreamReader sr_inv = new StreamReader(fs_inv);
                leer = sr_inv.ReadLine();
                //Cerrar Objetos
                sr_inv.Close();
                fs_inv.Close();
                //Conexion Pega
                //SqlConnection cn = new SqlConnection(@"Data Source=10.10.231.30;Initial Catalog=bd_sistemas;Persist Security Info=True;User ID=sa;Password=Sa2011");
                //Conexion Casa
                SqlConnection cn = new SqlConnection(@"" + leer + "");
                return cn;
            }
            catch (Exception ex)
            {
                //return null;7
                throw new ArgumentException("Error al conectar o no Existe Archivo de Conexion", ex);
            }

        }
    }
}
