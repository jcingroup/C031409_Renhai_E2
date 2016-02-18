using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Script.Serialization;

using ProcCore.Business.Base;
using ProcCore.Business.Logic;

namespace DotWeb._Code.Ashx
{
    /// <summary>
    ///AjaxGetZip 的摘要描述
    /// </summary>
    public class AjaxGetZip :  BaseIHttpHandler
    {

        public override void ProcessRequest(HttpContext context)
        {
            String city= context.Request.Form["city"];
            String country = context.Request.Form["country"];
            context.Response.Write(Ajax_GetZip(city, country));
        }

        public string Ajax_GetZip(String city, String country)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            js.MaxJsonLength = 4096;

            LogicAddress LBase = new LogicAddress();
            LBase.Connection = getSQLConnection();
            String s = js.Serialize(LBase.GetZip(city, country));
            //LBase.Connection.Close();
            return s;
        }
    }
}