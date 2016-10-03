using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PersonalSite.Domain.Abstract;
using PersonalSite.DataAccess;

namespace PersonalSite.Domain.Concrete
{
    public class DbContextFactory: IDbContextFactory
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
