using System;
using System.Linq;
using System.Linq.Expressions;

namespace PersonalSite.Domain.Abstract
{
    public interface IRepo<T>
    {
        T Get(int id);
        IQueryable<T> GetAll();
        IQueryable<T> Where(Expression<Func<T, bool>> predicate);
        T Insert(T o);
        void Delete(T o);
        void Save();
    }
}
