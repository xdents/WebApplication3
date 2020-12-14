using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

namespace WebApplication3
{
    public class SqlHelper
    {
        //Database connection strings
        public static readonly string CONN_STRING = ConfigurationManager.ConnectionStrings["HRSQLConnectionString"].ToString();

        /// <summary>
        /// 批量操作每批次记录数
        /// </summary>
        public static int BatchSize = 2000;

        /// <summary>
        /// 超时时间
        /// </summary>
        public static int CommandTimeOut = 30000000;
        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>A SqlDataReader containing the results</returns>
        public static SqlDataReader ExecuteReader(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);

            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return rdr;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>A DataTable containing the results</returns>
        public static DataTable ExecuteDataTable(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            cmd.CommandTimeout = 30000000;
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtReturn = new DataTable();
                da.Fill(dtReturn);

                //SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dtReturn;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static DataSet GetDataTablesByStore(string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            cmd.CommandTimeout = 30000000;
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, cmdParms);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                if (ds.Tables.Count > 1)
                {
                    for (int i = 1; i < ds.Tables.Count; i++)
                    {
                        ds.Tables[0].Merge(ds.Tables[i]);
                    }

                }

                //SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }

        public static DataSet GetDataTablesByStore(string cmdText)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            cmd.CommandTimeout = 30000000;
            try
            {
                PrepareCommand(cmd, conn, null, CommandType.Text, cmdText, null);
                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapter.Fill(ds);
                return ds;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        /// <summary>
        /// Execute a SqlCommand that returns a resultset against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>A DataTable containing the results</returns>
        public static DataSet ExecuteDataSet(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();
            SqlConnection conn = new SqlConnection(CONN_STRING);
            cmd.CommandTimeout = 30000000;
            // we use a try/catch here because if the method throws an exception we want to 
            // close the connection throw code, because no datareader will exist, hence the 
            // commandBehaviour.CloseConnection will not work
            try
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataSet dtReturn = new DataSet();
                da.Fill(dtReturn);

                //SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
                cmd.Parameters.Clear();
                return dtReturn;
            }
            catch
            {
                conn.Close();
                throw;
            }
        }


        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against the database specified in the connection string 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {
            SqlCommand cmd = new SqlCommand();

            using (SqlConnection conn = new SqlConnection(CONN_STRING))
            {
                PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
                object val = cmd.ExecuteScalar();
                //cmd.Parameters.Clear();
                return val;
            }
        }

        /// <summary>
        /// Execute a SqlCommand that returns the first column of the first record against an existing database connection 
        /// using the provided parameters.
        /// </summary>
        /// <remarks>
        /// e.g.:  
        ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
        /// </remarks>
        /// <param name="conn">an existing database connection</param>
        /// <param name="cmdType">the CommandType (stored procedure, text, etc.)</param>
        /// <param name="cmdText">the stored procedure name or T-SQL command</param>
        /// <param name="cmdParms">an array of SqlParamters used to execute the command</param>
        /// <returns>An object that should be converted to the expected type using Convert.To{Type}</returns>
        public static object ExecuteScalar(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] cmdParms)
        {

            SqlCommand cmd = new SqlCommand();

            PrepareCommand(cmd, conn, null, cmdType, cmdText, cmdParms);
            object val = cmd.ExecuteScalar();
            cmd.Parameters.Clear();
            return val;
        }


        /// <summary>
        /// Prepare a command for execution
        /// </summary>
        /// <param name="cmd">SqlCommand object</param>
        /// <param name="conn">SqlConnection object</param>
        /// <param name="trans">SqlTransaction object</param>
        /// <param name="cmdType">Cmd type e.g. stored procedure or text</param>
        /// <param name="cmdText">Command text, e.g. Select * from Products</param>
        /// <param name="cmdParms">SqlParameters to use in the command</param>
        private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
        {

            if (conn.State != ConnectionState.Open)
                conn.Open();

            cmd.Connection = conn;
            cmd.CommandText = cmdText;

            if (trans != null)
                cmd.Transaction = trans;

            cmd.CommandType = cmdType;

            if (cmdParms != null)
            {
                foreach (SqlParameter parm in cmdParms)
                    cmd.Parameters.Add(parm);
            }

        }

