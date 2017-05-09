
namespace PersonalSite.Service.Extension
{
    using PersonalSite.DataAccess;
    using PersonalSite.Service.ViewModel;
    using System.Collections.Generic;

    public static class ExtensionHelper
    {
        public static ArticleViewModel GetViewModel(this Article article, bool loadPages)
        {
            var articleViewModel = new ArticleViewModel();
            if (article != null)
            {
                articleViewModel.Id = article.Id;
                articleViewModel.Title = article.Title;
                articleViewModel.Description = article.Description;
                articleViewModel.Category = article.Category;
                if (loadPages)
                {
                    articleViewModel.ArticlePages = new List<PageViewModel>();
                    articleViewModel.PagesIds = new List<int>();
                    foreach (var articlePage in article.ArticlePages)
                    {
                        articleViewModel.ArticlePages.Add(articlePage.GetViewModel());
                        articleViewModel.PagesIds.Add(articlePage.Id);
                    }
                }
            }

            return articleViewModel;
        }

        public static PageViewModel GetViewModel(this ArticlePage articlePage)
        {
            PageViewModel articlePageViewModel = null;
            if (articlePage != null)
            {
                articlePageViewModel = new PageViewModel()
                {
                    PageId = articlePage.Id,
                    PageContent = articlePage.PageContent,
                    ParentArticleId = articlePage.Article.Id
                };
            }

            return articlePageViewModel;
        }
    }
}
