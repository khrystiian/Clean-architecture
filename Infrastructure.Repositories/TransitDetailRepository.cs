using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class TransitDetailRepository : EntityRepository<TrainAppEntities, TransitDetail>, IRepository<TransitDetail>
    {
        public TransitDetailRepository(TrainAppEntities entities = null) : base(entities) { }

        public void AddList(List<TransitDetail> tDetails)
        {
            for (int i = 0; i < tDetails.Count; i++)
            {
                base.Create(tDetails[i]);
            }
        }

        public void Add(TransitDetail t) => throw new NotImplementedException();
        public TransitDetail FindByEmail(string email) => throw new NotImplementedException();
        public IEnumerable<TransitDetail> GetAll() => throw new NotImplementedException();
        public bool UpdateStatus(string id, string status) => throw new NotImplementedException();
    }
}
