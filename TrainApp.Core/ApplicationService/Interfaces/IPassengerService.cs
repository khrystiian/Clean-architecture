using Infrastructure.DataAccess;
using System.Collections.Generic;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService
{
    public interface IPassengerService
    {
        void AddPassenger(PassengerModel passengerModel);
        IList<PassengerModel> ReadAllPassengers();
        int GetPassengerId(string email);
        PassengerModel GetPassengerByEmail(string email);        
        void AddList(List<PassengersAge> pAges);
    }
}
