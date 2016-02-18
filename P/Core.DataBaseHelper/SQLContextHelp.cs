using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;
using System.Reflection;
using System.Data.SqlClient;

using System.Linq;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.NetExtension;
using ProcCore.DatabaseCore.TableFieldModule;

namespace ProcCore.DatabaseCore.SQLContextHelp
{
    public static class SQLText
    {
        public static String MakeCompare(String FieldName, Object FieldValue, SQLValueType ValueType, WhereCompareType CompareType)
        {
            return MakeCompare(FieldName, FieldValue, null, ValueType, CompareType);
        }
        public static String MakeCompare(String FieldName, Object FieldValueA, Object FieldValueB, SQLValueType ValueType, WhereCompareType CompareType)
        {
            String tpl_SQL = String.Empty;
            String HandleValueA = String.Empty;
            String HandleValueB = String.Empty;

            if (ValueType == SQLValueType.Int)
            {
                switch (CompareType)
                {
                    case WhereCompareType.Equel:
                        tpl_SQL = "{0}={1}"; break;
                    case WhereCompareType.Than:
                        tpl_SQL = "{0}>{1}"; break;
                    case WhereCompareType.Less:
                        tpl_SQL = "{0}<{1}"; break;
                    case WhereCompareType.ThanEquel:
                        tpl_SQL = "{0}>={1}"; break;
                    case WhereCompareType.LessEquel:
                        tpl_SQL = "{0}<={1}"; break;
                    case WhereCompareType.UnEquel:
                        tpl_SQL = "{0}!={1}"; break;
                    case WhereCompareType.Between:
                        tpl_SQL = "{0} between {1} and {2}"; break;
                    case WhereCompareType.NotBetween:
                        tpl_SQL = "{0} not between {1} and {2}"; break;

                    case WhereCompareType.In:
                        tpl_SQL = "{0} in ({1})"; break;
                }

                HandleValueA = FieldValueA.ToString();
                if (FieldValueB != null)
                {
                    HandleValueB = FieldValueB.ToString();
                }
            }

            else if (ValueType == SQLValueType.Boolean)
            {
                tpl_SQL = "{0}={1}";
                HandleValueA = (Boolean)FieldValueA ? "1" : "0";

            }

            else if (ValueType == SQLValueType.DateTime)
            {
                switch (CompareType)
                {
                    case WhereCompareType.Equel:
                        tpl_SQL = "{0}='{1}'"; break;
                    case WhereCompareType.Than:
                        tpl_SQL = "{0}>'{1}'"; break;
                    case WhereCompareType.Less:
                        tpl_SQL = "{0}<'{1}'"; break;
                    case WhereCompareType.ThanEquel:
                        tpl_SQL = "{0}>='{1}'"; break;
                    case WhereCompareType.LessEquel:
                        tpl_SQL = "{0}<='{1}'"; break;
                    case WhereCompareType.UnEquel:
                        tpl_SQL = "{0}!='{1}'"; break;

                    case WhereCompareType.Between:
                        tpl_SQL = "{0} between '{1}' and '{2}'"; break;
                    case WhereCompareType.NotBetween:
                        tpl_SQL = "{0} not between '{1}' and '{2}'"; break;

                    case WhereCompareType.In:
                        tpl_SQL = "{0} in ({1})"; break;
                }

                HandleValueA = ((DateTime)FieldValueA).ToStandardDateTime();
                if (FieldValueB != null)
                {
                    HandleValueB = ((DateTime)FieldValueB).ToStandardDateTime();
                }
            }
            else
            {
                switch (CompareType)
                {
                    case WhereCompareType.Like:
                        tpl_SQL = "{0} like '%{1}%'"; break;
                    case WhereCompareType.Equel:
                        tpl_SQL = "{0}='{1}'"; break;
                    case WhereCompareType.Than:
                        tpl_SQL = "{0}>'{1}'"; break;
                    case WhereCompareType.Less:
                        tpl_SQL = "{0}<'{1}'"; break;
                    case WhereCompareType.ThanEquel:
                        tpl_SQL = "{0}>='{1}'"; break;
                    case WhereCompareType.LessEquel:
                        tpl_SQL = "{0}<='{1}'"; break;
                    case WhereCompareType.UnEquel:
                        tpl_SQL = "{0}!='{1}'"; break;
                    case WhereCompareType.Between:
                        tpl_SQL = "{0} between '{1}' and '{2}'"; break;
                    case WhereCompareType.NotBetween:
                        tpl_SQL = "{0} not between '{1}' and '{2}'"; break;
                    case WhereCompareType.In:
                        tpl_SQL = "{0} in ({1})"; break;
                }

                HandleValueA = FieldValueA.ToString();
                if (FieldValueB != null)
                {
                    HandleValueB = FieldValueB.ToString();
                }
            }
            string r = string.Format(tpl_SQL, FieldName, HandleValueA, HandleValueB);

            return r;
        }