        /// <summary>
        /// using SqlBulkCopy to save DataTable into database
        /// </summary>
        /// <param name="table">DataTable name</param>
        public static void BulkInsert(DataTable table)
        {
            BulkInsert(CONN_STRING, table);
        }


        /// <summary>
        /// using SqlBulkCopy to save DataTable into database
        /// </summary>
        /// <param name="connectionString">数据库连接字符串</param>
        /// <param name="table">数据表</param>
        private static void BulkInsert(string connectionString, DataTable table)
        {
            if (string.IsNullOrEmpty(table.TableName)) throw new Exception("DataTable.TableName can't be empty");
            using (SqlBulkCopy bulk = new SqlBulkCopy(connectionString))
            {
                bulk.BatchSize = BatchSize;
                bulk.BulkCopyTimeout = CommandTimeOut;
                bulk.DestinationTableName = table.TableName;
                foreach (DataColumn col in table.Columns)
                {
                    bulk.ColumnMappings.Add(col.ColumnName, col.ColumnName);
                }
                bulk.WriteToServer(table);
                bulk.Close();
            }
        }

        public static DataTable ConvertDataReaderToDataTable(SqlDataReader dataReader)
        {
            ///定义DataTable  
            DataTable datatable = new DataTable();

            try
            {    ///动态添加表的数据列  
                for (int i = 0; i < dataReader.FieldCount; i++)
                {
                    DataColumn myDataColumn = new DataColumn();
                    myDataColumn.DataType = dataReader.GetFieldType(i);
                    myDataColumn.ColumnName = dataReader.GetName(i);
                    datatable.Columns.Add(myDataColumn);
                }

                ///添加表的数据  
                while (dataReader.Read())
                {
                    DataRow myDataRow = datatable.NewRow();
                    for (int i = 0; i < dataReader.FieldCount; i++)
                    {
                        myDataRow[i] = dataReader[i].ToString();
                    }
                    datatable.Rows.Add(myDataRow);
                    myDataRow = null;
                }
                ///关闭数据读取器  
                dataReader.Close();
                return datatable;
            }
            catch (Exception ex)
            {
                ///抛出类型转换错误  
                //SystemError.CreateErrorLog(ex.Message);  
                throw new Exception(ex.Message, ex);
            }
        }



        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static bool ExecteNonQuery(CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                return ExecteNonQuery(CONN_STRING, cmdType, cmdText, commandParameters);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        //#region ExecteNonQuery操作方法集合
        /// <summary>
        ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
        /// 使用参数数组形式提供参数列表 
        /// </summary>
        /// <param name="connectionString">一个有效的数据库连接字符串</param>
        /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
        /// <param name="cmdText">存储过程的名字或者 T-SQL 语句</param>
        /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        public static bool ExecteNonQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
        {
            try
            {
                SqlCommand cmd = new SqlCommand();
                using (SqlConnection conn = new SqlConnection(connectionString))
                {
                    //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
                    PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
                    int val = cmd.ExecuteNonQuery();
                    //清空SqlCommand中的参数列表
                    cmd.Parameters.Clear();
                    return val > 0 ? true : false;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public static SqlDataReader GetReader(string sql, params SqlParameter[] parameters)
        {
            SqlConnection conn = new SqlConnection(CONN_STRING);
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.CommandTimeout = 30000000;
            if (parameters != null) cmd.Parameters.AddRange(parameters);
            try
            {
                conn.Open();
                cmd.Prepare();
                return cmd.ExecuteReader(CommandBehavior.CloseConnection);
            }
            catch (Exception ex)
            {
                throw new Exception("SqlHelper Method [GetReader (string sql, params SqlParameter[] parameters) occurs error:] "
                    + ex.Message);
            }

        }

    }
}