using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.ModelsMapping
{
    /// <summary>
    /// Translate Json from API and convert to objects which can be normalized and saved to db.
    /// </summary>
    public class MiddlewareTranslator
    {
        private readonly ITripService _tripServ;
        private readonly IRouteService _rServ;
        private readonly IPassengerService _pServ;
        private readonly ITransitService _tService;


        private string id; //primary id for each object;
        private string tripId; //primary id for trip object
        private List<string> routesID = new List<string>(); //primary ids for route object list

        public MiddlewareTranslator(ITripService tripServ, IRouteService rServ, IPassengerService pServ, ITransitService tService) //IoC
        {
            _tripServ = tripServ ?? throw new ArgumentNullException(nameof(_tripServ));
            _rServ = rServ ?? throw new ArgumentNullException(nameof(_rServ));
            _pServ = pServ ?? throw new ArgumentNullException(nameof(_pServ));
            _tService = tService ?? throw new ArgumentNullException(nameof(_tService));
            tripId = GenerateGuid();
        }

        public string ModelsMapping(RootObjectAPI root)
        {
            LegModelMapping(root);
            RouteModelMapping(root);
            RouteSeats(root);
            PassengersAgeMapping(root);
            TransitDetailMapping(root);

            return tripId;
        }


        private void LegModelMapping(RootObjectAPI root)
        {
            var r = root.Routes[0].Legs[0];
            Trip trip = new Trip
            {
                Guid = tripId, 
                Arrival_time = r.Arrival_time,
                Departure_time = r.Departure_time,
                Distance = r.Distance,
                Duration = r.Duration,
                End_address = r.End_address,
                Preference = r.RoutePreference,
                Start_address = r.Start_address,
                Price = (decimal)(r.Price),
                Status = TripStatus.Searched.ToString(),
                PassengerCID = _pServ.GetPassengerId(r.Username)
            };
            _tripServ.AddTrip(trip);
        }

        private void RouteModelMapping(RootObjectAPI root)
        {
            var r = root.Routes[0].Legs[0].Steps;
            List<Route> routes = new List<Route>();

            foreach (var item in r)
            {
                var id = GenerateGuid();
                routesID.Add(id);
                routes.Add(new Route
                {
                    ID = id,
                    TripID = tripId,
                    Distance = item.Distance,
                    Duration = item.Duration,
                    Price = (decimal)(item.Price),
                    Travel_mode = item.Travel_mode                    
                });
            }
            _rServ.AddRoute(routes);
        }

        private void RouteSeats(RootObjectAPI root)
        {
            var r = root.Routes[0].Legs[0].Steps;
            for (int i = 0; i < r.Count; i++)
            {
                var routeSeatNr = RouteSeatsNumber(r[i].Travel_mode.ToString(), root.Routes[0].Legs[0].Seats);
                if (routeSeatNr > 0)
                {
                    var routeSeat = new RouteSeat
                    {
                        ID = GenerateGuid(),
                        RouteID = routesID[i],
                        SeatsNr = routeSeatNr
                    };
                    _rServ.AddRouteSeats(routeSeat);
                }
            }
        }

        private void PassengersAgeMapping(RootObjectAPI root)
        {
            var r = root.Routes[0].Legs[0].PassengersAge;
            List<PassengersAge> passengersAges = new List<PassengersAge>();
            foreach (var item in r)
            {
                passengersAges.Add(new PassengersAge
                {
                    ID = GenerateGuid(),
                    TripId = tripId,
                    Value = item
                });
            }
            _pServ.AddList(passengersAges);
        }

        private void TransitDetailMapping(RootObjectAPI root)
        {
            var transits = root.Routes[0].Legs[0].Steps;
            List<TransitDetail> transitDetails = new List<TransitDetail>();

            for (int i = 0; i < transits.Count; i++)
            {
                var t = transits[i].Transit;
                if (t.Arrival_stop != null && t.Departure_stop != null)
                {
                    transitDetails.Add(new TransitDetail
                    {
                        ID = GenerateGuid(),
                        RouteID = routesID[i],
                        Arrival_stop = t.Arrival_stop,
                        Arrival_time = t.Arrival_time,
                        Departure_stop = t.Departure_stop,
                        Departure_time = t.Departure_time
                    });
                }
            };
            _tService.AddList(transitDetails);
        }


        /// <summary>
        /// Generate an unique id.Used for saving in db and run queries and commands against them.
        /// </summary>
        /// <returns></returns>
        private string GenerateGuid()
        {
            id = Guid.NewGuid().ToString();
            return id;
        }

        /// <summary>
        /// Assign the number of seats to route if travel mode is not walking or bicycling
        /// </summary>
        /// <param name="item"></param>
        /// <param name="tripSeats"></param>
        /// <returns></returns>
        private int RouteSeatsNumber(string travelMode, int tripSeats)
        {
            if (travelMode.Equals("WALKING"))
            {
                return 0;
            }
            else if (travelMode.Equals("BICYCLING"))
            {
                return 0;
            }
            else return tripSeats;
        }
    }
}
