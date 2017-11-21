using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//using System.Data.OracleClient;
using System.Data;
using System.Configuration;
using Oracle.DataAccess.Client;
using Oracle.DataAccess.Types;
using System.Diagnostics;

namespace PDBUtility
{
    public static class OraUtility
    {
        #region Declarations

        private static Connection Con = new Connection();
        private static DataSet RtnDs = new DataSet();
        private static OracleClob CLob;
        private static OracleConnection oCon;
        private static OracleTransaction oTrans;
        private static OracleDataAdapter oDap;
        private static OracleCommand oCmd = new OracleCommand();
        private static OracleDataReader oDrdr;
        private static string RtnString = "";

        #endregion

        #region CheckMethods

        public static bool CheckOracleConnection()
        {
            try
            {
                string ConStr = ConfigurationManager.ConnectionStrings["OracleConnection"].ConnectionString;
                oCon = new OracleConnection(ConStr);
                oCon.Open();
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

        public static bool CheckOracleConnection(string Constr)
        {
            try
            {
                oCon = new OracleConnection();
                oCon.ConnectionString = Constr;
                oCon.Open();
                return true;
            }
            catch (Exception ex)
            {
                string msg = ex.Message;
                return false;
            }
        }

        #endregion

        #region Data Methods

        /// <summary>
        ///     Return Table Data as DataSet
        /// </summary>
        /// <param name="SqlQuery">string SqlQuery</param>
        /// <returns>DataSet</returns>
        public static DataSet GetData(string SqlQuery)
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
            { oCon.Close(); oCon.Dispose(); }
            return Ds;
        }

        public static DataSet GetData(string SqlQuery, string ConnString, bool WebConfigConStr)
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
            catch (OracleException OEx)
            { throw new Exception(OEx.Message); }
            catch (Exception Ex)
            { throw new Exception(Ex.Message); }
            finally
            { oCon.Close(); oCon.Dispose(); }
            return Ds;
        }

        public static DataTable GetDataTable(string SqlQuery)
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
            { oCon.Close(); oCon.Dispose(); }
            return Dt;
        }

