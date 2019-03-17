using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class TripRepository : EntityRepository<TrainAppEntities, Trip>, IRepository<Trip>
    {
        public TripRepository(TrainAppEntities entities = null) : base(entities) { }


        public void Add(Trip t)
        {
            base.Create(t);
        }

        public void AddList(List<Trip> ts) => throw new NotImplementedException();
        public Trip FindByEmail(string email) => throw new NotImplementedException();
        public IEnumerable<Trip> GetAll() => throw new NotImplementedException();
        public void UpdateStatus(string status) => throw new NotImplementedException();
    }
}
