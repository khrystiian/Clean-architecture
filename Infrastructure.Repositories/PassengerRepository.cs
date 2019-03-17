using Infrastructure.DataAccess;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Web.Mvc;
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
        public void UpdateStatus(string status) => throw new NotImplementedException();
    }
}
