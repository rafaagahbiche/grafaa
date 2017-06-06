namespace PersonalSite.WebUI.Controllers
{
    using PersonalSite.Service.Abstract;
    using PersonalSite.Service.ViewModel;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.Mvc;
    using Kaliko;
    using System.Net;
    using System.IO;

    public class ArticleController : Controller
    {
        private readonly IArticleService articleService;
        
        public ArticleController(IArticleService articleService)
        {
            this.articleService = articleService;
        }

        [HttpGet]
        public PartialViewResult DeletePage(int pageId, int articleId)
        {
            if (pageId > 0)
            {
                articleService.DeleteArticlePage(pageId);
            }

            var pageCount = this.articleService.PageCount(articleId);
            if (pageCount > 0)
            {
                var firstPageViewModel = this.articleService.GetFirstPage(articleId);
                return PartialView("CreateArticlePage", firstPageViewModel);
            }

            return PartialView("CreateArticlePage", new PageViewModel() { PageId = -1, ParentArticleId = articleId });
        }

        [HttpGet]
        public PartialViewResult ShowPageContent(int pageId, int articleId)
        {
            var pageViewModel = this.articleService.GetArticlePageById(pageId);
            if (pageViewModel != null)
            {
                return PartialView("CreateArticlePage", pageViewModel);
            }

            return PartialView("CreateArticlePage", new PageViewModel() { PageId = -1, ParentArticleId = articleId });
        }

        [HttpGet]
        public PartialViewResult AddNewTab(int articleId)
        {
            return PartialView("PageTab", new PageViewModel() { PageId = -1, ParentArticleId = articleId });
        }

        [HttpPost]
        public PartialViewResult SavePage(PageViewModel pageViewModel)
        {
            if (pageViewModel == null || pageViewModel.PageId == 0)
            {
                return PartialView("EditPageInfos", new PageViewModel() { PageId = -1, ParentArticleId = -1 });
            }

            // Old article + New Page
            if (pageViewModel.PageId == -1)
            {
                pageViewModel.PageId = this.articleService.CreateArticlePage(pageViewModel);
                if (pageViewModel.PageId == -1)
                {
                    return PartialView("EditPageInfos", new PageViewModel() { PageId = -1, ParentArticleId = -1 });
                }
                else
                {
                    return PartialView("EditPageInfos", pageViewModel);
                }
            }
            else
            {
                var updateSucceeded = this.articleService.UpdatePageContent(pageViewModel);
                return PartialView("EditPageInfos", pageViewModel);
            }
        }

        #region Details
        public ActionResult Details()
        {
            return View("Error");
        }

        [Route("pages/{title}-{id}")]
        public ActionResult Details(int id)
        {
            try
            {
                var article = this.articleService.Get(id);
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
        public ActionResult Create(Service.ViewModel.ArticleViewModel articleViewModel
            , ICollection<int> articlePageIds)
        {
            try
            {
                if (articleViewModel != null)
                {
                    articleViewModel.Id = this.articleService.Create(articleViewModel);
                    if (articlePageIds != null)
                    {
                        foreach (var pageId in articlePageIds)
                        {
                            this.articleService.AddArticleToPage(pageId, articleViewModel.Id);
                        }
                    }

                    return RedirectToAction("Edit", new { id = articleViewModel.Id });
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
                var article = this.articleService.Get(id);
                if (article != null)
                {
                    return View(article);
                }
                else
                {
                    Logger.Write("Article is null");
                    return View("Error");
                }
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
                if (collection != null && collection["articlePageIds"] != null)
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

                article = this.articleService.Update(article);
                return View(article);
            }
            catch(Exception ex)
            {
                Logger.Write(ex, Logger.Severity.Major);
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
