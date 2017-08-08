using DotWeb.CommSetup;
using Newtonsoft.Json;
using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.Business.Logic.TablesDescription;
using ProcCore.DatabaseCore;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.DatabaseCore.DatabaseName;
using ProcCore.ReturnAjaxResult;
using System;
using System.Collections.Generic;
using System.IO;
using System.Transactions;
using System.Web.Mvc;
using Work.WebApp.Models;
using System.Linq;

namespace DotWeb
{
    public abstract class SourceController : System.Web.Mvc.Controller
    {
        protected DBA DB = new DBA();
        protected String GetRecMessage(String MsgId)
        {
            return Resources.Res.ResourceManager.GetString(MsgId);
        }
        protected CommConnection conn;
        protected CommConnection getSQLConnection()
        {
            //直接採用預設的資料庫
            return getSQLConnection(DataBases.DB_RenHai2012);
        }
        protected CommConnection getSQLConnection(DataBases DBName)
        {
            //SQL Server & NySql 採用
            BaseConnection BConn = new BaseConnection();
            BConn.Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            BConn.Account = System.Configuration.ConfigurationManager.AppSettings["Account"];
            BConn.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];

            //Access Database 採用
            BConn.Path = Server.MapPath(Url.Content("~"));

            //專案採用何種資料庫在此指定
            BConn.TranactionName = DateTime.Now.Ticks.ToString();
            return BConn.GetConnection(DBName, ConnectionType.SqlClient);
        }
        protected CommConnection getSQLConnection(DataBases DBName, ConnectionType ConnType, String ServerIP, String Account, String Password)
        {
            BaseConnection BConn = new BaseConnection();
            BConn.Server = ServerIP;
            BConn.Account = Account;
            BConn.Password = Password;
            BConn.Path = Server.MapPath(Url.Content("~"));

            return BConn.GetConnection(DBName, ConnType);
        }