        public static String MakeParm(String FieldName, WhereCompareType CompareType)
        {
            String tpl_SQL = String.Empty;
            String HandleParmName = String.Empty;
            String HandleValueB = String.Empty;

            switch (CompareType)
            {
                case WhereCompareType.Like:
                    tpl_SQL = "{0} like '%'+{1}+'%'"; break;
                case WhereCompareType.LikeRight:
                    tpl_SQL = "{0} like {1}+'%'"; break;
                case WhereCompareType.LikeLeft:
                    tpl_SQL = "{0} like '%'+{1}"; break;
                case WhereCompareType.Equel:
                    tpl_SQL = "{0}={1}"; break;
                case WhereCompareType.Than:
                    tpl_SQL = "{0}>{1}"; break;
                case WhereCompareType.Less:
                    tpl_SQL = "{0}<{1}"; break;
                case WhereCompareType.ThanEquel:
                    tpl_SQL = "{0}>={1}"; break;
                case WhereCompareType.LessEquel:
                    tpl_SQL = "{0}<={1}"; break;
                case WhereCompareType.UnEquel:
                    tpl_SQL = "{0}!={1}"; break;
            }

            HandleParmName = "@" + FieldName;

            return String.Format(tpl_SQL, FieldName, HandleParmName, "");
        }
        public static String MakeParm(String FieldName, String FieldVar, WhereCompareType CompareType)
        {
            String tpl_SQL = String.Empty;
            String HandleParmName = String.Empty;
            String HandleValueB = String.Empty;

            switch (CompareType)
            {
                case WhereCompareType.Like:
                    tpl_SQL = "{0} like '%'+{1}+'%'"; break;
                case WhereCompareType.LikeRight:
                    tpl_SQL = "{0} like {1}+'%'"; break;
                case WhereCompareType.LikeLeft:
                    tpl_SQL = "{0} like '%'+{1}"; break;
                case WhereCompareType.Equel:
                    tpl_SQL = "{0}={1}"; break;
                case WhereCompareType.Than:
                    tpl_SQL = "{0}>{1}"; break;
                case WhereCompareType.Less:
                    tpl_SQL = "{0}<{1}"; break;
                case WhereCompareType.ThanEquel:
                    tpl_SQL = "{0}>={1}"; break;
                case WhereCompareType.LessEquel:
                    tpl_SQL = "{0}<={1}"; break;
                case WhereCompareType.UnEquel:
                    tpl_SQL = "{0}!={1}"; break;
            }

            HandleParmName = "@" + FieldVar;

            return String.Format(tpl_SQL, FieldName, HandleParmName, "");
        }
        public static String MakeParmIn(String FieldName, int ValuesCount)
        {
            String tpl_SQL = String.Empty;
            String HandleParmName = String.Empty;

            List<String> ls_p = new List<String>();

            for (int i = 1; i <= ValuesCount; i++)
            {
                ls_p.Add(String.Format("@{0}{1}", FieldName, i));
            }

            tpl_SQL = String.Format("[{0}] IN ({1})", FieldName, string.Join(",", ls_p.ToArray()));

            HandleParmName = "@" + FieldName;

            return String.Format(tpl_SQL, FieldName, HandleParmName, "");
        }
    }

