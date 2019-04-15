using Infrastructure.DataAccess;
using System.Collections;
using System.Collections.Generic;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.Interfaces
{
    public interface ITripService
    {
        void AddTrip(Trip trip);
        IEnumerable<LegModel> GetAllTrips();
    }
}
