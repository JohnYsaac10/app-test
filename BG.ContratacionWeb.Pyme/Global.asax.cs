using BG.ContratacionWeb.Pyme.App_Start;
using BG.ContratacionWeb.Pyme.Helpers;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;

namespace BG.ContratacionWeb.Pyme
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            UnityConfig.RegisterComponents();

            RouteConfig.RegisterRoutes(RouteTable.Routes);

            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutomapperProfile.run();
        }
    }
}
