using System.Web;
using System.Web.Optimization;

namespace PhotoWork
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                         "~/Scripts/jquery.min.js",
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/js").Include(
                        "~/Scripts/popper.min.js",
                      "~/Scripts/bootstrap.js",
                       "~/Scripts/bootstrap.min.js",
                       "~/Scripts/owl.carousel.min.js"
                       ));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.min.css",
                      "~/Content/one-page-wonder.css",
                      "~/Content/owl.carousel*"));
            bundles.Add(new StyleBundle("~/Content/owl").Include(
                    "~/Content/owl.carousel.min.css",
                     "~/Content/owl.theme.default.min.css"
                     ));


        }
    }
}
