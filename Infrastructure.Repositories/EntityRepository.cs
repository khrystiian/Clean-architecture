using Infrastructure.DataAccess;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Core.Objects.DataClasses;
using System.Data.Entity.Infrastructure;
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
        public DbSet<Trip> SetTrip{ get; set; }

        public EntityRepository(C entities = null)
        {
            if (entities != null)
                this.entities = entities;
            else
                this.entities = new C();

            Set = this.entities.Set<T>();
            SetTrip = this.entities.Set<Trip>();
        }

        public virtual int Save()
        {
            try
            {
                //Context.Database.Log = s => Console.WriteLine(s);
                return entities.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " STACKTRACE " + e.StackTrace + " innerexception " + e.InnerException);
                throw;
            }
        }


        public int SaveChanges(bool refreshOnConcurrencyException, RefreshMode refreshMode = RefreshMode.ClientWins)
        {
            try
            {
                return entities.SaveChanges();
            }
            catch (DbUpdateConcurrencyException ex)
            {
                foreach (DbEntityEntry entry in ex.Entries)
                {
                    if (refreshMode == RefreshMode.ClientWins)
                        try
                        {
                            var a = entry.GetDatabaseValues();
                            entry.OriginalValues.SetValues(a);
                        }
                        catch (Exception e)
                        {
                            Console.WriteLine(e.Message + " STACKTRACE " + e.StackTrace + " innerexception " + e.InnerException);
                            throw;
                        }
                    else
                        entry.Reload();
                }
                return entities.SaveChanges();
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
            try
            {
                Set.Attach(t);
                var entry = Context.Entry(t);
                entry.State = EntityState.Modified;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message + " STACKTRACE " + e.StackTrace + " innerexception " + e.InnerException);
                throw;
            }
            return await SaveAsync();
        }

        public virtual bool Edit(T t)
        {
            Set.Attach(t);
            var entry = Context.Entry(t).State = EntityState.Modified;
            int saved = Save();

            return (saved != -1) ? true : false;
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
