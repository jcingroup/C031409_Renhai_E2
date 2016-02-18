using System;
using System.Data;

using System.Data.SqlClient;
using ProcCore.DatabaseCore.DatabaseName;

namespace ProcCore.DatabaseCore.DataBaseConnection
{
    public class BaseConnection
    {
        public String Server { get; set; }
        public String Account { get; set; }
        public String Password { get; set; }

        public String Path { get; set; }

        public String TranactionName { get; set; }

        public CommConnection conn;

        public CommConnection GetConnection()
        {
            return GetConnection(DataBases.DB_RenHai2012);
        }

        public CommConnection GetConnection(DataBases DBName)
        {
            return GetConnection(DBName, ConnectionType.SqlClient);
        }

        public CommConnection GetConnection(DataBases DBName, ConnectionType type)
        {
            String connectionstring = String.Empty;
            String GetDBName = String.Empty;

            #region Get need used database name

            if (DBName.ToString().StartsWith("Default"))
            {
                GetDBName = DBName.ToString().Replace("Default_", "");
            }
            else
            {
                GetDBName = DBName.ToString().Replace("DB_", "");
            }
            #endregion

            if (type == ConnectionType.SqlClient)
            {
                //SQL Server
                connectionstring = String.Format("Data Source={0};Initial Catalog={1};Persist Security Info=True;User ID={2};Password={3};MultipleActiveResultSets=True", Server, GetDBName, Account, Password);
            }

            if (type == ConnectionType.OleDb)
            {
                //Access
                String FilePath = Path + "\\_Code\\Database\\MainDB.mdb";
                connectionstring = String.Format("Provider=Microsoft.Jet.OLEDB.4.0;Data Source={0};", FilePath);
            }

            if (type == ConnectionType.MySqlClient)
            {
                //SQL Server
                connectionstring = String.Format("Network Address={0};Initial Catalog='{1}';Persist Security Info=no;User Name='{2}';Password='{3}'", Server, GetDBName, Account, Password);
            }

            if (conn == null)
            {
                conn = new CommConnection(connectionstring, type);
                conn.TransactionName = TranactionName;
                //conn.Open();
            }

            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Dispose();
                conn = null;
                conn = new CommConnection(connectionstring, type);
                //conn.Open();
            }
            return conn;
        }

        public void CloseDB()
        {
            if (conn != null)
            {
                if (conn.State == System.Data.ConnectionState.Open)
                {
                    conn.Close();
                    conn.Dispose();
                }
            }
        }
    }

    public class CommConnection : IDisposable
    {

        private SqlConnection SqlConn;

        private SqlTransaction _SQLTrans;


        public Object Connection
        {
            get
            {

                    return SqlConn;

            }
        }
        public Object Transaction
        {
            get
            {

                    return _SQLTrans;

            }
        }
        public String TransactionName { get; set; }

        /// <summary>
        /// 不使用物件內定的DataAdapter 可由自訂傳入
        /// </summary>
        public ConnectionType connType { get; set; }

        public CommConnection(String ConnectionString)

        {
            SqlConn = new SqlConnection(ConnectionString);
            connType = ConnectionType.SqlClient;
            MustRunTrans = false;
        }
        public CommConnection(String ConnectionString, ConnectionType type)
        {
            TransactionName = "Tran001";
            connType = type;
            MustRunTrans = false;

            if (type == ConnectionType.SqlClient)
            {
                SqlConn = new SqlConnection(ConnectionString);
            }
        }

        public System.Data.ConnectionState State
        {
            get
            {
                if (connType == ConnectionType.SqlClient)
                {
                    return SqlConn.State;
                }
                else
                {
                    return SqlConn.State;
                }
            }
        }

        public void Open()
        {
            if (connType == ConnectionType.SqlClient)
            {
                SqlConn.Open();
            }
        }
        public void Close()
        {
            if (connType == ConnectionType.SqlClient)
            {
                SqlConn.Close();
                SqlConn.Dispose();

            }
        }

        public void Tran()
        {
            if (connType == ConnectionType.SqlClient)
            {
                if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                _SQLTrans = SqlConn.BeginTransaction(TransactionName);
            }
        }
        public void Commit()
        {
            if (connType == ConnectionType.SqlClient)
            {
                _SQLTrans.Commit();
                if (SqlConn.State == ConnectionState.Open)
                {
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
        }

        public void Roll()
        {
            if (connType == ConnectionType.SqlClient)
            {
                _SQLTrans.Rollback(TransactionName);
                if (SqlConn.State == ConnectionState.Open) { 
                    SqlConn.Close();
                    SqlConn.Dispose();
                }
            }
        }

        /// <summary>
        /// 如果您要做BeginTransaction、EndCommit及Rollback，這個屬性要先設成true才會實際運做。預設是false。
        /// </summary>
        public Boolean MustRunTrans { get; set; }
        public void BeginTransaction()
        {
            if (MustRunTrans)
            {
                if (connType == ConnectionType.SqlClient)
                {
                    if (SqlConn.State == ConnectionState.Closed) SqlConn.Open();
                    _SQLTrans = SqlConn.BeginTransaction(TransactionName);
                }

            }
        }
        public void EndCommit()
        {
            if (MustRunTrans)
            {
                if (connType == ConnectionType.SqlClient)
                {
                    _SQLTrans.Commit();
                    if (SqlConn.State == ConnectionState.Open) { 
                        SqlConn.Close();
                        SqlConn.Dispose();
                    }
                }

            }
        }
        public void Rollback()
        {
            if (MustRunTrans)
            {
                if (connType == ConnectionType.SqlClient)
                {
                    _SQLTrans.Rollback(TransactionName);
                }
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (connType == ConnectionType.SqlClient)
                {
                    SqlConn.Dispose();
                }
            }
        }

        public void ExecuteNonQuery(String SQL)
        {
            if (connType == ConnectionType.SqlClient)
            {
                SqlCommand Cmd = new SqlCommand(SQL, SqlConn);
                if (_SQLTrans != null)
                {
                    Cmd.Transaction = _SQLTrans;
                }
                Cmd.ExecuteNonQuery();
                Cmd.Dispose();
            }
        }
        public DataTable ExecuteData(String SQL)
        {
            DataTable dt = new DataTable();

            if (connType == ConnectionType.SqlClient)
            {
                SqlDataAdapter _SQLAdp = new SqlDataAdapter(SQL, SqlConn);
                if (_SQLTrans != null) _SQLAdp.SelectCommand.Transaction = _SQLTrans;
                _SQLAdp.Fill(dt);
            }
            return dt;
        }
        public Object ExecuteScalar(String SQL)
        {
            if (connType == ConnectionType.SqlClient)
            {
                SqlCommand Cmd = new SqlCommand(SQL, SqlConn);
                if (_SQLTrans != null)
                {
                    Cmd.Transaction = _SQLTrans;
                }
                return Cmd.ExecuteScalar();
            }
            else
            {

                return null;
            }
        }
    }
}
