using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server
{
    // Database Management Interface.
    public partial class DatabaseHandler : Form
    {
        private static string connectionStrings;
        private static SqlConnection SqlConn;

        public DatabaseHandler()
        {
            InitializeComponent();
        }

        #region Load config and connect to Sql Server.

        private static void LoadConfig()
        {
            ConnectionStringSettings connectionSettings = ConfigurationManager.ConnectionStrings["iChat"];
            connectionStrings = connectionSettings.ConnectionString;
        }

        public static void ConnectToDatabaseServer()
        {
            LoadConfig();

            try
            {
                SqlConn = new SqlConnection(connectionStrings);
            }
            catch (Exception e)
            {
                MessageBox.Show("Can't connect to Sql Server, please re-configure your Sql Server connection!\nMore details:\n" + e.ToString());
            }
        }

        #endregion

        #region Work with Sql Server Database.

        public static DataTable ExecuteSqlQuery(string SqlQueryStatement)
        {
            try
            {
                SqlConn.Open();
                SqlCommand SqlCmd = new SqlCommand(SqlQueryStatement, SqlConn);
                SqlDataAdapter SqlDtA = new SqlDataAdapter(SqlCmd);
                DataTable dtResult = new DataTable();
                SqlDtA.Fill(dtResult);
                SqlConn.Close();
                return dtResult;
            }
            catch
            {
                SqlConn.Close();
                return new DataTable();
            }
        }

        public static bool ExecuteSqlNonQuery(string SqlNonQueryStatement)
        {
            try
            {
                SqlConn.Open();
                SqlCommand SqlCmd = new SqlCommand(SqlNonQueryStatement, SqlConn);
                SqlCmd.CommandType = CommandType.Text;
                int r = SqlCmd.ExecuteNonQuery();
                SqlConn.Close();
                if (r > 0)
                    return true;
                else return false;
            }
            catch
            {
                SqlConn.Close();
                return false;
            }
        }

        public static bool InsertImage(string Username, byte[] Image)
        {
            try
            {
                SqlConn.Open();
                string Statement = "Update Users set Avatar = @img where Username = '" + Username + "'";
                SqlCommand SqlCmd = new SqlCommand(Statement, SqlConn);
                SqlCmd.Parameters.Add(new SqlParameter("@img", Image));
                int r = SqlCmd.ExecuteNonQuery();
                SqlConn.Close();
                if (r > 0)
                    return true;
                else return false;
            }
            catch
            {
                SqlConn.Close();
                return false;
            }
        }

        public static byte[] LoadImage(string Username)
        {
            try
            {
                SqlConn.Open();
                string Statement = "Select Avatar from Users where Username = '" + Username + "'";
                SqlCommand SqlCmd = new SqlCommand(Statement, SqlConn);
                SqlDataReader sr = SqlCmd.ExecuteReader();
                sr.Read();
                byte[] image = null;
                if (sr.HasRows)
                {
                    image = (byte[])sr[0];
                }

                SqlConn.Close();
                return image;
            }
            catch
            {
                SqlConn.Close();
                return null;
            }
        }

#endregion

        private void DatabaseHandler_Load(object sender, EventArgs e)
        {
            
        }

        private void btn_Cancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
