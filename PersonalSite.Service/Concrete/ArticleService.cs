
namespace PersonalSite.Service.Concrete
{
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.Service.Abstract;
    using PersonalSite.Service.ViewModel;
    using PersonalSite.Service.Extension;
    using System;
    using System.Collections.Generic;
    using Kaliko;

    public class ArticleService : IArticleService
    {
        private readonly IRepo<Article> articleRepo;
        private readonly IRepo<ArticlePage> articlePageRepo;
        public ArticleService(IRepo<Article> articleRepo, IRepo<ArticlePage> articlePageRepo)
        {
            this.articleRepo = articleRepo;
            this.articlePageRepo = articlePageRepo;
        }


        public ArticleViewModel Get(int id)
        {
            ArticleViewModel articleViewModel = null;
            try
            {
                var articleObject = this.articleRepo.Get(id);
                articleViewModel = articleObject.GetViewModel(true);
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return articleViewModel;
        }

        public IEnumerable<ArticleViewModel> GetAll()
        {
            var articles = this.articleRepo.GetAll();
            foreach (var article in articles)
            {
                yield return article.GetViewModel(false);
            }
        }

        public PageViewModel GetArticlePageById(int id)
        {
            PageViewModel articlePageViewModel = null;
            try
            {
                var articlePageObject = this.articlePageRepo.Get(id);
                if (articlePageObject != null)
                {
                    articlePageViewModel = articlePageObject.GetViewModel();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return articlePageViewModel;
        }

        public int Create(ArticleViewModel articleViewModel)
        {
            int newArticleId = -1;
            try
            {
                var articleObject = new Article()
                {
                    Description = articleViewModel.Description,
                    Title = articleViewModel.Title,
                    Category = articleViewModel.Category
                };

                var newArticleObject = this.articleRepo.Insert(articleObject);
                this.articleRepo.Save();
                newArticleId = newArticleObject.Id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return newArticleId;
        }

        public bool UpdatePageContent(PageViewModel articlePageViewModel)
        {
            var updateSucceeded = false;
            try
            {
                var articlePageObject = this.articlePageRepo.Get(articlePageViewModel.PageId);
                if (articlePageObject != null)
                {
                    articlePageObject.PageContent = articlePageViewModel.PageContent;
                    this.articlePageRepo.Save();
                    updateSucceeded = true;
                }
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return updateSucceeded;
        }

        public ArticleViewModel Update(ArticleViewModel articleViewModel)
        {
            var articleObject = this.articleRepo.Get(articleViewModel.Id);
            if (articleObject != null)
            {
                try
                {
                    articleObject.Description = articleViewModel.Description;
                    articleObject.Title = articleViewModel.Title;
                    articleObject.Category = articleViewModel.Category;
                    articleRepo.Save();
                }
                catch (Exception ex)
                {
                    Kaliko.Logger.Write(ex, Logger.Severity.Critical);
                }
            }

            articleViewModel = articleObject.GetViewModel(true);
            return articleViewModel;
        }

        public void AddArticleToPage(int articlePageId, int articleId)
        {
            var articlePageObject = articlePageRepo.Get(articlePageId);
            if (articlePageObject != null)
            {
                articlePageObject.Article = this.articleRepo.Get(articleId);
                articlePageRepo.Save();
            }
        }

        public int CreateArticlePage(PageViewModel articlePageViewModel)
        {
            int newPageId = -1;
            try
            {
                var articlePageObject = new ArticlePage()
                {
                    PageContent = articlePageViewModel.PageContent
                };

                var articleObject = articleRepo.Get(articlePageViewModel.ParentArticleId);
                if (articleObject != null)
                {
                    articlePageObject.Article = articleObject;
                    articlePageObject.ParentArticle = articlePageViewModel.ParentArticleId; 
                }

                var newArticlePageObj = this.articlePageRepo.Insert(articlePageObject);
                this.articlePageRepo.Save();
                newPageId = newArticlePageObj.Id;
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return newPageId;
        }

        public bool DeleteArticlePage(int id)
        {
            bool success = false;
            try
            {
                var articlePageObject = this.articlePageRepo.Get(id);
                this.articlePageRepo.Delete(articlePageObject);
                this.articlePageRepo.Save();
                success = true;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return success;
        }
    }
}
