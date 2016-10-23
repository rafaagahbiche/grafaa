using PersonalSite.Service.Concrete;
using System.Web.Mvc;

namespace PersonalSite.WebUI.Controllers
{
    public class PartialController : Controller
    {
        private readonly ArticleService articleService;

        public PartialController(ArticleService articleService)
        {
            this.articleService = articleService;
        }

        [ChildActionOnly]
        public ActionResult Menu()
        {
            return PartialView(articleService.GetAllArticles());
        }
    }
}