using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
//using CrystalDecisions.CrystalReports.Engine;
//using CrystalDecisions.Shared;

namespace BLL_SMS.DAL
{
    /// <summary>
    /// Summary description for DBBridge.
    /// </summary>
    [Serializable]
    public class DBBridge
    {
        public DBBridge()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        /// <summary>
        /// [METHOD] connect to the db
        /// </summary>
        /// <returns>TRUE, if it is saved</returns>
        /// 
        public static string DBConnection()
        {
            try
            {
                return "Data Source=DESKTOP-6CO4517;Initial Catalog=ERP;User ID=umarfarooq;Password=farooque12";  //ConfigurationSettings.AppSettings["ERPConStr"].ToString();

            }
            catch (Exception ce)
            {

               throw new ApplicationException("Unable to get DB Connection string from Config File. Contact Administrator" + ce);
            }
        }
       
        //for crystal report
        //public ConnectionInfo RptDBConnection()
        //{
        //    try
        //    {
        //        ConnectionInfo myConnectionInfo = new ConnectionInfo();
        //        myConnectionInfo.ServerName = "ZAHIDA-PC";
        //        myConnectionInfo.DatabaseName = "HIMS";
        //        myConnectionInfo.UserID = "sa";
        //        myConnectionInfo.Password = "123456";
        //        return myConnectionInfo;
        //    }
        //    catch (Exception ce)
        //    {
        //        throw new ApplicationException("Unable to get DB Connection string from Config File. Contact Administrator" + ce);
        //    }
        //}


        public int ExecuteNonQuery(string storedProcedure, SqlParameter[] param)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(DBConnection(), CommandType.StoredProcedure, storedProcedure, param);
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public int ExecuteNonQuerywithTrans(string storedProcedure, SqlParameter[] param)
        {
            SqlConnection conTrans = new SqlConnection(DBConnection());
            SqlTransaction sqlTrans;
            conTrans.Open();
            int returnResult = 0;
            using (sqlTrans = conTrans.BeginTransaction())
            {
                try
                {
                    returnResult = SqlHelper.ExecuteNonQuery(sqlTrans, CommandType.StoredProcedure, storedProcedure, param);
                    sqlTrans.Commit();
                    return returnResult;
                }
                catch (Exception sq)
                {
                    sqlTrans.Rollback();
                    throw sq;
                }
                finally
                {
                    conTrans.Close();
                }
            }
        }

        public int ExecuteNonQuery(string storedProcedure)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(DBConnection(), CommandType.StoredProcedure, storedProcedure);
            }
            catch (SqlException sq)
            {
               throw sq;
            }
        }

        public int ExecuteNonQuerySQL(string sqlquery)
        {
            try
            {
                return SqlHelper.ExecuteNonQuery(DBConnection(), CommandType.Text, sqlquery);
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public int ExecuteNonQuerywithTrans(string storedProcedure)
        {
            SqlConnection conTrans = new SqlConnection(DBConnection());
            SqlTransaction sqlTrans;
            conTrans.Open();
            int returnResult = 0;
            using (sqlTrans = conTrans.BeginTransaction())
            {
                try
                {
                    returnResult = SqlHelper.ExecuteNonQuery(sqlTrans, CommandType.StoredProcedure, storedProcedure);
                    sqlTrans.Commit();
                    return returnResult;
                }
                catch (Exception sq)
                {
                    sqlTrans.Rollback();
                    throw sq;
                }
                finally
                {
                    conTrans.Close();
                }
            }
        }

        public int ExecuteNonQuerywithTransfromFrontEnd(SqlTransaction sqlTrans, string storedProcedure, SqlParameter[] param)
        {
            try
            {
                int returnResult = SqlHelper.ExecuteNonQuery(sqlTrans, CommandType.StoredProcedure, storedProcedure, param);
                return returnResult;
            }
            catch (SqlException sq)
            {
               sqlTrans.Rollback();
               throw sq;
            }
        }

        public DataSet ExecuteDataset(string storedProcedure, SqlParameter[] param)
        {
            try
            {
                string con = "Data Source=DESKTOP-6CO4517;Initial Catalog=ERP;User ID=umarfarooq;Password=farooque12";
                return SqlHelper.ExecuteDataset(con, CommandType.StoredProcedure, storedProcedure, param);
            }
            catch (SqlException sq)
            {
                    throw sq;
            }
        }

        public DataSet ExecuteDatasetSQL(string storedProcedure)
        {
            try
            {
                return SqlHelper.ExecuteDataset(DBConnection(), CommandType.Text, storedProcedure);
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public DataSet ExecuteDataset(string storedProcedure)
        {
            try
            {
                return SqlHelper.ExecuteDataset(DBConnection(), CommandType.StoredProcedure, storedProcedure);
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public object ExecuteScalar(string storedProcedure, SqlParameter[] param)
        {
            try
            {
                return SqlHelper.ExecuteScalar(DBConnection(), CommandType.StoredProcedure, storedProcedure, param);
            }
            catch (SqlException sq)
            {
               throw sq;
            }
        }

        public object ExecuteScalar(SqlTransaction sqlTrans, string storedProcedure, SqlParameter[] param)
        {
            try
            {
                return SqlHelper.ExecuteScalar(sqlTrans, CommandType.StoredProcedure, storedProcedure, param);
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public SqlDataReader ExecuteReader(string storedProcedure, SqlParameter[] param)
        {
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(DBConnection(), CommandType.StoredProcedure, storedProcedure, param);
                return reader;
            }
            catch (SqlException sq)
            {
               throw sq;
            }
        }

        public SqlDataReader ExecuteReader(string storedProcedure)
        {
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(DBConnection(), CommandType.StoredProcedure, storedProcedure);
                return reader;
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public SqlDataReader ExecuteReaderSQL(string sqlquery)
        {
            SqlDataReader reader = null;
            try
            {
                reader = SqlHelper.ExecuteReader(DBConnection(), CommandType.Text, sqlquery);
                return reader;
            }
            catch (SqlException sq)
            {
                throw sq;
            }
        }

        public int ExecuteNonQuerywithMultipleTrans(SqlTransaction sqlTrans, string storedProcedure)
        {
            int returnResult = 0;
            try
            {
                returnResult = SqlHelper.ExecuteNonQuery(sqlTrans, CommandType.StoredProcedure, storedProcedure);
                return returnResult;
            }
            catch (Exception sq)
            {
                throw sq;
            }
        }

        public int ExecuteNonQuerywithMultipleTrans(SqlTransaction sqlTrans, string storedProcedure, SqlParameter[] param)
        {
            int returnResult = 0;
            try
            {
                returnResult = SqlHelper.ExecuteNonQuery(sqlTrans, CommandType.StoredProcedure, storedProcedure, param);
                return returnResult;
            }
            catch (Exception sq)
            {
                throw sq;
            }
        }


       
    }
}

