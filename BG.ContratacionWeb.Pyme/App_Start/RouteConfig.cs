using BG.ContratacionWeb.Pyme.Helpers;
using BG.ContratacionWeb.Pyme.Infrastructure;
using System;
using System.Web.Mvc;
using System.Web.Routing;

namespace BG.ContratacionWeb.Pyme
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            
            if(InitialConfig.getEnvironment().mode.Equals("development", StringComparison.InvariantCultureIgnoreCase))
            {
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "Test", action = "Index", id = UrlParameter.Optional }
                );
            }
            else
            {
                routes.MapRoute(
                    name: "Default",
                    url: "{controller}/{action}/{id}",
                    defaults: new { controller = "TokenAccessValidation", action = "Index", id = UrlParameter.Optional }
                );
            }

            
        }
    }
}
