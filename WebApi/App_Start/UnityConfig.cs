using Infrastructure.DataAccess;
using Infrastructure.Repositories;
using System.Web.Http;
using TrainApp.Core.ApplicationService;
using TrainApp.Core.ApplicationService.Interfaces;
using TrainApp.Core.ApplicationService.Services;
using TrainApp.Core.Entity;
using UI.Controllers;
using Unity;
using Unity.WebApi;

namespace UI
{
    public static class UnityConfig
    {
        public static void RegisterComponents()
        {
			var container = new UnityContainer();

            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            container.RegisterType<IPassengerService, PassengerService>();
            container.RegisterType<IEntityRepository<Passenger>, EntityRepository<TrainAppEntities, Passenger>>();
            container.RegisterType<IRepository<Passenger>, PassengerRepository>();

            container.RegisterType<IRouteService, RouteService>();
            container.RegisterType<IEntityRepository<Route>, EntityRepository<TrainAppEntities, Route>>();
            container.RegisterType<IRepository<Route>, RouteRepository>();

            container.RegisterType<ITripService, TripService>();
            container.RegisterType<IEntityRepository<LegModel>, EntityRepository<TrainAppEntities, LegModel>>();
            container.RegisterType<IRepository<Trip>, TripRepository>();

            container.RegisterType<ITransitService, TransitDetailService>();
            container.RegisterType<IEntityRepository<TransitDetail>, EntityRepository<TrainAppEntities, TransitDetail>>();
            container.RegisterType<IRepository<TransitDetail>, TransitDetailRepository>();

            container.RegisterType<IEntityRepository<PassengersAge>, EntityRepository<TrainAppEntities, PassengersAge>>();
            container.RegisterType<IRepository<PassengersAge>, PassengersAgesRepository>();



            GlobalConfiguration.Configuration.DependencyResolver = new UnityDependencyResolver(container);
        }
    }
}