using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class RouteSeatsRepository : EntityRepository<TrainAppEntities, RouteSeat>, IRepository<RouteSeat>
    {
        public RouteSeatsRepository(TrainAppEntities entities = null) : base(entities) { }

        public void Add(RouteSeat t)
        {
            base.Create(t);
        }

        public void AddList(List<RouteSeat> ts) => throw new NotImplementedException();
        public RouteSeat FindByEmail(string email) => throw new NotImplementedException();
        public IEnumerable<RouteSeat> GetAll() => throw new NotImplementedException();
        public bool UpdateStatus(string id, string status) => throw new NotImplementedException();
    }
}
