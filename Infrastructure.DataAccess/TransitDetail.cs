//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Infrastructure.DataAccess
{
    using System;
    using System.Collections.Generic;
    
    public partial class TransitDetail
    {
        public string ID { get; set; }
        public string RouteID { get; set; }
        public Nullable<System.DateTime> Arrival_time { get; set; }
        public string Arrival_stop { get; set; }
        public Nullable<System.DateTime> Departure_time { get; set; }
        public string Departure_stop { get; set; }
    
        public virtual Route Route { get; set; }
    }
}
