using Infrastructure.DataAccess;
using System;
using TrainApp.Core.ApplicationService.Interfaces;

namespace TrainApp.Core.ApplicationService.Services
{
    public class TripService: ITripService
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IRepository<Trip> _tripRepo;

        public TripService(IRepository<Trip> tripRepo)
        {
            this._tripRepo = tripRepo ?? throw new ArgumentNullException(nameof(_tripRepo));
        }

        public void AddTrip(Trip trip)
        {
            try
            {
                if (trip == null)
                {
                    log.Debug("Error in AddTrip method.");
                }
                else
                {
                    _tripRepo.Add(trip);
                }
            }
            catch (Exception e)
            {
                log.Fatal(e);
            }
        }
    }
}
