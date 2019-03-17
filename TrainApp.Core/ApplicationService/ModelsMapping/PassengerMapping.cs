using Infrastructure.DataAccess;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.Mapping
{
    /// <summary>
    /// Responsible with converting domain objects to data transfer object
    /// Recommended since, no domain objects should be exposed to client.
    /// </summary>
    public class PassengerMapping
    {
        public static PassengerModel MapPassenger(Passenger p) => new PassengerModel
        {
            ID = p.CID,
            FirstName = p.First_name,
            LastName = p.Last_name,
            Address = p.Address,
            Email = p.Email,
            Password = p.Password
        };

        public static Passenger MapPassengerModel(PassengerModel p) => new Passenger
        {
            CID = p.ID,
            First_name = p.FirstName,
            Last_name = p.LastName,
            Address = p.Address,
            Email = p.Email,
            Password = p.Password
        };
    }
}
