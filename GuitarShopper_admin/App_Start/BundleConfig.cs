using System.Web;
using System.Web.Optimization;

namespace GuitarShopper_admin
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js","~/Scripts/jquery.dataTables.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*", "~/Scripts/globalize/globalize.js", "~/Scripts/globalize/cultures/globalize.culture.nl-BE.js"));

            bundles.Add(new ScriptBundle("~/bundles/dataTables").Include("~/Scripts/dataTables.bootstrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/css/bootstrap.min.css",
                "~/Content/css/bootstrap-theme.css",
                "~/Content/css/dataTables.bootstrap.css",
                "~/Content/css/Site.css"));

            bundles.Add(new StyleBundle("~/Content/logincss").Include("~/Content/css/bootstrap.min.css", "~/Content/css/Login.css"));

        }
    }
}