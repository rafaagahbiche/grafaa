
namespace PersonalSite.Service.Abstract
{
    using PersonalSite.Service.ViewModel;

    public interface IArticlePageService
    {
        PageViewModel GetArticlePageById(int id);
        int Create(PageViewModel pageViewModel);
        bool UpdatePageContent(PageViewModel pageViewModel);
        bool Delete(int id);
    }
}
