using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Script.Serialization;
using System.Web.Configuration;

using ProcCore.DatabaseCore.TableFieldModule;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.DatabaseCore.DatabaseName;
using ProcCore.Business.Logic;
using System.Data.SqlClient;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    ///AjaxGetCountry 的摘要描述
    /// </summary>
    public class AjaxGetCountry : BaseIHttpHandler
    {
        HttpContext hc;
        public override void ProcessRequest(HttpContext context)
        {
            try
            {
                String city = context.Request.QueryString["city"];
                hc = context;
                //BaseConnection.Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
                //BaseConnection.Account = System.Configuration.ConfigurationManager.AppSettings["Account"];
                //BaseConnection.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];

                //context.Response.Write(BaseConnection.Server);
                //context.Response.Write(BaseConnection.Account);
                //context.Response.Write(BaseConnection.Password);
                context.Response.Write(Ajax_GetCountry(city));
            }
            catch (Exception ex)
            {
                context.Response.Write(ex.Message + ":" + ex.StackTrace);
            }
        }

        public string Ajax_GetCountry(string city)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;
            LogicAddress LBase = new LogicAddress();
            string s=String.Empty;
            try
            {
                LBase.Connection = getSQLConnection();
                s = js.Serialize(LBase.GetCountry(city));
            }
            catch(Exception ex) {
                hc.Response.Write(ex.Message);
            }
            return s;
        }
    }
}