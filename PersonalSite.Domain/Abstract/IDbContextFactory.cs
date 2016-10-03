using PersonalSite.DataAccess;

namespace PersonalSite.Domain.Abstract
{
    public interface IDbContextFactory
    {
        GrafaaEntities GetContext();
    }
}
