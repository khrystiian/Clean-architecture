using System;
using System.Diagnostics;
using System.Web.Http;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microsoft.Owin.Security.OAuth;
using Owin;
using UI.Controllers;

[assembly: OwinStartup(typeof(UI.Startup))]

namespace UI
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            GlobalHost.HubPipeline.AddModule(new ErrorHandlingPipelineModule());
            app.MapSignalR(new HubConfiguration() { EnableJSONP = true });
            app.UseCors(CorsOptions.AllowAll);

            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                map.RunSignalR();
            });

            ConfigureAuth(app);
        }

        /// <summary>
        /// This method handles authentication.
        /// </summary>
        /// <param name="app"></param>
        public void ConfigureAuth(IAppBuilder app)
        {

            var OAuthOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(20),
                Provider = new SimpleAuthorizationServerProvider()
            };

            app.UseOAuthBearerTokens(OAuthOptions);
            app.UseOAuthAuthorizationServer(OAuthOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

            HttpConfiguration config = new HttpConfiguration();
            WebApiConfig.Register(config);
        }

        /// <summary>
        /// This inner class handles error handling within hub pipeline module
        /// </summary>
        public class ErrorHandlingPipelineModule : HubPipelineModule
        {
            protected override void OnIncomingError(ExceptionContext exceptionContext, IHubIncomingInvokerContext invokerContext)
            {
                Debug.WriteLine("=> Exception " + exceptionContext.Error.Message);
                if (exceptionContext.Error.InnerException != null)
                {
                    Debug.WriteLine("=> Inner Exception " + exceptionContext.Error.InnerException.Message);
                }
                base.OnIncomingError(exceptionContext, invokerContext);

            }
        }
    }
}
