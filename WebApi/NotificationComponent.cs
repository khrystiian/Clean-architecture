using Infrastructure.DataAccess;
using Microsoft.AspNet.SignalR;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using TrainApp.Core.ApplicationService.ModelsMapping;
using TrainApp.Core.Entity;

namespace UI
{
    public class NotificationComponent
    {
        public LegModel RegisterNotification()
        {
            LegModel trip = new LegModel();
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(@"SELECT [End_address],[Start_address],[Arrival_time],[Departure_time],[Duration],[Distance] FROM [dbo].[Trips]", con))
                {
                    if (con.State != System.Data.ConnectionState.Open) { con.Open(); }

                    cmd.Notification = null;
                    SqlDependency sqlDep = new SqlDependency(cmd);
                    sqlDep.OnChange += sqlDep_OnChange;

                    //we must have to execute the command here
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        using (TrainAppEntities dc = new TrainAppEntities())
                        {
                            var lastInserted = dc.Trips.ToList().LastOrDefault();
                            if (lastInserted != null)
                            {
                                trip = TripMapping.TripMap(lastInserted);
                            }
                        }
                    }
                }
            }
            return trip;
        }


        public void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                LegModel trip = RegisterNotification();

                //from here we will send sql notification to the client
                var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                notificationHub.Clients.All.tripNotification(trip);
            }
        }
    }
}