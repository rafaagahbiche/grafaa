using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microsoft.Practices.Unity;
using PersonalSite.WebUI.Models;
using PersonalSite.WebUI.Controllers;
using PersonalSite.WebUI.IoC;

namespace PersonalSite.WebUI
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ControllerBuilder.Current.SetControllerFactory(new UnityControllerFactory(GetUnityContainer()));
        }
        private IUnityContainer GetUnityContainer()
        {
            IUnityContainer container = new UnityContainer();
            container.RegisterType<IArticleRepository, ArticleRepository>();
            container.RegisterType<IController, ArticleController>("Article");
            container.RegisterType<IController, ArticleController>("detail");
            return container;
        }
    }
}
