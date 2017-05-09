namespace PersonalSite.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using PersonalSite.WebUI.Controllers;
    using Moq;
    using PersonalSite.Service.Abstract;
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.Service.Concrete;
    using PersonalSite.Domain.Concrete;

    [TestClass]
    public class ControllerTest
    {
        [TestMethod]
        public void ArticleServiceTest()
        {
            var articleRepo = new Mock<IRepo<Article>>();
            articleRepo.Setup(a => a.Get(It.Is<int>(x => x > 0))).Returns<Article>(x => x);
            var articlePageRepo = new Mock<IRepo<ArticlePage>>();
            var articleService = new ArticleService(articleRepo.Object, articlePageRepo.Object);
            var article = articleService.Get(1);
            Assert.AreEqual(article, null);
        }
    }
}
