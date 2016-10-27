namespace PersonalSite.Service.Concrete
{
    using System.Web.Security;
    using PersonalSite.Service.Abstract;

    public class FormsAuthProvider: IAuthProvider
    {
        public bool Authenticate(string userName, string password)
        {
            bool result = FormsAuthentication.Authenticate(userName, password);
            if (result)
            {
                FormsAuthentication.SetAuthCookie(userName, false);
            }

            return result;
        }
    }
}
