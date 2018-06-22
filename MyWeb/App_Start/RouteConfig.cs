using System.Web.Mvc;
using System.Web.Routing;

namespace MyWeb
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
              name: "Post",
              url: "Post/{id}/{title}",
              defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional, title = UrlParameter.Optional },
              namespaces: new[] { "MyWeb.Controllers" }
            );

            routes.MapRoute(
              name: "Page",
              url: "Page/{id}",
              defaults: new { controller = "Page", action = "Index" },
              namespaces: new[] { "MyWeb.Controllers" }
            );

            routes.MapRoute(
              name: "Category",
              url: "Category/{id}/{title}/{page}",
              defaults: new { controller = "Category", action = "Index", title = UrlParameter.Optional, page = UrlParameter.Optional },
              namespaces: new[] { "MyWeb.Controllers" }
            );

            routes.MapRoute(
              name: "Search",
              url: "Search/{id}/{page}",
              defaults: new { controller = "Search", action = "Index", page = UrlParameter.Optional },
              namespaces: new[] { "MyWeb.Controllers" }
            );

            routes.MapRoute(
                name: "Statistics",
                url: "Statistics/{id}",
                defaults: new { controller = "Statistics", action = "Index", id = UrlParameter.Optional, title = UrlParameter.Optional },
                namespaces: new[] { "MyWeb.Controllers" }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Post", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "MyWeb.Controllers" }
            );
        }
    }
}