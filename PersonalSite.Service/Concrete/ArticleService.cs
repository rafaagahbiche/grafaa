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
                articleViewModel = articleObject.GetViewModel();
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }

            return articleViewModel;
        }

        public IEnumerable<ArticleViewModel> GetAllArticles()
        {
            var articles = this.articleRepo.GetAll();
            var articlesList = new List<ArticleViewModel>();
            foreach (var article in articles)
            {
                articlesList.Add(article.GetViewModel());
            }

            return articlesList;
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

        public void Create(ArticleViewModel articleViewModel)
        {
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
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
            }
        }

        public int AddArticlePage(ArticlePageViewModel articlePageViewModel)
        {
            int idRet = -1;
            try
            {
                var articlePageObject = new ArticlePage()
                {
                    Article = articleRepo.Get(articlePageViewModel.Article.Id),
                    PageContent = articlePageViewModel.PageContent
                };

                this.articlePageRepo.Insert(articlePageObject);
                this.articlePageRepo.Save();
                idRet = articlePageObject.Id;
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
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
