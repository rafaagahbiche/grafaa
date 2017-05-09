
namespace PersonalSite.WebUI.Controllers
{
    using System.IO;
    using System.Web.Mvc;
    
    public static class ControllerHelper
    {
        /// <summary>
        /// Controller helper to write partial view in output
        /// </summary>
        /// <param name="viewName">Partial view name</param>
        /// <param name="model">ViewModel used</param>
        /// <returns></returns>
        public static string RenderViewToString(this Controller self, string viewName, object model)
        {
            self.ViewData.Model = model;
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindPartialView(self.ControllerContext, viewName);
                var viewContext = new ViewContext(self.ControllerContext, viewResult.View, self.ViewData, self.TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(self.ControllerContext, viewResult.View);
                return sw.ToString();
            }
        }
    }
}