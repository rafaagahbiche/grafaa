using PersonalSite.Service.ViewModel;
using System.Collections.Generic;

namespace PersonalSite.Service.Abstract
{
    public interface IArticleService
    {
        ArticleViewModel GetArticleById(int id);
        ArticlePageViewModel GetArticlePageById(int id);
        IEnumerable<ArticleViewModel> GetAllArticles();
        int Create(ArticleViewModel articleViewModel);
        int CreateArticlePage(ArticlePageViewModel articlePageViewModel);
        bool DeleteArticlePage(int id);
        void AddArticleToPage(int articlePageId, int articleId);
        void UpdatePageContent(ArticlePageViewModel articlePageViewModel);
        ArticleViewModel UpdateArticleDetails(ArticleViewModel articleViewModel);
    }
}
