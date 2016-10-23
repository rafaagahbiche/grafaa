using PersonalSite.DataAccess;
using PersonalSite.Domain.Abstract;
using PersonalSite.Service.Abstract;
using PersonalSite.Service.ViewModel;
using PersonalSite.Service.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kaliko;

namespace PersonalSite.Service.Concrete
{
    public class ArticleService : IArticleService
    {
        private readonly IRepo<Article> articleRepo;
        private readonly IRepo<ArticlePage> articlePageRepo;
        public ArticleService(IRepo<Article> articleRepo, IRepo<ArticlePage> articlePageRepo)
        {
            this.articleRepo = articleRepo;
            this.articlePageRepo = articlePageRepo;
        }


        public ArticleViewModel GetArticleById(int id)
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

        public IEnumerable<ArticleViewModel> GetAllArticles()
        {
            var articles = this.articleRepo.GetAll();
            foreach (var article in articles)
            {
                yield return article.GetViewModel(false);
            }
        }

        public ArticlePageViewModel GetArticlePageById(int id)
        {
            var articlePageViewModel = new ArticlePageViewModel();
            try
            {
                var articlePageObject = this.articlePageRepo.Get(id);
                articlePageViewModel = articlePageObject.GetViewModel();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return articlePageViewModel;
        }

        public int Create(ArticleViewModel articleViewModel)
        {
            int id = -1;
            try
            {
                var articleObject = new Article()
                {
                    Description = articleViewModel.Description,
                    Title = articleViewModel.Title,
                    Category = articleViewModel.Category
                };

                this.articleRepo.Insert(articleObject);
                this.articleRepo.Save();
                id = articleObject.Id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return id;
        }

        public void UpdatePageContent(ArticlePageViewModel articlePageViewModel)
        {
            try
            {
                var articlePageObject = this.articlePageRepo.Get(articlePageViewModel.Id);
                if (articlePageObject != null)
                {
                    articlePageObject.PageContent = articlePageViewModel.PageContent;
                    this.articlePageRepo.Save();
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public ArticleViewModel UpdateArticleDetails(ArticleViewModel articleViewModel)
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

        public int CreateArticlePage(ArticlePageViewModel articlePageViewModel)
        {
            int idRet = -1;
            try
            {
                var articlePageObject = new ArticlePage()
                {
                    Article = articlePageViewModel.Article != null 
                            ? articleRepo.Get(articlePageViewModel.Article.Id) : null,
                    PageContent = articlePageViewModel.PageContent
                };

                this.articlePageRepo.Insert(articlePageObject);
                this.articlePageRepo.Save();
                idRet = articlePageObject.Id;
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return idRet;
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
