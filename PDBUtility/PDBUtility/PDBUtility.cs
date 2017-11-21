using System;
using System.Data;
using System.Configuration;
using System.Data.OracleClient;
using System.Text;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

namespace PDBUtility
{

    public static class Utility
    {

        #region Declarations

        private static Connection Con = new Connection();
        private static OracleLob CLob;

        private static string RtnString = "";
        private static DataSet RtnDs = new DataSet();

        private static OracleConnection oCon = new OracleConnection();
        private static SqlConnection sCon = new SqlConnection();

        private static OracleTransaction oTrans;
        private static SqlTransaction sTrans;

        private static OracleDataAdapter oDap;
        private static SqlDataAdapter sDap;

        private static OracleCommand oCmd = new OracleCommand();
        private static SqlCommand sCmd = new SqlCommand();

        private static OracleDataReader oDrdr;
        private static SqlDataReader sDrdr;
        
        #endregion

        #region Oracle Data Methods

        /// <summary>
        ///     Return Table Data as DataSet
        /// </summary>
        /// <param name="SqlQuery">string SqlQuery</param>
        /// <returns>DataSet</returns>
        public static DataSet GetOracleData(string SqlQuery)
        {
            DataSet Ds = new DataSet();
            try
            {
                string conname = Con.GetOracleConnection();

                oCon = new OracleConnection(conname);
                oCmd = new OracleCommand();
                oDap = new OracleDataAdapter();
                oCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        oDap = new OracleDataAdapter(SQL[I], oCon);
                        oDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return Ds;
        }

        public static DataSet GetOracleData(string SqlQuery, string ConnString, bool WebConfigConStr)
        {
            DataSet Ds = new DataSet();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;

                oCon = new OracleConnection(Con);
                oCmd = new OracleCommand();
                oDap = new OracleDataAdapter();
                oCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        oDap = new OracleDataAdapter(SQL[I], oCon);
                        oDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return Ds;
        }

        public static DataTable GetOracleDataTable(string SqlQuery)
        {
            DataTable Dt = new DataTable();
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = SqlQuery;
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return Dt;
        }

        public static DataTable GetOracleDataTable(string SqlQuery, string ConnString, bool WebConfigConStr)
        {
            DataTable Dt = new DataTable();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;
                string sqlstr = SqlQuery;
                OracleConnection conn = new OracleConnection(Con);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataAdapter da = new OracleDataAdapter(cmd);
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return Dt;
        }

        /// <summary>
        /// Gets data from Oracle Stored Procedure
        /// </summary>
        /// <param name="PrcName">Stored Procedure Name</param>
        /// <returns>Returns OracleDataReader</returns>
        public static OracleDataReader GetOracleDataByStoredProcedure(string PrcName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.CommandType = CommandType.StoredProcedure;

                oDrdr = oCmd.ExecuteReader();
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return oDrdr;
        }

        public static bool OracleUpdateMethod(string Table, string InputString, string cond)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "update " + Table + " set " + InputString + " where " + cond;

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                int cnt = cmd.ExecuteNonQuery();

                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool OracleUpdateMethod(string Table, string InputString, string cond, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                string sqlstr = "update " + Table + " set " + InputString + " where " + cond;

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                int cnt = cmd.ExecuteNonQuery();

                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool OracleInsertMethod(string Table, string FieldList, string ValueList)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "Insert into " + Table + "(" + FieldList + ") values(" + ValueList + ")";

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool OracleInsertMethod(string Table, string FieldList, string ValueList, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;
                string sqlstr = "Insert into " + Table + "(" + FieldList + ") values(" + ValueList + ")";

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool OracleInsertClob(string Sql, string ClobData)
        {
            try
            {
                string ConStr = Con.GetOracleConnection();
                oCon = new OracleConnection(ConStr);
                byte[] Lob = Encoding.Unicode.GetBytes(ClobData);
                oCon.Open();
                oCmd = new OracleCommand(Sql, oCon);
                oDrdr = oCmd.ExecuteReader();
                oDrdr.Read();
                oTrans = oCon.BeginTransaction();
                CLob = oDrdr.GetOracleLob(0);
                CLob.Write(Lob, 0, Lob.Length);
                oTrans.Commit();
                return true;
            }
            catch (Exception)
            { return false; }
            finally
            {
                oCon.Close();
            }
        }

        public static bool OracleInsertClob(string Sql, string ClobData, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string ConStr = "";
                if (WebConfigConstr)
                    ConStr = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else ConStr = ConnString;
                oCon = new OracleConnection(ConStr);
                byte[] Lob = Encoding.Unicode.GetBytes(ClobData);
                oCon.Open();
                oCmd = new OracleCommand(Sql, oCon);
                oDrdr = oCmd.ExecuteReader();
                oDrdr.Read();
                oTrans = oCon.BeginTransaction();
                CLob = oDrdr.GetOracleLob(0);
                CLob.Write(Lob, 0, Lob.Length);
                oTrans.Commit();
                return true;
            }
            catch (Exception)
            { return false; }
            finally
            {
                oCon.Close();
            }
        }

        public static bool OracleDeleteMethod(string Table, string cond)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "Delete From " + Table + " where " + cond;

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool OracleDeleteMethod(string Table, string cond, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                string sqlstr = "Delete From " + Table + " where " + cond;

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Method OverLoading
        /// </summary>

        /// <summary>
        ///     Return Table Data as DataSet
        /// </summary>
        /// <param name="SqlQuery">string SqlQuery</param>
        /// <returns>DataSet</returns>
        public static string RunOracleProc(string PrcName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = "Success";
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            { oCon.Close(); }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = "Success";
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = "Success";
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = "Success";
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams, DataSet Ds)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    oDap.InsertCommand = oCmd;
                    oDap.Update(Ds, Ds.Tables[0].TableName);

                    oTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    oDap.InsertCommand = oCmd;
                    oDap.Update(Ds, Ds.Tables[0].TableName);

                    oTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams, DataTable Dt)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    oDap.InsertCommand = oCmd;
                    oDap.Update(Dt);

                    oTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleProc(string PrcName, OracleParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    oDap.InsertCommand = oCmd;
                    oDap.Update(Dt);

                    oTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static DataSet RunOracleProc_DataSet(string PrcName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                OracleDataAdapter Da = new OracleDataAdapter();
                Da.SelectCommand = oCmd;
                Da.Fill(RtnDs);
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunOracleProc_DataSet(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                OracleDataAdapter Da = new OracleDataAdapter();
                Da.SelectCommand = oCmd;
                Da.Fill(RtnDs);
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunOracleProc_DataSet(string PrcName, OracleParameter[] oParams)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                OracleDataAdapter Da = new OracleDataAdapter();
                Da.SelectCommand = oCmd;
                Da.Fill(RtnDs);
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunOracleProc_DataSet(string PrcName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                OracleDataAdapter Da = new OracleDataAdapter();
                Da.SelectCommand = oCmd;
                Da.Fill(RtnDs);
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnDs;
        }

        public static string RunOracleFunc(string FunName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleFunc(string FunName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleFunc(string FunName, OracleParameter[] oParams)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                RtnString = oex.Message;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                RtnString = ex.Message;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleFunc(string FunName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                oCmd.ExecuteNonQuery();
                oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleFunc(string FunName, OracleParameter[] oParams, DataSet Ds)
        {
            try
            {

                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.Cursor, 2000,
                   ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));

                if (Ds != null)
                {
                    oDap.SelectCommand = oCmd;
                    oCmd.ExecuteNonQuery();
                    oTrans.Commit();

                }
                //oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static string RunOracleFunc(string FunName, OracleParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
        {
            try
            {

                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);
                oCmd.Transaction = oTrans;
                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter parameter in oParams)
                        oCmd.Parameters.Add(parameter);
                }

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleType.Cursor, 2000,
                   ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));

                if (Ds != null)
                {
                    oDap.SelectCommand = oCmd;
                    oCmd.ExecuteNonQuery();
                    oTrans.Commit();

                }
                //oTrans.Commit();
                RtnString = oCmd.Parameters["ReturnValue"].ToString();
            }
            catch (OracleException oex)
            {
                oTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                oTrans.Rollback();
                throw ex;
            }
            finally
            {
                oCon.Close();
            }
            return RtnString;
        }

        public static OracleParameter MakeOracleInValParam(string ParamName, OracleType DbType, int Size, object Value)
        {
            return MakeOracleValParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static OracleParameter MakeOracleOutValParam(string ParamName, OracleType DbType, int Size)
        {
            return MakeOracleValParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static OracleParameter MakeOracleInOutValParam(string ParamName, OracleType DbType, int Size, object Value)
        {
            return MakeOracleValParam(ParamName, DbType, Size, ParameterDirection.InputOutput, Value);
        }

        public static OracleParameter MakeOracleValParam(string ParamName, OracleType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            OracleParameter param;

            if (Size > 0)
                param = new OracleParameter(ParamName, DbType, Size);
            else
                param = new OracleParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        public static OracleParameter MakeOracleInSrcParam(string ParamName, OracleType DbType, int Size, object Value)
        {
            return MakeOracleSrcParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static OracleParameter MakeOracleSrcParam(string ParamName, OracleType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            OracleParameter param;

            if (Size > 0)
                param = new OracleParameter(ParamName, DbType, Size);
            else
                param = new OracleParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
            {
                param.SourceColumn = Value.ToString();
            }

            return param;
        }

        #endregion

        #region SqlServer Data Methods 

        public static DataSet GetSqlServerData(string SqlQuery)
        {
            DataSet Ds = new DataSet();
            try
            {
                string conname = Con.GetSqlServerConnection();

                sCon = new SqlConnection(conname);
                sCmd = new SqlCommand();
                sDap = new SqlDataAdapter();
                sCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        sDap = new SqlDataAdapter(SQL[I], sCon);
                        sDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                sCon.Close();
            }
            return Ds;
        }

        public static DataSet GetSqlServerData(string SqlQuery, int CommandTimeOut)
        {
            DataSet Ds = new DataSet();
            try
            {
                string conname = Con.GetSqlServerConnection();

                sCon = new SqlConnection(conname);
                sCmd = new SqlCommand();
                sDap = new SqlDataAdapter();
                sCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        sDap = new SqlDataAdapter(SQL[I], sCon);
                        sDap.SelectCommand.CommandTimeout = CommandTimeOut;
                        sDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                sCon.Close();
            }
            return Ds;
        }

        public static DataSet GetSqlServerData(string SqlQuery, string ConnString, bool WebConfigConStr)
        {
            DataSet Ds = new DataSet();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;

                sCon = new SqlConnection(Con);
                sCmd = new SqlCommand();
                sDap = new SqlDataAdapter();
                sCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        sDap = new SqlDataAdapter(SQL[I], sCon);
                        sDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                sCon.Close();
            }
            return Ds;
        }

        public static DataSet GetSqlServerData(string SqlQuery, string ConnString, bool WebConfigConStr, int CommandTimeOut)
        {
            DataSet Ds = new DataSet();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;

                sCon = new SqlConnection(Con);
                sCmd = new SqlCommand();
                sDap = new SqlDataAdapter();
                sCon.Open();

                string[] SQL = null;
                SQL = SqlQuery.Split(Convert.ToChar(";"));
                for (int I = 0; I <= SQL.Length - 1; I++)
                {
                    if (SQL.ToString().Trim() != string.Empty)
                    {
                        DataTable DT = new DataTable();
                        sDap = new SqlDataAdapter(SQL[I], sCon);
                        sDap.SelectCommand.CommandTimeout = CommandTimeOut;
                        sDap.Fill(DT);
                        Ds.Tables.Add(DT);
                    }
                }
            }
            catch (Exception)
            { }
            finally
            {
                sCon.Close();
            }
            return Ds;
        }

        public static DataTable GetSqlServerDataTable(string SqlQuery)
        {
            DataTable Dt = new DataTable();
            try
            {
                string conname = Con.GetSqlServerConnection();
                string sqlstr = SqlQuery;
                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception)
            { }
            return Dt;
        }

        public static DataTable GetSqlServerDataTable(string SqlQuery, int CommandTimeOut)
        {
            DataTable Dt = new DataTable();
            try
            {
                string conname = Con.GetSqlServerConnection();
                string sqlstr = SqlQuery;
                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = CommandTimeOut;
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception)
            { }
            return Dt;
        }

        public static DataTable GetSqlServerDataTable(string SqlQuery, string ConnString, bool WebConfigConStr)
        {
            DataTable Dt = new DataTable();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;
                string sqlstr = SqlQuery;
                SqlConnection conn = new SqlConnection(Con);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception)
            { }
            return Dt;
        }

        public static DataTable GetSqlServerDataTable(string SqlQuery, string ConnString, bool WebConfigConStr, int CommandTimeOut)
        {
            DataTable Dt = new DataTable();
            try
            {
                string Con = "";
                if (WebConfigConStr)
                    Con = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else Con = ConnString;
                string sqlstr = SqlQuery;
                SqlConnection conn = new SqlConnection(Con);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.SelectCommand.CommandTimeout = CommandTimeOut;
                da.Fill(Dt);
                conn.Close();
            }
            catch (Exception)
            { }
            return Dt;
        }

        /// <summary>
        /// Gets data from SqlServer Stored Procedure
        /// </summary>
        /// <param name="PrcName">Stored Procedure Name</param>
        /// <returns>Returns SqlServerDataReader</returns>
        public static SqlDataReader GetSqlServerDataByStoredProcedure(string PrcName)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                SqlCommand oCmd = new SqlCommand(PrcName, sCon);
                sCmd.CommandType = CommandType.StoredProcedure;

                sDrdr = sCmd.ExecuteReader();
            }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); }
            return sDrdr;
        }

