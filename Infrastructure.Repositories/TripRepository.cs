﻿using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class TripRepository : EntityRepository<TrainAppEntities, Trip>, IRepository<Trip>
    {
        public TripRepository(TrainAppEntities entities = null) : base(entities) { }


        public void Add(Trip t) => base.Create(t);

        public bool UpdateStatus(string id, string status)
        {
            Trip trip = base.FindFirst(x => x.Guid == id);
            if (trip != null)
            {
                trip.Status = status;
            }

            return base.Edit(trip);
        }

        public IEnumerable<Trip> GetAll() => base.ReadAll();

        public void AddList(List<Trip> ts) => throw new NotImplementedException();
        public Trip FindByEmail(string email) => throw new NotImplementedException();
    }
}
