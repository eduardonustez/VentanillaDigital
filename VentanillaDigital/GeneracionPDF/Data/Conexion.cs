using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generacion_PDF_Notaria.Data
{
    public class Conexion
    {
        private SqlDataReader m_dr;
        private string m_stringConexion;
        private SqlConnection m_cnn;
        private SqlCommand cmd;

        public Conexion()
        {
            m_stringConexion = System.Configuration.ConfigurationManager.ConnectionStrings["Conexion"].ConnectionString;
            m_cnn = new SqlConnection(m_stringConexion);
        }
        public bool PruebaConexion()
        {
            SqlConnection conexion = new SqlConnection(m_stringConexion);
            conexion.Open();
            if (conexion.State == System.Data.ConnectionState.Open)
            {
                conexion.Close();
                return true;
            }
            else
                return false;
        }
        public void Consulta(string query)
        {
            if (m_cnn.State == System.Data.ConnectionState.Open)
            {
                cmd = new SqlCommand(query, m_cnn);
                Dr.Close();
                cmd.CommandType = CommandType.StoredProcedure;
                Dr = cmd.ExecuteReader();
            }
            else
            {
                m_cnn.Open();
                cmd = new SqlCommand(query, m_cnn);
                cmd.CommandType = CommandType.StoredProcedure;
                Dr = cmd.ExecuteReader();
            }
        }

        public void EjecutarParametros(List<SqlParameter> arrayList, string query)
        {
            using (SqlConnection cn = new SqlConnection(m_stringConexion))
            {
                if (cn.State == System.Data.ConnectionState.Open)
                {
                    cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter paramObj in arrayList)
                    {
                        cmd.Parameters.Add(paramObj);
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
                else
                {
                    cn.Open();
                    cmd = new SqlCommand(query, cn);
                    cmd.CommandType = CommandType.StoredProcedure;
                    foreach (SqlParameter paramObj in arrayList)
                    {
                        cmd.Parameters.Add(paramObj);
                    }
                    cmd.ExecuteNonQuery();
                    cmd.Parameters.Clear();
                }
            }
        }
        public SqlDataReader Dr
        {
            get { return m_dr; }
            set { m_dr = value; }
        }

        public void Cerrar()
        {
            Dr.Close();
            m_cnn.Close();
        }

        public SqlConnection Cnn
        {
            get { return m_cnn; }
            set { m_cnn = value; }
        }
    }
}
