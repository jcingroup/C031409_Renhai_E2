using System;
using System.Collections.Generic;

using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.SQLContextHelp;
using ProcCore.DatabaseCore.TableFieldModule;

namespace ProcCore.Business.Logic.TablesDescription
{
    #region DataBase Module

    #region Area
    public class DBA
    {
        public UnitData UnitData() { return new UnitData(); }
        public _Parm _Parm() { return new _Parm(); }
        public _AddressCity _AddressCity() { return new _AddressCity(); }
        public _AddressCountry _AddressCountry() { return new _AddressCountry(); }
        public _PowerName _PowerName() { return new _PowerName(); }
        public _PowerUsers _PowerUsers() { return new _PowerUsers(); }
        public _PowerUnit _PowerUnit() { return new _PowerUnit(); }
        public _BooleanSheet _BooleanSheet() { return new _BooleanSheet(); }
        public ProgData ProgData() { return new ProgData(); }
        public UsersData UsersData() { return new UsersData(); }
        public _CodeSheet _CodeSheet() { return new _CodeSheet(); }
        public _IDX _IDX() { return new _IDX(); }
        public TF表 TF表() { return new TF表(); }
        public 會員資料表 會員資料表() { return new 會員資料表(); }
        public 年度生肖表 年度生肖表() { return new 年度生肖表(); }
        public 太歲表 太歲表() { return new 太歲表(); }
        public 星運表 星運表() { return new 星運表(); }
        public 訂單主檔 訂單主檔() { return new 訂單主檔(); }
        public 點燈位置資料表 點燈位置資料表() { return new 點燈位置資料表(); }
        public 訂單明細檔 訂單明細檔() { return new 訂單明細檔(); }
        public 性別表 性別表() { return new 性別表(); }
        public 會員戶長資料 會員戶長資料() { return new 會員戶長資料(); }
        public 西元農曆對照表 西元農曆對照表() { return new 西元農曆對照表(); }
        public 文疏梯次時間表 文疏梯次時間表() { return new 文疏梯次時間表(); }
        public 人員 人員() { return new 人員(); }
        public 產品資料表 產品資料表() { return new 產品資料表(); }
        public 人員權限 人員權限() { return new 人員權限(); }
        public 單位權限 單位權限() { return new 單位權限(); }
        public 權限 權限() { return new 權限(); }
    }
    public class UnitData : TableMap<UnitData>
    {
        public UnitData() { N = "UnitData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { N = "name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { N = "sort", T = SQLValueType.Int };
    }
    public class _Parm : TableMap<_Parm>
    {
        public _Parm() { N = "_Parm"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.ParmName.N, this.ParmName }, { this.Value.N, this.Value }}; }
        public FieldModule ParmName = new FieldModule() { N = "ParmName", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { N = "Value", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { N = "memo", T = SQLValueType.String };
    }
    public class _AddressCity : TableMap<_AddressCity>
    {
        public _AddressCity() { N = "_AddressCity"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }}; }
        public FieldModule city = new FieldModule() { N = "city", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { N = "sort", T = SQLValueType.Int };
    }
    public class _AddressCountry : TableMap<_AddressCountry>
    {
        public _AddressCountry() { N = "_AddressCountry"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.city.N, this.city }, { this.country.N, this.country }}; }
        public FieldModule city = new FieldModule() { N = "city", T = SQLValueType.String };
        public FieldModule country = new FieldModule() { N = "country", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { N = "zip", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { N = "sort", T = SQLValueType.Int };
        public FieldModule code = new FieldModule() { N = "code", T = SQLValueType.String };
    }
    public class _PowerName : TableMap<_PowerName>
    {
        public _PowerName() { N = "_PowerName"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { N = "id", T = SQLValueType.Int };
        public FieldModule name = new FieldModule() { N = "name", T = SQLValueType.String };
        public FieldModule memo = new FieldModule() { N = "memo", T = SQLValueType.String };
    }
    public class _PowerUsers : TableMap<_PowerUsers>
    {
        public _PowerUsers() { N = "_PowerUsers"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule ProgID = new FieldModule() { N = "ProgID", T = SQLValueType.Int };
        public FieldModule UserID = new FieldModule() { N = "UserID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { N = "PowerID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { N = "UnitID", T = SQLValueType.Int };
    }
    public class _PowerUnit : TableMap<_PowerUnit>
    {
        public _PowerUnit() { N = "_PowerUnit"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule ProgID = new FieldModule() { N = "ProgID", T = SQLValueType.Int };
        public FieldModule UnitID = new FieldModule() { N = "UnitID", T = SQLValueType.Int };
        public FieldModule PowerID = new FieldModule() { N = "PowerID", T = SQLValueType.Int };
        public FieldModule AccessUnit = new FieldModule() { N = "AccessUnit", T = SQLValueType.Int };
    }
    public class _BooleanSheet : TableMap<_BooleanSheet>
    {
        public _BooleanSheet() { N = "_BooleanSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.Boolean.N, this.Boolean }}; }
        public FieldModule Boolean = new FieldModule() { N = "Boolean", T = SQLValueType.Boolean };
        public FieldModule sex = new FieldModule() { N = "sex", T = SQLValueType.String };
        public FieldModule yn = new FieldModule() { N = "yn", T = SQLValueType.String };
        public FieldModule ynv = new FieldModule() { N = "ynv", T = SQLValueType.String };
        public FieldModule ynvx = new FieldModule() { N = "ynvx", T = SQLValueType.String };
    }
    public class ProgData : TableMap<ProgData>
    {
        public ProgData() { N = "ProgData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { N = "id", T = SQLValueType.Int };
        public FieldModule area = new FieldModule() { N = "area", T = SQLValueType.String };
        public FieldModule controller = new FieldModule() { N = "controller", T = SQLValueType.String };
        public FieldModule action = new FieldModule() { N = "action", T = SQLValueType.String };
        public FieldModule path = new FieldModule() { N = "path", T = SQLValueType.String };
        public FieldModule page = new FieldModule() { N = "page", T = SQLValueType.String };
        public FieldModule prog_name = new FieldModule() { N = "prog_name", T = SQLValueType.String };
        public FieldModule sort = new FieldModule() { N = "sort", T = SQLValueType.String };
        public FieldModule isfolder = new FieldModule() { N = "isfolder", T = SQLValueType.Boolean };
        public FieldModule ishidden = new FieldModule() { N = "ishidden", T = SQLValueType.Boolean };
        public FieldModule isRoute = new FieldModule() { N = "isRoute", T = SQLValueType.Boolean };
        public FieldModule power_serial = new FieldModule() { N = "power_serial", T = SQLValueType.Int };
    }
    public class UsersData : TableMap<UsersData>
    {
        public UsersData() { N = "UsersData"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.id.N, this.id }}; }
        public FieldModule id = new FieldModule() { N = "id", T = SQLValueType.Int };
        public FieldModule account = new FieldModule() { N = "account", T = SQLValueType.String };
        public FieldModule password = new FieldModule() { N = "password", T = SQLValueType.String };
        public FieldModule name = new FieldModule() { N = "name", T = SQLValueType.String };
        public FieldModule unit = new FieldModule() { N = "unit", T = SQLValueType.Int };
        public FieldModule state = new FieldModule() { N = "state", T = SQLValueType.String };
        public FieldModule isadmin = new FieldModule() { N = "isadmin", T = SQLValueType.Boolean };
        public FieldModule type = new FieldModule() { N = "type", T = SQLValueType.String };
        public FieldModule email = new FieldModule() { N = "email", T = SQLValueType.String };
        public FieldModule zip = new FieldModule() { N = "zip", T = SQLValueType.String };
        public FieldModule city = new FieldModule() { N = "city", T = SQLValueType.String };
        public FieldModule country = new FieldModule() { N = "country", T = SQLValueType.String };
        public FieldModule address = new FieldModule() { N = "address", T = SQLValueType.String };
    }
    public class _CodeSheet : TableMap<_CodeSheet>
    {
        public _CodeSheet() { N = "_CodeSheet"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.CodeGroup.N, this.CodeGroup }, { this.Code.N, this.Code }}; }
        public FieldModule CodeGroup = new FieldModule() { N = "CodeGroup", T = SQLValueType.String };
        public FieldModule Code = new FieldModule() { N = "Code", T = SQLValueType.String };
        public FieldModule Value = new FieldModule() { N = "Value", T = SQLValueType.String };
        public FieldModule Sort = new FieldModule() { N = "Sort", T = SQLValueType.Int };
        public FieldModule IsUse = new FieldModule() { N = "IsUse", T = SQLValueType.Boolean };
    }
    public class _IDX : TableMap<_IDX>
    {
        public _IDX() { N = "_IDX"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule IDX = new FieldModule() { N = "IDX", T = SQLValueType.Int };
    }
    #endregion

    #region Area

    public class 訂單主檔 : TableMap<訂單主檔>
    {
        public 訂單主檔() { N = "訂單主檔"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.訂單編號.N, this.訂單編號 }}; }
        public FieldModule 訂單序號 = new FieldModule() { N = "訂單序號", T = SQLValueType.Int };
        public FieldModule 訂單編號 = new FieldModule() { N = "訂單編號", T = SQLValueType.String };
        public FieldModule 會員編號 = new FieldModule() { N = "會員編號", T = SQLValueType.Int };
        public FieldModule 申請人姓名 = new FieldModule() { N = "申請人姓名", T = SQLValueType.String };
        public FieldModule 申請人電話 = new FieldModule() { N = "申請人電話", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
        public FieldModule 申請人地址 = new FieldModule() { N = "申請人地址", T = SQLValueType.String };
        public FieldModule 申請人手機 = new FieldModule() { N = "申請人手機", T = SQLValueType.String };
        public FieldModule 申請人性別 = new FieldModule() { N = "申請人性別", T = SQLValueType.String };
        public FieldModule 申請人生日 = new FieldModule() { N = "申請人生日", T = SQLValueType.String };
        public FieldModule 申請人EMAIL = new FieldModule() { N = "申請人EMAIL", T = SQLValueType.String };
        public FieldModule 總額 = new FieldModule() { N = "總額", T = SQLValueType.Int };
        public FieldModule 付款方式 = new FieldModule() { N = "付款方式", T = SQLValueType.String };
        public FieldModule 訂單時間 = new FieldModule() { N = "訂單時間", T = SQLValueType.DateTime };
        public FieldModule 付款時間 = new FieldModule() { N = "付款時間", T = SQLValueType.DateTime };
        public FieldModule 訂單狀態 = new FieldModule() { N = "訂單狀態", T = SQLValueType.Int };
        public FieldModule 查詢序號 = new FieldModule() { N = "查詢序號", T = SQLValueType.String };
        public FieldModule 付款方式名稱 = new FieldModule() { N = "付款方式名稱", T = SQLValueType.String };
        public FieldModule 訂單狀態名稱 = new FieldModule() { N = "訂單狀態名稱", T = SQLValueType.String };
        public FieldModule 銀行帳號 = new FieldModule() { N = "銀行帳號", T = SQLValueType.String };
        public FieldModule 新增時間 = new FieldModule() { N = "新增時間", T = SQLValueType.DateTime };
        public FieldModule 新增人員 = new FieldModule() { N = "新增人員", T = SQLValueType.Int };
        public FieldModule 戶長SN = new FieldModule() { N = "戶長SN", T = SQLValueType.Int };
        public FieldModule orders_type = new FieldModule() { N = "orders_type", T = SQLValueType.Int };
        public FieldModule y = new FieldModule() { N = "y", T = SQLValueType.Int };
    }
    public class 訂單明細檔 : TableMap<訂單明細檔>
    {
        public 訂單明細檔() { N = "訂單明細檔"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.訂單編號.N, this.訂單編號 }, { this.產品編號.N, this.產品編號 }, { this.會員編號.N, this.會員編號 }}; }
        public FieldModule 訂單序號 = new FieldModule() { N = "訂單序號", T = SQLValueType.Int };
        public FieldModule 訂單編號 = new FieldModule() { N = "訂單編號", T = SQLValueType.String };
        public FieldModule 產品編號 = new FieldModule() { N = "產品編號", T = SQLValueType.String };
        public FieldModule 會員編號 = new FieldModule() { N = "會員編號", T = SQLValueType.Int };
        public FieldModule 年度 = new FieldModule() { N = "年度", T = SQLValueType.Int };
        public FieldModule 產品名稱 = new FieldModule() { N = "產品名稱", T = SQLValueType.String };
        public FieldModule 價格 = new FieldModule() { N = "價格", T = SQLValueType.Int };
        public FieldModule 數量 = new FieldModule() { N = "數量", T = SQLValueType.Int };
        public FieldModule 申請人姓名 = new FieldModule() { N = "申請人姓名", T = SQLValueType.String };
        public FieldModule 申請人地址 = new FieldModule() { N = "申請人地址", T = SQLValueType.String };
        public FieldModule 申請人性別 = new FieldModule() { N = "申請人性別", T = SQLValueType.String };
        public FieldModule 申請人生日 = new FieldModule() { N = "申請人生日", T = SQLValueType.String };
        public FieldModule 申請人年齡 = new FieldModule() { N = "申請人年齡", T = SQLValueType.String };
        public FieldModule 申請人時辰 = new FieldModule() { N = "申請人時辰", T = SQLValueType.String };
        public FieldModule 申請人生肖 = new FieldModule() { N = "申請人生肖", T = SQLValueType.String };
        public FieldModule 購買時間 = new FieldModule() { N = "購買時間", T = SQLValueType.DateTime };
        public FieldModule 付款時間 = new FieldModule() { N = "付款時間", T = SQLValueType.DateTime };
        public FieldModule 祈福事項 = new FieldModule() { N = "祈福事項", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
        public FieldModule 點燈位置 = new FieldModule() { N = "點燈位置", T = SQLValueType.String };
        public FieldModule 經手人 = new FieldModule() { N = "經手人", T = SQLValueType.String };
        public FieldModule 新增時間 = new FieldModule() { N = "新增時間", T = SQLValueType.DateTime };
        public FieldModule 新增人員 = new FieldModule() { N = "新增人員", T = SQLValueType.Int };
        public FieldModule 白米 = new FieldModule() { N = "白米", T = SQLValueType.Int };
        public FieldModule 金牌 = new FieldModule() { N = "金牌", T = SQLValueType.Int };
        public FieldModule 異動標記 = new FieldModule() { N = "異動標記", T = SQLValueType.Boolean };
        public FieldModule 文疏梯次 = new FieldModule() { N = "文疏梯次", T = SQLValueType.Int };
        public FieldModule detail_sort = new FieldModule() { N = "detail_sort", T = SQLValueType.Int };
        public FieldModule is_reject = new FieldModule() { N = "is_reject", T = SQLValueType.Boolean };
    }
    public class 訂單明細暫存檔 : TableMap<訂單明細暫存檔>
    {
        public 訂單明細暫存檔() { N = "訂單明細暫存檔"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 訂單序號 = new FieldModule() { N = "訂單序號", T = SQLValueType.Int };
        public FieldModule 訂單編號 = new FieldModule() { N = "訂單編號", T = SQLValueType.String };
        public FieldModule 產品編號 = new FieldModule() { N = "產品編號", T = SQLValueType.String };
        public FieldModule 產品名稱 = new FieldModule() { N = "產品名稱", T = SQLValueType.String };
        public FieldModule 價格 = new FieldModule() { N = "價格", T = SQLValueType.Int };
        public FieldModule 數量 = new FieldModule() { N = "數量", T = SQLValueType.Int };
        public FieldModule 申請人姓名 = new FieldModule() { N = "申請人姓名", T = SQLValueType.String };
        public FieldModule 申請人地址 = new FieldModule() { N = "申請人地址", T = SQLValueType.String };
        public FieldModule 申請人性別 = new FieldModule() { N = "申請人性別", T = SQLValueType.String };
        public FieldModule 申請人生日 = new FieldModule() { N = "申請人生日", T = SQLValueType.String };
        public FieldModule 申請人年齡 = new FieldModule() { N = "申請人年齡", T = SQLValueType.String };
        public FieldModule 申請人時辰 = new FieldModule() { N = "申請人時辰", T = SQLValueType.String };
        public FieldModule 購買時間 = new FieldModule() { N = "購買時間", T = SQLValueType.DateTime };
        public FieldModule 祈福事項 = new FieldModule() { N = "祈福事項", T = SQLValueType.String };
        public FieldModule 會員編號 = new FieldModule() { N = "會員編號", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
    }
    public class TF表 : TableMap<TF表>
    {
        public TF表() { N = "TF表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule Value = new FieldModule() { N = "Value", T = SQLValueType.Boolean };
        public FieldModule 是否 = new FieldModule() { N = "是否", T = SQLValueType.String };
        public FieldModule 是否二 = new FieldModule() { N = "是否二", T = SQLValueType.String };
    }
    public class 地址鄉鎮 : TableMap<地址鄉鎮>
    {
        public 地址鄉鎮() { N = "地址鄉鎮"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 縣市 = new FieldModule() { N = "縣市", T = SQLValueType.String };
        public FieldModule 鄉鎮 = new FieldModule() { N = "鄉鎮", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
        public FieldModule 排序 = new FieldModule() { N = "排序", T = SQLValueType.Int };
        public FieldModule 鄉鎮市區代碼 = new FieldModule() { N = "鄉鎮市區代碼", T = SQLValueType.String };
    }
    public class 地址縣市 : TableMap<地址縣市>
    {
        public 地址縣市() { N = "地址縣市"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 縣市 = new FieldModule() { N = "縣市", T = SQLValueType.String };
        public FieldModule 排序 = new FieldModule() { N = "排序", T = SQLValueType.Int };
    }
    public class 會員資料表 : TableMap<會員資料表>
    {
        public 會員資料表() { N = "會員資料表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.序號.N, this.序號 } }; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.Int };
        public FieldModule 會員編號 = new FieldModule() { N = "會員編號", T = SQLValueType.String };
        public FieldModule 戶長SN = new FieldModule() { N = "戶長SN", T = SQLValueType.Int };
        public FieldModule Is戶長 = new FieldModule() { N = "Is戶長", T = SQLValueType.Boolean };
        public FieldModule 姓名 = new FieldModule() { N = "姓名", T = SQLValueType.String };
        public FieldModule 電話區碼 = new FieldModule() { N = "電話區碼", T = SQLValueType.String };
        public FieldModule 電話尾碼 = new FieldModule() { N = "電話尾碼", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
        public FieldModule 地址 = new FieldModule() { N = "地址", T = SQLValueType.String };
        public FieldModule 手機 = new FieldModule() { N = "手機", T = SQLValueType.String };
        public FieldModule 性別 = new FieldModule() { N = "性別", T = SQLValueType.String };
        public FieldModule 生日 = new FieldModule() { N = "生日", T = SQLValueType.String };
        public FieldModule 時辰 = new FieldModule() { N = "時辰", T = SQLValueType.String };
        public FieldModule EMAIL = new FieldModule() { N = "EMAIL", T = SQLValueType.String };
        public FieldModule 祈福事項 = new FieldModule() { N = "祈福事項", T = SQLValueType.String };
        public FieldModule 生肖 = new FieldModule() { N = "生肖", T = SQLValueType.String };
        public FieldModule 縣市 = new FieldModule() { N = "縣市", T = SQLValueType.String };
        public FieldModule 鄉鎮 = new FieldModule() { N = "鄉鎮", T = SQLValueType.String };
        public FieldModule 建立日期 = new FieldModule() { N = "建立日期", T = SQLValueType.DateTime };
    }
    public class 年度生肖表 : TableMap<年度生肖表>
    {
        public 年度生肖表() { N = "年度生肖表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.民國年.N, this.民國年 }}; }
        public FieldModule 西元年 = new FieldModule() { N = "西元年", T = SQLValueType.Int };
        public FieldModule 民國年 = new FieldModule() { N = "民國年", T = SQLValueType.Int };
        public FieldModule 生肖 = new FieldModule() { N = "生肖", T = SQLValueType.String };
        public FieldModule 歲次年 = new FieldModule() { N = "歲次年", T = SQLValueType.String };
        public FieldModule 星君 = new FieldModule() { N = "星君", T = SQLValueType.String };
    }
    public class 太歲表 : TableMap<太歲表>
    {
        public 太歲表() { N = "太歲表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.Int };
        public FieldModule 星君 = new FieldModule() { N = "星君", T = SQLValueType.String };
    }
    public class 星運表 : TableMap<星運表>
    {
        public 星運表() { N = "星運表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.Int };
        public FieldModule 生肖 = new FieldModule() { N = "生肖", T = SQLValueType.String };
        public FieldModule 星運 = new FieldModule() { N = "星運", T = SQLValueType.String };
    }
    public class 性別表 : TableMap<性別表>
    {
        public 性別表() { N = "性別表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.String };
        public FieldModule 性別 = new FieldModule() { N = "性別", T = SQLValueType.String };
    }
    public class 會員戶長資料 : TableMap<會員戶長資料>
    {
        public 會員戶長資料() { N = "會員戶長資料"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.戶長SN.N, this.戶長SN }}; }
        public FieldModule 戶長SN = new FieldModule() { N = "戶長SN", T = SQLValueType.Int };
        public FieldModule 電話 = new FieldModule() { N = "電話", T = SQLValueType.String };
        public FieldModule 郵遞區號 = new FieldModule() { N = "郵遞區號", T = SQLValueType.String };
        public FieldModule 地址 = new FieldModule() { N = "地址", T = SQLValueType.String };
        public FieldModule 姓名 = new FieldModule() { N = "姓名", T = SQLValueType.String };
        public FieldModule 建立時間 = new FieldModule() { N = "建立時間", T = SQLValueType.DateTime };
    }

    public class 西元農曆對照表 : TableMap<西元農曆對照表>
    {
        public 西元農曆對照表() { N = "西元農曆對照表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule S年 = new FieldModule() { N = "S年", T = SQLValueType.Int };
        public FieldModule S月 = new FieldModule() { N = "S月", T = SQLValueType.Int };
        public FieldModule S日 = new FieldModule() { N = "S日", T = SQLValueType.Int };
        public FieldModule 時間 = new FieldModule() { N = "時間", T = SQLValueType.DateTime };
        public FieldModule L年 = new FieldModule() { N = "L年", T = SQLValueType.Int };
        public FieldModule L月 = new FieldModule() { N = "L月", T = SQLValueType.Int };
        public FieldModule L日 = new FieldModule() { N = "L日", T = SQLValueType.Int };
        public FieldModule Is年 = new FieldModule() { N = "Is年", T = SQLValueType.Boolean };
        public FieldModule Is月 = new FieldModule() { N = "Is月", T = SQLValueType.Boolean };
        public FieldModule Is日 = new FieldModule() { N = "Is日", T = SQLValueType.Boolean };
    }
    public class 文疏梯次時間表 : TableMap<文疏梯次時間表>
    {
        public 文疏梯次時間表() { N = "文疏梯次時間表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.梯次.N, this.梯次 }}; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.Int };
        public FieldModule 梯次 = new FieldModule() { N = "梯次", T = SQLValueType.String };
        public FieldModule 時間 = new FieldModule() { N = "時間", T = SQLValueType.DateTime };
        public FieldModule 農曆年 = new FieldModule() { N = "農曆年", T = SQLValueType.String };
        public FieldModule 農曆月 = new FieldModule() { N = "農曆月", T = SQLValueType.String };
        public FieldModule 農曆日 = new FieldModule() { N = "農曆日", T = SQLValueType.String };
        public FieldModule 人數 = new FieldModule() { N = "人數", T = SQLValueType.Int };
    }
    public class 點燈位置資料表 : TableMap<點燈位置資料表>
    {
        public 點燈位置資料表() { N = "點燈位置資料表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.位置名稱.N, this.位置名稱 }, { this.年度.N, this.年度 }}; }
        public FieldModule 序號 = new FieldModule() { N = "序號", T = SQLValueType.Int };
        public FieldModule 位置名稱 = new FieldModule() { N = "位置名稱", T = SQLValueType.String };
        public FieldModule 年度 = new FieldModule() { N = "年度", T = SQLValueType.Int };
        public FieldModule 空位 = new FieldModule() { N = "空位", T = SQLValueType.String };
        public FieldModule 產品編號 = new FieldModule() { N = "產品編號", T = SQLValueType.String };
        public FieldModule 價格 = new FieldModule() { N = "價格", T = SQLValueType.Int };
        public FieldModule _LockUserID = new FieldModule() { N = "_LockUserID", T = SQLValueType.Int };
        public FieldModule _LockDateTime = new FieldModule() { N = "_LockDateTime", T = SQLValueType.DateTime };
        public FieldModule _LockState = new FieldModule() { N = "_LockState", T = SQLValueType.Boolean };
    }
    public class 人員 : TableMap<人員>
    {
        public 人員() { N = "人員"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 人員代碼 = new FieldModule() { N = "人員代碼", T = SQLValueType.Int };
        public FieldModule 帳號 = new FieldModule() { N = "帳號", T = SQLValueType.String };
        public FieldModule 密碼 = new FieldModule() { N = "密碼", T = SQLValueType.String };
        public FieldModule 姓名 = new FieldModule() { N = "姓名", T = SQLValueType.String };
        public FieldModule 住址 = new FieldModule() { N = "住址", T = SQLValueType.String };
        public FieldModule 家裡電話 = new FieldModule() { N = "家裡電話", T = SQLValueType.String };
        public FieldModule 手機 = new FieldModule() { N = "手機", T = SQLValueType.String };
        public FieldModule 生日 = new FieldModule() { N = "生日", T = SQLValueType.DateTime };
        public FieldModule 單位代碼 = new FieldModule() { N = "單位代碼", T = SQLValueType.Int };
        public FieldModule 停權 = new FieldModule() { N = "停權", T = SQLValueType.Int };
        public FieldModule 職稱代碼 = new FieldModule() { N = "職稱代碼", T = SQLValueType.Int };
        public FieldModule 抬頭稱呼 = new FieldModule() { N = "抬頭稱呼", T = SQLValueType.String };
        public FieldModule 電子信箱 = new FieldModule() { N = "電子信箱", T = SQLValueType.String };
        public FieldModule isadmin = new FieldModule() { N = "isadmin", T = SQLValueType.Int };
        public FieldModule 使用IP = new FieldModule() { N = "使用IP", T = SQLValueType.String };
        public FieldModule MD5 = new FieldModule() { N = "MD5", T = SQLValueType.String };
    }
    public class 人員權限 : TableMap<人員權限>
    {
        public 人員權限() { N = "人員權限"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 程式代碼 = new FieldModule() { N = "程式代碼", T = SQLValueType.Int };
        public FieldModule 人員代碼 = new FieldModule() { N = "人員代碼", T = SQLValueType.Int };
        public FieldModule 權限代碼 = new FieldModule() { N = "權限代碼", T = SQLValueType.Int };
        public FieldModule 單位代碼 = new FieldModule() { N = "單位代碼", T = SQLValueType.Int };
    }
    public class 單位權限 : TableMap<單位權限>
    {
        public 單位權限() { N = "單位權限"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 程式代碼 = new FieldModule() { N = "程式代碼", T = SQLValueType.Int };
        public FieldModule 單位代碼 = new FieldModule() { N = "單位代碼", T = SQLValueType.Int };
        public FieldModule 權限代碼 = new FieldModule() { N = "權限代碼", T = SQLValueType.Int };
        public FieldModule 存取單位 = new FieldModule() { N = "存取單位", T = SQLValueType.Int };
    }
    public class 權限 : TableMap<權限>
    {
        public 權限() { N = "權限"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 權限代碼 = new FieldModule() { N = "權限代碼", T = SQLValueType.Int };
        public FieldModule 權限名稱 = new FieldModule() { N = "權限名稱", T = SQLValueType.String };
        public FieldModule 說明 = new FieldModule() { N = "說明", T = SQLValueType.String };
    }
    public class 程式 : TableMap<程式>
    {
        public 程式() { N = "程式"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { }; }
        public FieldModule 程式代碼 = new FieldModule() { N = "程式代碼", T = SQLValueType.Int };
        public FieldModule 程式名稱 = new FieldModule() { N = "程式名稱", T = SQLValueType.String };
        public FieldModule 程式別名 = new FieldModule() { N = "程式別名", T = SQLValueType.String };
        public FieldModule 程式路徑 = new FieldModule() { N = "程式路徑", T = SQLValueType.String };
        public FieldModule 圖片路徑 = new FieldModule() { N = "圖片路徑", T = SQLValueType.String };
        public FieldModule 起始程式 = new FieldModule() { N = "起始程式", T = SQLValueType.String };
        public FieldModule 排序 = new FieldModule() { N = "排序", T = SQLValueType.String };
        public FieldModule 分類 = new FieldModule() { N = "分類", T = SQLValueType.Boolean };
        public FieldModule 權碼 = new FieldModule() { N = "權碼", T = SQLValueType.Int };
        public FieldModule 隱藏 = new FieldModule() { N = "隱藏", T = SQLValueType.Boolean };
    }

    public class 產品資料表 : TableMap<產品資料表>
    {
        public 產品資料表() { N = "產品資料表"; GetTabObj = this; KeyFieldModules = new Dictionary<String, FieldModule>() { { this.產品編號.N, this.產品編號 }}; }
        public FieldModule 產品編號 = new FieldModule() { N = "產品編號", T = SQLValueType.String };
        public FieldModule 產品名稱 = new FieldModule() { N = "產品名稱", T = SQLValueType.String };
        public FieldModule 產品分類 = new FieldModule() { N = "產品分類", T = SQLValueType.String };
        public FieldModule 選擇 = new FieldModule() { N = "選擇", T = SQLValueType.Boolean };
        public FieldModule 燈位限制 = new FieldModule() { N = "燈位限制", T = SQLValueType.Boolean };
        public FieldModule 價格 = new FieldModule() { N = "價格", T = SQLValueType.Int };
        public FieldModule 隱藏 = new FieldModule() { N = "隱藏", T = SQLValueType.Int };
        public FieldModule 排序 = new FieldModule() { N = "排序", T = SQLValueType.Int };
    }
    #endregion

    #endregion
}