        protected string defJSON(object o)
        {
            return JsonConvert.SerializeObject(o, new JsonSerializerSettings() { NullValueHandling = NullValueHandling.Ignore });
        }
        protected TransactionScope defAsyncScope()
        {
            return new TransactionScope();
        }
        protected TransactionScope RepeatableReadAsyncScope()
        {
            TransactionOptions options = new TransactionOptions();
            options.IsolationLevel = System.Transactions.IsolationLevel.RepeatableRead;
            return new TransactionScope(TransactionScopeOption.Required, options);
        }
    }
    public abstract class BusLogicController : SourceController
    {
        protected int LightYear = CommWebSetup.WorkYear;
        protected int LoginUserID;
        protected Log.LogPlamInfo plamInfo = new Log.LogPlamInfo() { AllowWrite = true };

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {

            base.OnActionExecuting(filterContext);
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log.WriteToFile();
        }
        protected override void Initialize(System.Web.Routing.RequestContext requestContext)
        {
            base.Initialize(requestContext);

            if (Session["id"] != null)
            {
                LoginUserID = int.Parse(Session["id"].ToString());
                plamInfo.AccountId = LoginUserID;
            }

            plamInfo.BroswerInfo = System.Web.HttpContext.Current.Request.Browser.Browser + "." + System.Web.HttpContext.Current.Request.Browser.Version;
            plamInfo.IP = System.Web.HttpContext.Current.Request.UserHostAddress;

            Log.Enabled = true;
            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath("~\\_Code\\Log\\");
        }
        private void WorkingUnLock()
        {

            a_WorkingUnLock a_wk = new a_WorkingUnLock() { Connection = getSQLConnection() };
            a_wk.WorkingUnLock();
            a_wk.Dispose();
        }

        /// <summary>
        /// 檢查是否有錯誤，並將錯誤放置特定錯我處理區。
        /// try{}   catch(LogicException ex){}  catch(Exception ex){}
        /// </summary>
        /// <param name="h"></param>
        protected void HandleRunEnd(RunEnd h)
        {
            if (!h.Result)
            {
                if (h.ErrType == BusinessErrType.Logic)
                    throw new LogicError(h.ErrMessage);

                if (h.ErrType == BusinessErrType.System)
                    throw new Exception(h.ErrMessage);
            }
        }
        protected void rAjaxLogErrHandle(Exception ex, ResultInfo r)
        {
            r.result = false;
            r.message = GetRecMessage(ex.Message);
        }

        protected void rAjaxSysErrHandle(Exception ex, ResultInfo r)
        {
            String s = "[Message:{0}][Track:{1}]";
            r.result = false;
            r.message = String.Format(s, ex.Message, ex.StackTrace);
        }
        protected ReturnAjaxFiles HandleResultAjax(RunEnd h, String ReturnTrueMessage)
        {
            ReturnAjaxFiles r = new ReturnAjaxFiles();

            if (!h.Result)
            {
                if (h.ErrType == BusinessErrType.Logic)
                {
                    r.result = false;
                    r.message = GetRecMessage(h.ErrMessage);
                }

                if (h.ErrType == BusinessErrType.System)
                {
                    r.result = false;
                    r.message = h.ErrMessage;
                }

            }
            else
            {
                r.result = true;
                r.message = ReturnTrueMessage;
            }
            return r;
        }
    }
    public abstract class BaseController : SourceController
    {
        protected string aspUserId;
        protected int departmentId;
        protected int defPageSize = 0;
        protected string cookieCart = "cart";
        protected string sessionCart = "cart";
        //訂義取得本次執行 Controller Area Action名稱
        protected string getController = string.Empty;
        protected string getArea = string.Empty;
        protected string getAction = string.Empty;

        protected string sysUpFilePathTpl = "~/_Code/SysUpFiles/{0}.{1}/{2}/{3}/{4}";
        protected string sysDelSysId = "~/_Code/SysUpFiles/{0}.{1}/{2}";

        protected int LightYear = CommWebSetup.WorkYear;
        protected int UserId; //指的是登錄帳號ID
        protected string UserName;
        protected int UnitId; //指的是登錄帳號ID
        protected int allowReject;

        //系統認可圖片檔副檔名
        protected string[] imgExtDef = new string[] { ".jpg", ".jpeg", ".gif", ".png", ".bmp" };

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);

            //this.aspUserId = User.Identity.GetUserId();
            //this.departmentId = int.Parse(Request.Cookies[CommWebSetup.Cookie_DepartmentId].Value);

            Log.SetupBasePath = System.Web.HttpContext.Current.Server.MapPath(@"~\_Code\LogNew\");
            Log.Enabled = true;

            //defPageSize = CommWebSetup.MasterGridDefPageSize;
            this.getController = ControllerContext.RouteData.Values["controller"].ToString();
            //this.getArea = ControllerContext.RouteData.DataTokens["area"].ToString();
            //this.getAction = ControllerContext.RouteData.Values["action"].ToString();

            var getUserIdCookie = Request.Cookies[CommWebSetup.Cookie_user_id];
            if (getUserIdCookie != null)
            {
                var getCookieValue = Server.UrlDecode(getUserIdCookie.Value);
                UserId = int.Parse(EncryptString.desDecryptBase64(getCookieValue));
            }

            var getUserNameCookie = Request.Cookies[CommWebSetup.Cookie_user_name];
            if (getUserNameCookie != null)
            {
                var getCookieValue = Server.UrlDecode(getUserNameCookie.Value);
                UserName = EncryptString.desDecryptBase64(getCookieValue);
            }

            var getUnitIdCookie = Request.Cookies[CommWebSetup.Cookie_unit_id];
            if (getUnitIdCookie != null)
            {
                var getCookieValue = Server.UrlDecode(getUserIdCookie.Value);
                UnitId = int.Parse(EncryptString.desDecryptBase64(getCookieValue));
            }

            var getAllowRejectCookie = Request.Cookies["allowReject"];
            if (getAllowRejectCookie != null)
            {
                var getCookieValue = getAllowRejectCookie.Value;
                allowReject = int.Parse(getCookieValue);
            }
        }
        protected override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            base.OnActionExecuted(filterContext);
            Log.WriteToFile();
        }
        protected void ActionRun()
        {
            ViewBag.area = this.getArea;
            ViewBag.controller = this.getController;
        }
        protected List<SelectListItem> MakeNumOptions(Int32 num, Boolean FirstIsBlank)
        {

            List<SelectListItem> r = new List<SelectListItem>();
            if (FirstIsBlank)
            {
                SelectListItem sItem = new SelectListItem();
                sItem.Value = "";
                sItem.Text = "";
                r.Add(sItem);
            }

            for (int n = 1; n <= num; n++)
            {
                SelectListItem s = new SelectListItem();
                s.Value = n.ToString();
                s.Text = n.ToString();
                r.Add(s);
            }
            return r;
        }
        public FileResult DownLoadFile(Int32 Id, String FilesKind, String FileName)
        {
            String SearchPath = String.Format(sysUpFilePathTpl + "\\" + FileName, getArea, getController, Id, FilesKind, "OriginFile");
            String DownFilePath = Server.MapPath(SearchPath);

            FileInfo fi = null;
            if (System.IO.File.Exists(DownFilePath))
                fi = new FileInfo(DownFilePath);

            return File(DownFilePath, "application/" + fi.Extension.Replace(".", ""), Url.Encode(fi.Name));
        }
        public string ImgSrc(String AreaName, String ContorllerName, Int32 Id, String FilesKind, String ImageSize)
        {
            String ImgSizeString = ImageSize;
            String SearchPath = String.Format(sysUpFilePathTpl, AreaName, ContorllerName, Id, FilesKind, ImgSizeString);
            String FolderPth = Server.MapPath(SearchPath);

            if (Directory.Exists(FolderPth))
            {
                String[] SFiles = Directory.GetFiles(FolderPth);

                if (SFiles.Length > 0)
                {
                    FileInfo f = new FileInfo(SFiles[0]);
                    return Url.Content(SearchPath) + "/" + f.Name;
                }
                else
                    return null;
            }
            else
                return null;
        }
        public string ModelStateErrorPack()
        {
            List<string> errMessage = new List<string>();
            foreach (ModelState modelState in ModelState.Values)
                foreach (ModelError error in modelState.Errors)
                    errMessage.Add(error.ErrorMessage);

            return string.Join(":", errMessage);
        }
        public int GetNewId(ProcCore.Business.CodeTable tab)
        {

            using (TransactionScope tx = new TransactionScope())
            {
                var db = getDB();
                try
                {
                    string tab_name = Enum.GetName(typeof(ProcCore.Business.CodeTable), tab);
                    var items = db.i_IDX.Where(x => x.table_name == tab_name).FirstOrDefault();

                    if (items == null)
                    {
                        return 0;
                    }
                    else
                    {
                        //var item = items.FirstOrDefault();
                        items.IDX++;
                        db.SaveChanges();
                        tx.Complete();
                        return items.IDX;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    return 0;
                }
                finally
                {
                    db.Dispose();
                }
            }
        }
        protected RenHai2012Entities getDB()
        {
            return new RenHai2012Entities();
        }
    }
    public class CommAttibute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
        }
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            Log.WriteToFile();
        }
        public override void OnResultExecuting(ResultExecutingContext filterContext)
        {

        }
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
        }
    }
}