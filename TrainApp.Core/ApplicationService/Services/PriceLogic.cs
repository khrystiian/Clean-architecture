using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Globalization;
using TrainApp.Core.ApplicationService.ModelsMapping;
using TrainApp.Core.Entity;

namespace TrainApp.Core.ApplicationService.Services
{
    public class PriceLogic
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly MiddlewareTranslator _mTranslator;
        private readonly IRepository<Route> _routeRepo;
        private readonly IRepository<Trip> _tripRepo;

        public PriceLogic(IRepository<Route> routeRepo, IRepository<Trip> tripRepo, MiddlewareTranslator rootTranslator)
        {
            _mTranslator = rootTranslator ?? throw new ArgumentNullException(nameof(_mTranslator));
            _routeRepo = routeRepo ?? throw new ArgumentNullException(nameof(_routeRepo));
            _tripRepo = tripRepo ?? throw new ArgumentNullException(nameof(_tripRepo));
        }


        /// <summary>
        /// Calculate the trip price based on a number of parameters: nr of seats, passengers age, route distance, transit departure time.
        /// 
        /// The algorithm behind the price is constructed for every trip route/step multiplied by the number of seats. 
        /// The distance is the key element since it determines driving time and costs, therefore the number of km of every route is split by 2 or more
        /// depending on the desired result.
        /// 
        /// The departure time plays a role in calculating the final price since rush hours may cause a more busy schedule. 
        /// </summary>
        /// <param name="tripModel"></param>
        /// <returns></returns>
        public TripResponseModel TripPrice(RootObjectAPI trip) //TO DO
        {
            double tripPrice = 0;
            string tripId = "";

            //Instead of calculating the price for every route check if there is a an existing route with a price set. 
            if (tripPrice != 0)
            {
                tripId = _mTranslator.ModelsMapping(trip);
                return new TripResponseModel
                {
                    ID = tripId,
                    TripPrice = tripPrice
                };
            }
            else
            {
                var root = trip.Routes[0].Legs[0];
                try
                {
                        for (int i = 0; i < root.Steps.Count; i++)
                        {
                            var route = root.Steps[i];
                            double distance = 0;
                            double routePrice = 0;
                            string departureHour = route.Transit.Departure_time.ToString("HH:mm");

                            if (route.Distance.Contains("km") && departureHour != "00:00")
                            {
                                DateTime departureTime = DateTime.ParseExact(departureHour, "HH:mm", CultureInfo.InvariantCulture);
                                distance = Double.Parse(route.Distance.Replace(" km", ""));

                                var priceByTime = PriceByDepartureTime(departureTime.Hour, distance); //the departure time contributes to the trip price.
                                routePrice = PriceByPassengersAge(root.PassengersAge, distance, priceByTime); //the passengers age contributes to the trip price. 

                                routePrice *= root.Seats;                    
                                route.Price = routePrice;  //set route price 
                                tripPrice += routePrice;
                            }
                        }
                        root.Price = tripPrice;
                        tripId = _mTranslator.ModelsMapping(trip);
                }
                catch (Exception e)
                {
                    log.Fatal(e);
                }
            }

            return new TripResponseModel {
                ID = tripId,
                TripPrice = tripPrice
            };
        }

        /// <summary>
        /// The passenger age is a factor that determines the trip price.
        /// </summary>
        /// <param name="ages"></param>
        /// <param name="distance">The distance of each route. Used to avoid having different prices when selecting multiple instances of the same age.(fx.2 x Adult)</param>
        /// <param name="currentPrice">The current price of the trip. Used for consistency reasons</param>
        /// <returns></returns>
        private double PriceByPassengersAge(List<string> ages, double distance, double currentPrice)
        {
            double totalPrice = 0;
            foreach (var item in ages)
            {
                if (item.Contains(" years old"))
                {
                    var age = item.Replace(" years old", "");
                    switch (age)
                    {
                        case "0-11":
                            currentPrice += 0;
                            break;
                        case "12-15":
                            totalPrice += currentPrice + (distance / 4 + (distance / 2));
                            break;
                        case "16-25":
                            totalPrice += currentPrice + (distance / 3 + (distance / 2));
                            break;
                    }
                }
                else if (item.Contains("Adult"))
                {
                    totalPrice += currentPrice + (distance / 2 + (distance / 2)); //123.45
                }
                else if (item.Contains("10 tickets"))
                {
                    totalPrice += (currentPrice + (distance / 3 + (distance / 2))) * 8;
                }
            }
            return Math.Round(totalPrice, 2);
        }

        /// <summary>
        /// Departure time is a factor in calculating the trip price due to the rush hours
        /// </summary>
        /// <param name="departureHour"></param>
        /// <param name="distance"></param>
        /// <returns></returns>
        private double PriceByDepartureTime(int departureHour, double distance)
        {
            switch (departureHour)
            {
                case int n when (n >= 05 && n <= 08):
                    return distance / 2.3;
                case int n when (n >= 09 && n <= 19):
                    return distance / 1.3;
                case int n when (n >= 20):
                    return distance / 2.5;
            }
            return 0;
        }

        //set trip status confirmed if payment accepted, or searched if canceled
        public bool ConfirmTrip(string tripId, bool confirmStatus) => _tripRepo.UpdateStatus(tripId, (confirmStatus) ? "Confirmed" : "Searched");
    }
}
