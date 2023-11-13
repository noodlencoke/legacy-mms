using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Daimler.HELM.DBInterface
{

    public class DBHelper : IDisposable
    {

        public readonly string connStr = System.Configuration.ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
       
        /// <summary>
        /// 执行查询的方法
        /// </summary>
        /// <param name="strSql">T-SQL语句</param>
        /// <param name="sqlPar">参数数组</param>
        /// <returns>返回一个DataReader对象</returns>
        public SqlDataReader ExecuteReader(string strSql, SqlParameter[] sqlPar, CommandType type)
        {
            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(strSql, sqlCon);
                sqlCommand.CommandType = type;
                sqlCommand.CommandTimeout = 300;
                if (sqlPar != null)
                {
                    sqlCommand.Parameters.AddRange(sqlPar);
                }
                return sqlCommand.ExecuteReader(CommandBehavior.CloseConnection);
            }
        }
        /// <summary>
        ///  增删改查
        /// </summary>
        /// <param name="strSql">TSQl / Proc</param>
        /// <param name="sqlPar"></param>
        /// <param name="type">Text / Proc</param>
        /// <returns></returns>
        public int Execute(string strSql, SqlParameter[] sqlPar, CommandType type)
        {
            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(strSql, sqlCon);
                sqlCommand.CommandType = type;
                sqlCommand.CommandTimeout = 300;
                if (sqlPar != null)
                {
                    sqlCommand.Parameters.AddRange(sqlPar);
                }
                int cou = sqlCommand.ExecuteNonQuery();
                sqlCon.Close();
                return cou;
            }
        }
        /// <summary>
        /// 执行查询的方法
        /// </summary>
        /// <param name="strcon">连接字符串</param>
        /// <param name="cmdText">sql语句</param>
        /// <returns>返回DataSet</returns>
        public DataSet ExcuteQuery(string cmdText, SqlParameter[] sqlpar, CommandType type)
        {
            try
            {
                using (SqlConnection sqlCon = new SqlConnection(connStr))
                {
                    if (sqlCon.State == ConnectionState.Closed)
                    {
                        sqlCon.Open();
                    }
                    SqlCommand sqlcom = new SqlCommand(cmdText, sqlCon);
                    sqlcom.CommandType = type;
                    sqlcom.CommandTimeout = 300;
                    if (sqlpar != null)
                    {
                        sqlcom.Parameters.AddRange(sqlpar);
                    }
                    SqlDataAdapter sqlda = new SqlDataAdapter(sqlcom);
                    DataSet ds = new DataSet();
                    sqlda.Fill(ds, "table");
                    sqlCon.Close();
                    return ds;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 查询第一行第一列的数据
        /// </summary>
        /// <param name="sql"></param>
        /// <param name="sqlpar"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public object ExcuteMyScalar(string sql, SqlParameter[] sqlpar, CommandType type)
        {
            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlCommand sqlCommand = new SqlCommand(sql, sqlCon);
                sqlCommand.CommandType = type;
                sqlCommand.CommandTimeout = 300;
                if (sqlpar != null)
                {
                    sqlCommand.Parameters.AddRange(sqlpar);
                }
                object obj = sqlCommand.ExecuteScalar();
                sqlCon.Close();
                return obj;
            }
        }


        //public void Dispose()
        //{
        //    if (sqlCon.State != ConnectionState.Closed)
        //    {
        //        sqlCon.Close();
        //    }
        //    sqlCon.Dispose();
        //}


        public bool BatchInsert(string sql, SqlParameter[] sqlpar, DataTable dt)
        {
            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand();
                da.InsertCommand.CommandText = sql;
                da.InsertCommand.Connection = sqlCon;
                da.InsertCommand.Parameters.AddRange(sqlpar);
                int cou = da.Update(dt);
                sqlCon.Close();
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public bool BatchUpdate(string sql, SqlParameter[] sqlpar, DataTable dt)
        {
            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.UpdateCommand = new SqlCommand();
                da.UpdateCommand.CommandText = sql;
                da.UpdateCommand.Connection = sqlCon;
                da.UpdateCommand.Parameters.AddRange(sqlpar);
                da.UpdateCommand.UpdatedRowSource = UpdateRowSource.None;
                sqlCon.Close();
                int cou = da.Update(dt);
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public bool BatchInsertDt(DataTable dt)
        {
            DataColumnCollection columnColl = dt.Columns;
            string strColumns = string.Empty;
            string strValueParams = string.Empty;

            List<SqlParameter> paramList = new List<SqlParameter>();
            foreach (DataColumn column in columnColl)
            {
                strColumns += string.Format("{0},", column.ColumnName);
                strValueParams += string.Format("@{0},", column.ColumnName);

                SqlParameter sp = GetParam(column);

                paramList.Add(sp);

            }
            strColumns = strColumns.Substring(0, strColumns.Length - 1);
            strValueParams = strValueParams.Substring(0, strValueParams.Length - 1);

            string sql = string.Format("INSERT INTO [dbo].{0}({1}) VALUES ({2})", dt.TableName, strColumns, strValueParams);

            using (SqlConnection sqlCon = new SqlConnection(connStr))
            {
                if (sqlCon.State == ConnectionState.Closed)
                {
                    sqlCon.Open();
                }
                SqlDataAdapter da = new SqlDataAdapter();
                da.InsertCommand = new SqlCommand();
                da.InsertCommand.CommandText = sql;
                da.InsertCommand.Connection = sqlCon;
                da.InsertCommand.Parameters.AddRange(paramList.ToArray());
                int cou = da.Update(dt);
                sqlCon.Close();
                if (cou > 0)
                {
                    return true;
                }
                return false;
            }
        }


        public bool InsertTable(string tableName, Dictionary<string, object> dicColumns)
        {
            string strColumns = string.Empty;
            string strValues = string.Empty;
            List<SqlParameter> paramList = new List<SqlParameter>();
            foreach (string column in dicColumns.Keys)
            {
                if (dicColumns[column] != null && dicColumns[column].ToString() != "")
                {
                    strColumns += string.Format("{0},", column);
                    strValues += string.Format("@{0},", column);
                    paramList.Add(new SqlParameter("@" + column, dicColumns[column]));
                }
            }
            strColumns = strColumns.Substring(0, strColumns.Length - 1);
            strValues = strValues.Substring(0, strValues.Length - 1);
            string sql = string.Format("INSERT INTO {0}({1}) VALUES({2})",tableName,strColumns,strValues);
            int cou = Execute(sql,paramList.ToArray(),CommandType.Text);
            if (cou > 0)
            {
                return true;
            }
            return false;
        }


        public bool UpdateTable(string tableName,string id, Dictionary<string, object> dicColumns)
        {
            if (dicColumns.Keys.Count > 0)
            {
                string strUpdateList = string.Empty;
                List<SqlParameter> paramList = new List<SqlParameter>();
                paramList.Add(new SqlParameter("@id", id));
                foreach (string key in dicColumns.Keys)
                {
                    if (dicColumns[key] == null)
                    {
                        strUpdateList += string.Format(" {0} = null ,", key);
                    }
                    else
                    {
                        strUpdateList += string.Format(" {0} = @{0} ,", key);
                        paramList.Add(new SqlParameter("@" + key, dicColumns[key]));
                    }
                }

                strUpdateList = strUpdateList.Substring(0, strUpdateList.Length - 1);

                string sql = string.Format("Update [dbo].[{0}] set {1} Where id = @id", tableName, strUpdateList);

                int cou = Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);

                if (cou > 0)
                {
                    return true;
                }
            }
            return false;
        }

        public bool UpdateTable(string tableName, Dictionary<string,object> conditions, Dictionary<string, object> dicColumns)
        {
            if (dicColumns.Keys.Count > 0)
            {
                string strUpdateList = string.Empty;
                string conditionList = string.Empty;
                List<SqlParameter> paramList = new List<SqlParameter>();
                foreach (string columnName in conditions.Keys)
                {
                    if (conditions[columnName] == null)
                    {
                        conditionList += string.Format(" AND [{0}] is null ", columnName);
                    }
                    else
                    {
                        paramList.Add(new SqlParameter("@" + columnName, conditions[columnName]));
                        conditionList += string.Format(" AND [{0}] = @{0} ", columnName);
                    }
                }
                
                foreach (string key in dicColumns.Keys)
                {
                    if (dicColumns[key] == null)
                    {
                        strUpdateList += string.Format(" [{0}] = null ,", key);
                    }
                    else
                    {
                        strUpdateList += string.Format(" [{0}] = @{0} ,", key);
                        paramList.Add(new SqlParameter("@" + key, dicColumns[key]));
                    }
                }

                strUpdateList = strUpdateList.Substring(0, strUpdateList.Length - 1);

                string sql = string.Format("Update [dbo].[{0}] set {1} Where 1=1 {2}", tableName, strUpdateList, conditionList);

                int cou = Execute(sql, paramList.ToArray(), System.Data.CommandType.Text);

                if (cou > 0)
                {
                    return true;
                }
            }
            return false;
        }


        private SqlParameter GetParam(DataColumn column)
        {
            Type type = column.DataType;
            string columnName = column.ColumnName;
            int length = column.MaxLength;
            if (type == Type.GetType("System.Guid"))
            {
                SqlParameter sp = new SqlParameter("@" + columnName, SqlDbType.UniqueIdentifier);
                sp.SourceVersion = DataRowVersion.Current;
                sp.SourceColumn = columnName;
                return sp;
            }
            else if (type == Type.GetType("System.String"))
            {
                SqlParameter sp = new SqlParameter("@" + columnName, SqlDbType.VarChar, length);
                sp.SourceVersion = DataRowVersion.Current;
                sp.SourceColumn = columnName;
                return sp;
            }
            else if (type == Type.GetType("System.DateTime"))
            {
                SqlParameter sp = new SqlParameter("@" + columnName, SqlDbType.DateTime);
                sp.SourceVersion = DataRowVersion.Current;
                sp.SourceColumn = columnName;
                return sp;
            }
            else if (type == Type.GetType("System.Int32"))
            {
                SqlParameter sp = new SqlParameter("@" + columnName, SqlDbType.Int);
                sp.SourceVersion = DataRowVersion.Current;
                sp.SourceColumn = columnName;
                return sp;
            }
            else
            {
                SqlParameter sp = new SqlParameter("@" + columnName, SqlDbType.VarChar);
                sp.SourceVersion = DataRowVersion.Current;
                sp.SourceColumn = columnName;
                return sp;
            }
        }




        public void Dispose()
        {
          
        }
    }
}
