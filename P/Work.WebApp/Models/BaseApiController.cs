using DotWeb.CommSetup;
using ProcCore;
using ProcCore.Business.Base;
using ProcCore.ReturnAjaxResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Transactions;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using Work.WebApp.Models;

namespace DotWeb.Api
{
    public class BaseApiController : ApiController
    {
        protected int defPageSize = 10;
        protected string aspUserId;
        protected int departmentId;
        protected int UserId; //指的是登錄帳號ID
        protected string UserName;
        protected int UnitId; //指的是登錄單位ID

        protected RenHai2012Entities db0;
        protected override void Initialize(System.Web.Http.Controllers.HttpControllerContext controllerContext)
        {
            base.Initialize(controllerContext);

            var getUserIdCookie = controllerContext.Request.Headers.GetCookies(CommWebSetup.Cookie_user_id).SingleOrDefault();
            UserId = getUserIdCookie == null ? 0 :
               int.Parse(EncryptString.desDecryptBase64(getUserIdCookie[CommWebSetup.Cookie_user_id].Value));

            var getUserNameCookie = controllerContext.Request.Headers.GetCookies(CommWebSetup.Cookie_user_name).SingleOrDefault();
            UserName = getUserNameCookie == null ? "" :
                EncryptString.desDecryptBase64(getUserIdCookie[CommWebSetup.Cookie_user_name].Value);

            var getUnitNameCookie = controllerContext.Request.Headers.GetCookies(CommWebSetup.Cookie_unit_id).SingleOrDefault();
            UnitId = getUnitNameCookie == null ? 0 :
                int.Parse(EncryptString.desDecryptBase64(getUserIdCookie[CommWebSetup.Cookie_unit_id].Value));
        
        }
        protected virtual string getRecMessage(string MsgId)
        {
            String r = Resources.Res.ResourceManager.GetString(MsgId);
            return String.IsNullOrEmpty(r) ? MsgId : r;
        }
        protected string getNowLnag()
        {
            return System.Globalization.CultureInfo.CurrentCulture.Name;
        }
        protected string getTaiwanCalendarDate(DateTime date) {
            System.Globalization.TaiwanCalendar TC = new System.Globalization.TaiwanCalendar();
            return string.Format("{0}/{1}/{2}", TC.GetYear(date), TC.GetMonth(date), TC.GetDayOfMonth(date));
        }
        protected static RenHai2012Entities getDB0()
        {
            return new RenHai2012Entities();
        }
        protected string ModelStateErrorPack()
        {
            List<string> errMessage = new List<string>();
            foreach (var modelState in ModelState.Values)
                foreach (var error in modelState.Errors)
                    errMessage.Add(error.ErrorMessage);

            return string.Join(":", errMessage);
        }
        protected int GetNewId()
        {
            return GetNewId(ProcCore.Business.CodeTable.Base);
        }
        protected int GetNewId(ProcCore.Business.CodeTable tab)
        {
            using (TransactionScope tx = new TransactionScope())
            {
                var db = getDB0();
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
        protected TransactionScope defAsyncScope()
        {
            return new TransactionScope();
        }
    }

    #region 泛型控制器擴充

    //[System.Web.Http.Authorize]
    public abstract class ajaxApi<M, Q> : BaseApiController
        where M : new()
        where Q : QueryBase
    {
        protected rAjaxGetData<M> r;
        protected rAjaxGetItems<M> rs;
        protected M item;
    }

    [System.Web.Http.Authorize]
    public abstract class ajaxBaseApi : BaseApiController
    {

    }
    #endregion
}