        public static DataTable GetDataTable(string SqlQuery, string ConnString, bool WebConfigConStr)
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
            { oCon.Close(); oCon.Dispose(); }
            return Dt;
        }

        /// <summary>
        /// Gets data from  Stored Procedure
        /// </summary>
        /// <param name="PrcName">Stored Procedure Name</param>
        /// <returns>Returns DataReader</returns>
        public static OracleDataReader GetDataByStoredProcedure(string PrcName)
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
            { oCon.Close(); oCon.Dispose(); }
            return oDrdr;
        }

        public static bool UpdateMethod(string Table, string InputString, string cond)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        public static bool UpdateMethod(string Table, string InputString, string cond, string ConnString, bool WebConfigConstr)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        public static bool InsertMethod(string Table, string FieldList, string ValueList)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        public static bool InsertMethod(string Table, string FieldList, string ValueList, string ConnString, bool WebConfigConstr)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        public static bool InsertClob(string Sql, string ClobData)
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
                CLob = oDrdr.GetOracleClob(0);
                CLob.Write(Lob, 0, Lob.Length);
                oTrans.Commit();
                return true;
            }
            catch (OracleException)
            { return false; }
            finally
            {
                oCon.Close(); oCon.Dispose();
            }
        }

        public static bool InsertClob(string Sql, string ClobData, string ConnString, bool WebConfigConstr)
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
                CLob = oDrdr.GetOracleClob(0);
                CLob.Write(Lob, 0, Lob.Length);
                oTrans.Commit();
                return true;
            }
            catch (OracleException)
            { return false; }
            finally
            {
                oCon.Close(); oCon.Dispose();
            }
        }

        public static bool DeleteMethod(string Table, string cond)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        public static bool DeleteMethod(string Table, string cond, string ConnString, bool WebConfigConstr)
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
            catch (OracleException ex)
            {
                throw (ex);
            }
            finally { oCon.Dispose(); }
        }

        /// <summary>
        /// Method OverLoading
        /// </summary>

        /// <summary>
        ///     Return Table Data as DataSet
        /// </summary>
        /// <param name="SqlQuery">string SqlQuery</param>
        /// <returns>DataSet</returns>
        public static string RunProc(string PrcName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);
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
            { oCon.Close(); oCon.Dispose(); }
            return RtnString;
        }

        public static string RunProc(string PrcName, string ConnString, bool WebConfigConstr)
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams, DataSet Ds)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams, DataTable Dt)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                oDap = new OracleDataAdapter();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, OracleParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static DataSet RunProc_DataSet(string PrcName)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

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
                oCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet RunProc_DataSet(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

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
                oCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet RunProc_DataSet(string PrcName, OracleParameter[] oParams)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet RunProc_DataSet(string PrcName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;

                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(PrcName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
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
                oCon.Dispose();
            }
            return RtnDs;
        }
        
        public static string RunFunc(string FunName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, OracleParameter[] oParams)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
                }

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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, OracleParameter[] oParams, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
                }
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, OracleParameter[] oParams, DataSet Ds)
        {
            try
            {

                string conname = Con.GetOracleConnection();
                oCon = new OracleConnection(conname);
                oCon.Open();
                oTrans = oCon.BeginTransaction();
                OracleCommand oCmd = new OracleCommand(FunName, oCon);

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
                }

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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, OracleParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
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

                oCmd.CommandType = CommandType.StoredProcedure;

                oCmd.Parameters.Add(new OracleParameter("ReturnValue", OracleDbType.Varchar2, 50,
                   ParameterDirection.ReturnValue, false, 0, 0, string.Empty, DataRowVersion.Default, null));
                if (oParams != null)
                {
                    foreach (OracleParameter OracleParameter in oParams)
                        oCmd.Parameters.Add(OracleParameter);
                }
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
                oCon.Close(); oCon.Dispose();
            }
            return RtnString;
        }

        public static OracleParameter MakeInValParam(string ParamName, OracleDbType DbType, int Size, object Value)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static OracleParameter MakeOutValParam(string ParamName, OracleDbType DbType, int Size)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static OracleParameter MakeInOutValParam(string ParamName, OracleDbType DbType, int Size, object Value)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.InputOutput, Value);
        }

        public static OracleParameter MakeValParam(string ParamName, OracleDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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

        public static OracleParameter MakeInSrcParam(string ParamName, OracleDbType DbType, int Size, object Value)
        {
            return MakeSrcParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static OracleParameter MakeSrcParam(string ParamName, OracleDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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

        public static void SaveException(Exception exception, string HostName, string UserId, string ApplicationName)
        {
            try
            {
                string conname = Con.GetOracleConnection();
                using (OracleConnection con = new OracleConnection(conname))
                {
                    OracleParameter[] par = new OracleParameter[9];

                    //Get a StackTrace object for the exception
                    var stackTrace = new StackTrace(exception, true);
                    //Get StackFrames
                    var stackFrames = stackTrace.GetFrames()
                                  .Select(frame => new
                                  {
                                      FileName = frame.GetFileName(),               //File name
                                      LineNumber = frame.GetFileLineNumber(),       //Line number
                                      ColumnNumber = frame.GetFileColumnNumber(),   //Column number
                                      Method = frame.GetMethod().Name,              //Method name
                                      Class = frame.GetMethod().DeclaringType,      //Class name
                                  });

                    foreach (var stackFrame in stackFrames)
                    {
                        if (stackFrame.FileName != null)
                        {
                            try
                            {
                                par[0] = new OracleParameter("@FileName", stackFrame.FileName.ToString());
                                par[1] = new OracleParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new OracleParameter("@LineNumber", int.Parse(stackFrame.LineNumber.ToString()));
                                par[3] = new OracleParameter("@ColumnNumber", int.Parse(stackFrame.ColumnNumber.ToString()));
                                par[4] = new OracleParameter("@Class", stackFrame.Class.ToString());
                                par[5] = new OracleParameter("@Exception", exception.Message);
                                par[6] = new OracleParameter("@InnerException", exception.ToString());
                                par[7] = new OracleParameter("@HostName", HostName);
                                par[8] = new OracleParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, conname, false);
                            }
                            catch (Exception)
                            {
                                par[0] = new OracleParameter("@FileName", string.Empty);
                                par[1] = new OracleParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new OracleParameter("@LineNumber", 0);
                                par[3] = new OracleParameter("@ColumnNumber", 0);
                                par[4] = new OracleParameter("@Class", string.Empty);
                                par[5] = new OracleParameter("@Exception", exception.Message);
                                par[6] = new OracleParameter("@InnerException", exception.ToString());
                                par[7] = new OracleParameter("@HostName", HostName);
                                par[8] = new OracleParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, conname, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ApplicationName, "Log File Path : " + ex.ToString());
            }
        }

        public static void SaveException(Exception exception, string HostName, string UserId, string ApplicationName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;
                using (OracleConnection con = new OracleConnection(conname))
                {
                    OracleParameter[] par = new OracleParameter[9];

                    //Get a StackTrace object for the exception
                    var stackTrace = new StackTrace(exception, true);
                    //Get StackFrames
                    var stackFrames = stackTrace.GetFrames()
                                  .Select(frame => new
                                  {
                                      FileName = frame.GetFileName(),               //File name
                                      LineNumber = frame.GetFileLineNumber(),       //Line number
                                      ColumnNumber = frame.GetFileColumnNumber(),   //Column number
                                      Method = frame.GetMethod().Name,              //Method name
                                      Class = frame.GetMethod().DeclaringType,      //Class name
                                  });

                    foreach (var stackFrame in stackFrames)
                    {
                        if (stackFrame.FileName != null)
                        {
                            try
                            {
                                par[0] = new OracleParameter("@FileName", stackFrame.FileName.ToString());
                                par[1] = new OracleParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new OracleParameter("@LineNumber", int.Parse(stackFrame.LineNumber.ToString()));
                                par[3] = new OracleParameter("@ColumnNumber", int.Parse(stackFrame.ColumnNumber.ToString()));
                                par[4] = new OracleParameter("@Class", stackFrame.Class.ToString());
                                par[5] = new OracleParameter("@Exception", exception.Message);
                                par[6] = new OracleParameter("@InnerException", exception.ToString());
                                par[7] = new OracleParameter("@HostName", HostName);
                                par[8] = new OracleParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, conname, false);
                            }
                            catch (Exception)
                            {
                                par[0] = new OracleParameter("@FileName", string.Empty);
                                par[1] = new OracleParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new OracleParameter("@LineNumber", 0);
                                par[3] = new OracleParameter("@ColumnNumber", 0);
                                par[4] = new OracleParameter("@Class", string.Empty);
                                par[5] = new OracleParameter("@Exception", exception.Message);
                                par[6] = new OracleParameter("@InnerException", exception.ToString());
                                par[7] = new OracleParameter("@HostName", HostName);
                                par[8] = new OracleParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, conname, false);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.EventLog.WriteEntry(ApplicationName, "Log File Path : " + ex.ToString());
            }
        }

        #endregion
    }
}