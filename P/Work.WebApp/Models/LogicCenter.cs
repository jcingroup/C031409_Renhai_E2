using System.Transactions;
using Work.WebApp.Models;

namespace ProcCore.Business
{
    public enum CodeTable
    {
        Base, Reject, Fortune_Light, Reject_Detail, TempleAccount, TempleMember
    }
    public enum SNType
    {
        Orders, Product, Receiver, Import, Stock, Export, StockAdj
    }
    public enum Orders_Type : int
    {
        general = 0, //一般訂單
        mdlight = 1, //主副斗
        sdlight = 2, //大中小斗
        fortune_order = 3, //福燈
        wishlight = 4, //祈福許願燈
        doulight = 5 //媽祖/藥師佛斗燈
    }
    public enum Orders_State : int
    {
        waiting = 0,//等待處理中
        handling = 1, //處理中
        complete = 2, //確認完成
        reject = 3, //退貨
        invalid = 4 //訂單無效
    }
}
namespace ProcCore.Business.LogicConect
{
    #region Parm Section
    public enum ParmDefine
    {
        Open, ValidDate, Apply_Max_Day, bufferNorth_Max, bufferSouth_Max, N_Max_joinnum, S_Max_joinnum, receiveMails, BccMails
    }
    #endregion

    public class LogicCenter
    {
        private RenHai2012Entities db0;
        protected TransactionScope tx;
        public int DepartmentId { get; set; }
        public string Lang { get; set; }
        public string IP { get; set; }
        public string AspUserID { get; set; }
        private void OpenDB()
        {
            db0 = new RenHai2012Entities();
        }
    }
}