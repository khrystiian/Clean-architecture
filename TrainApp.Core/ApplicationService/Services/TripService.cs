using Infrastructure.DataAccess;
using System;
using System.Collections;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.ApplicationService.ModelsMapping;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.Services
{
    public class TripService: ITripService
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IRepository<Trip> _tripRepo;

        public TripService(IRepository<Trip> tripRepo)
        {
            _tripRepo = tripRepo ?? throw new ArgumentNullException(nameof(_tripRepo));
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

        public IEnumerable<LegModel> GetAllTrips()
        {
            var legModels = new List<LegModel>();
            var trips = _tripRepo.GetAll();
            foreach (var item in trips)
            {
                legModels.Add(TripMapping.TripMap(item));
            }
            return legModels;
        }

    }
}
