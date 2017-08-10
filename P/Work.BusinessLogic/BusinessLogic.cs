using System;
using System.Collections.Generic;
using System.Linq;
using System.Collections;
using System.Data;

using ProcCore;
using ProcCore.Business.Base;
using ProcCore.NetExtension;

using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.Business.Logic.TablesDescription;

namespace ProcCore.Business.Logic
{
    #region System Base Install
    #region Address Logic Handle
    public class LogicAddress : LogicBase
    {
        public Dictionary<String, String> GetCity()
        {
            地址縣市 TObj = new 地址縣市();
            DataTableWorking<地址縣市> dataWork = new DataTableWorking<地址縣市>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.縣市);
            dataWork.OrderByFields(x => x.排序);
            return dataWork.DataByAdapter().dicMakeKeyValue(0, 0);
        }
        public Dictionary<String, String> GetCountry(String city)
        {
            地址鄉鎮 TObj = new 地址鄉鎮();
            DataTableWorking<地址鄉鎮> dataWork = new DataTableWorking<地址鄉鎮>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.鄉鎮);
            dataWork.WhereFields(x => x.縣市, city);
            dataWork.OrderByFields(x => x.排序);
            return dataWork.DataByAdapter().dicMakeKeyValue(0, 0);
        }
        public String GetZip(String city, String country)
        {
            地址鄉鎮 TObj = new 地址鄉鎮();
            DataTableWorking<地址鄉鎮> dataWork = new DataTableWorking<地址鄉鎮>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.郵遞區號);
            dataWork.WhereFields(x => x.縣市, city);
            dataWork.WhereFields(x => x.鄉鎮, country);

            DataTable dt = dataWork.DataByAdapter();
            return dt.Rows.Count > 0 ? dt.Rows[0][TObj.郵遞區號.N].ToString() : "";
        }
    }
    #endregion

    #region Code Sheet

    public class BaseSheet
    {
        public String CodeGroup { get; set; }
        protected List<_Code> Codes { get; set; }

        public virtual List<_Code> MakeCodes()
        {
            return this.Codes;
        }
        public Dictionary<String, String> ToDictionary()
        {
            Dictionary<String, String> d = new Dictionary<string, string>();
            foreach (_Code _C in this.MakeCodes())
            {
                d.Add(_C.Code, _C.Value);
            }
            return d;
        }
    }
    public class _Code
    {
        public String Code { get; set; }
        public String Value { get; set; }
        public int Sort { get; set; }
        public Boolean IsUse { get; set; }
    }
    //=============================================================
    #region ReplayArea

    public static class CodeSheet
    {

        public static NewsKind NewsKind = new NewsKind()
        {

            News = new _Code() { Code = "News", Value = "最新消息", Sort = 1, IsUse = true },
            Active = new _Code() { Code = "Active", Value = "最新活動", Sort = 2, IsUse = true },
            Post = new _Code() { Code = "Post", Value = "最新公告", Sort = 3, IsUse = true }
        };


        public static OrderState OrderState = new OrderState()
        {

            Normal = new _Code() { Code = "Normal", Value = "正常", Sort = 1, IsUse = true },
            Hold = new _Code() { Code = "Hold", Value = "暫停", Sort = 2, IsUse = true },
            Cancel = new _Code() { Code = "Cancel", Value = "取消", Sort = 3, IsUse = true }
        };


        public static ProductState ProductState = new ProductState()
        {

            Normal = new _Code() { Code = "Normal", Value = "上架", Sort = 1, IsUse = true },
            Stop = new _Code() { Code = "Stop", Value = "下架", Sort = 2, IsUse = true },
            Pause = new _Code() { Code = "Pause", Value = "暫停", Sort = 3, IsUse = true }
        };


        public static UsersState UsersState = new UsersState()
        {

            Normal = new _Code() { Code = "Normal", Value = "正常", Sort = 1, IsUse = true },
            Stop = new _Code() { Code = "Stop", Value = "停止", Sort = 2, IsUse = true },
            Pause = new _Code() { Code = "Pause", Value = "暫停", Sort = 3, IsUse = true }
        };


        public static 最新消息分類 最新消息分類 = new 最新消息分類()
        {

            News = new _Code() { Code = "News", Value = "最新消息", Sort = 1, IsUse = true }
        };


    }
    public class NewsKind : BaseSheet
    {
        public NewsKind() { this.CodeGroup = "NewsKind"; }

        public _Code News { get; set; }
        public _Code Active { get; set; }
        public _Code Post { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>();
            this.Codes.AddRange(new _Code[] { this.News, this.Active, this.Post });
            return base.MakeCodes();
        }
    }
    public class OrderState : BaseSheet
    {
        public OrderState() { this.CodeGroup = "OrderState"; }

        public _Code Normal { get; set; }
        public _Code Hold { get; set; }
        public _Code Cancel { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>();
            this.Codes.AddRange(new _Code[] { this.Normal, this.Hold, this.Cancel });
            return base.MakeCodes();
        }
    }
    public class ProductState : BaseSheet
    {
        public ProductState() { this.CodeGroup = "ProductState"; }

        public _Code Normal { get; set; }
        public _Code Stop { get; set; }
        public _Code Pause { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>();
            this.Codes.AddRange(new _Code[] { this.Normal, this.Stop, this.Pause });
            return base.MakeCodes();
        }
    }
    public class UsersState : BaseSheet
    {
        public UsersState() { this.CodeGroup = "UsersState"; }

        public _Code Normal { get; set; }
        public _Code Stop { get; set; }
        public _Code Pause { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>();
            this.Codes.AddRange(new _Code[] { this.Normal, this.Stop, this.Pause });
            return base.MakeCodes();
        }
    }
    public class 最新消息分類 : BaseSheet
    {
        public 最新消息分類() { this.CodeGroup = "最新消息分類"; }
        public _Code News { get; set; }
        public override List<_Code> MakeCodes()
        {
            this.Codes = new List<_Code>();
            this.Codes.AddRange(new _Code[] { this.News });
            return base.MakeCodes();
        }

    }
    #endregion

    //=============================================================
    #endregion

    #region Boolean Sheet

    public class BooleanSheetBase
    {
        public String TrueValue { get; set; }
        public String FalseValue { get; set; }
        public String ToSQL { get; set; }
    }

    public static class BooleanSheet
    {
        #region ReplayArea

        public static BooleanSheetBase Boolean = new BooleanSheetBase()
        {
            TrueValue = "True",
            FalseValue = "False",
            ToSQL = "Select Boolean as id,Boolean as Value From _BooleanSheet"
        };


        public static BooleanSheetBase sex = new BooleanSheetBase()
        {
            TrueValue = "男",
            FalseValue = "女",
            ToSQL = "Select Boolean as id,sex as Value From _BooleanSheet"
        };


        public static BooleanSheetBase yn = new BooleanSheetBase()
        {
            TrueValue = "是",
            FalseValue = "否",
            ToSQL = "Select Boolean as id,yn as Value From _BooleanSheet"
        };


        public static BooleanSheetBase ynv = new BooleanSheetBase()
        {
            TrueValue = "✔",
            FalseValue = "",
            ToSQL = "Select Boolean as id,ynv as Value From _BooleanSheet"
        };


        public static BooleanSheetBase ynvx = new BooleanSheetBase()
        {
            TrueValue = "✔",
            FalseValue = "✕",
            ToSQL = "Select Boolean as id,ynvx as Value From _BooleanSheet"
        };

        #endregion
    }

    #endregion

    #region System Program Menu

    public class SYSMenu : LogicBase
    {
        private int _LoginUserID;
        private String _WebAppPath;

        public SYSMenu(int LoginUserID, String WebAppPath, CommConnection conn)
        {
            _LoginUserID = LoginUserID;
            _WebAppPath = WebAppPath;
            this.Connection = conn;

            ProgData TObj = new ProgData();
            DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = TObj };
            dataWork.WhereFields(x => x.ishidden, false);
            dataWork.WhereFields(x => x.isfolder, true);
            dataWork.OrderByFields(x => x.sort);

            DataTable dt = dataWork.DataByAdapter();

            List<MenuFoler> menuFolder = new List<MenuFoler>();

            foreach (DataRow dr in dt.Rows)
            {
                MenuFoler Folder = new MenuFoler(this._LoginUserID, WebAppPath);
                Folder.Connection = this.Connection;
                Folder.prod_id = dr[TObj.id.N].ToString();

                if (Folder.GetMenuItem.Count() > 0)
                {
                    menuFolder.Add(Folder);
                }
                this.GetMenuFolder = menuFolder.ToArray();
            }
        }

        public MenuFoler[] GetMenuFolder { get; set; }
    }
    public class MenuFoler : LogicBase
    {
        private String _id;
        private String _WebAppPath;
        private int _LoginUserID;

        public MenuFoler(int LoginUserID, string WebAppPath)
        {
            _WebAppPath = WebAppPath;
            _LoginUserID = LoginUserID;
        }

        public String prod_id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                ProgData TObj = new ProgData();
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = this._id;

                DataTable dt = dataWork.GetDataByKey();

                FolderName = dt.Rows[0][TObj.prog_name.N].ToString();
                Sort = dt.Rows[0][TObj.sort.N].ToString();

                dataWork = null;
                dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = TObj };
                //dataWork.Reset();
                dataWork.WhereFields(x => x.ishidden, false);
                dataWork.WhereFields(x => x.isfolder, false);
                dataWork.WhereFields(x => x.sort, Sort, WhereCompareType.LikeRight);
                dataWork.OrderByFields(x => x.sort);

                DataTable item = dataWork.DataByAdapter();

                List<MenuItem> Item = new List<MenuItem>();
                foreach (DataRow q in item.Rows)
                {
                    PowerHave pHave = new PowerHave();
                    pHave.Connection = this.Connection;
                    pHave.SetPower(this._LoginUserID, q[TObj.id.N].CInt());

                    if (pHave.PowerDataSet.GetPower(PowersName.瀏覽).HavePower)
                    {
                        MenuItem mItem = new MenuItem(_WebAppPath);
                        mItem.Connection = this.Connection;
                        mItem.prod_id = q[TObj.id.N].ToString();
                        Item.Add(mItem);
                    }
                }
                this.GetMenuItem = Item.ToArray();
            }
        }
        public String FolderName { get; set; }
        public String Sort { get; set; }
        public String Link { get; set; }
        public MenuItem[] GetMenuItem { get; set; }
    }
    public class MenuItem : LogicBase
    {
        string _WebAppPath = String.Empty;
        public MenuItem(String WebAppPath)
        {
            _WebAppPath = WebAppPath;
        }
        private String _id;
        public String prod_id
        {
            get
            {
                return _id;
            }
            set
            {
                this._id = value;
                ProgData TObj = new ProgData();
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = this._id;

                DataTable dt = dataWork.GetDataByKey();

                this.Link = _WebAppPath + dt.Rows[0][TObj.area.N].ToString() + "/" + dt.Rows[0][TObj.controller.N].ToString() + "/" + dt.Rows[0][TObj.action.N].ToString();
                this.ItemName = dt.Rows[0][TObj.prog_name.N].ToString();
            }
        }
        public String ItemName { get; set; }
        public String Link { get; set; }
    }

    #endregion

    #region Power

    public enum PowersName
    {
        完全控制 = 0, 管理 = 1, 瀏覽 = 2, 新增 = 3, 修改 = 4, 刪除 = 5, 審核 = 6, 回覆 = 7
    }

    public class Power
    {
        public int Id { get; set; }
        public PowersName name { get; set; }

        public int ManagementIntSerial { get; set; }
        public Boolean IsManagement { get; set; }
        public Boolean HavePower { get; set; }
    }

    public class PowerData
    {
        private List<Power> ls_Powers;

        public PowerData()
        {
            ls_Powers = new List<Power>();

            #region 設定權限核心資料
            //String[] GetPowersName = Enum.GetNames(typeof(PowersName));
            for (int i = 0; i < Enum.GetNames(typeof(PowersName)).Length; i++)
            {
                Power p = new Power { Id = i + 1, name = (PowersName)i, ManagementIntSerial = System.Math.Pow(2, i).CInt() };
                ls_Powers.Add(p);
            }

            Powers = ls_Powers.ToArray();
            #endregion
        }

        public Power[] Powers { get; set; }

        public Power GetPower(PowersName p)
        {
            return ls_Powers.Where(x => x.name == p).FirstOrDefault();
        }
    }

    public class PowerManagement : PowerData
    {
        int _PowerInt;

        public PowerManagement()
            : base()
        {

        }

        public PowerManagement(int GetPowerInt)
            : base()
        {
            this._PowerInt = GetPowerInt;
            foreach (Power p in Powers)
            {
                p.IsManagement = (p.ManagementIntSerial & this.PowerInt) > 0;
            }
        }

        public int PowerInt
        {
            get { return this._PowerInt; }
            set
            {
                this._PowerInt = value;
                foreach (Power p in Powers)
                {
                    p.IsManagement = (p.ManagementIntSerial & this.PowerInt) > 0;
                }
            }
        }
        public int Unit { get; set; }
    }

    public class PowerHave : LogicBase
    {
        public PowerHave()
        {
            PowerDataSet = new PowerData();
        }

        public void SetPower(int UserId, int ProgId)
        {
            a_Users ac = new a_Users();
            m_Users md;

            ac.Connection = this.Connection;
            RunQueryEnd HResult = ac.GetDataMaster(UserId, out md, "0");

            if (md.isadmin)
            {
                foreach (Power p in this.PowerDataSet.Powers)
                {
                    p.HavePower = true;
                }
            }
            else
            {
                String sql = String.Format("Select PowerID From _PowerUnit Where UnitID={0} and ProgID={1}", md.unit, ProgId);
                DataTable dt = ExecuteData(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    int powerId = dr["PowerID"].CInt();

                    if (powerId == 1)
                    {
                        foreach (Power p in this.PowerDataSet.Powers)
                        {
                            p.HavePower = true;
                        }
                    }

                    //if (powerId == 2) { this.HaveManagementPower = true; }
                    //if (powerId == 3) { this.HaveListPower = true; }
                    //if (powerId == 4) { this.HaveInsertPower = true; }
                    //if (powerId == 5) { this.HaveModifyPower = true; }
                    //if (powerId == 6) { this.HaveDeletePower = true; }
                    //if (powerId == 7) { this.HaveVerifyPower = true; }
                    //if (powerId == 8) { this.HaveReplayPower = true; }

                    this.PowerDataSet.Powers[powerId - 1].HavePower = true;
                }

                dt.Dispose();
                dt = null;
            }
        }

        /// <summary>
        /// 陣列式權限處理
        /// </summary>
        public PowerData PowerDataSet { get; set; }

        #region 基礎權限名稱
        //public Boolean HaveCompleteControlPower { get; set; }
        //public Boolean HaveManagementPower        {            get;            set;        }
        //public Boolean HaveListPower        {            get;            set;        }
        //public Boolean HaveInsertPower        {            get;            set;        }
        //public Boolean HaveModifyPower        {            get;            set;        }
        //public Boolean HaveDeletePower        {            get;            set;        }
        //public Boolean HaveVerifyPower        {            get;            set;        }
        //public Boolean HaveReplayPower        {            get;            set;        }
        #endregion
    }
    #endregion

    #region Power for unit
    public class q_PowerUnit : QueryBase
    {
        public int Unit { get; set; }
    }
    public class a_PowerUnit : LogicBase
    {
        public RunUpdateEnd UpdateMaster(m_PowerUnit md, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            _PowerUnit TObj = new _PowerUnit();

            try
            {
                Connection.BeginTransaction();
                DataTableWorking<_PowerUnit> dataWork = new DataTableWorking<_PowerUnit>(Connection) { TableModule = TObj };
                if (md.Checked)
                {
                    //新增
                    dataWork.NewRow();
                    dataWork.SetDataRowValue(x => x.ProgID, md.prog);
                    dataWork.SetDataRowValue(x => x.PowerID, md.power);
                    dataWork.SetDataRowValue(x => x.UnitID, md.unit);
                    dataWork.SetDataRowValue(x => x.AccessUnit, 1);
                    dataWork.AddRow();
                    dataWork.UpdateDataAdapter();
                }
                else
                {
                    //刪除

                    dataWork.WhereFields(x => x.ProgID, md.prog, WhereCompareType.Equel);
                    dataWork.WhereFields(x => x.PowerID, md.power, WhereCompareType.Equel);
                    dataWork.WhereFields(x => x.UnitID, md.unit, WhereCompareType.Equel);

                    DataTable dt_Origin = dataWork.DataByAdapter();
                    dataWork.DeleteAll();
                    dataWork.UpdateDataAdapter();
                }

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {

            }

        }
        public RunQueryEnd SearchMaster(q_PowerUnit qr, string AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            _PowerUnit TObj = new _PowerUnit();

            try
            {
                ProgData ProgWork = new ProgData();
                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn(ProgWork.id.N, typeof(int)) { AllowDBNull = false });
                dt.Columns.Add(new DataColumn(ProgWork.prog_name.N, typeof(String)) { AllowDBNull = false });

                PowerManagement pCollect = new PowerManagement();

                foreach (Power p in pCollect.Powers)
                    dt.Columns.Add(new DataColumn(p.name.ToString(), typeof(String)) { AllowDBNull = false, DefaultValue = "" });

                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = new ProgData() };
                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.prog_name);
                dataWork.SelectFields(x => x.isfolder);
                dataWork.SelectFields(x => x.power_serial);
                dataWork.WhereFields(x => x.isfolder, false, WhereCompareType.Equel);
                dataWork.OrderByFields(x => x.sort, OrderByType.ASC);

                DataTable dt_ProgData = dataWork.DataByAdapter();

                DataTableWorking<_PowerUnit> PowerUnitWork = new DataTableWorking<_PowerUnit>(Connection) { TableModule = new _PowerUnit() };
                PowerUnitWork.SelectFields(x => x.ProgID);
                PowerUnitWork.SelectFields(x => x.PowerID);
                PowerUnitWork.WhereFields(x => x.UnitID, qr.Unit);

                DataTable dt_PowerQuery = PowerUnitWork.DataByAdapter();

                foreach (DataRow dr in dt_ProgData.Rows)
                {
                    DataRow NewDR = dt.NewRow();

                    pCollect.PowerInt = dr[ProgWork.power_serial.N].CInt();
                    NewDR[ProgWork.prog_name.N] = dr[ProgWork.prog_name.N];
                    NewDR[ProgWork.id.N] = dr[ProgWork.id.N];

                    foreach (Power p in pCollect.Powers)
                    {
                        IEnumerable<DataRow> query =
                        from PowerUnit in dt_PowerQuery.AsEnumerable()
                        where
                        PowerUnit.Field<Int32>(TObj.ProgID.N) == dr[ProgWork.id.N].CInt() &&
                        PowerUnit.Field<Byte>(TObj.PowerID.N) == p.Id
                        select PowerUnit;

                        NewDR[p.name.ToString()] = "{\"PowerID\":" + p.Id.ToString() + ",\"ShowPower\":" + p.IsManagement.BooleanValue("true", "false") + ",\"HavePower\":" + (query.Count() > 0).BooleanValue("true", "false") + "}";
                    }
                    dt.Rows.Add(NewDR);
                }

                r.SearchData = dt;

                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }
        public Dictionary<String, String> CollectKeyValueData_Unit()
        {
            String sql = String.Empty;
            sql = string.Format("Select id,name From UnitData Order By sort");
            DataTable dt = ExecuteData(sql);

            Dictionary<String, String> Key_Value = new Dictionary<String, String>();
            foreach (DataRow dr in dt.Rows)
            {
                Key_Value.Add(dr[0].ToString(), dr[1].ToString());
            }

            return Key_Value;
        }
    }
    public class m_PowerUnit : ModuleBase
    {
        public int unit { get; set; }
        public int prog { get; set; }
        public int power { get; set; }
        public Boolean Checked { get; set; }
    }
    #endregion

    #region Power for users
    public class q_PowerUser : QueryBase
    {
        public int user { get; set; }
    }
    public class a_PowerUser : LogicBase
    {
        public RunUpdateEnd UpdateMaster(m_PowerUser md, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            _PowerUsers TObj = new _PowerUsers();
            try
            {
                Connection.BeginTransaction();
                DataTableWorking<_PowerUsers> dataWork = new DataTableWorking<_PowerUsers>(Connection) { TableModule = TObj };
                if (md.Checked)
                {
                    //新增
                    dataWork.NewRow();
                    dataWork.SetDataRowValue(x => x.ProgID, md.prog);
                    dataWork.SetDataRowValue(x => x.PowerID, md.power);
                    dataWork.SetDataRowValue(x => x.UserID, md.user);
                    dataWork.SetDataRowValue(x => x.UnitID, 1);
                    dataWork.AddRow();
                    dataWork.UpdateDataAdapter();
                }
                else
                {
                    //刪除
                    dataWork.WhereFields(x => x.ProgID, md.prog);
                    dataWork.WhereFields(x => x.PowerID, md.power);
                    dataWork.WhereFields(x => x.UserID, md.user);

                    DataTable dt_Origin = dataWork.DataByAdapter();
                    dataWork.DeleteAll();
                    dataWork.UpdateDataAdapter();
                }

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {

            }

        }
        public RunQueryEnd SearchMaster(q_PowerUser qr, string AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            _PowerUsers TObj = new _PowerUsers();

            try
            {
                ProgData ProgWork = new ProgData();
                DataTable dt = new DataTable();

                dt.Columns.Add(new DataColumn(ProgWork.id.N, typeof(int)) { AllowDBNull = false });
                dt.Columns.Add(new DataColumn(ProgWork.prog_name.N, typeof(String)) { AllowDBNull = false });

                PowerManagement pCollect = new PowerManagement();

                foreach (Power p in pCollect.Powers)
                    dt.Columns.Add(new DataColumn(p.name.ToString(), typeof(String)) { AllowDBNull = false, DefaultValue = "" });

                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = new ProgData() };
                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.prog_name);
                dataWork.SelectFields(x => x.isfolder);
                dataWork.SelectFields(x => x.power_serial);
                dataWork.WhereFields(x => x.isfolder, false, WhereCompareType.Equel);
                dataWork.OrderByFields(x => x.sort, OrderByType.ASC);

                DataTable dt_ProgData = dataWork.DataByAdapter();

                DataTableWorking<_PowerUsers> PowerUnitWork = new DataTableWorking<_PowerUsers>(Connection) { TableModule = new _PowerUsers() };
                PowerUnitWork.SelectFields(x => x.ProgID);
                PowerUnitWork.SelectFields(x => x.PowerID);
                PowerUnitWork.WhereFields(x => x.UserID, qr.user);

                DataTable dt_PowerQuery = PowerUnitWork.DataByAdapter();

                foreach (DataRow dr in dt_ProgData.Rows)
                {
                    DataRow NewDR = dt.NewRow();

                    pCollect.PowerInt = dr[ProgWork.power_serial.N].CInt();
                    NewDR[ProgWork.prog_name.N] = dr[ProgWork.prog_name.N];
                    NewDR[ProgWork.id.N] = dr[ProgWork.id.N];

                    foreach (Power p in pCollect.Powers)
                    {
                        IEnumerable<DataRow> query =
                        from PowerUnit in dt_PowerQuery.AsEnumerable()
                        where
                        PowerUnit.Field<Int32>(TObj.ProgID.N) == dr[ProgWork.id.N].CInt() &&
                        PowerUnit.Field<Byte>(TObj.PowerID.N) == p.Id
                        select PowerUnit;

                        NewDR[p.name.ToString()] = "{\"PowerID\":" + p.Id.ToString() + ",\"ShowPower\":" + p.IsManagement.BooleanValue("true", "false") + ",\"HavePower\":" + (query.Count() > 0).BooleanValue("true", "false") + "}";
                    }
                    dt.Rows.Add(NewDR);
                }

                r.SearchData = dt;

                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }
        public Dictionary<String, String> CollectKeyValueData_Users()
        {
            UsersData TObj = new UsersData();
            DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.id);
            dataWork.SelectFields(x => x.name);
            dataWork.OrderByFields(x => x.id, OrderByType.DESC);

            DataTable dt = dataWork.DataByAdapter();
            return dt.dicMakeKeyValue(0, 1);
        }
    }
    public class m_PowerUser : ModuleBase
    {
        public int prog { get; set; }
        public int power { get; set; }
        public int user { get; set; }

        public Boolean Checked { get; set; }
    }
    #endregion

    #region ProgData
    public class a_WebInfo : LogicBase
    {
        public m_ProgData GetSystemInfo(String area, String controller, String action)
        {
            m_ProgData md = new m_ProgData();

            ProgData TObj = new ProgData();
            DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = TObj };

            if (!String.IsNullOrEmpty(area))
            {
                dataWork.WhereFields(x => x.area, area);
            }

            if (!String.IsNullOrEmpty(controller))
            {
                dataWork.WhereFields(x => x.controller, controller);
            }

            if (!String.IsNullOrEmpty(action))
            {
                dataWork.WhereFields(x => x.action, action);
            }

            DataTable dt = dataWork.DataByAdapter();
            if (dt.Rows.Count > 0)
            {
                md.prog_name = dt.Rows[0][TObj.prog_name.N].ToString();
                md.id = dt.Rows[0][TObj.id.N].CInt();
                return md;
            }
            else
            {
                return md;
            }
        }
    }
    public class q_ProgData : QueryBase
    {
        public string s_prog_name { set; get; }
        public string s_controller { set; get; }
        public string s_area { set; get; }
    }
    public class n_ProgData : SubQueryBase
    {
    }
    public class m_ProgData : ModuleBase
    {
        public int id { get; set; }
        public String area { get; set; }
        public String controller { get; set; }
        public String action { get; set; }
        public String path { get; set; }
        public String page { get; set; }
        public String prog_name { get; set; }
        public String sort { get; set; }
        public Boolean isfolder { get; set; }
        public Boolean ishidden { get; set; }
        public Boolean isRoute { get; set; }
        public int power_serial { get; set; }
        public PowerManagement PowerItem { get; set; }
        public List<int> GetPowerItems { get; set; }

    }
    public class a_ProgData : LogicBase<m_ProgData, q_ProgData, ProgData>
    {
        public a_ProgData()
        {
            GetTableModule = new ProgData();//宣告本區使用的主Table物件並指派給變數GetTableModule
        }
        public override RunInsertEnd InsertMaster(m_ProgData md, int AccountID)
        {
            #region Declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.area, md.area);
                dataWork.SetDataRowValue(x => x.controller, md.controller);
                dataWork.SetDataRowValue(x => x.action, md.action);
                dataWork.SetDataRowValue(x => x.path, md.path);
                dataWork.SetDataRowValue(x => x.page, md.page);
                dataWork.SetDataRowValue(x => x.prog_name, md.prog_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.isfolder, md.isfolder);
                dataWork.SetDataRowValue(x => x.ishidden, md.ishidden);
                dataWork.SetDataRowValue(x => x.isRoute, md.isRoute);

                foreach (int i in md.GetPowerItems)
                {
                    md.power_serial += i;
                }

                dataWork.SetDataRowValue(x => x.power_serial, md.power_serial);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_ProgData md, int AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            try
            {
                Connection.BeginTransaction();
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.TableModule.KeyFieldModules[GetTableModule.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();
                #region 指派值
                dataWork.SetDataRowValue(x => x.area, md.area);
                dataWork.SetDataRowValue(x => x.controller, md.controller);
                dataWork.SetDataRowValue(x => x.action, md.action);
                dataWork.SetDataRowValue(x => x.path, md.path);
                dataWork.SetDataRowValue(x => x.page, md.page);
                dataWork.SetDataRowValue(x => x.prog_name, md.prog_name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                dataWork.SetDataRowValue(x => x.isfolder, md.isfolder);
                dataWork.SetDataRowValue(x => x.ishidden, md.ishidden);
                dataWork.SetDataRowValue(x => x.isRoute, md.isRoute);

                foreach (int i in md.GetPowerItems)
                {
                    md.power_serial += i;
                }

                dataWork.SetDataRowValue(x => x.power_serial, md.power_serial);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public override RunDeleteEnd DeleteMaster(String[] DeleteID, int AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            try
            {
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, DeleteID);

                DataTable dt_Origin = dataWork.DataByAdapter(null);

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }

        public override RunQueryPackage<m_ProgData> SearchMaster(q_ProgData qr, int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => new { x.id, x.prog_name, x.isfolder, x.area, x.controller, x.sort });
                dataWork.TopLimit = 1000;
                #endregion
                #region 設定Where條件
                if (qr.s_prog_name != null)
                {
                    dataWork.WhereFields(x => x.prog_name, qr.s_prog_name, WhereCompareType.Like);
                }
                #endregion
                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                #endregion
                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }
        public RunQueryPackage<m_ProgData> SearchMasterLVL1(int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.prog_name);
                dataWork.SelectFields(x => x.isfolder);
                dataWork.SelectFields(x => x.area);
                dataWork.SelectFields(x => x.controller);
                dataWork.SelectFields(x => x.sort);
                dataWork.TopLimit = 100;
                #endregion
                #region 設定Where條件
                dataWork.WhereFields(x => x.isfolder, true);
                #endregion
                #region 設定排序
                dataWork.OrderByFields(x => x.sort);
                #endregion
                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }
        public RunQueryPackage<m_ProgData> SearchMasterLVL2(n_ProgData qr, int AccountID)
        {
            #region 全域變數宣告
            RunQueryPackage<m_ProgData> r = new RunQueryPackage<m_ProgData>();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { LoginUserID = AccountID };

                dataWork.SelectFields(x => x.sort);
                GetTableModule.KeyFieldModules[GetTableModule.id.N].V = qr.id;
                dataWork.TopLimit = 1;
                #endregion
                #region 設定Where條件
                #endregion

                #region 輸出Class
                m_ProgData md = dataWork.GetDataByKey<m_ProgData>();

                dataWork.Reset();

                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.prog_name);
                dataWork.SelectFields(x => x.isfolder);
                dataWork.SelectFields(x => x.area);
                dataWork.SelectFields(x => x.controller);
                dataWork.SelectFields(x => x.sort);
                dataWork.TopLimit = 100;

                dataWork.WhereFields(x => x.sort, md.sort.Substring(0, 3), WhereCompareType.LikeRight);
                dataWork.OrderByFields(x => x.sort);

                r.SearchData = dataWork.DataByAdapter<m_ProgData>();
                r.Result = true;

                dataWork.Dispose();

                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }

        public override RunOneDataEnd<m_ProgData> GetDataMaster(int id, int AccountID)
        {
            RunOneDataEnd<m_ProgData> r = new RunOneDataEnd<m_ProgData>();
            //md = new m_ProgData();
            try
            {
                DataTableWorking<ProgData> dataWork = new DataTableWorking<ProgData>(Connection) { TableModule = this.GetTableModule };
                GetTableModule.KeyFieldModules[GetTableModule.id.N].V = id; //設定KeyValue
                m_ProgData md = dataWork.GetDataByKey<m_ProgData>(); //取得Key該筆資料

                if (md == null)
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                md.PowerItem = new PowerManagement(md.power_serial);

                r.SearchData = md;
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }
    }
    #endregion

    #region User

    public class m_人員
    {
        public Int32 人員代碼 { get; set; }
        public String 帳號 { get; set; }
        public String 密碼 { get; set; }
        public String 姓名 { get; set; }
        public String 住址 { get; set; }
        public String 家裡電話 { get; set; }
        public String 手機 { get; set; }
        public DateTime 生日 { get; set; }
        public Int32 單位代碼 { get; set; }
        public Int32 停權 { get; set; }
        public Int32 職稱代碼 { get; set; }
        public String 抬頭稱呼 { get; set; }
        public String 電子信箱 { get; set; }
        public Int32 isadmin { get; set; }
        public String 使用IP { get; set; }
        public String MD5 { get; set; }
    }


    public class Password : ModuleBase
    {
        public int id { get; set; }
        public string password_o { get; set; }
        public string password_n { get; set; }
        public string password_s { get; set; }
    }

    public class q_Users : QueryBase
    {
        public string s_name { set; get; }
        public string s_account { set; get; }
    }
    public class m_Users : ModuleBase
    {
        public int id { get; set; }
        public String account { get; set; }
        public String password { get; set; }
        public String name { get; set; }
        public int unit { get; set; }
        public String state { get; set; }
        public Boolean isadmin { get; set; }
        public String type { get; set; }
        public String email { get; set; }

        public String zip { get; set; }
        public String city { get; set; }
        public String country { get; set; }
        public String address { get; set; }
    }
    public class a_Users : LogicAddress
    {
        public RunUpdateEnd UpdateMasterPassword(Password pwd, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            UsersData TObj = new UsersData();
            Connection.BeginTransaction();

            try
            {
                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };

                dataWork.SelectFields(x => x.password);
                dataWork.WhereFields(x => x.id, pwd.id, WhereCompareType.Equel);

                DataTable dt = dataWork.DataByAdapter();

                String GetNowPassword = dt.Rows[0][0].ToString();

                if (GetNowPassword != pwd.password_o)
                {
                    throw new ExceptionRoll("Log_Err_Password");
                }

                if (GetNowPassword == pwd.password_n)
                {
                    throw new ExceptionRoll("Log_Err_NewPasswordSame");
                }

                if (pwd.password_s != pwd.password_n)
                {
                    throw new ExceptionRoll("Log_Err_NewPasswordNotSure");
                }

                dataWork.EditFirstRow();
                dataWork.SetDataRowValue(x => x.password, pwd.password_n);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public RunInsertEnd InsertMaster(m_Users md, string AccountID)
        {
            RunInsertEnd r = new RunInsertEnd();

            UsersData TObj = new UsersData();

            try
            {
                Connection.BeginTransaction();
                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };

                dataWork.NewRow();

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.account, md.account);
                dataWork.SetDataRowValue(x => x.password, md.password);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.unit, md.unit);
                dataWork.SetDataRowValue(x => x.state, md.state);
                dataWork.SetDataRowValue(x => x.isadmin, md.isadmin);
                dataWork.SetDataRowValue(x => x.type, md.type);
                dataWork.SetDataRowValue(x => x.email, md.email);
                dataWork.SetDataRowValue(x => x.zip, md.zip);
                dataWork.SetDataRowValue(x => x.city, md.city);
                dataWork.SetDataRowValue(x => x.country, md.country);
                dataWork.SetDataRowValue(x => x.address, md.address);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);

                dataWork.AddRow();

                dataWork.UpdateDataAdapter();
                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值

                r.Result = true;

                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }

        }
        public RunUpdateEnd UpdateMaster(m_Users md, string AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            Connection.BeginTransaction();
            UsersData TObj = new UsersData();
            try
            {
                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };
                dataWork.TableModule.KeyFieldModules[TObj.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();

                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.account, md.account);
                dataWork.SetDataRowValue(x => x.password, md.password);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.unit, md.unit);
                dataWork.SetDataRowValue(x => x.state, md.state);
                dataWork.SetDataRowValue(x => x.isadmin, md.isadmin);
                dataWork.SetDataRowValue(x => x.type, md.type);
                dataWork.SetDataRowValue(x => x.email, md.email);
                dataWork.SetDataRowValue(x => x.zip, md.zip);
                dataWork.SetDataRowValue(x => x.city, md.city);
                dataWork.SetDataRowValue(x => x.country, md.country);
                dataWork.SetDataRowValue(x => x.address, md.address);

                dataWork.UpdateDataAdapter();
                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public RunDeleteEnd DeleteMaster(string[] DeleteID, string AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            UsersData TObj = new UsersData();
            Connection.BeginTransaction();

            try
            {
                //1、要刪除的資料先選出來
                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, DeleteID);

                DataTable dt_Origin = dataWork.DataByAdapter(null);

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }
        public RunQueryEnd SearchMaster(q_Users qr, string AccountID)
        {
            #region 全域變數宣告
            RunQueryEnd r = new RunQueryEnd();
            UsersData TObj = new UsersData();
            #endregion

            try
            {
                #region Select Data 區段 By 條件

                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => new { x.id, x.name, x.unit, x.state, x.isadmin });
                #endregion

                #region 設定Where條件
                if (qr.s_account != null)
                {
                    dataWork.WhereFields(x => x.account, qr.s_account, WhereCompareType.Equel);
                }

                if (qr.s_name != null)
                {
                    dataWork.WhereFields(x => x.name, qr.s_name, WhereCompareType.Equel);
                }
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.id, OrderByType.DESC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.id, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter(null);
                r.Result = true;
                return r;
                #endregion

                #endregion
            }

            catch (ExceptionRoll ex)
            {
                #region 羅輯錯誤區區段
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region 系統錯誤區
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
                #endregion
            }
        }
        public RunQueryEnd GetDataMaster(int id, out m_Users md, string AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            md = new m_Users();
            try
            {
                // 取得Table物件 簡化長度
                UsersData TObj = new UsersData();

                DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = id; //設定KeyValue
                r.SearchData = dataWork.GetDataByKey(); //取得Key該筆資料

                if (r.SearchData.Rows.Count > 0) //如有資料
                {
                    r.SearchData.Rows[0].LoadDataToModule(md); //將資料指派給out md
                }
                else
                {
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤
                }

                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }

        public Dictionary<String, String> MakeOption_Unit()
        {
            UnitData TObj = new UnitData();

            DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };

            dataWork.SelectFields(x => x.id);
            dataWork.SelectFields(x => x.name);
            dataWork.OrderByFields(x => x.sort, OrderByType.ASC);

            return dataWork.DataByAdapter().dicMakeKeyValue(0, 1);
        }
        public Dictionary<String, String> MakeOption_UsersState()
        {
            return CodeSheet.UsersState.ToDictionary();
        }
        public LoginSate SystemLogin(String account, String password)
        {
            UsersData TObj = new UsersData();
            DataTableWorking<UsersData> dataWork = new DataTableWorking<UsersData>(Connection) { TableModule = TObj };
            LoginSate loginState = new LoginSate();
            try
            {
                dataWork.SelectFields(x => x.id);
                dataWork.SelectFields(x => x.account);
                dataWork.SelectFields(x => x.password);
                dataWork.SelectFields(x => x.name);
                dataWork.SelectFields(x => x.unit);
                dataWork.SelectFields(x => x.state);
                dataWork.SelectFields(x => x.isadmin);

                dataWork.WhereFields(x => x.account, account);
                dataWork.WhereFields(x => x.password, password);

                DataTable dt = dataWork.DataByAdapter();

                if (dt.Rows.Count == 0)
                {
                    throw new ExceptionRoll("Login_Err_Password");
                }

                if (dt.Rows[0][TObj.state.N].ToString() != "Normal")
                {
                    throw new ExceptionRoll("Login_Err_Normal");
                }
                else
                {
                    loginState.Id = (int)dt.Rows[0][TObj.id.N];
                    loginState.IsAdmin = (Boolean)dt.Rows[0][TObj.isadmin.N];
                    loginState.Unit = (int)dt.Rows[0][TObj.unit.N];
                    loginState.Result = true;
                    return loginState;
                }
            }
            catch (ExceptionRoll ex)
            {
                loginState.Result = false;
                loginState.ErrType = BusinessErrType.Logic;
                loginState.ErrMessage = ex.Message;
                return loginState;
            }
            catch (Exception ex)
            {
                loginState.Result = false;
                loginState.ErrType = BusinessErrType.System;
                loginState.ErrMessage = PackErrMessage(ex);
                return loginState;
            }
        }

    }
    #endregion

    #region Unit
    public class q_Unit : QueryBase
    {
        public string s_name { set; get; }
    }

    public class m_Unit : ModuleBase
    {
        public int id { get; set; }
        public String name { get; set; }
        public int sort { get; set; }
    }

    public class a_Unit : LogicBase
    {
        public RunInsertEnd InsertMaster(m_Unit md, String AccountID)
        {
            RunInsertEnd r = new RunInsertEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.id, md.id);
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }
        public RunUpdateEnd UpdateMaster(m_Unit md, String AccountID)
        {
            RunUpdateEnd r = new RunUpdateEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };

                dataWork.TableModule.KeyFieldModules[TObj.id.N].V = md.id; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                dataWork.EditFirstRow();
                #region 指派值
                dataWork.SetDataRowValue(x => x.name, md.name);
                dataWork.SetDataRowValue(x => x.sort, md.sort);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter();

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount;
                r.Result = true;

                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();

                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
            finally
            {
            }

        }
        public RunDeleteEnd DeleteMaster(String[] DeleteID, String AccountID)
        {
            RunDeleteEnd r = new RunDeleteEnd();
            Connection.BeginTransaction();
            UnitData TObj = new UnitData();
            try
            {
                //1、要刪除的資料先選出來
                DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => x.id);
                dataWork.WhereFields(x => x.id, DeleteID);

                DataTable dt_Origin = dataWork.DataByAdapter(null);

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                Connection.Rollback();
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }
        public RunQueryEnd SearchMaster(q_Unit qr, String AccountID)
        {
            #region 全域變數宣告
            RunQueryEnd r = new RunQueryEnd();
            UnitData TObj = new UnitData();
            #endregion

            try
            {
                #region Select Data 區段 By 條件

                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };
                dataWork.SelectFields(x => new { x.id, x.name, x.sort });
                #endregion

                #region 設定Where條件
                if (qr.s_name != null)
                {
                    dataWork.WhereFields(x => x.name, qr.s_name, WhereCompareType.Like);
                }
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    dataWork.OrderByFields(x => x.sort, OrderByType.DESC);
                }
                else
                {
                    dataWork.OrderByFields(x => x.sort, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter(null);
                r.Result = true;
                return r;
                #endregion

                #endregion
            }

            catch (ExceptionRoll ex)
            {
                #region 羅輯錯誤區區段
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region 系統錯誤區
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
                #endregion
            }
        }
        public RunQueryEnd GetDataMaster(int id, out m_Unit md, string AccountID)
        {
            RunQueryEnd r = new RunQueryEnd();
            string sql = string.Empty;
            md = new m_Unit();
            try
            {
                // 取得Table物件 簡化長度
                UnitData TObj = new UnitData();

                DataTableWorking<UnitData> dataWork = new DataTableWorking<UnitData>(Connection) { TableModule = TObj };
                TObj.KeyFieldModules[TObj.id.N].V = id; //設定KeyValue
                r.SearchData = dataWork.GetDataByKey(); //取得Key該筆資料

                if (r.SearchData.Rows.Count > 0) //如有資料
                {
                    r.SearchData.Rows[0].LoadDataToModule(md); //將資料指派給out md
                }
                else
                {
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤
                }

                r.Result = true;
                return r;
            }
            catch (ExceptionRoll ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.Logic;
                r.ErrMessage = ex.Message;
                return r;
            }
            catch (Exception ex)
            {
                r.Result = false;
                r.ErrType = BusinessErrType.System;
                r.ErrMessage = PackErrMessage(ex);
                return r;
            }
        }
    }
    #endregion


    #endregion

    #region Systm Extension

    public class a_WorkingUnLock : LogicBase
    {
        public void WorkingUnLock()
        {

            int UnLockMinutes = 1; //UnLock時間

            String[] Tabs = new String[] { "點燈位置資料表" }; //放置需做Unlock檢查的資料表
            List<String> l = new List<String>();

            String SqlTpl = "Update {0} Set _LockState = 0,_LockDateTime=null,_LockUserID=null Where _LockState=1 and _LockDateTime < '{1}'";

            foreach (String Tab in Tabs)
            {
                String Sql = String.Format(SqlTpl, Tab, DateTime.Now.AddMinutes(0 - UnLockMinutes).ToString("yyyy/MM/dd HH:mm:ss"));
                l.Add(Sql);
            }
            try
            {
                Connection.Tran();
                //Log.Write(logPlamInfo, "Start BeginTransaction");
                Connection.ExecuteNonQuery(String.Join(";", l.ToArray()));
                //Log.Write(logPlamInfo, "SQL Command OK!");
                Connection.Commit();
                //Log.Write(logPlamInfo, "Commit OK!");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Connection.Roll();
            }
        }
    }

    #region 祈福產品訂義表
    public static class e_祈福產品
    {
        public static string 光明燈 = "1";
        public static string 媽祖燈 = "3";
        public static string 關聖燈 = "5";
        public static string 文昌燈 = "2";
        public static string 財神燈 = "12";
        public static string 姻緣燈 = "13";
        public static string 觀音燈 = "4";
        public static string 媽祖頭燈 = "31";
        public static string 關聖頭燈 = "51";
        public static string 文昌頭燈 = "21";
        public static string 財神頭燈 = "121";
        public static string 月老頭燈 = "131";
        public static string 觀音頭燈 = "41";
        public static string 入斗 = "8";
        public static string 保運 = "9";
        public static string 香油錢 = "7";
        public static string 香油_祈願卡 = "701";
        public static string 香油_信徒觀摩 = "702";
        public static string 香油_契子觀摩 = "753";
        //public static String 香油_進香活動 = "704";
        //public static String 香油_自強活動 = "705";
        public static string 香油_媽祖回鑾 = "703";
        public static string 香油_媽祖聖誕擲筊 = "704";//2017/3/2香油_媽祖聖誕改名為香油_媽祖聖誕擲筊
        public static string 香油_媽祖聖誕典禮 = "705";
        public static string 香油_專案專款 = "706";
        public static string 租金 = "707";
        public static string 香油_其它 = "760";

        public static string 香油_牛軋糖 = "761";
        public static string 香油_農民曆廣告 = "762";//2016/12/7 下架
        public static string 香油_衣服 = "763";
        public static string 香油_薦拔祖先 = "764";//2017/7/19 下架
        public static string 香油_冤親債主 = "765";//2017/7/19 下架
        public static string 香油_嬰靈 = "766";//2017/7/19 下架
        public static string 香油_屋頂整修費 = "767";//2016/12/7 加入
        public static string 香油_祈福玉珮 = "768";//2016/12/30 加入

        public static string 契子會_入會 = "751";
        //public static String 契子會_自強活動 = "752";
        public static string 契子會_大會 = "752";
        public static string 安太歲 = "6";
        public static string 捐金牌 = "10";
        public static string 捐白米 = "11";
        public static string 主斗 = "380";
        public static string 副斗 = "381";//2017/3/2改名為斗首
        public static string 福斗 = "385";//2017/3/2新增
        public static string 大斗 = "382";
        public static string 中斗 = "383";
        public static string 小斗 = "384";

        public static string 頭燈前排福燈 = "390";
        public static string 頭燈後排福燈 = "391";
        public static string 前排福燈 = "392";
        public static string 右排福燈 = "393";
        public static string 左排福燈 = "394";
        public static string 後排福燈 = "395";
        public static string 上排福燈 = "396";

        //2017/7/17
        public static string 超渡法會_祖先甲 = "1401";
        public static string 超渡法會_祖先乙 = "1402";
        public static string 超渡法會_冤親債主 = "1403";
        public static string 超渡法會_嬰靈 = "1404";

    }
    public static class e_祈福產品分類
    {
        public static string 點燈 = "點燈";
        public static string 金牌 = "金牌";
        public static string 白米 = "白米";
        public static string 超渡法會 = "超渡法會";
        public static string 禮斗 = "禮斗";
        public static string 福燈 = "福燈";
        public static string 太歲 = "太歲";
        public static string 香油 = "香油";
        public static string 契子 = "契子";
        public static string 入斗 = "入斗";
        public static string 保運 = "保運";
        public static string 祈福許願燈 = "祈福許願燈";
    }
    #endregion

    #region 會員資料表

    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_會員資料表 : QueryBase
    {
        public Int32 戶長SN { get; set; }
        public String 會員編號 { set; get; }
    }
    /// <summary>
    /// 資料編輯模組
    /// </summary>
    public class m_會員資料表 : ModuleBase
    {
        public Int64 序號 { get; set; }
        public String 會員編號 { get; set; }
        public Int32 戶長SN { get; set; }
        public Boolean Is戶長 { get; set; }
        public String 姓名 { get; set; }
        public String 電話區碼 { get; set; }
        public String 電話尾碼 { get; set; }
        public String 郵遞區號 { get; set; }
        public String 地址 { get; set; }
        public String 手機 { get; set; }
        public String 性別 { get; set; }
        public String 生日 { get; set; }
        public String 時辰 { get; set; }
        public String EMAIL { get; set; }
        public String 祈福事項 { get; set; }
        public String 生肖 { get; set; }
        public String 縣市 { get; set; }
        public String 鄉鎮 { get; set; }
        public DateTime 建立日期 { get; set; }
    }
    /// <summary>
    /// 最新消息系統-SQL溝通模組
    /// </summary>
    public class a_會員資料表 : LogicBase<m_會員資料表, q_會員資料表, 會員資料表>
    {
        public override RunInsertEnd InsertMaster(m_會員資料表 md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunUpdateEnd UpdateMaster(m_會員資料表 md, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m_會員資料表> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunDeleteEnd DeleteMaster(string[] deleteIds, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunQueryPackage<m_會員資料表> SearchMaster(q_會員資料表 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_會員資料表> r = new RunQueryPackage<m_會員資料表>();
            會員資料表 TObj = new 會員資料表();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再交由至ExecuteData執行
                DataTableWorking<會員資料表> dataWork = new DataTableWorking<會員資料表>(Connection) { LoginUserID = accountId };
                #endregion

                #region 設定Where條件
                if (qr.會員編號 != null)
                {
                    dataWork.WhereFields(x => x.會員編號, qr.會員編號);
                }
                if (qr.戶長SN != 0)
                {
                    dataWork.WhereFields(x => x.戶長SN, qr.戶長SN);
                }
                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                }
                else
                {
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_會員資料表>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message + ex.StackTrace; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_會員資料表> GetDataMaster(String Id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_會員資料表> r = new RunOneDataEnd<m_會員資料表>();
            #endregion
            try
            {
                #region Main working
                會員資料表 TObj = new 會員資料表();// 取得Table物
                DataTableWorking<會員資料表> dataWork = new DataTableWorking<會員資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };
                TObj.KeyFieldModules[TObj.會員編號.N].V = Id; //設定KeyValue
                m_會員資料表 md = dataWork.GetDataByKey<m_會員資料表>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
    }
    #endregion

    #region 點燈位置資料表

    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_點燈位置資料表 : QueryBase
    {
        public Int64 s_序號 { set; get; }
        public int s_年度 { set; get; }
        public String s_產品編號 { set; get; }
        public String s_空位 { get; set; }
        public DateTime s_LockDateTime { get; set; }
    }
    /// <summary>
    /// 資料編輯模組
    /// </summary>
    public class m_點燈位置資料表 : ModuleBase
    {
        public Int64 序號 { get; set; }
        public String 位置名稱 { get; set; }
        public Int32 年度 { get; set; }
        public String 空位 { get; set; }
        public String 產品編號 { get; set; }
        public Int32 價格 { get; set; }

    }
    public class m_燈位數統計狀態 : ModuleBase
    {
        public int 年度 { get; set; }
        public String 產品名稱 { get; set; }
        public String 產品編號 { get; set; }
        public int 總計 { get; set; }
        public int 剩餘 { get; set; }
        public int 已點 { get; set; }
    }
    /// <summary>
    /// 最新消息系統-SQL溝通模組
    /// </summary>
    public class a_點燈位置資料表 : LogicBase
    {
        public RunInsertEnd GeneralLightInsert(String FromPordId, int ProdPrice,
            String LightBeforeLabel,
            String[] 面s,
            String[] 柱s, int 排數, int 每排人數, int 排起始數, int 人啟始數, int 年度, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            點燈位置資料表 TObj = new 點燈位置資料表(); ;//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion

            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection)
                {
                    TableModule = TObj,
                    LoginUserID = accountId
                };

                String LightTPL = "{0}{1} {2}{3:00}-{4:00}";

                foreach (String 面 in 面s)
                {
                    foreach (String 柱 in 柱s)
                    {
                        for (int i = 排起始數; i <= 排數; i++)
                        {
                            for (int j = 人啟始數; j <= 每排人數; j++)
                            {
                                #region Main Working
                                dataWork.NewRow(); //開始新橧作業 產生新的一行

                                String SiteName = String.Format(LightTPL, LightBeforeLabel, 面, 柱, i, j);
                                dataWork.SetDataRowValue(x => x.年度, 年度);
                                dataWork.SetDataRowValue(x => x.位置名稱, SiteName);
                                dataWork.SetDataRowValue(x => x.空位, 0);
                                dataWork.SetDataRowValue(x => x.價格, ProdPrice);
                                dataWork.SetDataRowValue(x => x.產品編號, FromPordId);

                                dataWork.AddRow(); //加載至DataTable 
                                #endregion
                            }
                        }
                    }
                }

                dataWork.UpdateDataAdapter(100); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }

        public RunInsertEnd HeadLightInsert(String FromPordId, int ProdPrice,
            String LightBeforeLabel,
            String[] 面s,
              int[] 每排人數, int 年度, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            點燈位置資料表 TObj = new 點燈位置資料表(); ;//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion

            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection)
                {
                    TableModule = TObj,
                    LoginUserID = accountId
                };

                String LightTPL = "{0}{1} {2:00}-{3:00}";

                foreach (String 面 in 面s)
                {
                    int k = 1;
                    foreach (int 人數 in 每排人數)
                    {
                        for (int i = 1; i <= 人數; i++)
                        {
                            #region Main Working
                            dataWork.NewRow(); //開始新橧作業 產生新的一行Ｉ

                            String SiteName = String.Format(LightTPL, LightBeforeLabel, 面, k, i);
                            dataWork.SetDataRowValue(x => x.年度, 年度);
                            dataWork.SetDataRowValue(x => x.位置名稱, SiteName);
                            dataWork.SetDataRowValue(x => x.空位, 0);
                            dataWork.SetDataRowValue(x => x.價格, 0);
                            dataWork.SetDataRowValue(x => x.價格, ProdPrice);
                            dataWork.SetDataRowValue(x => x.產品編號, FromPordId);


                            dataWork.AddRow(); //加載至DataTable 
                            #endregion
                        }
                        k++;
                    }
                }

                dataWork.UpdateDataAdapter(100); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }


        public RunInsertEnd Insert主斗副斗(m_點燈位置資料表[] mds, int accountId)
        {//int 年度, String 主斗名稱, int 價格, String 產品編號
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            點燈位置資料表 TObj = new 點燈位置資料表();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

                foreach (m_點燈位置資料表 md in mds)
                {
                    dataWork.NewRow(); //開始新橧作業 產生新的一行
                    #region 指派值
                    dataWork.SetDataRowValue(x => x.年度, md.年度);
                    dataWork.SetDataRowValue(x => x.位置名稱, md.位置名稱);
                    dataWork.SetDataRowValue(x => x.空位, 0);
                    dataWork.SetDataRowValue(x => x.價格, md.價格);
                    dataWork.SetDataRowValue(x => x.產品編號, md.產品編號);
                    #endregion
                    dataWork.AddRow(); //加載至DataTable
                }

                dataWork.UpdateDataAdapter(10); //批次更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunDeleteEnd Delete主斗(int 年度, String 產品編號, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            點燈位置資料表 TObj = new 點燈位置資料表(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { TableModule = TObj, LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.年度);
                dataWork.SelectFields(x => x.位置名稱);

                dataWork.WhereFields(x => x.空位, 0); //只Select 主Key欄位
                dataWork.WhereFields(x => x.年度, 年度);
                dataWork.WhereFields(x => x.產品編號, 產品編號);

                DataTable dt_Origin = dataWork.DataByAdapter(null); //取得資料

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }

        public RunInsertEnd Insert大斗中斗(int 年度, int 數量, int 價格, String 產品編號, int accountId)
        {//int 年度, String 主斗名稱, int 價格, String 產品編號
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            點燈位置資料表 TObj = new 點燈位置資料表();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };
                String pName = String.Empty;
                if (產品編號 == "382")
                {
                    pName = "大斗";
                }
                if (產品編號 == "383")
                {
                    pName = "中斗";
                }

                for (int i = 0; i < 數量; i++)
                {
                    dataWork.NewRow(); //開始新橧作業 產生新的一行
                    #region 指派值
                    dataWork.SetDataRowValue(x => x.年度, 年度);
                    dataWork.SetDataRowValue(x => x.位置名稱, pName + String.Format("{0:00}", i + 1));
                    dataWork.SetDataRowValue(x => x.空位, 0);
                    dataWork.SetDataRowValue(x => x.價格, 價格);
                    dataWork.SetDataRowValue(x => x.產品編號, 產品編號);
                    #endregion
                    dataWork.AddRow(); //加載至DataTable
                }

                dataWork.UpdateDataAdapter(10); //批次更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunInsertEnd Insert小斗(int 年度, int 數量, String[] 面s, String[] 柱s, int 價格, String 產品編號, int accountId)
        {//int 年度, String 主斗名稱, int 價格, String 產品編號
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            點燈位置資料表 TObj = new 點燈位置資料表();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection)
                {
                    TableModule = TObj,
                    LoginUserID = accountId
                };

                String LightTPL = "小斗{0}{1}{2:00}";

                foreach (String 面 in 面s)
                {
                    foreach (String 柱 in 柱s)
                    {
                        for (int i = 1; i <= 數量; i++)
                        {

                            #region Main Working
                            dataWork.NewRow(); //開始新橧作業 產生新的一行

                            String SiteName = String.Format(LightTPL, 面, 柱, i);
                            dataWork.SetDataRowValue(x => x.年度, 年度);
                            dataWork.SetDataRowValue(x => x.位置名稱, SiteName);
                            dataWork.SetDataRowValue(x => x.空位, 0);
                            dataWork.SetDataRowValue(x => x.價格, 價格);
                            dataWork.SetDataRowValue(x => x.產品編號, 產品編號);

                            dataWork.AddRow(); //加載至DataTable 
                            #endregion
                        }
                    }
                }

                dataWork.UpdateDataAdapter(100); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunQueryPackage<m_點燈位置資料表> SearchMaster(q_點燈位置資料表 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_點燈位置資料表> r = new RunQueryPackage<m_點燈位置資料表>();
            //點燈位置資料表 TObj = new 點燈位置資料表();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { LoginUserID = accountId };
                #endregion

                #region 設定Where條件
                if (qr.s_年度 != 0)
                    dataWork.WhereFields(x => x.年度, qr.s_年度);

                if (qr.s_產品編號 != null)
                    dataWork.WhereFields(x => x.產品編號, qr.s_產品編號);

                if (qr.s_序號 != 0)
                    dataWork.WhereFields(x => x.序號, qr.s_序號);


                if (qr.s_空位 != null)
                    dataWork.WhereFields(x => x.空位, qr.s_空位);

                if (qr.FliterLock)
                    dataWork.WhereFilterDataLock();

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    //dataWork.OrderByFields(x => x.產品編號, OrderByType.DESC);
                }
                else
                {
                    //dataWork.OrderByFields(x => x.產品編號, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_點燈位置資料表>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_點燈位置資料表> GetDataMaster(Int64 序號, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_點燈位置資料表> r = new RunOneDataEnd<m_點燈位置資料表>();
            #endregion
            try
            {
                #region Main working
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { LoginUserID = accountId };

                dataWork.WhereFields(x => x.序號, 序號);
                r.SearchData = dataWork.DataByAdapter<m_點燈位置資料表>().FirstOrDefault(); //取得Key該筆資料
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }

        /// <summary>
        /// 針對單一產品統計
        /// </summary>
        public RunOneDataEnd<m_燈位數統計狀態> q_點燈數狀態統計(int 年度, String 產品編號, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_燈位數統計狀態> r = new RunOneDataEnd<m_燈位數統計狀態>();
            r.SearchData = new m_燈位數統計狀態();

            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { LoginUserID = accountId };

                dataWork.AggregateFields(x => x.序號, "Num", AggregateType.COUNT);

                dataWork.WhereFields(x => x.年度, 年度);
                dataWork.WhereFields(x => x.產品編號, 產品編號);

                r.SearchData.產品編號 = 產品編號;

                r.SearchData.總計 = dataWork.DataByAdapter().Rows[0]["Num"].CInt();
                dataWork.Reset();

                dataWork.AggregateFields(x => x.序號, "Num", AggregateType.COUNT);
                dataWork.WhereFields(x => x.年度, 年度);
                dataWork.WhereFields(x => x.產品編號, 產品編號);
                dataWork.WhereFields(x => x.空位, "1");

                r.SearchData.已點 = dataWork.DataByAdapter(null).Rows[0]["Num"].CInt();
                dataWork.Reset();

                dataWork.AggregateFields(x => x.序號, "Num", AggregateType.COUNT);
                dataWork.WhereFields(x => x.年度, 年度);
                dataWork.WhereFields(x => x.產品編號, 產品編號);
                dataWork.WhereFields(x => x.空位, "0");

                r.SearchData.剩餘 = dataWork.DataByAdapter(null).Rows[0]["Num"].CInt();
                dataWork.Dispose();

                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".System", ex);
                return r;
                #endregion
            }
        }
        /// <summary>
        /// 針對全部產品計算
        /// </summary>
        public RunQueryPackage<m_燈位數統計狀態> q_點燈數狀態統計(int 年度, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_燈位數統計狀態> r = new RunQueryPackage<m_燈位數統計狀態>();

            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再夜由至ExecuteData達行
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = new 產品資料表(), LoginUserID = accountId };

                //總計部份
                dataWork.SelectFields(x => new { x.產品編號, x.產品名稱 });

                dataWork.WhereFields(x => x.隱藏, false);
                dataWork.WhereFields(x => x.燈位限制, true);

                dataWork.OrderByFields(x => x.產品編號);

                m_產品資料表[] mds = dataWork.DataByAdapter<m_產品資料表>();

                List<m_燈位數統計狀態> Cnt = new List<m_燈位數統計狀態>();

                foreach (m_產品資料表 md in mds)
                {
                    m_燈位數統計狀態 sti = q_點燈數狀態統計(年度, md.產品編號, accountId).SearchData;

                    sti.產品名稱 = md.產品名稱;
                    sti.年度 = 年度;
                    Cnt.Add(sti);
                }

                r.SearchData = Cnt.ToArray();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼

                Log.Write(logPlamInfo, "a_點燈位置資料表:q_點燈數狀態統計(int 年度, int accountId)", ex);

                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息

                Log.Write(logPlamInfo, "a_點燈位置資料表:q_點燈數狀態統計(int 年度, int accountId)", ex);

                return r;
                #endregion
            }
        }

        public RunOneDataEnd<m_點燈位置資料表> g_取得未使用燈位(int 年度, String 產品編號, int accountId)
        {
            #region Variable declare area
            RunOneDataEnd<m_點燈位置資料表> r = new RunOneDataEnd<m_點燈位置資料表>();
            點燈位置資料表 TObj = new 點燈位置資料表(); //宣告本區使用的主Table物件並指派給變數Tobj

            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start BeginTransaction");
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.TopLimit = 1;
                dataWork.WhereFields(x => x.年度, 年度);
                dataWork.WhereFields(x => x.空位, "0");
                dataWork.WhereFields(x => x.產品編號, 產品編號);
                dataWork.OrderByFields(x => x.序號);

                m_點燈位置資料表[] mds = dataWork.DataByAdapter<m_點燈位置資料表>();
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, 產品編號 + " Get Data OK!");
                if (mds.Length == 0)
                {
                    r.MessageCode = 1;
                    throw new ExceptionRoll("Log_Err_LightHasFinish");
                }

                dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, mds[0].序號 + "," + mds[0].位置名稱 + "," + mds[0].產品編號 + "," + mds[0].價格);

                #region 指派值
                dataWork.SetDataRowValue(x => x.空位, 1);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                dataWork.UpdateDataAdapter(); //進行變更至Database Server
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "UpdateDataAdapter OK!");
                Connection.EndCommit();
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Commit OK!");

                r.SearchData = mds[0];
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".System", ex);
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_點燈位置資料表> g_取得可選用燈位(Int64 序號, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_點燈位置資料表> r = new RunOneDataEnd<m_點燈位置資料表>();
            #endregion
            try
            {
                #region Main working
                DataTableWorking<點燈位置資料表> dataWork = new DataTableWorking<點燈位置資料表>(Connection) { LoginUserID = accountId };

                dataWork.WhereFields(x => x.序號, 序號);
                dataWork.WhereFields(x => x.空位, "0"); // 因為在購物車記錄在Session中時 即更改資料庫Flag為1，主是是為了鎖定已必免發生多人選到同一產品。
                dataWork.WhereFilterDataLock();
                //dataWork.WhereFields();

                r.SearchData = dataWork.DataByAdapter<m_點燈位置資料表>().FirstOrDefault(); //取得該筆資料
                if (r.SearchData == null)
                    throw new ExceptionRoll("Log_Err_LightHasOrdering");

                dataWork.EditFirstRow();
                //dataWork.SetDataRowValue(x => x.空位, "1");
                //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Lock); //設定Lock資訊
                dataWork.UpdateDataAdapter();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".System", ex);
                return r;
                #endregion
            }
        }
    }
    #endregion

    #region 產品資料表

    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_產品資料表 : QueryBase
    {
        public String 產品名稱 { set; get; }
        public String 產品分類 { get; set; }
        public Boolean? 燈位限制 { get; set; }
    }
    /// <summary>
    /// 資料編輯模組
    /// </summary>
    public class m_產品資料表 : ModuleBase
    {
        public String 產品編號 { get; set; }
        public String 產品名稱 { get; set; }
        public String 產品分類 { get; set; }
        public Boolean 選擇 { get; set; }
        public Boolean 燈位限制 { get; set; }
        public Int32 價格 { get; set; }

    }
    public class m_產品表單處理 : ModuleBase
    {
        /// <summary>
        /// 戶長SN
        /// </summary>
        public int MasterID { get; set; }
        /// <summary>
        /// 家庭成員個人Id
        /// </summary>
        public String SF_EPD_p1 { get; set; }
        /// <summary>
        /// 家庭成員個人姓名
        /// </summary>
        public String SF_EPD_p1_Name { get; set; }
        /// <summary>
        /// 祈福種類，也就是產品序號。
        /// </summary>
        public String SF_EPD_p2 { get; set; }

        /// <summary>
        /// 祈福種類，可選產品使用 例主副斗
        /// </summary>
        public Int64 SF_EPD_p2_Sub { get; set; }


        /// <summary>
        /// 性別
        /// </summary>
        public String SF_EPD_p5 { get; set; }
        /// <summary>
        /// 單價
        /// </summary>
        public int SF_EPD_p16 { get; set; }

        /// <summary>
        /// 農曆年
        /// </summary>
        public String SF_EPD_p6 { get; set; }
        /// <summary>
        /// 農曆月
        /// </summary>
        public String SF_EPD_p8 { get; set; }
        /// <summary>
        /// 農曆日
        /// </summary>
        public String SF_EPD_p17 { get; set; }

        /// <summary>
        /// 出生時辰
        /// </summary>
        public String SF_EPD_p3 { get; set; }

        /// <summary>
        /// 祈福地址
        /// </summary>
        public String SF_EPD_p7 { get; set; }

        /// <summary>
        /// 電話
        /// </summary>
        public String SF_EPD_p10 { get; set; }
        /// <summary>
        /// 行動電話
        /// </summary>
        public String SF_EPD_p11 { get; set; }

        /// <summary>
        /// 捐白米
        /// </summary>
        public int SF_EPD_p14 { get; set; }

        /// <summary>
        /// 捐金牌
        /// </summary>
        public int SF_EPD_p15 { get; set; }

        /// <summary>
        /// 點燈位置
        /// </summary>
        public String SF_EPD_p13 { get; set; }

        /// <summary>
        /// 文疏梯次
        /// </summary>
        public int SF_EPD_p18 { get; set; }

        /// <summary>
        /// 祈福事項
        /// </summary>
        public String SF_EPD_p12 { get; set; }
    }
    public class m_訂購人表單處理 : ModuleBase
    {
        /// <summary>
        /// 戶長SN
        /// </summary>
        public int MasterID { get; set; }
        /// <summary>
        /// 會員編號
        /// </summary>
        public String MemberID { get; set; }


        /// <summary>
        /// 訂單序號
        /// </summary>
        public int p0 { get; set; }
        /// <summary>
        /// 訂單編號
        /// </summary>
        public String p0SN { get; set; }
        /// <summary>
        /// 申請人姓名
        /// </summary>
        public String p1 { get; set; }
        /// <summary>
        /// 申請人性別
        /// </summary>
        public String p2 { get; set; }
        /// <summary>
        /// 郵遞區號
        /// </summary>
        public String p3 { get; set; }
        /// <summary>
        /// 申請人地址
        /// </summary>
        public String p4 { get; set; }
        /// <summary>
        ///  申請人手機 
        /// </summary>
        public String p5 { get; set; }
        /// <summary>
        ///  申請人電話
        /// </summary>
        public String p6 { get; set; }

    }

    /// <summary>
    /// 最新消息系統-SQL溝通模組
    /// </summary>
    public class a_產品資料表 : LogicBase<m_產品資料表, q_產品資料表, 產品資料表>
    {
        public override RunInsertEnd InsertMaster(m_產品資料表 md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            產品資料表 TObj = new 產品資料表();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.產品編號, md.產品編號);
                dataWork.SetDataRowValue(x => x.產品名稱, md.產品名稱);
                //dataWork.SetDataRowValue(x => x.產品分類, md.產品分類);
                dataWork.SetDataRowValue(x => x.價格, md.價格);
                //dataWork.SetDataRowValue(x => x.隱藏, md.隱藏);
                //dataWork.SetDataRowValue(x => x.標籤, md.標籤);
                //dataWork.SetDataRowValue(x => x.位置, md.位置);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_產品資料表 md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            產品資料表 TObj = new 產品資料表(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[TObj.產品編號.N].V = md.產品編號; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                if (dt_Origin.Rows.Count == 0) //如有資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.產品名稱, md.產品名稱);
                //dataWork.SetDataRowValue(x => x.產品分類, md.產品分類);
                dataWork.SetDataRowValue(x => x.價格, md.價格);
                //dataWork.SetDataRowValue(x => x.隱藏, md.隱藏);
                //dataWork.SetDataRowValue(x => x.標籤, md.標籤);
                //dataWork.SetDataRowValue(x => x.位置, md.位置);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(String[] deleteIds, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            產品資料表 TObj = new 產品資料表(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.產品編號); //只Select 主Key欄位
                dataWork.WhereFields(x => x.產品編號, deleteIds); //代入陣列Id值

                DataTable dt_Origin = dataWork.DataByAdapter(null); //取得資料

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_產品資料表> SearchMaster(q_產品資料表 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_產品資料表> r = new RunQueryPackage<m_產品資料表>();
            產品資料表 TObj = new 產品資料表();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再交由至ExecuteData執行
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.產品編號, x.產品名稱, x.產品分類, x.燈位限制, x.價格 });
                #endregion

                #region 設定Where條件
                if (qr.產品名稱 != null)
                    dataWork.WhereFields(x => x.產品名稱, qr.產品名稱);

                if (qr.產品分類 != null)
                    dataWork.WhereFields(x => x.產品分類, qr.產品分類);

                if (qr.燈位限制 != null)
                    dataWork.WhereFields(x => x.燈位限制, qr.燈位限制);

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                }
                else
                {
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_產品資料表>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_產品資料表> GetDataMasters(String Id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_產品資料表> r = new RunOneDataEnd<m_產品資料表>();
            #endregion
            try
            {
                #region Main working
                產品資料表 TObj = new 產品資料表();// 取得Table物
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };
                TObj.KeyFieldModules[TObj.產品編號.N].V = Id; //設定KeyValue
                m_產品資料表 md = dataWork.GetDataByKey<m_產品資料表>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_產品資料表> GetDataMaster(int Id, int accountId)
        {
            return null;
        }
        public RunOneDataEnd<m_產品資料表> 檢查訂購表單配置正確性(m_產品表單處理 md, pcar_MasterData car)
        {
            if (md.SF_EPD_p2 == null)
                throw new LogicError("Log_Err_未選擇燈位");

            RunOneDataEnd<m_產品資料表> r = this.GetDataMasters(md.SF_EPD_p2, 0);

            try
            {
                if (car != null)
                {
                    if (car.Item.Count >= 10)
                        throw new LogicError("Log_Err_超過訂購項目10");

                    if (car.Item.Where(x => x.產品編號 == e_祈福產品.保運).Count() >= 6)
                        throw new LogicError("Log_Err_保運超過訂購項目6");

                    if (car.Item.Where(x => x.會員編號 == md.SF_EPD_p1 && x.產品編號 == md.SF_EPD_p2).Count() > 0)
                        throw new LogicError("Log_Err_該產品已訂購");
                }

                if (md.SF_EPD_p7 == null)
                    throw new LogicError("Log_Err_地址未輸入");

                if (md.SF_EPD_p2 == e_祈福產品.捐金牌 && md.SF_EPD_p15 <= 0)
                    throw new LogicError("Log_Err_捐金牌需輸入金牌數");

                if (md.SF_EPD_p2 == e_祈福產品.捐白米 && md.SF_EPD_p14 <= 0)
                    throw new LogicError("Log_Err_捐白米需輸入白米斤數");

                if (r.SearchData.產品分類 == "香油" && md.SF_EPD_p16 == 0)
                    throw new LogicError("Log_Err_香油錢需輸入金額");

                if (md.SF_EPD_p2 == e_祈福產品.保運 && md.SF_EPD_p18 <= 0) // 保運
                    throw new LogicError("Log_Err_文殊梯次未選擇");

                if (md.SF_EPD_p2 == e_祈福產品.姻緣燈 && (md.SF_EPD_p6 + md.SF_EPD_p8 + md.SF_EPD_p17 == "10101"))
                    throw new LogicError("Log_Err_姻緣燈其生日資料需填寫完整");

                if (md.SF_EPD_p2 == e_祈福產品.安太歲 && md.SF_EPD_p6 + md.SF_EPD_p8 + md.SF_EPD_p17 == "10101") // 安太歲
                    throw new LogicError("Log_Err_安太歲生日不正確");

                r.Result = true;
                r.ErrMessage = "";

                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, md.SF_EPD_p2 + "," + md.SF_EPD_p1_Name + ":檢查訂購表單配置正確性 OK!");

                return r;
            }
            catch (LogicError ex)
            {
                #region Error handle
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message;
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
    }
    #endregion

    #region 訂單主檔、明細檔

    #region 主檔
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_訂單主檔 : QueryBase
    {
        public String 訂單編號 { set; get; }
    }
    /// <summary>
    /// 資料編輯模組
    /// </summary>
    public class m_訂單主檔 : ModuleBase
    {
        public Int64 訂單序號 { get; set; }
        public String 訂單編號 { get; set; }
        public String 會員編號 { get; set; }
        public String 申請人姓名 { get; set; }
        public String 申請人電話 { get; set; }
        public String 郵遞區號 { get; set; }
        public String 申請人地址 { get; set; }
        public String 申請人手機 { get; set; }
        public String 申請人性別 { get; set; }
        public String 申請人生日 { get; set; }
        public String 申請人EMAIL { get; set; }
        public Int32 總額 { get; set; }
        public String 付款方式 { get; set; }
        public DateTime 訂單時間 { get; set; }
        public DateTime 付款時間 { get; set; }
        public String 訂單狀態 { get; set; }
        public String 查詢序號 { get; set; }
        public String 付款方式名稱 { get; set; }
        public String 訂單狀態名稱 { get; set; }
        public String 銀行帳號 { get; set; }
        public DateTime 新增時間 { get; set; }
        public Int32 新增人員 { get; set; }
        public Int32 戶長SN { get; set; }
        public int orders_type { get; set; }
        public int y { get; set; }
    }
    /// <summary>
    /// 最新消息系統-SQL溝通模組
    /// </summary>
    public class a_訂單主檔 : LogicBase<m_訂單主檔, q_訂單主檔, 訂單主檔>
    {
        public a_訂單主檔()
        {
            GetTableModule = new 訂單主檔();
        }
        public override RunInsertEnd InsertMaster(m_訂單主檔 md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            訂單主檔 TObj = new 訂單主檔();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值
                dataWork.SetDataRowValue(x => x.y, md.y);
                dataWork.SetDataRowValue(x => x.訂單編號, md.訂單編號);
                dataWork.SetDataRowValue(x => x.會員編號, md.會員編號);
                dataWork.SetDataRowValue(x => x.申請人姓名, md.申請人姓名);
                dataWork.SetDataRowValue(x => x.申請人電話, md.申請人電話);
                dataWork.SetDataRowValue(x => x.郵遞區號, md.郵遞區號);
                dataWork.SetDataRowValue(x => x.申請人地址, md.申請人地址);
                dataWork.SetDataRowValue(x => x.申請人手機, md.申請人手機);
                dataWork.SetDataRowValue(x => x.申請人性別, md.申請人性別);
                dataWork.SetDataRowValue(x => x.申請人生日, md.申請人生日);
                dataWork.SetDataRowValue(x => x.申請人EMAIL, md.申請人EMAIL);
                dataWork.SetDataRowValue(x => x.總額, md.總額);
                dataWork.SetDataRowValue(x => x.付款方式, md.付款方式);
                dataWork.SetDataRowValue(x => x.訂單時間, DateTime.Now);
                dataWork.SetDataRowValue(x => x.付款時間, DateTime.Now);
                dataWork.SetDataRowValue(x => x.訂單狀態, "2");
                //dataWork.SetDataRowValue(x => x.查詢序號, md.查詢序號);
                //dataWork.SetDataRowValue(x => x.付款方式名稱, md.付款方式名稱);
                //dataWork.SetDataRowValue(x => x.訂單狀態名稱, md.訂單狀態名稱);
                //dataWork.SetDataRowValue(x => x.銀行帳號, md.銀行帳號);
                dataWork.SetDataRowValue(x => x.新增時間, DateTime.Now);
                dataWork.SetDataRowValue(x => x.新增人員, accountId);
                dataWork.SetDataRowValue(x => x.戶長SN, md.戶長SN);

                dataWork.SetDataRowValue(x => x.orders_type, md.orders_type);
                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_訂單主檔 md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            訂單主檔 TObj = new 訂單主檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[TObj.訂單編號.N].V = md.訂單編號; //取得ID欄位的值
                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                if (dt_Origin.Rows.Count == 0) //如有資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.申請人姓名, md.申請人姓名);
                dataWork.SetDataRowValue(x => x.申請人電話, md.申請人電話);
                dataWork.SetDataRowValue(x => x.郵遞區號, md.郵遞區號);
                dataWork.SetDataRowValue(x => x.申請人地址, md.申請人地址);
                dataWork.SetDataRowValue(x => x.申請人手機, md.申請人手機);
                dataWork.SetDataRowValue(x => x.申請人性別, md.申請人性別);
                dataWork.SetDataRowValue(x => x.申請人生日, md.申請人生日);
                dataWork.SetDataRowValue(x => x.申請人EMAIL, md.申請人EMAIL);
                dataWork.SetDataRowValue(x => x.總額, md.總額);
                dataWork.SetDataRowValue(x => x.新增時間, DateTime.Now);
                dataWork.SetDataRowValue(x => x.新增人員, accountId);
                dataWork.SetDataRowValue(x => x.戶長SN, md.戶長SN);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(String[] deleteIds, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            訂單主檔 TObj = new 訂單主檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = TObj, LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.訂單編號); //只Select 主Key欄位
                dataWork.WhereFields(x => x.訂單編號, deleteIds); //代入陣列Id值

                DataTable dt_Origin = dataWork.DataByAdapter(null); //取得資料

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_訂單主檔> SearchMaster(q_訂單主檔 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_訂單主檔> r = new RunQueryPackage<m_訂單主檔>();
            訂單主檔 TObj = new 訂單主檔();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再交由至ExecuteData執行
                DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = TObj, LoginUserID = accountId };
                #endregion

                #region 設定Where條件
                if (qr.訂單編號 != null)
                {
                    dataWork.WhereFields(x => x.訂單編號, qr.訂單編號);
                }

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                }
                else
                {
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_訂單主檔>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_訂單主檔> GetDataMaster(String Id, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_訂單主檔> r = new RunOneDataEnd<m_訂單主檔>();
            #endregion
            try
            {
                #region Main working
                DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = GetTableModule, LoginUserID = accountId };
                GetTableModule.KeyFieldModules[GetTableModule.訂單編號.N].V = Id; //設定KeyValue
                m_訂單主檔 md = dataWork.GetDataByKey<m_訂單主檔>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                //Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                //Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunOneDataEnd<m_訂單主檔> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }
        public String GetOrderNewSerial(int accountId)
        {
            #region 處理取得訂單編號
            DataTableWorking<訂單主檔> dataWork = new DataTableWorking<訂單主檔>(Connection) { TableModule = (new 訂單主檔()), LoginUserID = accountId };
            dataWork.AggregateFields(x => x.訂單編號, "編號", AggregateType.MAX);
            dataWork.WhereFields(x => x.訂單編號, DateTime.Now.ToString("yyMMdd"), WhereCompareType.LikeRight);
            DataTable dt = dataWork.DataByAdapter();

            return DateTime.Now.ToString("yyMMdd") +
                (dt.Rows[0][0].ToString() == "" ? "0001" : (dt.Rows[0][0].ToString().Right(4).CInt() + 1).ToString().PadLeft(4, '0'));
            #endregion
        }
    }
    #endregion

    #region 明細
    /// <summary>
    /// 查詢表單模組
    /// </summary>
    public class q_訂單明細檔 : QueryBase
    {
        public String 訂單編號 { set; get; }
        public String 產品編號 { set; get; }
        public String 會員編號 { set; get; }
    }
    /// <summary>
    /// 資料編輯模組
    /// </summary>
    public class m_訂單明細檔 : ModuleBase
    {
        public Int64 訂單序號 { get; set; }
        public String 訂單編號 { get; set; }
        public Int32 年度 { get; set; }
        public String 產品編號 { get; set; }
        public String 產品名稱 { get; set; }
        public Int32 價格 { get; set; }
        public Int32 數量 { get; set; }
        public String 申請人姓名 { get; set; }
        public String 申請人地址 { get; set; }
        public String 申請人性別 { get; set; }
        public String 申請人生日 { get; set; }
        public String 申請人年齡 { get; set; }
        public String 申請人時辰 { get; set; }
        public String 申請人生肖 { get; set; }
        public DateTime 購買時間 { get; set; }
        public DateTime 付款時間 { get; set; }
        public String 祈福事項 { get; set; }
        public String 會員編號 { get; set; }
        public String 郵遞區號 { get; set; }
        public String 點燈位置 { get; set; }
        public String 經手人 { get; set; }
        public DateTime 新增時間 { get; set; }
        public Int32 新增人員 { get; set; }
        public Int32 白米 { get; set; }
        public Int32 金牌 { get; set; }
        public Boolean 異動標記 { get; set; }
        public Int32 文疏梯次 { get; set; }
        public int detail_sort { get; set; }
        public bool is_reject { get; set; }

    }
    /// <summary>
    /// 最新消息系統-SQL溝通模組
    /// </summary>
    public class a_訂單明細檔 : LogicBase<m_訂單明細檔, q_訂單明細檔, 訂單明細檔>
    {
        public override RunInsertEnd InsertMaster(m_訂單明細檔 md, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            訂單明細檔 TObj = new 訂單明細檔();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction(); //開始交易鎖定
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.NewRow(); //開始新橧作業 產生新的一行
                #region 指派值

                dataWork.SetDataRowValue(x => x.訂單編號, md.訂單編號);
                dataWork.SetDataRowValue(x => x.年度, md.年度);
                dataWork.SetDataRowValue(x => x.產品編號, md.產品編號);
                dataWork.SetDataRowValue(x => x.產品名稱, md.產品名稱);
                dataWork.SetDataRowValue(x => x.價格, md.價格);
                dataWork.SetDataRowValue(x => x.數量, md.數量);
                dataWork.SetDataRowValue(x => x.申請人姓名, md.申請人姓名);
                dataWork.SetDataRowValue(x => x.申請人地址, md.申請人地址);
                dataWork.SetDataRowValue(x => x.申請人性別, md.申請人性別);
                dataWork.SetDataRowValue(x => x.申請人生日, md.申請人生日);
                dataWork.SetDataRowValue(x => x.申請人年齡, md.申請人年齡);
                dataWork.SetDataRowValue(x => x.申請人時辰, md.申請人時辰);
                dataWork.SetDataRowValue(x => x.申請人生肖, md.申請人生肖);
                //dataWork.SetDataRowValue(x => x.購買時間, md.購買時間);
                //dataWork.SetDataRowValue(x => x.付款時間, md.付款時間);
                dataWork.SetDataRowValue(x => x.祈福事項, md.祈福事項);
                dataWork.SetDataRowValue(x => x.會員編號, md.會員編號);
                dataWork.SetDataRowValue(x => x.郵遞區號, md.郵遞區號);
                dataWork.SetDataRowValue(x => x.點燈位置, md.點燈位置);
                dataWork.SetDataRowValue(x => x.經手人, md.經手人);
                dataWork.SetDataRowValue(x => x.新增時間, DateTime.Now);
                dataWork.SetDataRowValue(x => x.新增人員, accountId);
                dataWork.SetDataRowValue(x => x.白米, md.白米);
                dataWork.SetDataRowValue(x => x.金牌, md.金牌);
                dataWork.SetDataRowValue(x => x.異動標記, md.異動標記);
                dataWork.SetDataRowValue(x => x.文疏梯次, md.文疏梯次);

                dataWork.SetDataRowValue(x => x.is_reject, md.is_reject);
                dataWork.SetDataRowValue(x => x.detail_sort, md.detail_sort);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert); //進行更新時 需同時更新系統欄位 _InsertUserID，_InsertDateTime
                #endregion
                dataWork.AddRow(); //加載至DataTable
                dataWork.UpdateDataAdapter(); //更新 DataBase Server

                Connection.EndCommit(); //交易確認

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.InsertId = dataWork.InsertAutoFieldsID; //取得新增後自動新增欄位的值
                r.Result = true; //回傳本次執行結果為成功

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunUpdateEnd UpdateMaster(m_訂單明細檔 md, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            訂單明細檔 TObj = new 訂單明細檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                dataWork.TableModule.KeyFieldModules[TObj.訂單編號.N].V = md.訂單編號; //取得ID欄位的值
                dataWork.TableModule.KeyFieldModules[TObj.產品編號.N].V = md.產品編號; //取得ID欄位的值
                dataWork.TableModule.KeyFieldModules[TObj.會員編號.N].V = md.會員編號; //取得ID欄位的值

                DataTable dt_Origin = dataWork.GetDataByKey(); //取得Key值的Data

                if (dt_Origin.Rows.Count == 0) //如有資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                #region 指派值

                dataWork.SetDataRowValue(x => x.申請人姓名, md.申請人姓名);
                dataWork.SetDataRowValue(x => x.申請人地址, md.申請人地址);
                dataWork.SetDataRowValue(x => x.申請人性別, md.申請人性別);
                dataWork.SetDataRowValue(x => x.申請人生日, md.申請人生日);
                dataWork.SetDataRowValue(x => x.申請人年齡, md.申請人年齡);
                dataWork.SetDataRowValue(x => x.申請人時辰, md.申請人時辰);
                dataWork.SetDataRowValue(x => x.申請人生肖, md.申請人生肖);
                dataWork.SetDataRowValue(x => x.祈福事項, md.祈福事項);
                dataWork.SetDataRowValue(x => x.郵遞區號, md.郵遞區號);

                dataWork.SetDataRowValue(x => x.產品編號, md.產品編號);
                dataWork.SetDataRowValue(x => x.產品名稱, md.產品名稱);
                dataWork.SetDataRowValue(x => x.價格, md.價格);
                dataWork.SetDataRowValue(x => x.數量, 1);
                dataWork.SetDataRowValue(x => x.白米, md.白米);
                dataWork.SetDataRowValue(x => x.金牌, md.金牌);
                dataWork.SetDataRowValue(x => x.異動標記, md.異動標記);
                dataWork.SetDataRowValue(x => x.文疏梯次, md.文疏梯次);

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                dt_Origin.Dispose();
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunUpdateEnd UpdateMaster異動標記1(String OrderSerial, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            訂單明細檔 TObj = new 訂單明細檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.訂單編號, x.年度, x.會員編號, x.產品編號, x.異動標記 });
                dataWork.WhereFields(x => x.訂單編號, OrderSerial);

                DataTable dt_Origin = dataWork.DataByAdapter();

                if (dt_Origin.Rows.Count == 0) //如有資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                //dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                #region 指派值
                foreach (DataRow dr in dt_Origin.Rows)
                {
                    dr["異動標記"] = 1;
                    //dataWork.SetDataRowValue(x => x.異動標記, 1);
                }

                dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                #endregion
                dataWork.UpdateDataAdapter(); //進行變更至Database Server

                Connection.EndCommit();

                r.Rows = dataWork.AffetCount; //取得影響筆數
                r.Result = true;

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunDeleteEnd DeleteMasterOneData(String p_訂單編號, String p_產品編號, String p_會員編號, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            訂單明細檔 TObj = new 訂單明細檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.產品編號); //只Select 主Key欄位
                dataWork.SelectFields(x => x.會員編號); //只Select 主Key欄位
                dataWork.SelectFields(x => x.訂單編號); //只Select 主Key欄位

                dataWork.WhereFields(x => x.產品編號, p_產品編號); //
                dataWork.WhereFields(x => x.訂單編號, p_訂單編號); //
                dataWork.WhereFields(x => x.訂單編號, p_會員編號); //

                DataTable dt_Origin = dataWork.DataByAdapter(); //取得資料

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunDeleteEnd DeleteMasterAllData(String p_訂單編號, int accountId)
        {
            //此功能主要搭配Grid介面刪除功能製作
            #region Variable declare area
            RunDeleteEnd r = new RunDeleteEnd(); //宣告刪除Result回物件
            訂單明細檔 TObj = new 訂單明細檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main working
                Connection.BeginTransaction();
                //1、要刪除的資料先選出來
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId }; //宣告泛型物件並連接Connection
                dataWork.SelectFields(x => x.訂單編號); //只Select 主Key欄位
                dataWork.WhereFields(x => x.訂單編號, p_訂單編號); //

                DataTable dt_Origin = dataWork.DataByAdapter(); //取得資料

                //2、進行全部刪除
                dataWork.DeleteAll(); //先刪除DataTable
                dataWork.UpdateDataAdapter(); //在更新至DataBase Server
                Connection.EndCommit();
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunQueryPackage<m_訂單明細檔> SearchMaster(q_訂單明細檔 qr, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_訂單明細檔> r = new RunQueryPackage<m_訂單明細檔>();
            訂單明細檔 TObj = new 訂單明細檔();
            #endregion
            try
            {
                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再交由至ExecuteData執行
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };
                #endregion

                #region 設定Where條件
                if (qr.訂單編號 != null)
                {
                    dataWork.WhereFields(x => x.訂單編號, qr.訂單編號);
                }

                #endregion

                #region 設定排序
                if (qr.sidx == null)
                {
                    //預設排序
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.DESC);
                }
                else
                {
                    //dataWork.OrderByFields(x => x.SetDate, OrderByType.ASC);
                }
                #endregion

                #region 輸出成DataTable
                r.SearchData = dataWork.DataByAdapter<m_訂單明細檔>();
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public RunOneDataEnd<m_訂單明細檔> GetDataMaster(String p_訂單編號, String p_產品編號, String p_會員編號, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_訂單明細檔> r = new RunOneDataEnd<m_訂單明細檔>();
            #endregion
            try
            {
                #region Main working
                訂單明細檔 TObj = new 訂單明細檔();// 取得Table物
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                TObj.KeyFieldModules[TObj.產品編號.N].V = p_產品編號; //設定KeyValue
                TObj.KeyFieldModules[TObj.訂單編號.N].V = p_訂單編號; //設定KeyValue
                TObj.KeyFieldModules[TObj.會員編號.N].V = p_會員編號; //設定KeyValue

                m_訂單明細檔 md = dataWork.GetDataByKey<m_訂單明細檔>(); //取得Key該筆資料

                if (md == null) //如無資料
                    throw new ExceptionRoll("Log_Err_MustHaveData"); //此區一定有資料傳出，如無資料因檢查前端id值是否有誤

                r.SearchData = md;
                r.Result = true;
                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
        public override RunDeleteEnd DeleteMaster(string[] deleteIds, int accountId)
        {
            throw new NotImplementedException();
        }
        public override RunOneDataEnd<m_訂單明細檔> GetDataMaster(int id, int accountId)
        {
            throw new NotImplementedException();
        }

        public RunUpdateEnd UpdateOrderMember(m_訂單明細檔[] mds, int accountId)
        {
            #region Variable declare area
            RunUpdateEnd r = new RunUpdateEnd();
            訂單明細檔 TObj = new 訂單明細檔(); //宣告本區使用的主Table物件並指派給變數Tobj
            會員資料表 MObj = new 會員資料表(); //宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region Main Working
                Connection.BeginTransaction();

                foreach (var md in mds)
                {
                    DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { TableModule = TObj, LoginUserID = accountId };

                    //dataWork.TableModule.KeyFieldModules[TObj.訂單編號.N].V = md.訂單編號; //取得ID欄位的值
                    //dataWork.TableModule.KeyFieldModules[TObj.會員編號.N].V = md.會員編號; //取得ID欄位的值
                    Log.Write(md.ToString());

                    dataWork.SelectFields(x => new { x.產品編號, x.訂單編號, x.會員編號, x.申請人姓名, x.申請人性別, x.申請人生肖, x.申請人時辰, x.申請人地址, x.點燈位置, x.產品名稱, x.申請人生日 });

                    dataWork.WhereFields(x => x.訂單編號, md.訂單編號);
                    dataWork.WhereFields(x => x.會員編號, md.會員編號);
                    dataWork.WhereFields(x => x.產品編號, md.產品編號);

                    var dt_Origin = dataWork.DataByAdapter<m_訂單明細檔>();

                    Log.Write(String.Format("{0},{1},{2},{3}", md.訂單編號, md.會員編號, md.產品編號, dt_Origin == null));

                    if (dt_Origin.Count() > 0) //如有資料
                    {
                        //dataWork.EditFirstRow(); //編輯第一筆資料，正常來說只會有一筆資料。
                        #region 指派值
                        dataWork.EditFirstRow();
                        dataWork.SetDataRowValue(x => x.申請人姓名, md.申請人姓名);
                        dataWork.SetDataRowValue(x => x.申請人地址, md.申請人地址);
                        dataWork.SetDataRowValue(x => x.申請人性別, md.申請人性別);
                        dataWork.SetDataRowValue(x => x.申請人生日, md.申請人生日);
                        //dataWork.SetAllRowValue("申請人年齡", md.申請人年齡);
                        dataWork.SetDataRowValue(x => x.申請人時辰, md.申請人時辰);
                        dataWork.SetDataRowValue(x => x.申請人生肖, md.申請人生肖);
                        //dataWork.SetAllRowValue("郵遞區號", md.郵遞區號);

                        //dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update); //進行更新時 需同時更新系統欄位 _UpdateUserID，_UpdateDateTime
                        #endregion
                        dataWork.UpdateDataAdapter(); //進行變更至Database Server

                        //Log.Write("會員編號：" + md.會員編號.ToString());

                        //DataTableWorking<會員資料表> dataMember =
                        //    new DataTableWorking<會員資料表>(Connection) { TableModule = MObj, LoginUserID = accountId };
                        //Log.Write("設定Data");
                        //MObj.KeyFieldModules[MObj.序號.N].V = md.會員編號; //設定KeyValue

                        ////dataMember.WhereFields(x => x.序號, md.會員編號);

                        //var org_md = dataMember.GetDataByKey();
                        //Log.Write("取得Data" + org_md.Rows.Count);

                        //if (org_md == null)
                        //{
                        //    throw new ExceptionRoll("No Data");
                        //}
                        //dataMember.EditFirstRow();
                        //dataMember.SetDataRowValue(x => x.姓名, md.申請人姓名);
                        //dataMember.SetDataRowValue(x => x.地址, md.申請人地址);
                        //dataMember.SetDataRowValue(x => x.時辰, md.申請人時辰);
                        //dataMember.SetDataRowValue(x => x.性別, md.申請人性別);

                        //String[] bd = md.申請人生日.Split('/');
                        //String dt = (bd[0].CInt() + 1911) + "/" + bd[1] + "/" + bd[2];

                        //dataMember.SetDataRowValue(x => x.生日, dt);
                        //Log.Write("更新資料前:" + md.會員編號);
                        //dataMember.UpdateDataAdapter();
                        //Log.Write("更新資料後:" + md.會員編號);
                    }
                }

                Connection.EndCommit();
                r.Result = true;

                return r;
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Log.Write(ex.Message);
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Log.Write(ex.Message + ":" + ex.StackTrace);
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }


    }
    #endregion

    #region 主副處理
    public class a_訂單主副 : LogicBase
    {
        public RunInsertEnd InsertData(int Y, m_訂購人表單處理 md, pcar_MasterData car, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            //訂單主檔 MTObj = new 訂單主檔();//宣告本區使用的主Table物件並指派給變數Tobj
            //訂單明細檔 DTObj = new 訂單明細檔();//宣告本區使用的主Table物件並指派給變數Tobj
            #endregion
            try
            {
                #region 檢查區
                #region MyRegion
                if (car == null)
                    throw new LogicError("Log_Err_未選購任何產品");

                #endregion
                #endregion

                #region Main Working
                Connection.Tran(); //開始交易鎖定
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start BeginTransaction");

                a_產品資料表 acProduct = new a_產品資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                RunQueryPackage<m_產品資料表> 產品總資料 = acProduct.SearchMaster(new q_產品資料表() { }, accountId); //取得產品資料
                var 群組_各項產品訂購數量 = car.Item.GroupBy(x => x.產品編號,
                    (key, Num) => new
                    {
                        產品編號 = key,
                        預訂產品數量 = Num.Count()
                    });

                #region 燈位剩下檢查

                var 燈限產品 = 產品總資料.SearchData.Where(x => x.燈位限制 == true);
                a_點燈位置資料表 ac燈位 = new a_點燈位置資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };

                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "產品訂購數量查詢");

                foreach (var 產品訂購數量 in 群組_各項產品訂購數量)
                {
                    var 產品 = 燈限產品.Where(x => x.產品編號 == 產品訂購數量.產品編號);
                    if (產品.Count() > 0)
                    {
                        RunOneDataEnd<m_燈位數統計狀態> m_燈位_數量狀態統計 = ac燈位.q_點燈數狀態統計(Y, 產品訂購數量.產品編號, accountId);
                        if (m_燈位_數量狀態統計.SearchData.剩餘 < 產品訂購數量.預訂產品數量)
                            throw new LogicError("Log_Err_燈位位置不足或已用完(" + 產品.FirstOrDefault().產品名稱 + "/燈位數：" + m_燈位_數量狀態統計.SearchData.總計 +
                                "/剩餘：" + m_燈位_數量狀態統計.SearchData.剩餘 + ")");
                    }
                }
                Log.Write(logPlamInfo, "產品訂購數量查詢", "End");
                #endregion

                #region 取得會員戶單位全員資料
                a_會員資料表 acMember = new a_會員資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                RunQueryPackage<m_會員資料表> r_Members = acMember.SearchMaster(new q_會員資料表() { 戶長SN = md.MasterID }, accountId);
                m_會員資料表[] mdMembers = r_Members.SearchData; //取得該戶全員資料 
                #endregion

                #region 主檔處理
                a_訂單主檔 acM = new a_訂單主檔() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                m_訂單主檔 mdM = new m_訂單主檔();

                String 新訂單編號 = acM.GetOrderNewSerial(accountId);
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "新訂單編號" + 新訂單編號);

                r.CustomId = 新訂單編號;
                mdM.訂單編號 = 新訂單編號;

                mdM.會員編號 = md.MemberID;
                mdM.戶長SN = md.MasterID;
                mdM.申請人姓名 = md.p1;
                mdM.申請人電話 = md.p6;
                mdM.申請人手機 = md.p5;
                mdM.申請人地址 = md.p4;
                mdM.郵遞區號 = md.p3;
                mdM.申請人性別 = md.p2;
                mdM.付款方式 = "3";
                mdM.總額 = car.Item.Sum(x => x.價格);
                mdM.新增人員 = accountId;
                mdM.orders_type = 0;
                mdM.y = Y;
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "訂單主檔 Start");
                RunInsertEnd rM = acM.InsertMaster(mdM, accountId);
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "訂單主檔 End");
                r.InsertId = rM.InsertId;

                if (rM.Result == false)
                {
                    if (rM.ErrType == BusinessErrType.Logic)
                        throw new ExceptionRoll(rM.ErrMessage);

                    if (rM.ErrType == BusinessErrType.System)
                        throw new Exception(rM.ErrMessage);
                }
                #endregion

                #region 明細檔處理
                a_訂單明細檔 acD = new a_訂單明細檔() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };

                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "訂單明細 Start");
                foreach (pcar_DetailData p in car.Item)
                {
                    #region MyRegion
                    //取得會員資料
                    m_會員資料表 mdMember = mdMembers.Single(x => x.序號 == p.會員編號.CInt());
                    m_訂單明細檔 mdD = new m_訂單明細檔();

                    String 燈位名稱 = String.Empty;
                    if (燈限產品.Any(x => x.產品編號 == p.產品編號))
                    {
                        if (p.點燈位置序號 == 0) //一般由電腦選位會走這一段
                        {
                            RunOneDataEnd<m_點燈位置資料表> 取得點燈位置 = ac燈位.g_取得未使用燈位(Y, p.產品編號, accountId);
                            if (取得點燈位置.Result)
                                燈位名稱 = 取得點燈位置.SearchData.位置名稱;
                            else
                            {
                                if (取得點燈位置.ErrType == BusinessErrType.Logic) throw new ExceptionRoll(取得點燈位置.ErrMessage);
                                if (取得點燈位置.ErrType == BusinessErrType.System) throw new Exception(取得點燈位置.ErrMessage);
                            }
                        }
                        else //由人工選位會走這一段
                        {
                            //主副斗直接取燈位

                            DataTableWorking<點燈位置資料表> workData = new DataTableWorking<點燈位置資料表>(this.Connection);
                            workData.WhereFields(x => x.序號, p.點燈位置序號);
                            m_點燈位置資料表[] mds = workData.DataByAdapter<m_點燈位置資料表>();

                            燈位名稱 = mds[0].位置名稱;

                            workData.EditFirstRow();
                            workData.SetDataRowValue(x => x.空位, "1");
                            workData.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                            workData.UpdateFieldsInfo(UpdateFieldsInfoType.UnLock);
                            workData.UpdateDataAdapter();
                            workData.Dispose();
                        }
                    }
                    else
                    {
                        燈位名稱 = "";
                    }

                    mdD.年度 = Y;
                    mdD.訂單編號 = 新訂單編號;
                    mdD.會員編號 = p.會員編號;
                    mdD.產品編號 = p.產品編號;

                    mdD.產品名稱 = p.產品名稱;
                    mdD.申請人姓名 = p.申請人姓名;
                    mdD.申請人生日 = p.申請人生日;
                    mdD.申請人生肖 = mdMember.生肖;
                    mdD.申請人時辰 = p.申請人時辰;
                    mdD.申請人地址 = p.申請人地址;
                    mdD.申請人性別 = p.申請人性別;
                    mdD.文疏梯次 = p.文疏;
                    mdD.價格 = p.價格;
                    mdD.數量 = 1;
                    mdD.點燈位置 = 燈位名稱;

                    mdD.白米 = p.白米;
                    mdD.金牌 = p.金牌;

                    mdD.新增人員 = accountId;
                    mdD.is_reject = false;
                    mdD.detail_sort = 0;

                    Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, p.產品名稱 + "," + p.申請人姓名);
                    //mdD.新增時間 = DateTime.Now;

                    RunInsertEnd rD = acD.InsertMaster(mdD, accountId);
                    if (rD.Result == false)
                    {
                        if (rD.ErrType == BusinessErrType.Logic)
                            throw new ExceptionRoll(rD.ErrMessage);

                        if (rD.ErrType == BusinessErrType.System)
                            throw new Exception(rD.ErrMessage);
                    }
                    #endregion
                }
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "訂單明細 End");
                #endregion

                Connection.Commit();
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Commit");
                r.Result = true; //回傳本次執行結果為成功
                return r;
                #endregion
            }
            catch (LogicError ex)
            {
                #region Error handle
                Connection.Roll(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗訊息

                Log.Write(logPlamInfo, "a_訂單主副.InsertData.LogicException", ex);

                return r;
                #endregion
            }

            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Roll(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗訊息

                Log.Write(logPlamInfo, "a_訂單主副.InsertData.LogicExceptionRoll", ex);

                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Roll(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息

                Log.Write(logPlamInfo, "a_訂單主副.InsertData.Exception", ex);

                return r;
                #endregion
            }
        }
        public RunInsertEnd UpdateData(int Y, m_訂購人表單處理 md, pcar_MasterData car, int accountId)
        {
            #region Variable declare area
            RunInsertEnd r = new RunInsertEnd(); //宣告回傳物件
            #endregion
            try
            {
                #region 檢查區
                #region MyRegion
                if (car == null)
                    throw new LogicError("Log_Err_未選購任何產品");
                //有可能為空訂單 ，故下列檢查暫停
                //if (car.Item == null)
                //    throw new LogicException("Log_Err_未選購任何產品");

                //if (car.Item.Count == 0)
                //    throw new LogicException("Log_Err_未選購任何產品");
                #endregion
                #endregion

                #region Main Working
                Connection.Tran(); //開始交易鎖定
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start BeginTransaction");

                a_產品資料表 acProduct = new a_產品資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                RunQueryPackage<m_產品資料表> 產品總資料 = acProduct.SearchMaster(new q_產品資料表() { }, accountId); //取得產品資料

                var 燈限產品 = 產品總資料.SearchData.Where(x => x.燈位限制 == true);

                #region 取得會員戶單位全員資料
                a_會員資料表 acMember = new a_會員資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                RunQueryPackage<m_會員資料表> r_Members = acMember.SearchMaster(new q_會員資料表() { 戶長SN = md.MasterID }, accountId);
                m_會員資料表[] mdMembers = r_Members.SearchData; //取得該戶全員資料 
                #endregion

                #region 主檔處理
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Start 主檔");

                a_訂單主檔 acM = new a_訂單主檔() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };
                m_訂單主檔 mdM = new m_訂單主檔();
                //String 新訂單編號 = acM.GetOrderNewSerial(accountId);
                mdM.訂單編號 = md.p0SN;

                mdM.會員編號 = md.MemberID;
                mdM.戶長SN = md.MasterID;
                mdM.申請人姓名 = md.p1;
                mdM.申請人電話 = md.p6;
                mdM.申請人手機 = md.p5;
                mdM.申請人地址 = md.p4;
                mdM.郵遞區號 = md.p3;
                mdM.申請人性別 = md.p2;
                mdM.付款方式 = "3";
                mdM.總額 = car.Item.Sum(x => x.價格);
                mdM.新增人員 = accountId;

                RunUpdateEnd rM = acM.UpdateMaster(mdM, accountId);
                if (rM.Result == false)
                {
                    if (rM.ErrType == BusinessErrType.Logic)
                        throw new ExceptionRoll(rM.ErrMessage);

                    if (rM.ErrType == BusinessErrType.System)
                        throw new Exception(rM.ErrMessage);
                }
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "End 主檔");

                #endregion

                #region 明細檔處理
                Log.Write(logPlamInfo, "明細", "Start");
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { LoginUserID = accountId };
                dataWork.WhereFields(x => x.訂單編號, md.p0SN);
                m_訂單明細檔[] md_OrderDetail = dataWork.DataByAdapter<m_訂單明細檔>();
                dataWork.SetAllRowValue(dataWork.TableModule.異動標記.N, true);

                a_點燈位置資料表 ac燈位 = new a_點燈位置資料表() { Connection = this.Connection, logPlamInfo = this.logPlamInfo };

                foreach (pcar_DetailData car明細 in car.Item)
                {
                    #region MyRegion
                    //
                    m_會員資料表 mdMember = mdMembers.First(x => x.序號 == car明細.會員編號.CInt());
                    m_訂單明細檔 mdD = new m_訂單明細檔();

                    dataWork.TableModule.KeyFieldModules["訂單編號"].V = md.p0SN;
                    dataWork.TableModule.KeyFieldModules["會員編號"].V = car明細.會員編號;
                    dataWork.TableModule.KeyFieldModules["產品編號"].V = car明細.產品編號;

                    if (dataWork.GetDataRowByKeyHasData())
                    {
                        #region 有資料狀態處理

                        dataWork.SetDataRowValue(x => x.產品名稱, car明細.產品名稱);
                        dataWork.SetDataRowValue(x => x.申請人姓名, car明細.申請人姓名);
                        dataWork.SetDataRowValue(x => x.申請人生日, car明細.申請人生日);
                        dataWork.SetDataRowValue(x => x.申請人生肖, mdMember.生肖);
                        dataWork.SetDataRowValue(x => x.申請人時辰, car明細.申請人時辰);
                        dataWork.SetDataRowValue(x => x.申請人地址, car明細.申請人地址);
                        dataWork.SetDataRowValue(x => x.申請人性別, car明細.申請人性別);
                        dataWork.SetDataRowValue(x => x.文疏梯次, car明細.文疏);
                        //                    mdD.文疏梯次 = p.文疏;
                        dataWork.SetDataRowValue(x => x.價格, car明細.價格);
                        dataWork.SetDataRowValue(x => x.白米, car明細.白米);
                        dataWork.SetDataRowValue(x => x.金牌, car明細.金牌);
                        dataWork.SetDataRowValue(x => x.異動標記, false);
                        dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Update);
                        #endregion
                    }
                    else
                    {
                        #region 新增產品項目

                        String 燈位名稱 = String.Empty;
                        if (燈限產品.Any(x => x.產品編號 == car明細.產品編號))
                        {
                            RunOneDataEnd<m_點燈位置資料表> 取得點燈位置 = ac燈位.g_取得未使用燈位(Y, car明細.產品編號, accountId);
                            if (取得點燈位置.Result)
                            {
                                燈位名稱 = 取得點燈位置.SearchData.位置名稱;
                            }
                            else
                            {
                                if (取得點燈位置.ErrType == BusinessErrType.Logic) throw new ExceptionRoll(取得點燈位置.ErrMessage);
                                if (取得點燈位置.ErrType == BusinessErrType.System) throw new Exception(取得點燈位置.ErrMessage);
                            }
                        }
                        else
                            燈位名稱 = "";

                        dataWork.NewRow();
                        dataWork.SetDataRowValue(x => x.年度, Y);
                        dataWork.SetDataRowValue(x => x.訂單編號, md.p0SN);
                        dataWork.SetDataRowValue(x => x.會員編號, car明細.會員編號);
                        dataWork.SetDataRowValue(x => x.產品編號, car明細.產品編號);
                        dataWork.SetDataRowValue(x => x.點燈位置, 燈位名稱);

                        dataWork.SetDataRowValue(x => x.產品名稱, car明細.產品名稱);
                        dataWork.SetDataRowValue(x => x.申請人姓名, car明細.申請人姓名);
                        dataWork.SetDataRowValue(x => x.申請人生日, car明細.申請人生日);
                        dataWork.SetDataRowValue(x => x.申請人生肖, mdMember.生肖);
                        dataWork.SetDataRowValue(x => x.申請人時辰, car明細.申請人時辰);
                        dataWork.SetDataRowValue(x => x.申請人地址, car明細.申請人地址);
                        dataWork.SetDataRowValue(x => x.申請人性別, car明細.申請人性別);
                        dataWork.SetDataRowValue(x => x.文疏梯次, car明細.文疏);

                        dataWork.SetDataRowValue(x => x.數量, 1);
                        dataWork.SetDataRowValue(x => x.新增人員, accountId);
                        dataWork.SetDataRowValue(x => x.新增時間, DateTime.Now);
                        dataWork.SetDataRowValue(x => x.價格, car明細.價格);
                        dataWork.SetDataRowValue(x => x.白米, car明細.白米);
                        dataWork.SetDataRowValue(x => x.金牌, car明細.金牌);
                        dataWork.SetDataRowValue(x => x.異動標記, false);

                        dataWork.AddRow();
                        dataWork.UpdateFieldsInfo(UpdateFieldsInfoType.Insert);
                        #endregion
                    }
                    #endregion
                }

                IEnumerable<DataRow> DeleteRows = dataWork.DataTable.AsEnumerable().Where(x => x.Field<Boolean>("異動標記") == true);
                List<String> restore_rows = new List<String>();
                foreach (DataRow dr in DeleteRows)
                {
                    if (dr["點燈位置"] != DBNull.Value)
                    {
                        restore_rows.Add(dr["點燈位置"].ToString());
                    }
                    dr.Delete();
                }
                Log.Write(logPlamInfo, "明細", "End");
                #endregion

                dataWork.UpdateDataAdapter();

                //如果刪單要還原 原位置值 為0，如果是有選燈位

                if (restore_rows.Count > 0)
                {
                    Log.Write(logPlamInfo, "刪除", "Start");
                    DataTableWorking<點燈位置資料表> dataWorkLight = new DataTableWorking<點燈位置資料表>(Connection) { LoginUserID = accountId };
                    dataWorkLight.WhereFields(x => x.位置名稱, restore_rows.ToArray());
                    dataWorkLight.WhereFields(x => x.年度, Y);

                    Log.Write(logPlamInfo, "刪除Select", "Start");
                    DataTable dt_Lights = dataWorkLight.DataByAdapter();
                    //LogWrite.Write("刪除Select SQL", dataWorkLight.ExeSQL);
                    Log.Write(logPlamInfo, "刪除Select", "End");

                    foreach (DataRow dr in dt_Lights.Rows)
                    {
                        dr["空位"] = "0";
                    }

                    dataWorkLight.UpdateDataAdapter();
                }
                Log.Write(logPlamInfo, "刪除", "End");

                Connection.Commit();
                Log.Write(logPlamInfo, "Tran", "End");
                r.Result = true; //回傳本次執行結果為成功
                return r;
                #endregion
            }
            catch (LogicError ex)
            {
                #region Error handle
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗訊息

                //LogWrite.ErrWrite("a_訂單主副.UpdateData.LogicException", ex);
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                return r;
                #endregion
            }

            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Roll(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗訊息

                //LogWrite.ErrWrite("a_訂單主副.UpdateData.LogicExceptionRoll", ex);
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Roll(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息

                //LogWrite.ErrWrite("a_訂單主副.UpdateData.Exception", ex);
                Log.Write(logPlamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".System", ex);
                return r;
                #endregion
            }
        }

        public RunQueryPackage<m_訂單明細檔> SearchMaster禮斗明細表(int Y, int accountId)
        {
            #region 變數宣告
            RunQueryPackage<m_訂單明細檔> r = new RunQueryPackage<m_訂單明細檔>();
            產品資料表 TObj = new 產品資料表();
            #endregion
            try
            {
                Log.Write(logPlamInfo, "SearchMaster禮斗明細表", "Start");
                #region Select Data 區段 By 條件
                //先找出禮料類別的產品編號
                DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.產品編號 });
                dataWork.WhereFields(x => x.產品分類, "禮斗");
                var data = dataWork.DataByAdapter<m_產品資料表>();
                Log.Write(logPlamInfo, "SQL", dataWork.ExeSQL);

                //在從訂單明細找出申請人
                DataTableWorking<訂單明細檔> dataDetail = new DataTableWorking<訂單明細檔>(Connection) { TableModule = new 訂單明細檔(), LoginUserID = accountId };
                dataDetail.SelectFields(x => new { x.產品編號, x.產品名稱, x.申請人姓名, x.申請人地址 });
                dataDetail.WhereFields(x => x.產品編號, data.Select(x => x.產品編號).ToArray());
                dataDetail.WhereFields(x => x.年度, Y);
                dataDetail.OrderByFields(x => x.新增時間, OrderByType.DESC);
                r.SearchData = dataDetail.DataByAdapter<m_訂單明細檔>();
                Log.Write(logPlamInfo, "SQL", dataDetail.ExeSQL);

                Log.Write(logPlamInfo, "SearchMaster禮斗明細表", "End");
                r.Result = true;
                return r;

                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Log.Write(logPlamInfo, "SearchMaster禮斗明細表", ex);
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼

                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Log.Write(logPlamInfo, "SearchMaster禮斗明細表", ex);
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息

                return r;
                #endregion
            }
        }

    }
    #endregion

    #endregion

    #region 報表

    public class q_統計表列印
    {
        /// <summary>
        /// 列印日期：起 
        /// </summary>
        public String Date1 { get; set; }
        /// <summary>
        /// 起
        /// </summary>
        public int Time1 { get; set; }
        /// <summary>
        /// 列印日期：迄
        /// </summary>
        public String Date2 { get; set; }
        /// <summary>
        /// 迄
        /// </summary>
        public int Time2 { get; set; }
        /// <summary>
        /// 經手人員
        /// </summary>
        public int People { get; set; }
    }

    public class m_統計表
    {
        public String 負責人 { get; set; }
        public String 民國 { get; set; }
        public String 時間 { get; set; }

        public int 太歲 { get; set; }
        public int 法會_入斗 { get; set; }

        public int 文昌燈 { get; set; }
        public int 文昌頭燈 { get; set; }

        public int 媽祖燈 { get; set; }
        public int 媽祖頭燈 { get; set; }

        public int 觀音燈 { get; set; }
        public int 觀音頭燈 { get; set; }

        public int 關聖燈 { get; set; }
        public int 關聖頭燈 { get; set; }

        public int 財神燈 { get; set; }
        public int 財神頭燈 { get; set; }

        public int 姻緣燈 { get; set; }
        public int 姻緣頭燈 { get; set; }

        public int 香油錢 { get; set; }
        public int 香油_信徒觀摩 { get; set; }
        public int 香油_祈願卡 { get; set; }
        public int 香油_媽祖聖誕擲筊 { get; set; }
        public int 香油_媽祖回鑾 { get; set; }
        public int 香油_專案專款 { get; set; }
        public int 香油_媽祖聖誕典禮 { get; set; }

        public int 香油_其它 { get; set; }
        public int 香油_牛軋糖 { get; set; }
        public int 香油_農民曆廣告 { get; set; }//2016/12/7 下架
        public int 香油_衣服 { get; set; }
        public int 香油_薦拔祖先 { get; set; }//2017/7/19 下架
        public int 香油_冤親債主 { get; set; }//2017/7/19 下架
        public int 香油_嬰靈 { get; set; }//2017/7/19 下架
        public int 香油_屋頂整修費 { get; set; }//2016/12/7 加入
        public int 香油_祈福玉珮 { get; set; }//2016/12/30 加入

        public int 香油_契子觀摩 { get; set; }

        public int 契子會_入會 { get; set; }
        public int 契子會_大會 { get; set; }

        public int 主斗 { get; set; }
        public int 副斗 { get; set; }//2017/3/2改名為斗首
        public int 福斗 { get; set; }//2017/3/2新增
        public int 大斗 { get; set; }
        public int 中斗 { get; set; }
        public int 小斗 { get; set; }

        public int 租金 { get; set; }
        public int 保運 { get; set; }
        public int 金牌 { get; set; }
        public int 白米 { get; set; }
        public int 合計金額 { get; set; }
        public int 合計燈類金額 { get; set; }
        public int 合計禮斗 { get; set; }

        //總和
        public int 媽祖殿燈籠頭燈加總 { get; set; }
        public int 頭燈前排福燈 { get; set; }
        public int 頭燈後排福燈 { get; set; }
        public int 前排福燈 { get; set; }
        public int 右排福燈 { get; set; }
        public int 左排福燈 { get; set; }
        public int 後排福燈 { get; set; }
        public int 上排福燈 { get; set; }

        public int 超渡法會_薦拔祖先 { get; set; }
        public int 超渡法會_冤親債主 { get; set; }
        public int 超渡法會_嬰靈 { get; set; }
    }
    public class m_統計數據
    {
        public String 產品編號 { get; set; }
        public String 產品名稱 { get; set; }
        public Int32 數量 { get; set; }
        public int 金牌 { get; set; }
        public int 白米 { get; set; }
    }

    public class a_報表查詢 : LogicBase
    {
        public RunOneDataEnd<m_統計表> SearchMaster(q_統計表列印 qr, int accountId)
        {
            #region 變數宣告
            RunOneDataEnd<m_統計表> r = new RunOneDataEnd<m_統計表>();
            // 產品資料表 TObj = new 產品資料表();
            #endregion
            try
            {
                DataTableWorking<人員> data人員 = new DataTableWorking<人員>(Connection) { LoginUserID = accountId };
                data人員.SelectFields(x => x.人員代碼);
                data人員.SelectFields(x => x.姓名);
                //data人員.TableModule.KeyFieldModules["人員代碼"].V = qr.People;
                data人員.WhereFields(x => x.人員代碼, qr.People);
                m_人員 md人員 = data人員.DataByAdapter<m_人員>().FirstOrDefault();

                #region Select Data 區段 By 條件
                #region 設定輸出至Grid欄位 以下方式請注音 1、只適合單一Table 2、主要用於Grid顯示，如此方式不適合，可自行組SQL字串再交由至ExecuteData執行
                DataTableWorking<訂單明細檔> dataWork = new DataTableWorking<訂單明細檔>(Connection) { LoginUserID = accountId };
                dataWork.SelectFields(x => new { x.產品編號, x.產品名稱 });
                dataWork.AggregateFields(x => x.價格, "數量", AggregateType.SUM);

                dataWork.AggregateFields(x => x.白米, "白米", AggregateType.SUM);
                dataWork.AggregateFields(x => x.金牌, "金牌", AggregateType.SUM);
                #endregion

                #region 設定Where條件
                if (qr.Date1 != null)
                    dataWork.WhereFields(x => x.新增時間, DateTime.Parse(qr.Date1 + " " + (qr.Time1 - 1) + ":00:00").AddMinutes(59).AddSeconds(59), WhereCompareType.ThanEquel, "新增時間1");

                if (qr.Date2 != null)
                    dataWork.WhereFields(x => x.新增時間, DateTime.Parse(qr.Date2 + " " + (qr.Time2 - 1) + ":00:00").AddMinutes(59).AddSeconds(59), WhereCompareType.LessEquel, "新增時間2");

                if (qr.People > 0)
                    dataWork.WhereFields(x => x.新增人員, qr.People);
                #endregion

                #region 輸出成DataTable
                m_統計數據[] mds = dataWork.DataByAdapter<m_統計數據>();
                m_統計表 md = new m_統計表();

                md.負責人 = md人員.姓名;
                md.民國 = DateTime.Parse(qr.Date1).To民國年() + "~" + DateTime.Parse(qr.Date2).To民國年();
                md.時間 = String.Format("{0:00}:00", qr.Time1) + "~" + String.Format("{0:00}:00", qr.Time2);

                md.文昌燈 = mds.Any(x => x.產品編號 == e_祈福產品.文昌燈) ? mds.First(x => x.產品編號 == e_祈福產品.文昌燈).數量 : 0;
                md.文昌頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.文昌頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.文昌頭燈).數量 : 0;

                md.姻緣燈 = mds.Any(x => x.產品編號 == e_祈福產品.姻緣燈) ? mds.First(x => x.產品編號 == e_祈福產品.姻緣燈).數量 : 0;
                md.姻緣頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.月老頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.月老頭燈).數量 : 0;

                md.財神燈 = mds.Any(x => x.產品編號 == e_祈福產品.財神燈) ? mds.First(x => x.產品編號 == e_祈福產品.財神燈).數量 : 0;
                md.財神頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.財神頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.財神頭燈).數量 : 0;

                md.媽祖燈 = mds.Any(x => x.產品編號 == e_祈福產品.媽祖燈) ? mds.First(x => x.產品編號 == e_祈福產品.媽祖燈).數量 : 0;
                md.媽祖頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.媽祖頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.媽祖頭燈).數量 : 0;

                md.關聖燈 = mds.Any(x => x.產品編號 == e_祈福產品.關聖燈) ? mds.First(x => x.產品編號 == e_祈福產品.關聖燈).數量 : 0;
                md.關聖頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.關聖頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.關聖頭燈).數量 : 0;

                md.觀音燈 = mds.Any(x => x.產品編號 == e_祈福產品.觀音燈) ? mds.First(x => x.產品編號 == e_祈福產品.觀音燈).數量 : 0;
                md.觀音頭燈 = mds.Any(x => x.產品編號 == e_祈福產品.觀音頭燈) ? mds.First(x => x.產品編號 == e_祈福產品.觀音頭燈).數量 : 0;

                md.香油錢 = mds.Any(x => x.產品編號 == e_祈福產品.香油錢) ? mds.First(x => x.產品編號 == e_祈福產品.香油錢).數量 : 0;
                md.香油_媽祖聖誕擲筊 = mds.Any(x => x.產品編號 == e_祈福產品.香油_媽祖聖誕擲筊) ? mds.First(x => x.產品編號 == e_祈福產品.香油_媽祖聖誕擲筊).數量 : 0;
                md.香油_媽祖回鑾 = mds.Any(x => x.產品編號 == e_祈福產品.香油_媽祖回鑾) ? mds.First(x => x.產品編號 == e_祈福產品.香油_媽祖回鑾).數量 : 0;
                md.香油_祈願卡 = mds.Any(x => x.產品編號 == e_祈福產品.香油_祈願卡) ? mds.First(x => x.產品編號 == e_祈福產品.香油_祈願卡).數量 : 0;
                md.香油_契子觀摩 = mds.Any(x => x.產品編號 == e_祈福產品.香油_契子觀摩) ? mds.First(x => x.產品編號 == e_祈福產品.香油_契子觀摩).數量 : 0;
                md.香油_信徒觀摩 = mds.Any(x => x.產品編號 == e_祈福產品.香油_信徒觀摩) ? mds.First(x => x.產品編號 == e_祈福產品.香油_信徒觀摩).數量 : 0;
                md.香油_其它 = mds.Any(x => x.產品編號 == e_祈福產品.香油_其它) ? mds.First(x => x.產品編號 == e_祈福產品.香油_其它).數量 : 0;
                md.契子會_入會 = mds.Any(x => x.產品編號 == e_祈福產品.契子會_入會) ? mds.First(x => x.產品編號 == e_祈福產品.契子會_入會).數量 : 0;
                md.契子會_大會 = mds.Any(x => x.產品編號 == e_祈福產品.契子會_大會) ? mds.First(x => x.產品編號 == e_祈福產品.契子會_大會).數量 : 0;

                md.太歲 = mds.Any(x => x.產品編號 == e_祈福產品.安太歲) ? mds.First(x => x.產品編號 == e_祈福產品.安太歲).數量 : 0;
                md.法會_入斗 = mds.Any(x => x.產品編號 == e_祈福產品.入斗) ? mds.First(x => x.產品編號 == e_祈福產品.入斗).數量 : 0;
                md.保運 = mds.Any(x => x.產品編號 == e_祈福產品.保運) ? mds.First(x => x.產品編號 == e_祈福產品.保運).數量 : 0;

                md.白米 = mds.Any(x => x.產品編號 == e_祈福產品.捐白米) ? mds.First(x => x.產品編號 == e_祈福產品.捐白米).白米 : 0;
                md.金牌 = mds.Any(x => x.產品編號 == e_祈福產品.捐金牌) ? mds.First(x => x.產品編號 == e_祈福產品.捐金牌).金牌 : 0;

                md.大斗 = mds.Any(x => x.產品編號 == e_祈福產品.大斗) ? mds.First(x => x.產品編號 == e_祈福產品.大斗).數量 : 0;
                md.中斗 = mds.Any(x => x.產品編號 == e_祈福產品.中斗) ? mds.First(x => x.產品編號 == e_祈福產品.中斗).數量 : 0;
                md.小斗 = mds.Any(x => x.產品編號 == e_祈福產品.小斗) ? mds.First(x => x.產品編號 == e_祈福產品.小斗).數量 : 0;
                md.主斗 = mds.Any(x => x.產品編號 == e_祈福產品.主斗) ? mds.First(x => x.產品編號 == e_祈福產品.主斗).數量 : 0;
                md.副斗 = mds.Any(x => x.產品編號 == e_祈福產品.副斗) ? mds.First(x => x.產品編號 == e_祈福產品.副斗).數量 : 0;

                md.合計禮斗 = md.大斗 + md.中斗 + md.小斗 + md.主斗 + md.副斗;

                md.合計金額 = mds.Where(x => x.產品編號 != e_祈福產品.捐白米 && x.產品編號 != e_祈福產品.捐金牌).Sum(x => x.數量);

                md.合計燈類金額 = md.文昌燈 + md.文昌頭燈;

                r.SearchData = md;
                r.Result = true;
                return r;
                #endregion
                #endregion
            }
            catch (ExceptionRoll ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.Logic; //企業羅輯失敗
                r.ErrMessage = ex.Message; //回傳失敗代碼
                return r;
                #endregion
            }
            catch (Exception ex)
            {
                #region Error handle
                Connection.Rollback(); //取消並回復交易
                r.Result = false; //回傳本次執行失敗
                r.ErrType = BusinessErrType.System;//系統執行失敗
                r.ErrMessage = PackErrMessage(ex); //回傳失敗訊息
                return r;
                #endregion
            }
        }
    }
    #endregion

    public class ItemsManage : LogicBase
    {
        public Dictionary<String, String> i_產品資料表(int accountId)
        {
            #region Main working
            產品資料表 TObj = new 產品資料表();// 取得Table物
            DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

            dataWork.SelectFields(x => x.產品編號);
            dataWork.SelectFields(x => x.產品名稱);

            dataWork.WhereFields(x => x.隱藏, 0);
            dataWork.WhereFields(x => x.產品分類, "點燈");
            //dataWork.WhereFields(x => x.Y2013, true);
            dataWork.OrderByFields(x => x.產品編號);

            return dataWork.DataByAdapter().dicMakeKeyValue(0, 1);
            #endregion
        }
        public Dictionary<String, String> i_產品資料表禮斗(int accountId)
        {
            #region Main working
            產品資料表 TObj = new 產品資料表();// 取得Table物
            DataTableWorking<產品資料表> dataWork = new DataTableWorking<產品資料表>(Connection) { TableModule = TObj, LoginUserID = accountId };

            dataWork.SelectFields(x => x.產品編號);
            dataWork.SelectFields(x => x.產品名稱);

            dataWork.WhereFields(x => x.隱藏, 0);
            dataWork.WhereFields(x => x.產品分類, "禮斗");
            dataWork.OrderByFields(x => x.產品編號);

            return dataWork.DataByAdapter().dicMakeKeyValue(0, 1);
            #endregion
        }
    }

    public class pcar_MasterData
    {
        public int 戶長SN { get; set; }
        public String 姓名 { get; set; }
        public String 性別 { get; set; }
        public String Zip { get; set; }
        public String 地址 { get; set; }
        public String 手機 { get; set; }
        public String 電話 { get; set; }

        public int 總額 { get; set; }
        public int 白米 { get; set; }
        public int 金牌 { get; set; }

        public List<pcar_DetailData> Item { get; set; }
    }
    public class pcar_DetailData
    {
        public int 價格 { get; set; }
        public String 產品編號 { get; set; }
        /// <summary>
        /// 可選擇燈位需用到 主副斗用
        /// </summary>
        public Int64 點燈位置序號 { get; set; }

        public String 產品名稱 { get; set; }
        /// <summary>
        /// 這裡指的是會員資料表中的序號
        /// </summary>
        public String 會員編號 { get; set; }
        public String 申請人姓名 { get; set; }
        public String 申請人地址 { get; set; }
        public String 申請人性別 { get; set; }
        public String 申請人時辰 { get; set; }
        public String 申請人生日 { get; set; }
        public String 申請人年齡 { get; set; }
        public String 祈福事項 { get; set; }
        public int 白米 { get; set; }
        public int 金牌 { get; set; }
        public int 文疏 { get; set; }

        public String 點燈位置 { get; set; }
        public String 電話 { get; set; }
        public String 行動電話 { get; set; }
    }
    #endregion
    //============================
    #region For BusinessLogin Extension
    public static class BusinessLoginExtension
    {
        public static String BooleanValue(this Boolean s, BooleanSheetBase b)
        {
            if (s) { return b.TrueValue; } else { return b.FalseValue; }
        }
        public static String CodeValue(this String s, List<_Code> b)
        {
            var result = b.Where(x => x.Code == s);
            if (result.Count() > 0)
            {
                return result.FirstOrDefault().Value;
            }
            else
            {
                return "";
            }
        }
    }
    #endregion
}
