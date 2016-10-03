using PersonalSite.Service.ViewModel;

namespace PersonalSite.Service.Abstract
{
    public interface IArticleService
    {
        ArticleViewModel GetArticleById(int id);
        void Create(ArticleViewModel articleViewModel);
        int AddArticlePage(ArticlePageViewModel articlePageViewModel);
    }
}