        public static bool SqlServerUpdateMethod(string Table, string InputString, string cond)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                string sqlstr = "update " + Table + " set " + InputString + " where " + cond;

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                int cnt = cmd.ExecuteNonQuery();

                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool SqlServerUpdateMethod(string Table, string InputString, string cond, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                string sqlstr = "update " + Table + " set " + InputString + " where " + cond;

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                int cnt = cmd.ExecuteNonQuery();

                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool SqlServerInsertMethod(string Table, string FieldList, string ValueList)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                string sqlstr = "Insert into " + Table + "(" + FieldList + ") values(" + ValueList + ")";

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool SqlServerInsertMethod(string Table, string FieldList, string ValueList, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;
                string sqlstr = "Insert into " + Table + "(" + FieldList + ") values(" + ValueList + ")";

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        //public static bool SqlServerInsertClob(string Sql, string ClobData)
        //{
        //    try
        //    {
        //        string ConStr = Con.GetSqlServerConnection();
        //        sCon = new SqlConnection(ConStr);
        //        byte[] Lob = Encoding.Unicode.GetBytes(ClobData);
        //        sCon.Open();
        //        sCmd = new SqlCommand(Sql, sCon);
        //        sDrdr = sCmd.ExecuteReader();
        //        sDrdr.Read();
        //        sTrans = sCon.BeginTransaction();
        //        CLob = sDrdr..GetSqlServerLob(0);
        //        CLob.Write(Lob, 0, Lob.Length);
        //        sTrans.Commit();
        //        return true;
        //    }
        //    catch (Exception)
        //    { return false; }
        //    finally
        //    {
        //        sCon.Close();
        //    }
        //}

        //public static bool SqlServerInsertClob(string Sql, string ClobData, string ConnString, bool WebConfigConstr)
        //{
        //    try
        //    {
        //        string ConStr = "";
        //        if (WebConfigConstr)
        //            ConStr = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
        //        else ConStr = ConnString;
        //        sCon = new SqlConnection(ConStr);
        //        byte[] Lob = Encoding.Unicode.GetBytes(ClobData);
        //        sCon.Open();
        //        sCmd = new SqlCommand(Sql, sCon);
        //        sDrdr = sCmd.ExecuteReader();
        //        sDrdr.Read();
        //        sTrans = sCon.BeginTransaction();
        //        CLob = sDrdr.GetSqlServerLob(0);
        //        CLob.Write(Lob, 0, Lob.Length);
        //        sTrans.Commit();
        //        return true;
        //    }
        //    catch (Exception)
        //    { return false; }
        //    finally
        //    {
        //        sCon.Close();
        //    }
        //}

        public static bool SqlServerDeleteMethod(string Table, string cond)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                string sqlstr = "Delete From " + Table + " where " + cond;

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static bool SqlServerDeleteMethod(string Table, string cond, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                string sqlstr = "Delete From " + Table + " where " + cond;

                SqlConnection conn = new SqlConnection(conname);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);

                conn.Open();

                int cnt = cmd.ExecuteNonQuery();
                if (cnt == 0)
                {
                    conn.Close();
                    return false;
                }
                else
                {
                    conn.Close();
                    return true;
                }
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        /// <summary>
        /// Method OverLoading
        /// </summary>

        /// <summary>
        ///     Return Table Data as DataSet
        /// </summary>
        /// <param name="SqlQuery">string SqlQuery</param>
        /// <returns>DataSet</returns>
        public static string RunSqlServerProc(string PrcName)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, int CommandTimeOut)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] sParams)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (sParams != null)
                {
                    foreach (SqlParameter parameter in sParams)
                        sCmd.Parameters.Add(parameter);
                }
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] sParams, int CommandTimeOut)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (sParams != null)
                {
                    foreach (SqlParameter parameter in sParams)
                        sCmd.Parameters.Add(parameter);
                }
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = "Success";
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataSet Ds)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Ds, Ds.Tables[0].TableName);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataSet Ds, int CommandTimeOut)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Ds, Ds.Tables[0].TableName);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Ds, Ds.Tables[0].TableName);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                RtnString = oex.Message;
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                RtnString = ex.Message;
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Ds != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Ds, Ds.Tables[0].TableName);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                RtnString = oex.Message;
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                RtnString = ex.Message;
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataTable Dt)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Dt);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataTable Dt, int CommandTimeOut)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Dt);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Dt);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerProc(string PrcName, SqlParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;
                sCmd.CommandTimeout = CommandTimeOut;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                if (Dt != null)
                {
                    sDap.InsertCommand = sCmd;
                    sDap.Update(Dt);

                    sTrans.Commit();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static DataSet RunSqlServerProc_DataSet(string PrcName)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                Da.Fill(RtnDs);
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunSqlServerProc_DataSet(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                Da.Fill(RtnDs);
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunSqlServerProc_DataSet(string PrcName, SqlParameter[] sParams)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (sParams != null)
                {
                    foreach (SqlParameter parameter in sParams)
                        sCmd.Parameters.Add(parameter);
                }
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                Da.Fill(RtnDs);
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnDs;
        }

        public static DataSet RunSqlServerProc_DataSet(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }
                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                Da.Fill(RtnDs);
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnDs;
        }

        public static string RunSqlServerFunc(string FunName)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(FunName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = sCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerFunc(string FunName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(FunName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = sCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerFunc(string FunName, SqlParameter[] oParams)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(FunName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }

                sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = sCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        public static string RunSqlServerFunc(string FunName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(FunName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (SqlParameter parameter in oParams)
                        sCmd.Parameters.Add(parameter);
                }

                sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.VarChar, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                sCmd.ExecuteNonQuery();
                sTrans.Commit();
                RtnString = sCmd.Parameters["ReturnValue"].Value.ToString();
            }
            catch (SqlException oex)
            {
                sTrans.Rollback();
                throw oex;
            }
            catch (Exception ex)
            {
                sTrans.Rollback();
                throw ex;
            }
            finally
            {
                sCon.Close();
            }
            return RtnString;
        }

        //public static string RunSqlServerFunc(string FunName, SqlParameter[] oParams, DataSet Ds)
        //{
        //    try
        //    {

        //        string conname = Con.GetSqlServerConnection();
        //        sCon = new SqlConnection(conname);
        //        sCon.Open();
        //        sTrans = sCon.BeginTransaction();
        //        SqlCommand sCmd = new SqlCommand(FunName, sCon);
        //        sCmd.Transaction = sTrans;
        //        sCmd.CommandType = CommandType.StoredProcedure;

        //        if (oParams != null)
        //        {
        //            foreach (SqlParameter parameter in oParams)
        //                sCmd.Parameters.Add(parameter);
        //        }

        //        sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Cursor, 2000,
        //           ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));

        //        if (Ds != null)
        //        {
        //            sDap.SelectCommand = sCmd;
        //            sCmd.ExecuteNonQuery();
        //            sTrans.Commit();

        //        }
        //        //sTrans.Commit();
        //        RtnString = sCmd.Parameters["ReturnValue"].ToString();
        //    }
        //    catch (SqlException oex)
        //    {
        //        sTrans.Rollback();
        //        throw oex;
        //    }
        //    catch (Exception ex)
        //    {
        //        sTrans.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        sCon.Close();
        //    }
        //    return RtnString;
        //}

        //public static string RunSqlServerFunc(string FunName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
        //{
        //    try
        //    {

        //        string conname = "";
        //        if (WebConfigConstr)
        //            conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
        //        else conname = ConnString;

        //        sCon = new SqlConnection(conname);
        //        sCon.Open();
        //        sTrans = sCon.BeginTransaction();
        //        SqlCommand sCmd = new SqlCommand(FunName, sCon);
        //        sCmd.Transaction = sTrans;
        //        sCmd.CommandType = CommandType.StoredProcedure;

        //        if (oParams != null)
        //        {
        //            foreach (SqlParameter parameter in oParams)
        //                sCmd.Parameters.Add(parameter);
        //        }

        //        sCmd.Parameters.Add(new SqlParameter("ReturnValue", SqlDbType.Cursor, 2000,
        //           ParameterDirection.ReturnValue, false, 0, 0, String.Empty, DataRowVersion.Default, null));

        //        if (Ds != null)
        //        {
        //            sDap.SelectCommand = sCmd;
        //            sCmd.ExecuteNonQuery();
        //            sTrans.Commit();

        //        }
        //        //sTrans.Commit();
        //        RtnString = sCmd.Parameters["ReturnValue"].ToString();
        //    }
        //    catch (SqlException oex)
        //    {
        //        sTrans.Rollback();
        //        throw oex;
        //    }
        //    catch (Exception ex)
        //    {
        //        sTrans.Rollback();
        //        throw ex;
        //    }
        //    finally
        //    {
        //        sCon.Close();
        //    }
        //    return RtnString;
        //}

        public static SqlParameter MakeSqlServerInValParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeSqlServerValParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter MakeSqlServerOutValParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeSqlServerValParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static SqlParameter MakeSqlServerInOutValParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeSqlServerValParam(ParamName, DbType, Size, ParameterDirection.InputOutput, Value);
        }

        public static SqlParameter MakeSqlServerValParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
                param.Value = Value;

            return param;
        }

        public static SqlParameter MakeSqlServerInSrcParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeSqlServerSrcParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter MakeSqlServerSrcParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
        {
            SqlParameter param;

            if (Size > 0)
                param = new SqlParameter(ParamName, DbType, Size);
            else
                param = new SqlParameter(ParamName, DbType);

            param.Direction = Direction;
            if (!(Direction == ParameterDirection.Output && Value == null))
            {
                param.SourceColumn = Value.ToString();
            }

            return param;
        }

        #endregion

        #region Filling Methods

        public static void FillDropdownCode(string Table, string Text, string value, DropDownList DDL, string condition, string type)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                string[] STR = Text.Split(Convert.ToChar("."));
                string[] STR1 = value.Split(Convert.ToChar("."));
                if (STR.Length == 2)
                {
                    if (condition == "" && value != "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + "," + value + " " + STR1[1] + " from " + Table + " order by " + Text + "";
                    }
                    else if (condition == "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " from " + Table + " order by " + Text + "";
                    }
                    else if (condition != "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " from " + Table + " where " + condition + " order by " + Text + "";
                    }
                    else
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + "," + value + " " + STR1[1] + " from " + Table + " where " + condition + " order by " + Text + "";
                    }
                    Text = STR[1].ToString();
                    value = STR1[1].ToString();
                }
                else
                {
                    if (condition == "" && value != "")
                    {
                        sqlstr = "select distinct " + Text + "," + value + " from " + Table + " order by " + Text + "";
                    }
                    else if (condition == "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " from " + Table + " order by " + Text + "";
                    }
                    else if (condition != "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " from " + Table + " where " + condition + " order by " + Text + "";
                    }
                    else
                    {
                        sqlstr = "select distinct " + Text + "," + value + " from " + Table + " where " + condition + " order by " + Text + "";
                    }
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem lst = new ListItem();
                    lst.Text = dr[Text].ToString();
                    if (value == "")
                    { lst.Value = dr[Text].ToString(); }
                    else
                    { lst.Value = dr[value].ToString(); }

                    DDL.Items.Add(lst);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void FillDropdownCodeAlias(string Table, string Text, string value, DropDownList DDL, string condition, string type, string alias)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                string[] STR = Text.Split(Convert.ToChar("."));
                string[] STR1 = value.Split(Convert.ToChar("."));
                if (STR.Length == 2)
                {
                    if (condition == "" && value != "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " " + alias + "," + value + " " + STR1[1] + " from " + Table + " order by " + alias + "";
                    }
                    else if (condition == "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " " + alias + " from " + Table + " order by " + alias + "";
                    }
                    else if (condition != "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " " + alias + " from " + Table + " where " + condition + " order by " + alias + "";
                    }
                    else
                    {
                        sqlstr = "select distinct " + Text + " " + STR[1] + " " + alias + "," + value + " " + STR1[1] + " from " + Table + " where " + condition + " order by " + alias + "";
                    }
                    Text = STR[1].ToString();
                    value = STR1[1].ToString();
                }
                else
                {
                    if (condition == "" && value != "")
                    {
                        sqlstr = "select distinct " + Text + " " + alias + "," + value + " from " + Table + " order by " + alias + "";
                    }
                    else if (condition == "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + alias + " from " + Table + " order by " + alias + "";
                    }
                    else if (condition != "" && value == "")
                    {
                        sqlstr = "select distinct " + Text + " " + alias + " from " + Table + " where " + condition + " order by " + alias + "";
                    }
                    else
                    {
                        sqlstr = "select distinct " + Text + " " + alias + "," + value + " from " + Table + " where " + condition + " order by " + alias + "";
                    }
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem lst = new ListItem();
                    lst.Text = dr[alias].ToString();
                    if (value == "")
                    { lst.Value = dr[alias].ToString(); }
                    else
                    { lst.Value = dr[alias].ToString(); }

                    DDL.Items.Add(lst);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void FillDropdownCode2(string Table, string value, DropDownList DDL, string condition, string type)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                if (condition == "")
                {
                    sqlstr = "select distinct " + value + " from " + Table + " order by " + value + " ";
                }
                else
                {
                    sqlstr = "select distinct " + value + " from " + Table + " where " + condition + " order by " + value + " ";
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem lst = new ListItem();
                    lst.Text = dr[value].ToString();
                    lst.Value = dr[value].ToString();
                    DDL.Items.Add(lst);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }

        }

        public static void FillDropdown(string Table, string value, DropDownList DDL, string condition, string type)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                if (condition == "")
                {
                    sqlstr = "select distinct " + value + " from " + Table + " order by lower(" + value + ") ";
                }
                else
                {
                    sqlstr = "select distinct " + value + " from " + Table + " where " + condition + " order by lower(" + value + ") ";
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem lst = new ListItem();
                    lst.Text = dr[value].ToString();
                    lst.Value = dr[value].ToString();
                    DDL.Items.Add(lst);
                }
                dr.Close();
                conn.Close();
            }
            //catch (Exception ex)
            //{
            //    throw (ex);
            //}
            catch (OracleException oex)
            {
                throw (oex);
            }
        }

        public static void FillListBoxCode(string table, string textfield, string valuefield, ListBox drpList, string condition)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                if (valuefield == "" && condition == "")
                    sqlstr = "select distinct " + textfield + " from " + table + " order by " + textfield + " ";
                else if (valuefield != "" && condition == "")
                    sqlstr = "select distinct " + textfield + "," + valuefield + " from " + table + " order by " + textfield + " ";
                else if (valuefield == "" && condition != "")
                    sqlstr = "select distinct " + textfield + " from " + table + " where " + condition + " order by " + textfield + " ";
                else if (valuefield != "" && condition != "")
                    sqlstr = "select distinct " + textfield + "," + valuefield + " from " + table + " where " + condition + " order by " + textfield + " ";

                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    ListItem lst = new ListItem();
                    lst.Text = dr[textfield].ToString();
                    lst.Value = dr[valuefield].ToString();
                    drpList.Items.Add(lst);
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void FillCheckListBoxCode(string table, string textfield, string valuefield, CheckBoxList chklst, string condition)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                if (condition == "")
                {
                    sqlstr = "select " + textfield + "," + valuefield + " from " + table + " order by lower(" + textfield + ") ";
                }
                else
                {
                    sqlstr = "select " + textfield + "," + valuefield + " from " + table + " where " + condition + " order by lower(" + textfield + ") ";
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    while (dr.Read())
                    {
                        ListItem lst = new ListItem();
                        lst.Text = dr[textfield].ToString();
                        lst.Value = dr[valuefield].ToString();
                        chklst.Items.Add(lst);
                    }
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        public static void FillRadButListBoxCode(string table, string textfield, string valuefield, RadioButtonList chklst, string condition)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                string sqlstr = "";
                if (condition == "")
                {
                    sqlstr = "select " + textfield + "," + valuefield + " from " + table + " order by lower(" + textfield + ") ";
                }
                else
                {
                    sqlstr = "select " + textfield + "," + valuefield + " from " + table + " where " + condition + " order by lower(" + textfield + ") ";
                }
                OracleConnection conn = new OracleConnection(conname);
                OracleCommand cmd = new OracleCommand(sqlstr, conn);
                conn.Open();
                OracleDataReader dr;
                dr = cmd.ExecuteReader();
                while (dr.Read())
                {
                    while (dr.Read())
                    {
                        ListItem lst = new ListItem();
                        lst.Text = dr[textfield].ToString();
                        lst.Value = dr[valuefield].ToString();
                        chklst.Items.Add(lst);
                    }
                }
                dr.Close();
                conn.Close();
            }
            catch (Exception ex)
            {
                throw (ex);
            }
        }

        #endregion

    }

}