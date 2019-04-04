using Infrastructure.DataAccess;
using System;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.ModelsMapping
{
    public class TripMapping
    {
        public static LegModel TripMap(Trip t)
        {
            return new LegModel
            {
                Arrival_time = (DateTime)t.Arrival_time,
                Departure_time = (DateTime)t.Departure_time,
                Distance = t.Distance,
                End_address = t.End_address,
                Start_address = t.Start_address
            };
        } 
    }
}
