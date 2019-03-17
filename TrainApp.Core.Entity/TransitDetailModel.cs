using System;

namespace TrainApp.Core.Entity
{
    public class TransitDetailModel
    {
        public int ID { get; set; }
        public string Arrival_stop { get; set; }
        public DateTime Arrival_time { get; set; }
        public string Departure_stop { get; set; }
        public DateTime Departure_time { get; set; }
    }
}
