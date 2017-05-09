namespace PersonalSite.Test
{
    using System;
    using System.Text;
    using System.Collections.Generic;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.Domain.Concrete;
    using System.Linq;
    using System.Data.Entity;
    using PersonalSite.Service.Concrete;

    /// <summary>
    /// Summary description for ServiceTest
    /// </summary>
    [TestClass]
    public class ServiceTest
    {
        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion

        private IEnumerable<Article> articles;
        private Mock<IRepo<Article>> mockArticleRepository;
        private Mock<IRepo<ArticlePage>> mockArticlePageRepository;
        private ArticleService mockService;      
  
        [TestInitialize()]
        public void TestInitialize()
        {
            articles = new List<Article>()
            {
                new Article(){
                    Id = 1,
                    Title = "a1"
                },
                new Article(){
                    Id = 2,
                    Title = "a2"
                },
                new Article(){
                    Id = 3,
                    Title = "a3"
                },
            };

            var dbContextMock = new Mock<IDbContextFactory>();
            dbContextMock.Setup<GrafaaEntities>(x => x.GetContext());

            mockArticleRepository = new Mock<IRepo<Article>>();
            mockArticlePageRepository = new Mock<IRepo<ArticlePage>>();
            mockService = new ArticleService(mockArticleRepository.Object, mockArticlePageRepository.Object);
        }

        [TestMethod]
        public void TestGetMethod()
        {
            mockArticleRepository.Setup(m => m.Get(It.IsAny<int>()))
                .Returns((int i) => articles.SingleOrDefault(a => a.Id.Equals(i)));
            var article = mockService.Get(1);
            Assert.IsNotNull(article);
        }

        [TestMethod]
        public void TestGetAllMethod()
        {
            mockArticleRepository.Setup(m => m.GetAll())
                .Returns(articles.AsQueryable);
            var articlesFromRepository = mockService.GetAll();
            Assert.IsNotNull(articlesFromRepository);
            Assert.IsTrue(articlesFromRepository.Count() == 3);
        }
    }
}
