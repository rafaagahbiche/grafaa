using PersonalSite.DataAccess;
using PersonalSite.Service.Abstract;
using PersonalSite.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Kaliko;

namespace PersonalSite.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        
        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        public ActionResult DisplayEmptyCreateArticlePage()
        {
            return PartialView("CreateArticlePage", new ArticlePage { Id = -1 });
        }

        public ActionResult DeleteArticlePage(FormCollection postedFormData)
        {
            bool success = false;
            var pageId = postedFormData["id"];
            if (pageId != null)
            {
                success = articleService.DeleteArticlePage(Convert.ToInt32(pageId));
            }

            return Json(new
            {
                Status = success
            }); ;
        }
        
        public ActionResult CreateArticlePage(FormCollection postedFormData)
        {
            if (postedFormData == null)
            {
                return PartialView("CreateArticlePage", new ArticlePageViewModel { Id = -1 });
            }
            
            string content = postedFormData["content"];
            string pageId = postedFormData["id"];
            if (string.IsNullOrEmpty(pageId))
            {
                return PartialView("CreateArticlePage", new ArticlePageViewModel { Id = -1 });
            }

            // Old article + New Page
            if (Convert.ToInt32(pageId) == -1)
            {
                string articleId = postedFormData["articleId"];
                if (!string.IsNullOrEmpty(articleId))
                {
                    var articleViewModel = this.articleService.GetArticleById(Convert.ToInt32(articleId));
                    if (articleViewModel != null)
                    {
                        var articlePageViewModel = new ArticlePageViewModel { 
                            PageContent = content, 
                            Article = articleViewModel, 
                            ParentArticleId = articleViewModel.Id 
                        };
                        
                        int id = this.articleService.CreateArticlePage(articlePageViewModel);
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
            var oldArticlePageViewModel = this.articleService.GetArticlePageById(Convert.ToInt32(pageId));
            oldArticlePageViewModel.PageContent = content;
            this.articleService.UpdatePageContent(oldArticlePageViewModel);
            return Json(new
            {
                Status = "Page updated",
                Id = pageId,
                Content = HttpUtility.HtmlDecode(content)
            });
        }

        #region Details
        public ActionResult Details()
        {
            return View("Error");
        }

        // GET: Article/Details/5
        [Route("pages/{title}-{id}")]
        public ActionResult Details(int id)
        {
            try
            {
                var article = this.articleService.GetArticleById(id);
                if (article != null)
                {
                    return View(article);
                }

                return View();
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }
            
            return View();
        }

        #endregion

        #region Create

        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Service.ViewModel.ArticleViewModel articleViewModel, ICollection<int> articlePageIds)
        {
            try
            {
                if (articleViewModel != null)
                {
                    if (articlePageIds != null)
                    {
                        var articlePages = new List<ArticlePage>();
                        var articleId = this.articleService.Create(articleViewModel);
                        foreach (var pageId in articlePageIds)
                        {
                            this.articleService.AddArticleToPage(pageId, articleId);
                        }
                    }

                    return RedirectToRoute("Manager", new { id = articleViewModel.Id });
                }

                return View("Create");
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Edit article
        [Authorize]
        public ActionResult Edit()
        {
            return View("Error");
        }

        [Authorize]
        [Route("pages/edit/{id}")]
        public ActionResult Edit(int id)
        {
            // TODO if article not found 
            if (!id.Equals(0) && !id.Equals(-1))
            {
                var article = this.articleService.GetArticleById(id);
                return View(article);
            }

            return View();
        }
        
        // POST: Article/Edit/5
        [HttpPost, ValidateInput(false)]
        [Route("pages/edit/{id}")]
        public ActionResult Edit(ArticleViewModel article, FormCollection collection)
        {
            try
            {
                if (collection["articlePageIds"] != null)
                {
                    var pageIds = collection["articlePageIds"].Split(',');
                    var s = Array.ConvertAll(pageIds, Int32.Parse);
                    var newPageIds = article.PagesIds.Except(s);
                    if (newPageIds.Count() > 0)
                    {
                        newPageIds.ToList().ForEach(id =>
                        {
                            articleService.AddArticleToPage(article.Id, id);
                        });
                    }
                }

                article = this.articleService.UpdateArticleDetails(article);
                return View(article);
            }
            catch
            {
                return View();
            }
        }

        #endregion

        #region Delete article
        public ActionResult Delete(int id)
        {
            return View();
        }

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
        #endregion
    }
}
