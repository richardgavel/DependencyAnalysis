using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;

namespace Analyzer.Web
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.Routes.MapHttpRoute(
                name: "ApiGet",
                routeTemplate: "api/{controller}/Get/{id}",
                defaults: new { action = "Get" }
            );

            config.Routes.MapHttpRoute(
                name: "ApiOther",
                routeTemplate: "api/{controller}/{action}"
            );
        }
    }
}
