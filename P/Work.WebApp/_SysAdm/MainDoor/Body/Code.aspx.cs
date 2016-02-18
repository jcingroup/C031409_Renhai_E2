using DotWeb.CommSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Work.WebApp.Models;
using ProcCore;
namespace DotWeb._SysAdm.MainDoor.Body
{
    public partial class Code : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Timeout = 30;
            string getLoginId = Request.QueryString["Id"];

            int UserId = String.IsNullOrEmpty(getLoginId) ? 1000001 : int.Parse(getLoginId);
            Session["ID"] = UserId;

            RenHai2012Entities db0 = new RenHai2012Entities();
            var getUserData = db0.Users.Find(UserId);
            var ecryId = Server.UrlEncode(EncryptString.desEncryptBase64(getUserData.users_id.ToString()));
            var ecryName = Server.UrlEncode(EncryptString.desEncryptBase64(getUserData.users_name));
            var ecryUnitId = Server.UrlEncode(EncryptString.desEncryptBase64(getUserData.units_id.ToString()));

            int[] allwRejectUnit = CommSetup.CommWebSetup.AllowRejectUnit;

            int allowReject = allwRejectUnit.Any(x => x == getUserData.units_id) ? 1 : 0;

            Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_user_id, ecryId));
            Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_user_name, ecryName));
            Response.Cookies.Add(new HttpCookie(CommWebSetup.Cookie_unit_id, ecryUnitId));
            Response.Cookies.Add(new HttpCookie("allowReject", allowReject.ToString()));

        }
    }
}