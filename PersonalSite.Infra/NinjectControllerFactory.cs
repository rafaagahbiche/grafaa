using System;
using System.Web.Mvc;
using System.Web.Routing;
using Ninject;
using PersonalSite.Service.Abstract;
using PersonalSite.Service.Concrete;
using PersonalSite.Domain.Abstract;
using PersonalSite.DataAccess;
using PersonalSite.Domain.Concrete;


namespace PersonalSite.Infra
{

    public class NinjectControllerFactory : DefaultControllerFactory
    {
        private IKernel ninjectKernel;

        public NinjectControllerFactory()
        {
            ninjectKernel = new StandardKernel();
            AddBindings();
        }

        protected override IController GetControllerInstance(RequestContext
            requestContext, Type controllerType)
        {

            return controllerType == null
                ? null
                : (IController)ninjectKernel.Get(controllerType);
        }

        private void AddBindings()
        {
            ninjectKernel.Bind<IArticleService>().To<ArticleService>();
            ninjectKernel.Bind<IDbContextFactory>().To<DbContextFactory>();
            ninjectKernel.Bind<IRepo<Article>>().To<Repo<Article>>();
            ninjectKernel.Bind<IRepo<ArticlePage>>().To<Repo<ArticlePage>>();
            ninjectKernel.Bind<IAuthProvider>().To<FormsAuthProvider>();
        }
    }
}
