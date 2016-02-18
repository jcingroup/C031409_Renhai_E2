using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Work.WebApp
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "GetActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: null,
                constraints: new { controller = "GetAction" }
            );

            config.Routes.MapHttpRoute(
                name: "PutActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: null,
                constraints: new { controller = "PutAction" }
            );

            config.Routes.MapHttpRoute(
                name: "DeleteActionApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: null,
                constraints: new { controller = "DeleteAction" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
