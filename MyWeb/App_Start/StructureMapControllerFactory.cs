using System;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using MyWeb.IocConfig;

namespace MyWeb
{
    public class StructureMapControllerFactory : DefaultControllerFactory
    {
        #region override GetControllerInstance

        protected override IController GetControllerInstance(RequestContext requestContext, Type controllerType)
        {
            if (controllerType == null)
            {
                throw new HttpException(404, "Page not Found");
                //throw new InvalidOperationException($"Page not found: {requestContext.HttpContext.Request.RawUrl}");
            }
            return ProjectObjectFactory.Container.GetInstance(controllerType) as Controller;
        }

        #endregion
    }
}




















