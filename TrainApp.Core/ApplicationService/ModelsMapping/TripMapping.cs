using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.ModelsMapping
{
    public class TripMapping
    {
        public static LegModel TripMap(Trip t)
        {
            var routes = new List<StepModel>();
            var transit = new TransitDetailModel();

            for (int i = 0; i < t.Routes.Count(); i++)
            {
                var k = t.Routes.ToArray()[i].TransitDetails.ToArray();
                if (k.Count() > 0)
                {
                    transit = new TransitDetailModel
                    {
                        Arrival_stop = k[0].Arrival_stop,
                        Arrival_time = k[0].Arrival_time,
                        Departure_stop = k[0].Departure_stop,
                        Departure_time = k[0].Departure_time
                    };
                }
                routes.Add(new StepModel
                {
                    Distance = t.Routes.ToArray()[i].Distance,
                    Duration = t.Routes.ToArray()[i].Duration,
                    Price = (double)t.Routes.ToArray()[i].Price,
                    Travel_mode = t.Routes.ToArray()[i].Travel_mode,
                    Transit = transit
                });
            }

            return new LegModel
            {
                Arrival_time = (DateTime)t.Arrival_time,
                Departure_time = (DateTime)t.Departure_time,
                Distance = t.Distance,
                End_address = t.End_address,
                Start_address = t.Start_address,
                Duration = t.Duration,
                Price = (double)t.Price,
                RoutePreference = t.Preference,
                Steps = routes
            };
        }
    }
}