using System.Web.Http;

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
                name: "AppLaunch",
                routeTemplate: "",
                defaults: new
                {
                    controller = "Passenger",
                    action = "Get"
                }
           );
        }
    }
}
