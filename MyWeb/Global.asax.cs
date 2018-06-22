using System.Collections.Generic;
using System.Security.Principal;
using StructureMap.Web.Pipeline;
using System;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MyWeb.IocConfig;
using MyWeb.ServiceLayer.Common;
using MyWeb.Utility;

namespace MyWeb
{
    public class Global : HttpApplication
    {
        protected void Application_Start(object sender, EventArgs e)
        {
            try
            {
                // Code that runs on application startup
                AreaRegistration.RegisterAllAreas();
                GlobalConfiguration.Configure(WebApiConfig.Register);
                RouteConfig.RegisterRoutes(RouteTable.Routes);
                BundleConfig.RegisterBundles(BundleTable.Bundles);
                ControllerBuilder.Current.SetControllerFactory(new StructureMapControllerFactory());
                foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunAtInit>())
                {
                    task.Execute();
                }

                //ScheduledTasksRegistry.Init();
                ModelBinders.Binders.Add(typeof(DateTime), new PersianDateModelBinder());
                ModelBinders.Binders.Add(typeof(DateTime?), new PersianDateModelBinder());

                //Microsoft.AspNet.SignalR.GlobalHost.DependencyResolver = ProjectObjectFactory.Container.GetInstance<Microsoft.AspNet.SignalR.IDependencyResolver>();

            }
            catch (Exception)
            {
                HttpRuntime.UnloadAppDomain();
                throw;
            }

        }

        #region Application_EndRequest
        protected void Application_EndRequest(object sender, EventArgs e)
        {
            try
            {
                foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunAfterEachRequest>())
                {
                    task.Execute();
                }
            }
            catch (Exception)
            {
                HttpContextLifecycle.DisposeAndClearAll();
            }

        }
        #endregion

        #region Application_BeginRequest
        private void Application_BeginRequest(object sender, EventArgs e)
        {
            foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunOnEachRequest>())
            {
                task.Execute();
            }
        }
        #endregion

        #region ShouldIgnoreRequest
        private bool ShouldIgnoreRequest()
        {
            string[] reservedPath =
              {
                  "/__browserLink",
                  "/Scripts",
                  "/Content"
              };

            var rawUrl = Context.Request.RawUrl;
            if (reservedPath.Any(path => rawUrl.StartsWith(path, StringComparison.OrdinalIgnoreCase)))
            {
                return true;
            }

            return BundleTable.Bundles.Select(bundle => bundle.Path.TrimStart('~'))
                      .Any(bundlePath => rawUrl.StartsWith(bundlePath, StringComparison.OrdinalIgnoreCase));
        }
        #endregion

        #region Private
        private void SetPermissions(IEnumerable<string> permissions)
        {
            Context.User =
                new GenericPrincipal(Context.User.Identity, permissions.ToArray());
        }
        #endregion

        #region Application_Error
        protected void Application_Error()
        {
            foreach (var task in ProjectObjectFactory.Container.GetAllInstances<IRunOnError>())
            {
                task.Execute();
            }
        }
        #endregion
    }
}