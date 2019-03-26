using Infrastructure.DataAccess;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;

namespace Infrastructure.Repositories
{
    public class NotificationComponent
    {
        public void RegisterNotification()
        {
            using (SqlConnection con = new SqlConnection(ConfigurationManager.ConnectionStrings["sqlConString"].ConnectionString))
            {
                  SqlCommand cmd = new SqlCommand(@"SELECT [ID], [Start_address], [End_address], [Arrival_time], [Departure_time], [Price] from [dbo].[Trips]", con);
                //SqlCommand cmd = new SqlCommand(@"SELECT [CID], [First_name], [Last_name], [Address], [Email], [Password] from [dbo].[Passengers]", con);
                if (con.State != System.Data.ConnectionState.Open) { con.Open(); }

               // cmd.Notification = null;
                SqlDependency sqlDep = new SqlDependency(cmd);
                sqlDep.OnChange += sqlDep_OnChange;

                //we must have to execute the command here
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    using (TrainAppEntities dc = new TrainAppEntities())
                    {
                        var allObjects = dc.Trips.ToList().LastOrDefault();
                        //var allObjects = dc.Passengers.Last();
                        var theList = new JsonResult { Data = allObjects, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                    }
                }
            }
        }

        void sqlDep_OnChange(object sender, SqlNotificationEventArgs e)
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
