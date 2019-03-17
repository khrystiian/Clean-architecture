using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TrainApp.Core.ApplicationService.Services;

namespace Infrastructure.Repositories
{
    public class EntityRepository<C, T> : IEntityRepository<T> where T : class where C : TrainAppEntities, new()
    {
        private C entities;

        public C Context
        {
            get { return entities; }
            set { entities = value; }
        }

        public DbSet<T> Set { get; }

        public EntityRepository(C entities = null)
        {
            if (entities != null)
                this.entities = entities;
            else
                this.entities = new C();

            Set = this.entities.Set<T>();
        }

        public virtual int Save()
        {
            try
            {
                //Context.Database.Log = s => Console.WriteLine(s);
                return entities.SaveChanges();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public virtual async Task<int> SaveAsync()
        {
            try
            {
                return await entities.SaveChangesAsync();
            }
            catch (Exception)
            {
                throw;
            }
        }



        #region CRUD SYNC Operations
        public virtual int Create(T t)
        {
            //ChangeTracker
            //var changeTracker = entities.ChangeTracker.Entries();
            Set.Add(t);
            return Save();
        }

        public virtual IQueryable<T> FindBy(Expression<Func<T, bool>> predicate) => Set.Where(predicate);

        public virtual async Task<int> EditAsync(T t)
        {
            Set.Attach(t);
            var entry = Context.Entry(t);
            entry.State = EntityState.Modified;
            return await SaveAsync();
        }

        public virtual IEnumerable<T> ReadAll()
        {
           return Set.ToList();
        }

        public virtual T FindFirst(Expression<Func<T, bool>> predicate) => Set.FirstOrDefault(predicate);

        public virtual async Task<int> RemoveAsync(T t)
        {
            Set.Attach(t);
            var entry = Context.Entry(t);
            entry.State = EntityState.Deleted;
            return await SaveAsync();
        }

        #endregion

    }
}
