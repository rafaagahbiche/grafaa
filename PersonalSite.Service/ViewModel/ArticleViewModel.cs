using System.Collections.Generic;

namespace PersonalSite.Service.ViewModel
{
    public class ArticleViewModel
    {
        public int Id { get; set; }
        public List<ArticlePageViewModel> ArticlePages { get; set; }
        public List<int> PagesIds { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
    }
}
