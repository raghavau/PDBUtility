using System;
using System.Data;
//using System.Data.OracleClient;
using Oracle.DataAccess.Client;
using System.Configuration;
using System.Data.SqlClient;

namespace PDBUtility
{

    public class Connection
    {

        OracleConnection OCon;
        SqlConnection SCon = new SqlConnection();

        public string GetOracleConnection()
        {
            string ConStr = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
            return ConStr;
        }

        public string GetSqlServerConnection()
        {
            string ConStr = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
            //string ConStr = "Data Source=RAGHAVENDRA-PC;Initial Catalog=Student;User ID=sa;Password=raghava1986";
            return ConStr;
        }

        public string GetOracleConnection(string DataSource, string UserId, string Password)
        {
            string ConStr = "Data Source=" + DataSource + ";User Id=" + UserId + ";Password=" + Password;
            return ConStr;
        }
        
        public string GetSqlServerConnection(string DataSource,string InitialCatalog, string UserId, string Password)
        {
            string ConStr = "Data Source=" + DataSource + "Initial Catalog=" + InitialCatalog + ";User Id=" + UserId + ";Password=" + Password;
            return ConStr;
        }

        public void CloseOracleConnection()
        {
            OCon.Close();
        }

        public void CloseSqlServerConnection()
        {
            SCon.Close();
        }

        public bool CheckOracleConnection()
        {
            try
            {
                string ConStr = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                OCon = new OracleConnection(ConStr);
                OCon.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckSqlServerConnection()
        {
            try
            {
                string ConStr = ConfigurationManager.ConnectionStrings["SqlConnection"].ConnectionString;
                SCon = new SqlConnection(ConStr);
                SCon.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckOracleConnection(string Constr)
        {
            try
            {
                OCon = new OracleConnection();
                OCon.ConnectionString = Constr;
                OCon.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public bool CheckSqlServerConnection(string Constr)
        {
            try
            {
                SCon = new SqlConnection(Constr);
                SCon.Open();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}