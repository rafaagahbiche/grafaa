using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalSite.Helpers
{
    public static class HtmlHelperExtensions
    {
        public static string Menu(this HtmlHelper helper)
        {
            return MvcHtmlString.Create("<ul>").ToHtmlString();
        }
    }
}