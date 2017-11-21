using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Configuration;
using System.Diagnostics;

namespace PDBUtility
{
    public static class SqlUtility
    {
        #region Declarations

        private static Connection Con = new Connection();
        private static DataSet RtnDs = new DataSet();
        private static SqlConnection sCon = new SqlConnection();
        private static SqlTransaction sTrans;
        private static SqlDataAdapter sDap;
        private static SqlCommand sCmd = new SqlCommand();
        private static SqlDataReader sDrdr;
        private static string RtnString = "";

        #endregion

        #region Data Methods

        public static DataSet GetData(string SqlQuery)
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
                sCon.Close(); sCon.Dispose();
            }
            return Ds;
        }

        public static DataSet GetData(string SqlQuery, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
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
                sCon.Close(); sCon.Dispose();
            }
            return Ds;
        }

        public static DataSet GetData(string SqlQuery, string ConnString, bool WebConfigConStr, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return Ds;
        }

        public static DataSet GetDataByProcedure(string PrcName)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, int CommandTimeOut)
        {
            try
            {
                RtnDs = new DataSet();
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                SqlCommand sCmd = new SqlCommand(PrcName, sCon);
                sCmd.Transaction = sTrans;
                sCmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter Da = new SqlDataAdapter();
                Da.SelectCommand = sCmd;
                Da.SelectCommand.CommandTimeout = CommandTimeOut;
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                RtnDs = new DataSet();
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
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                RtnDs = new DataSet();
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
                Da.SelectCommand.CommandTimeout = CommandTimeOut;
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, SqlParameter[] sParams)
        {
            try
            {
                RtnDs = new DataSet();
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
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, SqlParameter[] sParams, int CommandTimeOut)
        {
            try
            {
                RtnDs = new DataSet();
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
                Da.SelectCommand.CommandTimeout = CommandTimeOut;
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
        {
            try
            {
                RtnDs = new DataSet();
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
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataSet GetDataByProcedure(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                RtnDs = new DataSet();
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
                Da.SelectCommand.CommandTimeout = CommandTimeOut;
                sTrans.Commit();
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
                sTrans.Dispose(); sCon.Close(); sCon.Dispose();
            }
            return RtnDs;
        }

        public static DataTable GetDataTable(string SqlQuery)
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
                conn.Dispose();
            }
            catch (Exception)
            { }
            return Dt;
        }

        public static DataTable GetDataTable(string SqlQuery, int CommandTimeOut)
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
                conn.Dispose();
            }
            catch (Exception)
            { }
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
                SqlConnection conn = new SqlConnection(Con);
                SqlCommand cmd = new SqlCommand(sqlstr, conn);
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(Dt);
                conn.Close();
                conn.Dispose();
            }
            catch (Exception)
            { }
            return Dt;
        }

        public static DataTable GetDataTable(string SqlQuery, string ConnString, bool WebConfigConStr, int CommandTimeOut)
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
                conn.Dispose();
            }
            catch (Exception)
            { }
            return Dt;
        }

        /// <summary>
        /// Gets data from  Stored Procedure
        /// </summary>
        /// <param name="PrcName">Stored Procedure Name</param>
        /// <returns>Returns DataReader</returns>
        public static SqlDataReader GetDataByStoredProcedure(string PrcName)
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
            { sCon.Close(); sCon.Dispose(); }
            return sDrdr;
        }

        public static bool UpdateMethod(string Table, string InputString, string cond)
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

        public static bool UpdateMethod(string Table, string InputString, string cond, string ConnString, bool WebConfigConstr)
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

        public static bool InsertMethod(string Table, string FieldList, string ValueList)
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

        public static bool InsertMethod(string Table, string FieldList, string ValueList, string ConnString, bool WebConfigConstr)
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

        //public static bool InsertClob(string Sql, string ClobData)
        //{
        //    try
        //    {
        //        string ConStr = Con.GetConnection();
        //        sCon = new SqlConnection(ConStr);
        //        byte[] Lob = Encoding.Unicode.GetBytes(ClobData);
        //        sCon.Open();
        //        sCmd = new SqlCommand(Sql, sCon);
        //        sDrdr = sCmd.ExecuteReader();
        //        sDrdr.Read();
        //        sTrans = sCon.BeginTransaction();
        //        CLob = sDrdr..GetLob(0);
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

        //public static bool InsertClob(string Sql, string ClobData, string ConnString, bool WebConfigConstr)
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
        //        CLob = sDrdr.GetLob(0);
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

        public static bool DeleteMethod(string Table, string cond)
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

        public static bool DeleteMethod(string Table, string cond, string ConnString, bool WebConfigConstr)
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
        public static string RunProc(string PrcName)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, string ConnString, bool WebConfigConstr, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] sParams)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] sParams, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataSet Ds)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataSet Ds, int CommandTimeOut)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                sTrans = sCon.BeginTransaction();
                sDap = new SqlDataAdapter();
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
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
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr, int CommandTimeOut)
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
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataTable Dt)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataTable Dt, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProc(string PrcName, SqlParameter[] oParams, DataTable Dt, string ConnString, bool WebConfigConstr, int CommandTimeOut)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProcWithDataTable(string PrcName, string sParam, DataTable Dt)
        {
            try
            {
                string conname = Con.GetSqlServerConnection();
                sCon = new SqlConnection(conname);
                sCon.Open();
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (Dt != null)
                {
                    sCmd.Parameters.AddWithValue(sParam, Dt);
                    sCmd.ExecuteNonQuery();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                throw oex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProcWithDataTable(string PrcName, string sParam, DataTable Dt, string ConnString, bool WebConfigConstr)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;
                sCon = new SqlConnection(conname);
                sCon.Open();
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (Dt != null)
                {
                    sCmd.Parameters.AddWithValue(sParam, Dt);
                    sCmd.ExecuteNonQuery();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                throw oex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunProcWithDataTable(string PrcName, string sParam, DataTable Dt, string ConnString, bool WebConfigConstr, int CommandTimeOut)
        {
            try
            {
                string conname = "";
                if (WebConfigConstr)
                    conname = ConfigurationManager.ConnectionStrings[ConnString].ConnectionString;
                else conname = ConnString;
                sCon = new SqlConnection(conname);
                sCon.Open();
                SqlCommand sCmd = new SqlCommand();
                sCmd.Connection = sCon;
                sCmd.CommandText = PrcName;
                sCmd.CommandTimeout = CommandTimeOut;
                sCmd.CommandType = CommandType.StoredProcedure;

                if (Dt != null)
                {
                    sCmd.Parameters.AddWithValue(sParam, Dt);
                    sCmd.ExecuteNonQuery();
                    RtnString = "Success";
                }
            }
            catch (SqlException oex)
            {
                throw oex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName)
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
                sCon.Close(); sCon.Dispose();
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, SqlParameter[] oParams)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        public static string RunFunc(string FunName, SqlParameter[] oParams, string ConnString, bool WebConfigConstr)
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
                sCon.Close(); sCon.Dispose();
            }
            return RtnString;
        }

        //public static string RunFunc(string FunName, SqlParameter[] oParams, DataSet Ds)
        //{
        //    try
        //    {

        //        string conname = Con.GetConnection();
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

        //public static string RunFunc(string FunName, SqlParameter[] oParams, DataSet Ds, string ConnString, bool WebConfigConstr)
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

        public static SqlParameter MakeInValParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter MakeOutValParam(string ParamName, SqlDbType DbType, int Size)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.Output, null);
        }

        public static SqlParameter MakeInOutValParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeValParam(ParamName, DbType, Size, ParameterDirection.InputOutput, Value);
        }

        public static SqlParameter MakeValParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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

        public static SqlParameter MakeInSrcParam(string ParamName, SqlDbType DbType, int Size, object Value)
        {
            return MakeSrcParam(ParamName, DbType, Size, ParameterDirection.Input, Value);
        }

        public static SqlParameter MakeSrcParam(string ParamName, SqlDbType DbType, Int32 Size, ParameterDirection Direction, object Value)
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

        public static void SaveException(Exception exception, string HostName, string UserId, string ApplicationName)
        {
            try
            {
                using (SqlConnection con = new SqlConnection(Con.GetSqlServerConnection()))
                {
                    SqlParameter[] par = new SqlParameter[9];

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
                                par[0] = new SqlParameter("@FileName", stackFrame.FileName.ToString());
                                par[1] = new SqlParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new SqlParameter("@LineNumber", int.Parse(stackFrame.LineNumber.ToString()));
                                par[3] = new SqlParameter("@ColumnNumber", int.Parse(stackFrame.ColumnNumber.ToString()));
                                par[4] = new SqlParameter("@Class", stackFrame.Class.ToString());
                                par[5] = new SqlParameter("@Exception", exception.Message);
                                par[6] = new SqlParameter("@InnerException", exception.ToString());
                                par[7] = new SqlParameter("@HostName", HostName);
                                par[8] = new SqlParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, Con.GetSqlServerConnection(), false);
                            }
                            catch (Exception)
                            {
                                par[0] = new SqlParameter("@FileName", string.Empty);
                                par[1] = new SqlParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new SqlParameter("@LineNumber", 0);
                                par[3] = new SqlParameter("@ColumnNumber", 0);
                                par[4] = new SqlParameter("@Class", string.Empty);
                                par[5] = new SqlParameter("@Exception", exception.Message);
                                par[6] = new SqlParameter("@InnerException", exception.ToString());
                                par[7] = new SqlParameter("@HostName", HostName);
                                par[8] = new SqlParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, Con.GetSqlServerConnection(), false);
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
                using (SqlConnection con = new SqlConnection(conname))
                {
                    SqlParameter[] par = new SqlParameter[9];

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
                                par[0] = new SqlParameter("@FileName", stackFrame.FileName.ToString());
                                par[1] = new SqlParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new SqlParameter("@LineNumber", int.Parse(stackFrame.LineNumber.ToString()));
                                par[3] = new SqlParameter("@ColumnNumber", int.Parse(stackFrame.ColumnNumber.ToString()));
                                par[4] = new SqlParameter("@Class", stackFrame.Class.ToString());
                                par[5] = new SqlParameter("@Exception", exception.Message);
                                par[6] = new SqlParameter("@InnerException", exception.ToString());
                                par[7] = new SqlParameter("@HostName", HostName);
                                par[8] = new SqlParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, Con.GetSqlServerConnection(), false);
                            }
                            catch (Exception)
                            {
                                par[0] = new SqlParameter("@FileName", string.Empty);
                                par[1] = new SqlParameter("@Method", stackFrame.Method.ToString());
                                par[2] = new SqlParameter("@LineNumber", 0);
                                par[3] = new SqlParameter("@ColumnNumber", 0);
                                par[4] = new SqlParameter("@Class", string.Empty);
                                par[5] = new SqlParameter("@Exception", exception.Message);
                                par[6] = new SqlParameter("@InnerException", exception.ToString());
                                par[7] = new SqlParameter("@HostName", HostName);
                                par[8] = new SqlParameter("@CreatedBy", UserId);
                                //SQLHelper.ExecuteNonQuery(con, CommandType.StoredProcedure, "sp_Exception_Insert_Tracker", par);
                                RunProc("sp_Exception_Insert_Tracker", par, Con.GetSqlServerConnection(), false);
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