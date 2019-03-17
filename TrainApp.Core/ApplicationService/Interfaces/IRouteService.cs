using Infrastructure.DataAccess;
using System.Collections.Generic;

namespace TrainApp.Core.ApplicationService.Interfaces
{
    public interface IRouteService
    {
        void AddRoute(List<Route> routes);
    }
}
