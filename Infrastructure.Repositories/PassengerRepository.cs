﻿using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class PassengerRepository : EntityRepository<TrainAppEntities, Passenger>, IRepository<Passenger>
    {
        public PassengerRepository(TrainAppEntities entities = null) : base(entities) { }


        public void Add(Passenger p) => base.Create(p);

        public IEnumerable<Passenger> GetAll() => base.ReadAll();

        public Passenger FindByEmail(string email) => base.FindFirst(p => p.Email == email);

        public void AddList(List<Passenger> ts) => throw new NotImplementedException();
        public bool UpdateStatus(string id, string status) => throw new NotImplementedException();
    }
}
