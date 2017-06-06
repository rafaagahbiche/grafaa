using PersonalSite.Service.ViewModel;
using System.Collections.Generic;

namespace PersonalSite.Service.Abstract
{
    public interface IArticleService
    {
        ArticleViewModel Get(int id);
        PageViewModel GetArticlePageById(int id);
        IEnumerable<ArticleViewModel> GetAll();
        PageViewModel GetFirstPage(int id);
        int PageCount(int id);
        int Create(ArticleViewModel articleViewModel);
        int CreateArticlePage(PageViewModel articlePageViewModel);
        bool DeleteArticlePage(int id);
        void AddArticleToPage(int articlePageId, int articleId);
        bool UpdatePageContent(PageViewModel articlePageViewModel);
        ArticleViewModel Update(ArticleViewModel articleViewModel);
    }
}
