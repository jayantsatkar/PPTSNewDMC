using System.Web;
using System.Web.Optimization;

namespace FactoryMagix
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                      "~/Scripts/jquery-{version}.js",
                      "~/Scripts/bootstrap.min.js",
                      "~/Scripts/jquery.scrollTo.min.js",
                      "~/Scripts/jquery.nicescroll.js",
                      "~/Scripts/scripts.js",
                      "~/Scripts/jtable/jquery.jtable.js",
                      "~/Scripts/jtable/jquery.jtable.min.js",
                      "~/Scripts/jquery-ui-1.10.4.min.js",
                      "~/Scripts/jquery-ui-1.9.2.custom.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/bootstrap-combobox.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/bootstrap.min.css",
                      "~/Content/bootstrap-theme.css",
                      "~/Content/bootstrap-combobox.css",
                      "~/Content/elegant-icons-style.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/style.css",
                      "~/Content/style-responsive.css",
                      "~/Content/PagedList.css"));

        }
    }
}
