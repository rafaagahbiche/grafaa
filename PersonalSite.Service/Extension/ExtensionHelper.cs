using PersonalSite.DataAccess;
using PersonalSite.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PersonalSite.Service.Extension
{
    public static class ExtensionHelper
    {
        public static ArticleViewModel GetViewModel(this Article article)
        {
            var articleViewModel = new ArticleViewModel();
            if (article != null)
            {
                articleViewModel.Id = article.Id;
                articleViewModel.Title = article.Title;
                articleViewModel.Description = article.Description;
                articleViewModel.Category = article.Category;
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
                articlePageViewModel.Article = articlePage.Article.GetViewModel();
            }

            return articlePageViewModel;
        }
    }
}
