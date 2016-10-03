using PersonalSite.DataAccess;
using PersonalSite.WebUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PersonalSite.WebUI.Controllers
{
    public class ArticleController : Controller
    {
        private IArticleRepository articleRepository;

        public ArticleController(IArticleRepository articleRepository)
        {
            this.articleRepository = articleRepository;
        }

        // GET: Article
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Menu()
        {
            var articles = this.articleRepository.GetAllArticles();
            return View(articles);
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
                success = articleRepository.DeleteArticlePage(Convert.ToInt32(pageId));
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
                return PartialView("CreateArticlePage", new ArticlePage { Id = -1 });
            }
            
            string content = postedFormData["content"];
            string pageId = postedFormData["id"];
            if (string.IsNullOrEmpty(pageId))
            {
                return PartialView("CreateArticlePage", new ArticlePage { Id = -1 });
            }
            
            if (Convert.ToInt32(pageId) == -1)
            {
                string articleId = postedFormData["articleId"];
                if (!string.IsNullOrEmpty(articleId))
                {
                    var article = this.articleRepository.GetArticleById(Convert.ToInt32(articleId));
                    if (article != null)
                    {
                        var articlePage = new ArticlePage { PageContent = content, Article = article };
                        int id = this.articleRepository.AddArticlePage(articlePage);
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

            var pageToUpdate = this.articleRepository.GetArticlePageById(Convert.ToInt32(pageId));
            pageToUpdate.PageContent = content;
            this.articleRepository.UpdateItems();
            return Json(new
            {
                Status = "Page updated",
                Id = pageId,
                Content = HttpUtility.HtmlDecode(content)
            });
        }

        public ActionResult Details()
        {
            return View("Error");
        }

        // GET: Article/Details/5
        [Route("pages/{title}-{id}")]
        public ActionResult Details(int id)
        {
            Article article = this.articleRepository.GetArticleById(id);
            if (article != null)
            {
                return View(article);
            }
        
            return View("Error");
        }

        // GET: Article/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Article/Create
        [HttpPost, ValidateInput(false)]
        public ActionResult Create(Article article, ICollection<int> articlePageIds)
        {
            try
            {
                if (article != null)
                {
                    if (articlePageIds != null)
                    {
                        var articlePages = new List<ArticlePage>();
                        foreach (var pageId in articlePageIds)
                        {
                            var articlePage = this.articleRepository.GetArticlePageById(pageId);
                            if (articlePage != null)
                            {
                                articlePages.Add(this.articleRepository.GetArticlePageById(pageId));
                            }
                        }

                        article.ArticlePages = articlePages;
                    }
                    this.articleRepository.AddArticle(article);
                    return RedirectToRoute("Manager", new { id = article.Id });
                }
                return View("Create");
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Edit()
        {
            return View("Error");
        }

        // GET: Article/Edit/5
        [Route("pages/edit/{id}")]
        public ActionResult Edit(int id)
        {
            // TODO if article not found 
            if (!id.Equals(0) && !id.Equals(-1))
            {
                var article = this.articleRepository.GetArticleById(id);
                return View(article);
            }

            return View();
        }
        
        // POST: Article/Edit/5
        [HttpPost, ValidateInput(false)]
        [Route("pages/edit/{id}")]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here
                string pageId = collection["id"];

                var article = this.articleRepository.GetArticleById(Int32.Parse(pageId));
                article.Description = collection["Description"] != null
                    ? collection["Description"].ToString() : string.Empty;

                article.Title = collection["Title"] != null
                    ? collection["Title"].ToString() : string.Empty;
                if (collection["articlePageIds"] != null)
                {
                    var pageIds = collection["articlePageIds"].Split(',');
                    foreach(var tmpId in pageIds){
                        int intId = Convert.ToInt32(tmpId);
                        if (intId > 0)
                        {
                            var pageAlreadyLinked = article.ArticlePages.Any(x => intId.Equals(x.Id));
                            if (!pageAlreadyLinked)
                            {
                                var articlePage = articleRepository.GetArticlePageById(intId);
                                if (articlePage != null)
                                {
                                    article.ArticlePages.Add(articlePage);
                                }
                            }
                        }
                    }
                }
                this.articleRepository.UpdateItems();
                return View(article);
            }
            catch
            {
                return View();
            }
        }

        // GET: Article/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: Article/Delete/5
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
    }
}
