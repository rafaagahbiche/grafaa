using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace PersonalSite.WebUI
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapMvcAttributeRoutes();
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                "DefaultHome",
                string.Empty,
                new { controller = "Home", action = "index" }
            );
            routes.MapRoute(
                "Pages",
                "pages/{title}-{id}",
                new { controller = "Article", action = "Details" }
            );
            routes.MapRoute(
                "Manager",
                "pages/edit/{id}",
                new { controller = "Article", action = "Edit" }
            );
            routes.MapRoute(
                "Contact",
                "contact",
                new { controller = "Home", action = "Contact" }
            );
        }
    }
}
