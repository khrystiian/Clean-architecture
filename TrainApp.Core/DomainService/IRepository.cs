using System.Collections.Generic;

namespace TrainApp.Core.ApplicationService
{
    public interface IRepository<T> where T : class
    {
        void Add(T t);
        void AddList(List<T> ts);
        IEnumerable<T> GetAll();
        T FindByEmail(string email);
        bool UpdateStatus(string id, string status);
    }
}
