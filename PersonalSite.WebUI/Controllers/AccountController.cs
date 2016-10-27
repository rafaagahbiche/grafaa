namespace PersonalSite.WebUI.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;
    using PersonalSite.Service.Abstract;
using PersonalSite.Service.ViewModel;

    public class AccountController : Controller
    {
        private readonly IAuthProvider authProvider;
        public AccountController(IAuthProvider authProvider)
        {
            this.authProvider = authProvider;
        }

        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(LoginViewModel loginViewModel, string returnUrl)
        {
            if (ModelState.IsValid)
            {
                if (authProvider.Authenticate(loginViewModel.UserName, loginViewModel.Password))
                {
                    return Redirect(returnUrl ?? Url.Action("Index", ""));
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Incorrect username or password");
                    return View();
                }
            }
            else
            {
                return View();
            }
        }
    }
}