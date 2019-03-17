using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace TrainApp.Core.ApplicationService.Services
{
    public interface IEntityRepository<T> where T : class
    {
        int Save();
        Task<int> SaveAsync();
        int Create(T t);
        IQueryable<T> FindBy(Expression<Func<T, bool>> predicate);
        Task<int> EditAsync(T t);
        IEnumerable<T> ReadAll();
        T FindFirst(Expression<Func<T, bool>> predicate);
        Task<int> RemoveAsync(T t);
    }
}
