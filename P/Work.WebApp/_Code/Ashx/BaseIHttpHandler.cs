using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Web.Configuration;

using ProcCore.DatabaseCore.TableFieldModule;
using ProcCore.DatabaseCore.DataBaseConnection;
using ProcCore.DatabaseCore.DatabaseName;

namespace DotWeb._Code
{
    public class BaseIHttpHandler : IHttpHandler
    {
        public virtual void ProcessRequest(HttpContext context)
        {

        }

        protected CommConnection getSQLConnection()
        {
            return getSQLConnection(DataBases.DB_RenHai2012);
        }
        protected CommConnection getSQLConnection(DataBases DBName)
        {
            BaseConnection BConn = new BaseConnection();
            BConn.Server = System.Configuration.ConfigurationManager.AppSettings["Server"];
            BConn.Account = System.Configuration.ConfigurationManager.AppSettings["Account"];
            BConn.Password = System.Configuration.ConfigurationManager.AppSettings["Password"];
            return BConn.GetConnection(DBName);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

    public class vmJsonResult
    {
        public Boolean result { get; set; }
    }
}