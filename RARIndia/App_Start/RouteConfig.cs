using System.Web.Mvc;
using System.Web.Routing;

namespace RARIndia
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "User", action = "Login", id = UrlParameter.Optional }
            );

            routes.MapRoute(
               name: "GeneralCountryMaster-List",
               url: "{controller}/{action}/{id}",
               defaults: new { controller = "GeneralCountryMaster", action = "List", id = UrlParameter.Optional }
           );
        }
    }
}
