using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.Practices.Unity;

namespace PersonalSite.WebUI.IoC
{
    public class UnityControllerFactory : IControllerFactory
    {
        private IUnityContainer container;
        private IControllerFactory defaultControllerFactory;

        public UnityControllerFactory(IUnityContainer container)
            : this(container, new DefaultControllerFactory())
        {
        }
        protected UnityControllerFactory(IUnityContainer container, IControllerFactory defaultControllerFactory)
        {
            this.container = container;
            this.defaultControllerFactory = defaultControllerFactory;
        }

        public IController CreateController(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            try
            {
                return container.Resolve<IController>(controllerName);
            }
            catch
            {
                return defaultControllerFactory.CreateController(requestContext, controllerName);
            }
        }

        public void ReleaseController(IController controller)
        {
            container.Teardown(controller);
        }

        public System.Web.SessionState.SessionStateBehavior GetControllerSessionBehavior(System.Web.Routing.RequestContext requestContext, string controllerName)
        {
            return System.Web.SessionState.SessionStateBehavior.Default;
        }
    }
}