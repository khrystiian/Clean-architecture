using Infrastructure.DataAccess;

namespace TrainApp.Core.ApplicationService.Interfaces
{
    public interface ITripService
    {
        void AddTrip(Trip trip);
    }
}
