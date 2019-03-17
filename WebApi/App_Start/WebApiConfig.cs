using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using System.Web.Http.Cors;

namespace UI
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Allow CORS for all origins. (Caution!)
           // var cors = [EnableCorsAttribute(origins: "*", headers: "*", methods: "*")];
            config.EnableCors();
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
