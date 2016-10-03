using PersonalSite.WebUI.Models;
using System.Web.Mvc;

namespace PersonalSite.WebUI.Controllers
{
    public class PartialController : Controller
    {
        // GET: Partial
        public ActionResult Index()
        {
            return View();
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            ArticleRepository rep = new ArticleRepository();
            var articles = rep.GetAllArticles();
            return PartialView(articles);
        }
    }
}