using Microsoft.AspNet.SignalR;
using Microsoft.AspNet.SignalR.Hubs;
using TrainApp.Core.Entity;

namespace UI
{
    [HubName("notify")]
    public class NotificationHub : Hub
    {


        //Client - Hub - Client
        public void SetMessage(string message)
        {
            Clients.Caller.setMessage(message + " is received.");
        }



        //SQL notification
        public void TripNotification(LegModel trip)
        {
            Clients.Caller.tripNotification(trip);
        }

    }
}