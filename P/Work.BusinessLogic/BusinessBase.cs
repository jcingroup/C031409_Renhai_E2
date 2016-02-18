using System;
using System.Data;
using System.Data.SqlClient;
using ProcCore.DatabaseCore.DataBaseConnection;
using System.Collections;
using System.Collections.Generic;

namespace ProcCore.Business.Base
{
    #region LogicBase
    public class LogicBase:IDisposable
    {
        public CommConnection Connection { get; set; }
        public Log.LogPlamInfo logPlamInfo { get; set; }

        public int GetIDX()
        {
            SqlDataAdapter SqlAdp = new SqlDataAdapter();
            SqlAdp.SelectCommand = new SqlCommand("Update _IDX Set IDX = IDX + 1;Select IDX From _IDX", (SqlConnection)Connection.Connection);
            SqlAdp.SelectCommand.Transaction = (SqlTransaction) Connection.Transaction;
            DataTable dt = new DataTable();

            SqlAdp.Fill(dt);
            SqlAdp.Dispose();
            return (int)dt.Rows[0][0];
        }
        protected void ExecuteSQL(String SQL)
        {
            Connection.ExecuteNonQuery(SQL);
        }
        protected DataTable ExecuteData(String SQL)
        {
            return Connection.ExecuteData(SQL);
        }
        protected Object ExecuteScalar(String SQL)
        {
            return Connection.ExecuteScalar(SQL);
        }
        protected String PackErrMessage(Exception ex)
        {
            return ex.Message + ":" + ex.StackTrace;
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
                if (Connection != null) Connection.Close(); Connection.Dispose();
            }
        }
    }

    public abstract class LogicBase<m_Module, q_Module, TabModule> : IDisposable
    {
        public TabModule GetTableModule {get;set;}
        public CommConnection Connection { get; set; }
        public Log.LogPlamInfo logPlamInfo { get; set; }

        public int GetIDX()
        {
            SqlDataAdapter SqlAdp = new SqlDataAdapter();
            SqlAdp.SelectCommand = new SqlCommand("Update _IDX Set IDX = IDX + 1;Select IDX From _IDX", (SqlConnection)Connection.Connection);
            SqlAdp.SelectCommand.Transaction = (SqlTransaction)Connection.Transaction;
            DataTable dt = new DataTable();

            SqlAdp.Fill(dt);
            SqlAdp.Dispose();
            return (int)dt.Rows[0][0];
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
                if (Connection != null) Connection.Close(); Connection.Dispose();
            }
        }
        protected String PackErrMessage(Exception ex)
        {
            String s = "[Message:{0}][Track:{1}]";
            return String.Format(s, ex.Message,ex.StackTrace);
        }

        public  abstract RunInsertEnd InsertMaster(m_Module md, int accountId);
        public  abstract RunUpdateEnd UpdateMaster(m_Module md, int accountId);
        public  abstract RunDeleteEnd DeleteMaster(String[] deleteIds, int accountId);
        public  abstract RunQueryPackage<m_Module> SearchMaster(q_Module qr, int accountId);
        public  abstract RunOneDataEnd<m_Module> GetDataMaster(int id, int accountId);
    }
    #endregion

    #region 基礎類別區
    public abstract class ModuleBase
    {
        public EditModeType EditType { get; set; }
    }
    public abstract class QueryBase
    {
        public QueryBase() {
            FliterMinute = 10;
        }
        public Boolean FliterLock {get;set;}
        /// <summary>
        /// Lock時間定 在這時間範圍會被過慮
        /// </summary>
        public int FliterMinute{get;set;}

        public int? page { get; set; }
        public String sidx { get; set; }

        /// <summary>
        /// JQGrid欄位排序行態 (asc or desc)由JQGrid自動產生
        /// </summary>
        public String sord { get; set; }
        public int rows { get; set; }
        public Boolean _search { get; set; }
    }
    public abstract class SubQueryBase
    {
        public String id { get; set; }
        public String nd_ { get; set; }
    }
    #endregion
 }
