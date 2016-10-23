using PersonalSite.DataAccess;
using PersonalSite.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Service.Extension
{
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
                    articleViewModel.ArticlePages = new List<ArticlePageViewModel>();
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

        public static ArticlePageViewModel GetViewModel(this ArticlePage articlePage)
        {
            var articlePageViewModel = new ArticlePageViewModel();
            if (articlePage != null)
            {
                articlePageViewModel.Id = articlePage.Id;
                articlePageViewModel.PageContent = articlePage.PageContent;
                articlePageViewModel.ParentArticleId = articlePage.Article.Id;
            }

            return articlePageViewModel;
        }
    }
}
