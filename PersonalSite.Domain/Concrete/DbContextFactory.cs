
namespace PersonalSite.Domain.Concrete
{
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using System;
    using System.Data.Entity;

    public class DbContextFactory : IDbContextFactory
    {
        private readonly GrafaaEntities dbContext;
        public DbContextFactory()
        {
            dbContext = new GrafaaEntities();
        }

        public GrafaaEntities GetContext()
        {
            return dbContext;
        }
    }
}