    public class DataTableWorking<TabObjSource> where TabObjSource : TableMap<TabObjSource>, IDisposable, new()
    {
        #region variable define

        private TabObjSource _TableModule;
        private String _UserID = String.Empty;

        private DataSet _DataSet;
        private DataTable _DataTable;
        private DataRow _DataRow;

        private CommConnection _Connection;
        private ConnectionType _ConnType;

        private SqlDataAdapter _SqlDataAdapter = null;
        private SqlCommandBuilder _SqlCommandBuilder;

        private List<String> _CollectSelectFields;
        private List<WhereFieldsObject> _CollectWhereFields;
        private List<String> _CollectOrderFields;
        private List<GroupFieldObject> _CollectGroupFields;
        private String _ExeSQL = String.Empty;

        #endregion
        #region function area
        #region general area
        public TabObjSource TableModule
        {
            set { _TableModule = value; }
            get { return _TableModule; }
        }

        public int InsertAutoFieldsID { get; set; }
        public int AffetCount { get; set; }
        public int LoginUserID { get; set; }

        public DataTableWorking(CommConnection conn)
        {
            _Connection = conn;
            _ConnType = _Connection.connType;
            _TableModule = new TabObjSource();
            //LogWrite.Enabled = true;
        }

        private void RaisDataAdapter()
        {
            if (_ConnType == ConnectionType.SqlClient)
            {
                if (_SqlDataAdapter == null) _SqlDataAdapter = new SqlDataAdapter("Select * From " + _TableModule.N, (SqlConnection)_Connection.Connection);
                if (_SqlDataAdapter.SelectCommand == null) _SqlDataAdapter.SelectCommand = new SqlCommand();
                if ((SqlTransaction)_Connection.Transaction != null) _SqlDataAdapter.SelectCommand.Transaction = (SqlTransaction)_Connection.Transaction;
            }
        }
        private void RaisDataTable()
        {
            if (_DataTable == null) _DataTable = new DataTable();

            if (_ConnType == ConnectionType.SqlClient)
                _SqlDataAdapter.FillSchema(_DataTable, SchemaType.Source);
        }

        public DataTable DataTable
        {
            get
            {
                return _DataTable;
            }
            set
            {
                _DataTable = value;
            }
        }
        public DataSet DataSet
        {
            get
            {
                return _DataSet;
            }
            set
            {
                _DataSet = value;
            }
        }
        public Object DataAdapter
        {
            set
            {
                if (_ConnType == ConnectionType.SqlClient)
                {
                    _SqlDataAdapter = (SqlDataAdapter)value;
                }
            }
        }

        public void NewRow()
        {
            RaisDataAdapter();
            RaisDataTable();

            _DataRow = _DataTable.NewRow();
        }
        public void EditFirstRow()
        {
            _DataRow = _DataTable.Rows[0];
        }
        public void AddRow()
        {
            _DataTable.Rows.Add(_DataRow);
        }
        public void DeleteAll()
        {
            foreach (DataRow dr in _DataTable.Rows)
            {
                dr.Delete();
            }
        }
        public void DeleteNowRow()
        {
            _DataRow.Delete();
        }
        /// <summary>
        /// 使用此功能，Module的主key欄位一定要有值，否則會引發錯誤。
        /// </summary>
        /// <typeparam name="m_Module"></typeparam>
        /// <param name="md"></param>
        public void ModifyNowRow<m_Module>(m_Module md)
        {
            PropertyInfo[] GetPropertyInfos = md.GetType().GetProperties();
            foreach (var GetPropInfo in GetPropertyInfos)
                if (_DataTable.Columns.Contains(GetPropInfo.Name))
                    _DataRow[GetPropInfo.Name] = GetPropInfo.GetValue(md, null);
        }

        public Boolean GetDataRowByKeyHasData()
        {
            GetDataRowByKeyFields();
            return _DataRow == null ? false : true;
        }
        private void GetDataRowByKeyFields()
        {
            List<Object> l = new List<object>();
            List<DataColumn> c = new List<DataColumn>();

            foreach (var FModule in _TableModule.KeyFieldModules)
            {
                l.Add(FModule.Value.V);
                c.Add(_DataTable.Columns[FModule.Value.N]);
            }

            _DataTable.PrimaryKey = c.ToArray();
            _DataRow = this._DataTable.Rows.Find(l.ToArray());
        }

