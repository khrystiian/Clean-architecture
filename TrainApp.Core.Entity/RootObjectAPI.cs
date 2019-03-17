using System.Collections.Generic;

namespace TrainApp.Core.Entity
{
    public class RootObjectAPI{
        public List<RouteModel> Routes { get; set; }
    }
    
    public class RouteModel {
        public List<LegModel> Legs { get; set; }
    }
}
