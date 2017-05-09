namespace PersonalSite.WebUI.Controllers
{
    using PersonalSite.Service.Abstract;
    using PersonalSite.Service.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web;
    using System.Web.Mvc;

    public class ArticlePageController : Controller
    {
        private readonly IArticlePageService service;

        public ArticlePageController(IArticlePageService service)
        {
            this.service = service;
        }

        public ActionResult DisplayEmpty()
        {
            return View("Create");
        }

        public ActionResult Delete(FormCollection postedFormData)
        {
            bool success = false;
            var pageId = postedFormData["id"];
            if (pageId != null)
            {
                success = this.service.Delete(Convert.ToInt32(pageId));
            }

            return Json(new
            {
                Status = success
            }); ;
        }

        public ActionResult Create(FormCollection postedFormData)
        {
            if (postedFormData == null)
            {
                return PartialView("CreateArticlePage", new PageViewModel { PageId = -1 });
            }

            string content = postedFormData["content"];
            string pageId = postedFormData["id"];
            if (string.IsNullOrEmpty(pageId))
            {
                return PartialView("CreateArticlePage", new PageViewModel { PageId = -1 });
            }

            // Old article + New Page
            if (Convert.ToInt32(pageId) == -1)
            {
                string articleId = postedFormData["articleId"];
                if (!string.IsNullOrEmpty(articleId))
                {
                    var articleViewModel = new ArticleViewModel();
                        // this.service.GetArticleById(Convert.ToInt32(articleId));
                    if (articleViewModel != null)
                    {
                        var articlePageViewModel = new PageViewModel
                        {
                            PageContent = content,
                            Article = articleViewModel,
                            ParentArticleId = articleViewModel.Id
                        };

                        int id = this.service.Create(articlePageViewModel);
                        string status = id.Equals(-1) ? "succeded" : "failed";
                        return Json(new
                        {
                            Status = status,
                            Id = id,
                            Content = HttpUtility.HtmlDecode(content)
                        });
                    }
                }
            }

            // Old article + Update Page
            var oldArticlePageViewModel = this.service.GetArticlePageById(Convert.ToInt32(pageId));
            oldArticlePageViewModel.PageContent = content;
            this.service.UpdatePageContent(oldArticlePageViewModel);
            return Json(new
            {
                Status = "Page updated",
                Id = pageId,
                Content = HttpUtility.HtmlDecode(content)
            });
        }
    }
}