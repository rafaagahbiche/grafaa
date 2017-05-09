
namespace PersonalSite.Domain.Concrete
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using Omu.ValueInjecter;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.DataAccess;

    public class Repo<T> : IRepo<T> where T : Entity, new()
    {
        protected readonly GrafaaEntities dbContext;

        public Repo(IDbContextFactory dbContextFactory)
        {
            dbContext = dbContextFactory.GetContext();
        }

        public void Save()
        {
            dbContext.SaveChanges();
        }

        public T Insert(T o)
        {
            var t = dbContext.Set<T>().Create();
            t.InjectFrom(o);
            dbContext.Set<T>().Add(t);
            return t;
        }

        public virtual void Delete(T o)
        {
            dbContext.Set<T>().Remove(o);
        }

        public T Get(int id)
        {
            T entity = null;
            entity = dbContext.Set<T>().Find(id);
            return entity;
        }

        public virtual IQueryable<T> Where(Expression<Func<T, bool>> predicate)
        {
            //if (typeof(IDel).IsAssignableFrom(typeof(T)))
            //    return IoC.Resolve<IDelRepo<T>>().Where(predicate, showDeleted);
            return dbContext.Set<T>().Where(predicate);
        }

        public virtual IQueryable<T> GetAll()
        {
            //if (typeof(IDel).IsAssignableFrom(typeof(T)))
            //    return IoC.Resolve<IDelRepo<T>>().GetAll();
            return dbContext.Set<T>();
        }
    }
}
