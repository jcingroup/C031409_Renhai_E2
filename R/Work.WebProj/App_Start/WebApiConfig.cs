using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace DotWeb.AppStart
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "OrdersApi",
                routeTemplate: "api/{controller}/{action}",
                defaults: new
                {
                    action = RouteParameter.Optional
                },
                constraints: new { controller = @"Orders" }
            );

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new {
                    id = RouteParameter.Optional 
                }
            );


            //config.Routes.MapHttpRoute(
            //    name: "ApiByName",
            //    routeTemplate: "api/{controller}/{action}/{name}",
            //    defaults: null,
            //    constraints: new { name = @"^[a-z]+$" }
            //);

            //config.Routes.MapHttpRoute(
            //    name: "ApiByAction",
            //    routeTemplate: "api/{controller}/{action}",
            //    defaults: new { action = "Get" }
            //);


            config.Formatters.JsonFormatter.SerializerSettings = 
                new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };

        }
    }
}
