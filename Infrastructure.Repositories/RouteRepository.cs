using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class RouteRepository : EntityRepository<TrainAppEntities, Route>, IRepository<Route>
    {
        public RouteRepository(TrainAppEntities entities = null) : base(entities) { }


        public void AddList(List<Route> routes)
        {
            for (int i = 0; i < routes.Count; i++)
            {
                base.Create(routes[i]);
            }
        }

        public bool UpdateStatus(string id, string status) => throw new NotImplementedException();

        public void Add(Route t) { }

        public Route FindByEmail(string email) => throw new System.NotImplementedException();

        public IEnumerable<Route> GetAll() => throw new System.NotImplementedException();
    }
}
