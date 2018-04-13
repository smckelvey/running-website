
using System.Web.Mvc;
using System.Web.Routing;

namespace Website
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Scott",
                url: "Scott/",
                defaults: new { controller = "Profile", action = "Index", name = "Scott" }
            );

            routes.MapRoute(
                name: "Crystal",
                url: "Crystal/",
                defaults: new { controller = "Profile", action = "Index", name = "Crystal" }
            );

            routes.MapRoute(
                name: "Profile",
                url: "Profile/{name}",
                defaults: new { controller = "Profile", action = "Index", name = "Scott" }
            );

            routes.MapRoute(
                name: "Calendar",
                url: "Calendar/{name}/{month}/{year}",
                defaults: new { controller = "Calendar", action = "Index", name="Scott", month = UrlParameter.Optional, year = UrlParameter.Optional }
            );

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
