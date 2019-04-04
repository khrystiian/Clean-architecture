using Infrastructure.DataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Infrastructure.Repositories
{
    public class NotificationRepository
    {
        public void RegisterNotification() //TO DO
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"SELECT [PassengerCID],[End_address],[Start_address],[Arrival_time],[Departure_time],[Price],[Distance] from [dbo].[Trips]", con);
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
                    }
                }
            }
        }

        public void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
        {
            if (e.Type == SqlNotificationType.Change)
            {
                SqlDependency sqlDep = sender as SqlDependency;
                sqlDep.OnChange -= sqlDep_OnChange;

                //from here we will send notification message to client
                // var notificationHub = GlobalHost.ConnectionManager.GetHubContext<NotificationHub>();
                // notificationHub.Clients.All.notify("added");

                //re-register notification
                RegisterNotification();
            }
        }
    }
}