        public void SetAllRowValue(String FieldName, object value)
        {
            foreach (DataRow dr in _DataTable.Rows)
            {
                _DataRow = dr;
                _DataRow[FieldName] = value;
            }
        }

        public void Reset()
        {
            if (_CollectSelectFields != null) _CollectSelectFields.Clear();
            if (_CollectWhereFields != null) _CollectWhereFields.Clear();
            if (_CollectOrderFields != null) _CollectOrderFields.Clear();
            if (_CollectGroupFields != null) _CollectGroupFields.Clear();

            _DataTable.Dispose();
            _DataTable = null;

            #region 連線類型設定
            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.Dispose();
                _SqlDataAdapter = null;
                if (_SqlCommandBuilder != null) _SqlCommandBuilder.Dispose();
                _SqlCommandBuilder = null;
            }

            #endregion
        }
        /// <summary>
        /// 使用Select功能
        /// </summary>

        public void SetDataRowValue(Func<TabObjSource, FieldModule> FieldObj, Object FieldValue)
        {
            var MakeFieldObj = FieldObj.Invoke(this._TableModule);
            _DataRow[MakeFieldObj.N] = FieldValue;
        }
        public void UpdateFieldsInfo(UpdateFieldsInfoType t)
        {
            if (t == UpdateFieldsInfoType.Insert)
            {
                _DataRow["_InsertUserID"] = LoginUserID;
                _DataRow["_InsertDateTime"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Update)
            {
                _DataRow["_UpdateUserID"] = LoginUserID;
                _DataRow["_UpdateDateTime"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Both)
            {
                _DataRow["_InsertUserID"] = LoginUserID;
                _DataRow["_InsertDateTime"] = DateTime.Now;
                _DataRow["_UpdateUserID"] = LoginUserID;
                _DataRow["_UpdateDateTime"] = DateTime.Now;
            }

            if (t == UpdateFieldsInfoType.Lock)
            {
                _DataRow["_LockUserID"] = LoginUserID;
                _DataRow["_LockDateTime"] = DateTime.Now;
                _DataRow["_LockState"] = true;
            }

            if (t == UpdateFieldsInfoType.UnLock)
            {
                _DataRow["_LockUserID"] = DBNull.Value;
                _DataRow["_LockDateTime"] = DBNull.Value;
                _DataRow["_LockState"] = false;
            }

        }

        public void WhereFilterDataLock()
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = "_LockState";
            w.WhereCompareStyle = WhereCompareType.Equel;
            w.SQLDataType = SQLValueType.Boolean;
            w.Value = 0;
            this._CollectWhereFields.Add(w);
        }

        private void BindAdapterSQLEvent()
        {
            RaisDataAdapter();

            if (_ConnType == ConnectionType.SqlClient)
            {
                #region MyRegion
                _SqlCommandBuilder = new SqlCommandBuilder(_SqlDataAdapter);
                _SqlDataAdapter.RowUpdated += new SqlRowUpdatedEventHandler(SQLServerOnRowUpdated);

                SqlParameter SqlParm = new SqlParameter("@id", SqlDbType.Int);
                SqlParm.Direction = ParameterDirection.Output;

                SqlCommand cmd = _SqlCommandBuilder.GetInsertCommand().Clone();
                cmd.Connection = (SqlConnection)_Connection.Connection;
                cmd.CommandText += " ;SET @id = SCOPE_IDENTITY()";
                cmd.Parameters.Add(SqlParm);

                _SqlDataAdapter.InsertCommand = cmd;
                _SqlDataAdapter.UpdateCommand = _SqlCommandBuilder.GetUpdateCommand().Clone();
                _SqlDataAdapter.DeleteCommand = _SqlCommandBuilder.GetDeleteCommand().Clone();

                if (_Connection.Transaction != null)
                {
                    _SqlDataAdapter.InsertCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                    _SqlDataAdapter.UpdateCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                    _SqlDataAdapter.DeleteCommand.Transaction = (SqlTransaction)_Connection.Transaction;
                }

                _SqlCommandBuilder.Dispose();
                #endregion
            }
        }

        public void UpdateDataAdapter()
        {
            BindAdapterSQLEvent();

            if (_ConnType == ConnectionType.SqlClient)
                this.AffetCount = _SqlDataAdapter.Update(_DataTable);

            _DataTable.AcceptChanges();
        }
        public void UpdateDataAdapter(int BathSize)
        {
            BindAdapterSQLEvent();

            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.UpdateBatchSize = BathSize;
                this.AffetCount = _SqlDataAdapter.Update(_DataTable);
            }

            _DataTable.AcceptChanges();
        }

