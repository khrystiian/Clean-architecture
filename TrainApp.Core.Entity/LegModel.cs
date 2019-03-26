using System;
using System.Collections.Generic;

namespace TrainApp.Core.Entity
{
    public class LegModel
    {
        public int ID { get; set; } //created by using the datetime of data insertion in the db
        public DateTime Arrival_time { get; set; }
        public DateTime Departure_time { get; set; }
        public string Distance { get; set; }
        public string Duration { get; set; }
        public string End_address { get; set; }
        public string Start_address { get; set; }
        public int Seats { get; set; }
        public string Username { get; set; }
        public double Price { get; set; }
        public string RoutePreference { get; set; }
        public List<StepModel> Steps { get; set; }
        public List<string> PassengersAge { get; set; }

    }
}
