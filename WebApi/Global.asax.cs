using Infrastructure.Repositories;
using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace UI
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        string con = ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString;

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            UnityConfig.RegisterComponents();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            SqlDependency.Start(con); //start the dependency
        }

        protected void Session_Start(object sender, EventArgs e) //because of the session
        {
            NotificationComponent NC = new NotificationComponent();
            NC.RegisterNotification();
        }
        protected void Application_End()
        {
            SqlDependency.Stop(con); //stop the dependency
        }
    }
}
