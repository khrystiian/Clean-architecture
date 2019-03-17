using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService.Interfaces;

namespace TrainApp.Core.ApplicationService.Services
{
    public class RouteService : IRouteService
    {
        private static readonly log4net.ILog log = LogHelper.GetLogger();
        private readonly IRepository<Route> _routeRepo;


        public RouteService(IRepository<Route> routeRepo)
        {
            _routeRepo = routeRepo ?? throw new ArgumentNullException(nameof(_routeRepo));
        }

        public void AddRoute(List<Route> routes)
        {
            try
            {
                if (routes.Count == 0)
                {
                    log.Debug("No passengers were added.");
                }
                else
                {
                   _routeRepo.AddList(routes);
                }
            }
            catch (Exception e)
            {
                log.Fatal(e); //ERROR !!!!!!!!
            }
        }

    
    }
}
