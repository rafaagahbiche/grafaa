using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PersonalSite.DataAccess;

namespace PersonalSite.WebUI.Models
{
    public interface IArticleRepository
    {
        Article GetArticleById(int id);
        void AddArticle(Article article);
        int AddArticlePage(ArticlePage articlePage);
        ArticlePage GetArticlePageById(int id);
        void UpdateItems();
        IEnumerable<Article> GetAllArticles();
        bool DeleteArticlePage(int id);
    }
}