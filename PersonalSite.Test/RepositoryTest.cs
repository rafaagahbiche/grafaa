namespace PersonalSite.Test
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using PersonalSite.DataAccess;
    using PersonalSite.Domain.Abstract;
    using PersonalSite.Domain.Concrete;
    using System.Linq;
    using System.Data.Entity;
    using System.Collections.Generic;

    [TestClass]
    public class RepositoryTest
    {
        private Repo<Article> articleRepository;
        private Mock<DbSet<Article>> mockSet;
        private Mock<GrafaaEntities> mockEntities;
        private Mock<IDbContextFactory> dbContextMock;

        [TestInitialize()]
        public void MyTestInitialize() 
        {
            mockSet = new Mock<DbSet<Article>>();
            mockEntities = new Mock<GrafaaEntities>();
            mockEntities.Setup(m => m.Articles).Returns(mockSet.Object);
            dbContextMock = new Mock<IDbContextFactory>();
            dbContextMock.Setup<GrafaaEntities>(x => x.GetContext()).Returns(mockEntities.Object);
            this.articleRepository = new Repo<Article>(dbContextMock.Object);
        }

        [TestMethod]
        public void InsertTest()
        {
            mockSet.Setup<Article>(m => m.Create()).Returns(new Article());
            mockEntities.Setup(m => m.Set<Article>()).Returns(mockSet.Object);
            var insertedArticle = this.articleRepository.Insert(new Article() { Id = 1, Title = "a1" });
            this.articleRepository.Save();
            mockSet.Verify(m => m.Add(It.IsAny<Article>()), Times.Once());
            mockEntities.Verify(m => m.SaveChanges(), Times.Once()); 
        }

        [TestMethod]
        public void QueryTest()
        {
            var data = new List<Article> 
            { 
                new Article { Id = 1, Title = "BBB" }, 
                new Article { Id = 2, Title = "ZZZ" }, 
                new Article { Id = 3, Title = "AAA" }, 
            }.AsQueryable(); 
 
            mockSet.As<IQueryable<Article>>().Setup(m => m.Provider).Returns(data.Provider); 
            mockSet.As<IQueryable<Article>>().Setup(m => m.Expression).Returns(data.Expression); 
            mockSet.As<IQueryable<Article>>().Setup(m => m.ElementType).Returns(data.ElementType);
            mockEntities.Setup(m => m.Set<Article>())
                .Returns(mockSet.Object);
            mockSet.Setup(m => m.Find(It.IsAny<object[]>()))
                .Returns<object[]>(i => data.SingleOrDefault(x => x.Id.Equals(i)));
            //(int i) => data.SingleOrDefault(x => x.Id.Equals(i))
                //{ return data.SingleOrDefault(x => x.Id.Equals(i)); });
            
            var articles = articleRepository.GetAll();
            Assert.IsNotNull(articles);
            Assert.AreEqual(3, articles.Count());
            Assert.AreEqual("AAA", articleRepository.Get(3).Title); 
            Assert.AreEqual("BBB", articleRepository.Get(1).Title); 
            Assert.AreEqual("ZZZ", articleRepository.Get(2).Title); 
        }

        [TestMethod]
        public void DeleteTest()
        {
            //var dbContextMock = new Mock<IDbContextFactory>();
            //dbContextMock.Setup(x => x.GetContext()).Returns<GrafaaEntities>(t => t);
            //var articleRepository = new Repo<Article>(dbContextMock.Object);
            var articleToDelete = articleRepository.Where(x => x.Title.Equals("articleTest")).FirstOrDefault();
            articleRepository.Delete(articleToDelete);
            articleRepository.Save();
            var deletedArticle = articleRepository.Where(x => x.Title.Equals("articleTest")).FirstOrDefault();
            Assert.IsNull(deletedArticle);
        }
    }
}
