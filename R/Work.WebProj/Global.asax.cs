﻿using DotWeb.CommSetup;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace DotWeb.AppStart
{
    public class MvcApplication : System.Web.HttpApplication
    {
        string VarCookie = CommWebSetup.WebCookiesId;
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            GlobalConfiguration.Configuration.Formatters.XmlFormatter.SupportedMediaTypes.Clear();
        }
        protected void Application_BeginRequest(Object sender, EventArgs e)
        {
            HttpCookie WebLang = Request.Cookies[VarCookie + ".Lang"];

            if (WebLang == null)
            {
                //強制預設語系
                //WebLang = new HttpCookie(VarCookie + ".Lang", "zh-TW");
                if (Request.UserLanguages != null)
                    if (Request.UserLanguages.Length > 0)
                        WebLang = new HttpCookie(VarCookie + ".Lang", Request.UserLanguages[0]);
                    else
                        WebLang = new HttpCookie(VarCookie + ".Lang", System.Threading.Thread.CurrentThread.CurrentCulture.Name);
                else
                    WebLang = new HttpCookie(VarCookie + ".Lang", System.Threading.Thread.CurrentThread.CurrentCulture.Name);

                Response.Cookies.Add(WebLang);
            }

            if (WebLang != null)
            {
                System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo(WebLang.Value);
                System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo(WebLang.Value);
            }
        }
    }
}
