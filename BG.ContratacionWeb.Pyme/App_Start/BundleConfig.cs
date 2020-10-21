using System.Web.Optimization;

namespace BG.ContratacionWeb.Pyme.App_Start
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new StyleBundle("~/bundles/css").Include(
                "~/Content/css/styles.css",
                "~/Content/css/romads.min.css",
                "~/Content/css/bootstrap.min.css",
                "~/Content/Alertify/alertify.min.css",
                "~/Content/Alertify/default.min.css"
            ));

            BundleTable.EnableOptimizations = true;
        }
    }
}