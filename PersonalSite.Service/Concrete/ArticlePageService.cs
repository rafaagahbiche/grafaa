

namespace PersonalSite.Service.Concrete
{
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.Service.Abstract;
    using PersonalSite.Service.ViewModel;
    using PersonalSite.Service.Extension;
    using System;
    using Kaliko;

    public class ArticlePageService : IArticlePageService
    {
        private readonly IRepo<ArticlePage> repository;
        public ArticlePageService(IRepo<ArticlePage> articlePageRepo)
        {
            this.repository = articlePageRepo;
        }


        public PageViewModel GetArticlePageById(int id)
        {
            var articlePageViewModel = new PageViewModel();
            try
            {
                var articlePageObject = this.repository.Get(id);
                articlePageViewModel = articlePageObject.GetViewModel();
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return articlePageViewModel;
        }

        public int Create(PageViewModel pageViewModel)
        {
            int newArticlePageId = -1;
            try
            {
                var articlePageObject = new ArticlePage()
                {
                    //Article = pageViewModel.Article != null
                    //        ? articleRepo.Get(pageViewModel.Article.Id) : null,
                    PageContent = pageViewModel.PageContent
                };

                var newArticlePage = this.repository.Insert(articlePageObject);
                this.repository.Save();
                newArticlePageId = newArticlePage.Id;
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return newArticlePageId;
        }

        public bool UpdatePageContent(PageViewModel pageViewModel)
        {
            bool updateSucceeded = false;
            try
            {
                var articlePageObject = this.repository.Get(pageViewModel.PageId);
                if (articlePageObject != null)
                {
                    articlePageObject.PageContent = pageViewModel.PageContent;
                    this.repository.Save();
                    updateSucceeded = true;
                }
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return updateSucceeded;
        }

        public bool Delete(int id)
        {
            bool success = false;
            try
            {
                var articlePageObject = this.repository.Get(id);
                this.repository.Delete(articlePageObject);
                this.repository.Save();
                success = true;
            }
            catch (Exception ex)
            {
                Kaliko.Logger.Write(ex, Logger.Severity.Critical);
            }

            return success;
        }
    }
}
