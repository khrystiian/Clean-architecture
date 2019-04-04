using Microsoft.AspNet.SignalR;
using UI;

namespace TestHub
{
    static class Program
    {
        static void Main(string[] args)
        {
            var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
            notificationHub.Clients.All.setMessage("added");
        }
    }
}