        protected void SQLServerOnRowUpdated(object sender, SqlRowUpdatedEventArgs args)
        {
            if (args.Status == UpdateStatus.Continue)
            {
                if (args.StatementType == StatementType.Insert)
                {
                    Boolean CheckHasAutoIdentify = false; ;

                    foreach (DataColumn dc in args.Row.Table.Columns)
                    {
                        if (dc.AutoIncrement == true)
                            CheckHasAutoIdentify = true;
                    }

                    if (CheckHasAutoIdentify)
                        InsertAutoFieldsID = (int)_SqlDataAdapter.InsertCommand.Parameters["@id"].Value;
                }
            }
        }
        #endregion
        #region Select Area

        public int TopLimit { get; set; }
        public String ExeSQL { get { return _ExeSQL; } }
        public void SelectFields(Expression<Func<TabObjSource, FieldModule>> Field)
        {
            Func<TabObjSource, FieldModule> CompileFieldObj = Field.Compile();
            FieldModule MakeFieldObj = CompileFieldObj.Invoke(this._TableModule);

            if (this._CollectSelectFields == null)
                this._CollectSelectFields = new List<String>();

            this._CollectSelectFields.Add(MakeFieldObj.N);
        }

        public void SelectFields<TResult>(Expression<Func<TabObjSource, TResult>> Fields)
        {
            Func<TabObjSource, TResult> CompileFieldObj = Fields.Compile();
            TResult MakeFieldObj = CompileFieldObj.Invoke(this._TableModule);

            if (this._CollectSelectFields == null)
                this._CollectSelectFields = new List<String>();

            if (Fields.Body.NodeType == ExpressionType.New)
            {
                PropertyInfo[] TypePropertys = MakeFieldObj.GetType().GetProperties();
                foreach (PropertyInfo Property in TypePropertys)
                {
                    FieldModule FieldObject = (FieldModule)Property.GetValue(MakeFieldObj, null);
                    this._CollectSelectFields.Add(FieldObject.N);
                }
            }

            if (Fields.Body.NodeType == ExpressionType.MemberAccess)
            {
                PropertyInfo TypeProperty = MakeFieldObj.GetType().GetProperty("N");
                String GetName = TypeProperty.GetValue(MakeFieldObj, null).ToString();
                this._CollectSelectFields.Add(GetName);
            }
        }

        /// <summary>
        /// 聚合Function
        /// </summary>
        /// <param name="FieldObj"></param>
        /// <param name="aggregate"></param>
        public void AggregateFields(Func<TabObjSource, FieldModule> FieldObj, String asName, AggregateType aggregate)
        {
            //var CompileFieldObj = FieldObj.Compile();
            var MakeFieldObj = FieldObj.Invoke(this._TableModule);

            GroupFieldObject g = new GroupFieldObject() { AggregateType = aggregate, AsName = asName, FieldName = MakeFieldObj.N };

            if (this._CollectGroupFields == null)
            {
                this._CollectGroupFields = new List<GroupFieldObject>();
            }
            this._CollectGroupFields.Add(g);
        }

