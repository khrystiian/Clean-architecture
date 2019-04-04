using System.Diagnostics;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Owin;

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
        }

        public class ErrorHandlingPipelineModule : HubPipelineModule //handle exception
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
