using System.Collections.Generic;
using Infrastructure.DataAccess;
using TrainApp.Core.ApplicationService;

namespace Infrastructure.Repositories
{
    public class PassengersAgesRepository : EntityRepository<TrainAppEntities, PassengersAge>, IRepository<PassengersAge>
    {
        public PassengersAgesRepository(TrainAppEntities entities = null) : base(entities) { }

        public void AddList(List<PassengersAge> pAges)
        {
            for (int i = 0; i < pAges.Count; i++)
            {
                base.Create(pAges[i]);
            }
        }

        public PassengersAge FindByEmail(string email) => throw new System.NotImplementedException();
        public IEnumerable<PassengersAge> GetAll() => throw new System.NotImplementedException();
        public void Add(PassengersAge t) => throw new System.NotImplementedException();
        public void UpdateStatus(string confirm) => throw new System.NotImplementedException();
    }
}
