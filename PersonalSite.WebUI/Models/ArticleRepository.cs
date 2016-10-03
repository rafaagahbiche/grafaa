using PersonalSite.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace PersonalSite.WebUI.Models
{
    public class ArticleRepository : IArticleRepository, IDisposable
    {
        private GrafaaEntities dataContext;
        public ArticleRepository()
        {
            this.dataContext = new GrafaaEntities();
        }

        
        public Article GetArticleById(int id)
        {
            Article article = null;
            try
            {
                article = this.dataContext.Articles.Find(id);
            }
            catch (Exception ex)
            {

            }
            return article;
        }

        public IEnumerable<Article> GetAllArticles()
        {
            var articles = this.dataContext.Articles;
            return articles;
        }

        public ArticlePage GetArticlePageById(int id)
        {
            ArticlePage articlePage = null;
            try
            {
                articlePage = this.dataContext.ArticlePages.Find(id);
            }
            catch (Exception ex)
            {

            }
            return articlePage;
        }

        public void AddArticle(Article article)
        {
            try
            {
                this.dataContext.Articles.Add(article);
                this.dataContext.SaveChanges();
            }
            catch (Exception ex)
            {

            }
        }

        public int AddArticlePage(ArticlePage articlePage)
        {
            int idRet = -1;
            try
            {
                this.dataContext.ArticlePages.Add(articlePage);
                this.dataContext.SaveChanges();
                idRet = articlePage.Id;
            }
            catch (Exception ex)
            {

            }
            return idRet;
        }

        public bool DeleteArticlePage(int id)
        {
            bool success = false;
            try
            {
                this.dataContext.ArticlePages.Remove(GetArticlePageById(id));
                this.dataContext.SaveChanges();
                success = true;
            }
            catch { }
            return success;
        }

        public void UpdateItems()
        {
            this.dataContext.SaveChanges();
        }
 
        public void Dispose()
        {
            if (dataContext == null)
            {
                return;
            }
            else
            {
                dataContext.Dispose();
                dataContext = null;
            }
        }
    }
}