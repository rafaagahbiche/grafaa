
namespace PersonalSite.Service.Abstract
{
    public interface IAuthProvider
    {
        bool Authenticate(string userName, string password);
    }
}
