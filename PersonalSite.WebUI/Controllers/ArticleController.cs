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

        public ActionResult DeleteArticlePage(PageViewModel pageViewModel)
        {
            if (pageViewModel != null && pageViewModel.PageId > 0)
            {
                articleService.DeleteArticlePage(pageViewModel.PageId);
                return RedirectToAction("Edit", new { id = pageViewModel.ParentArticleId });
            }

            return View("Edit");
        }

        [HttpGet]
        public PartialViewResult ShowPageContent(int pageId, int articleId)
        {
            var pageViewModel = this.articleService.GetArticlePageById(pageId);
            if (pageViewModel != null)
            {
                return PartialView("CreateArticlePage", pageViewModel);
            }
            else
            {
                return PartialView("CreateArticlePage", new PageViewModel() { PageId = -1, ParentArticleId = articleId });
            }
        }

        [HttpPost]
        public ActionResult CreateArticlePage(PageViewModel pageViewModel)
        {
            if (pageViewModel == null || pageViewModel.PageId == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.InternalServerError; 
                return Json(new 
                { 
                    success = false, 
                    responseText = "The view model is empty." 
                }, JsonRequestBehavior.AllowGet);
            }

            // Old article + New Page
            if (pageViewModel.PageId == -1)
            {
                pageViewModel.PageId = this.articleService.CreateArticlePage(pageViewModel);
                if (pageViewModel.PageId == -1)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new
                    {
                        success = false,
                        responseText = "Something went wrong. Please reload the page."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var viewResult = this.RenderViewToString("EditPageInfos", pageViewModel);
                    return Json(new 
                    { 
                        success = true, 
                        responseText = "New page was created successfully",
                        obj = viewResult
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                var updateSucceeded = this.articleService.UpdatePageContent(pageViewModel);
                if (updateSucceeded == false)
                {
                    Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    return Json(new
                    {
                        success = false,
                        responseText = "Something went wrong. Please reload the page."
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var viewResult = this.RenderViewToString("EditPageInfos", pageViewModel);
                    return Json(new 
                    { 
                        success = true, 
                        responseText = "Page was updated successfully.",
                        obj = viewResult
                    }, JsonRequestBehavior.AllowGet);
                }
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