        public void WhereFields(String FieldObj, Object FieldValue, WhereCompareType WhereType)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = FieldObj;
            w.FieldVar = FieldObj;
            w.WhereCompareStyle = WhereType;
            w.Value = FieldValue;
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(Func<TabObjSource, FieldModule> FieldObj, Object FieldValue)
        {
            WhereFields(FieldObj, FieldValue, WhereCompareType.Equel);
        }
        public void WhereFields(Func<TabObjSource, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType)
        {
            WhereFields(FieldObj, FieldValue, WhereType, null);
        }
        public void WhereFields(Func<TabObjSource, FieldModule> FieldObj, Object FieldValue, WhereCompareType WhereType, String FieldVar)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            var MakeFieldObj = FieldObj.Invoke(this._TableModule);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.FieldVar = FieldVar.NullValue(MakeFieldObj.N);

            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereType;
            w.Value = FieldValue;
            this._CollectWhereFields.Add(w);
        }
        public void WhereFields(Func<TabObjSource, FieldModule> FieldObj, Object[] FieldValue)
        {
            if (this._CollectWhereFields == null) this._CollectWhereFields = new List<WhereFieldsObject>();
            //var CompileFieldObj = FieldObj.Compile();
            var MakeFieldObj = FieldObj.Invoke(this._TableModule);

            WhereFieldsObject w = new WhereFieldsObject();
            w.FieldName = MakeFieldObj.N;
            w.SQLDataType = MakeFieldObj.T;
            w.WhereCompareStyle = WhereCompareType.In;

            List<String> ls_i = new List<String>();
            foreach (Object i in FieldValue)
            {
                ls_i.Add(i.ToString());
            }

            if (ls_i.Count > 0)
            {
                w.Values = ls_i.ToArray();
                this._CollectWhereFields.Add(w);
            }
        }

        public void OrderByFields(Func<TabObjSource, FieldModule> FieldObj, OrderByType OrderType)
        {
            if (this._CollectOrderFields == null)
            {
                this._CollectOrderFields = new List<String>();
            }

            //var get_F = FieldObj.Compile();
            var get_Obj = FieldObj.Invoke(this._TableModule);
            this._CollectOrderFields.Add(get_Obj.N + " " + OrderType);
        }
        public void OrderByFields(Func<TabObjSource, FieldModule> FieldObj)
        {
            OrderByFields(FieldObj, OrderByType.ASC);
        }
        public String ToSQLString()
        {
            List<String> ls_SelectSQL = new List<String>();
            List<String> ls_WhereSQL = new List<String>();

            String r = String.Empty;

            List<String> K = new List<String>();

            #region Handle Select
            if (_CollectSelectFields != null)
            {
                _CollectSelectFields = new List<String>();
                _CollectSelectFields.Add("*");
            }

            K.Add(string.Join(",", _CollectSelectFields.ToArray()));

            List<String> M = new List<String>();
            if (_CollectGroupFields != null)
            {
                foreach (GroupFieldObject g in _CollectGroupFields)
                {
                    M.Add(g.AggregateType + "(" + g.FieldName + ") as " + g.AsName);
                }
                K.Add(string.Join(",", M.ToArray()));
            }

            r = "Select " + string.Join(",", K.ToArray()) + " From " + _TableModule.N;

            #endregion

            #region Handle Where
            if (_CollectWhereFields != null)
            {
                foreach (WhereFieldsObject obj in _CollectWhereFields)
                {
                    ls_WhereSQL.Add(SQLText.MakeCompare(obj.FieldName, obj.Value, obj.SQLDataType, obj.WhereCompareStyle));
                }
            }

            if (ls_WhereSQL.Count > 0)
            {
                r += " Where " + ls_WhereSQL.ToArray().JoinArray("and", " ");
            }
            #endregion

            #region Handle GroupBy
            if (_CollectGroupFields != null)
            {
                if (_CollectGroupFields.Count > 0)
                {
                    //r += " Group By " + _CollectGroupFields.ToArray().JoinArray(",");
                }
                _CollectGroupFields.Clear();
                _CollectGroupFields = null;
            }
            #endregion

            #region Handle OrderBy
            if (_CollectOrderFields != null)
            {
                if (_CollectOrderFields.Count > 0)
                {
                    r += " Order by " + string.Join(",", _CollectOrderFields.ToArray());
                }
            }

            #endregion
            return r;
        }

        public DataTable DataByAdapter(SqlDataAdapter parmSqlAdp)
        {
            String Sql = String.Empty;

            List<String> ls_WhereSQL = new List<String>();

            _SqlDataAdapter = _SqlDataAdapter == null ? new SqlDataAdapter() : _SqlDataAdapter;

            #region Handle Select
            if (this._CollectSelectFields == null)
                _CollectSelectFields = new List<String>();

            if (this._CollectSelectFields.Count == 0 && _CollectGroupFields == null)
                _CollectSelectFields.Add("*");

            List<String> ls_CombinSelect = new List<String>();
            if (_CollectSelectFields.Count > 0)
                ls_CombinSelect.Add(string.Join(",", _CollectSelectFields.ToArray()));

            List<String> ls_AggregateFields = new List<String>();
            if (_CollectGroupFields != null)
            {
                foreach (GroupFieldObject GFields in _CollectGroupFields)
                {
                    ls_AggregateFields.Add(GFields.AggregateType + "(" + GFields.FieldName + ") as " + GFields.AsName);
                }
                ls_CombinSelect.Add(string.Join(",", ls_AggregateFields.ToArray()));
            }

            if (TopLimit > 0)
                Sql = "Select Top " + TopLimit + " " + string.Join(",", ls_CombinSelect.ToArray()) + " From " + _TableModule.N;
            else
                Sql = "Select " + string.Join(",", ls_CombinSelect.ToArray()) + " From " + _TableModule.N;
            #endregion

            #region Handle Where

            _SqlDataAdapter.SelectCommand = new SqlCommand();
            _SqlDataAdapter.SelectCommand.Connection = (SqlConnection)_Connection.Connection;
            _SqlDataAdapter.SelectCommand.Transaction = (SqlTransaction)_Connection.Transaction;

            if (_CollectWhereFields != null)
            {
                # region Handle Begin
                foreach (WhereFieldsObject obj in _CollectWhereFields)
                {
                    #region 型態配置
                    SqlDbType t = SqlDbType.Int;

                    FieldModule WhereFieldMD = _TableModule.GetFieldModules().Where(x => x.N == obj.FieldName).FirstOrDefault();

                    if (WhereFieldMD == null) throw new Exception("Sys_Err_FieldNotExists");

                    if (WhereFieldMD.T == SQLValueType.Int) t = SqlDbType.Int;
                    if (WhereFieldMD.T == SQLValueType.String) t = SqlDbType.NVarChar;
                    if (WhereFieldMD.T == SQLValueType.DateTime) t = SqlDbType.DateTime;
                    if (WhereFieldMD.T == SQLValueType.Boolean) t = SqlDbType.Bit;
                    #endregion

                    if (obj.WhereCompareStyle == WhereCompareType.In)
                    {
                        #region 處理In
                        ls_WhereSQL.Add(SQLText.MakeParmIn(obj.FieldName, obj.Values.Length));

                        for (int i = 1; i <= obj.Values.Length; i++)
                        {
                            String ParamName = "@" + obj.FieldName.Trim() + i.ToString().Trim();
                            SqlParameter Parm = new SqlParameter(ParamName, obj.Values[i - 1]);
                            _SqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                        }
                        #endregion
                    }
                    else
                    {
                        #region 一般比較式
                        ls_WhereSQL.Add(SQLText.MakeParm(obj.FieldName, obj.FieldVar, obj.WhereCompareStyle));
                        SqlParameter Parm = new SqlParameter("@" + obj.FieldVar, t);
                        Parm.Value = obj.Value;

                        _SqlDataAdapter.SelectCommand.Parameters.Add(Parm);
                        #endregion
                    }
                }
                #endregion
                _CollectWhereFields.Clear();
                _CollectWhereFields = null;
            }

            if (ls_WhereSQL.Count > 0)
            {
                Sql += " Where " + ls_WhereSQL.ToArray().JoinArray("and", "  ");
            }

            ls_WhereSQL.Clear();
            ls_WhereSQL = null;

            #endregion

            #region Handle GroupBy
            if (_CollectGroupFields != null)
            {
                if (_CollectGroupFields.Count > 0)
                {
                    var result = from CollectHaveGroupFields in _CollectSelectFields
                                 where !_CollectGroupFields.Select(x => x.FieldName).Contains(CollectHaveGroupFields)
                                 select CollectHaveGroupFields;

                    if (result.Count() > 0)
                        Sql += " Group By " + string.Join(",", result.ToArray());
                }
                _CollectGroupFields.Clear();
                _CollectGroupFields = null;
            }

            _CollectSelectFields.Clear();
            _CollectSelectFields = null;

            #endregion

            #region Handle OrderBy
            if (_CollectOrderFields != null)
            {
                if (_CollectOrderFields.Count > 0)
                {
                    Sql += " Order by " + string.Join(",", _CollectOrderFields.ToArray());
                }
                _CollectOrderFields.Clear();
                _CollectOrderFields = null;
            }

            #endregion

            if (_DataTable == null) _DataTable = new DataTable(_TableModule.N);

            #region 選擇連線類型

            //try
            //{
            if (_ConnType == ConnectionType.SqlClient)
            {
                _SqlDataAdapter.SelectCommand.CommandText = Sql;
                _SqlDataAdapter.Fill(_DataTable);
            }
            //LogWrite.Write(this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "OK!");
            //}
            //catch (Exception ex) {
            //LogWrite.ErrWrite(this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name , ex);
            //LogWrite.Write("Error SQL", Sql);
            //}
            #endregion
            _ExeSQL = Sql;
            Sql = String.Empty; //清空Sql

            return _DataTable;
        }
        public DataTable DataByAdapter()
        {
            return DataByAdapter(null);
        }

        public m_Module[] DataByAdapter<m_Module>()
        where m_Module : new()
        {
            List<m_Module> ls_assignValue = new List<m_Module>();

            DataTable dt = DataByAdapter();

            foreach (DataRow dr in dt.Rows)
            {
                m_Module md = new m_Module();
                dr.LoadDataToModule(md);
                ls_assignValue.Add(md);
            }
            return ls_assignValue.ToArray();
        }

        /// <summary>
        /// 使用此項功能要先給定 DataTableWorking.Table.KeyFieldModules[key].V 值
        /// </summary>
        public DataTable GetDataByKey()
        {
            String[] cols = _TableModule.KeyFieldModules.Select(x => x.Value).Select(x => x.N).ToArray();
            foreach (String col in cols)
            {
                WhereFields(col, _TableModule.KeyFieldModules[col].V, WhereCompareType.Equel);
            }
            return DataByAdapter(null);
        }
        public m_Module GetDataByKey<m_Module>()
            where m_Module : new()
        {
            String[] cols = _TableModule.KeyFieldModules.Select(x => x.Value).Select(x => x.N).ToArray();
            foreach (String col in cols)
            {
                WhereFields(col, _TableModule.KeyFieldModules[col].V, WhereCompareType.Equel);
            }
            return DataByAdapter<m_Module>().Single();
        }
        #endregion
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.Reset();
            }
        }
        #endregion
    }

    public enum AggregateType
    {
        AVG,
        CHECKSUM_AGG,
        COUNT,
        COUNT_BIG,
        GROUPING,
        GROUPING_ID,
        MAX,
        MIN,
        STDEV,
        STDEVP,
        SUM,
        VAR,
        VARP
    }
    public class WhereFieldsObject
    {
        public String FieldName { get; set; }
        public String FieldVar { get; set; }

        public Object Value { get; set; }
        /// <summary>
        /// 主要For In在使用
        /// </summary>
        public Object[] Values { get; set; }
        public WhereCompareType WhereCompareStyle { get; set; }
        public SQLValueType SQLDataType { get; set; }



    }
    public class GroupFieldObject
    {
        public AggregateType AggregateType { get; set; }
        public String FieldName { get; set; }
        public String AsName { get; set; }
    }
}
