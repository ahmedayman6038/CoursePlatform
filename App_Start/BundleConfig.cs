using System.Web;
using System.Web.Optimization;

namespace CoursePlatform
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-2.2.4.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/jquery2").Include(
                       "~/Scripts/jquery-1.11.0.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/umd/popper.js",
                      "~/Scripts/bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/theme").Include(
                   "~/Scripts/superfish.min.js",
                   "~/Scripts/jquery.magnific-popup.min.js",
                   "~/Scripts/jquery.tabs.min.js",
                   "~/Scripts/owl.carousel.min.js",
                    "~/Scripts/wow.min.js",
                   "~/Scripts/main.js",
                   "~/Scripts/scripts.js"));

            bundles.Add(new ScriptBundle("~/bundles/dashboard").Include(
                  "~/Scripts/bootstrapv3.min.js",
                  "~/Scripts/metisMenu/metisMenu.min.js",
                  "~/Scripts/jquery.dataTables.min.js",
                  "~/Scripts/dataTables.bootstrap.min.js",
                  "~/Scripts/sb-admin-2.js"));

            bundles.Add(new StyleBundle("~/Theme/css").Include(
                      "~/Content/linearicons.css",
                      "~/Content/bootstrapv4.css",
                      "~/Content/owl.carousel.css",
                      "~/Content/animate.css",
                      "~/Content/main.css",
                      "~/Content/Site.css"));

            bundles.Add(new StyleBundle("~/Dashboard/css").Include(
                      "~/Content/rtl/bootstrap.min.css",
                      "~/Content/rtl/bootstrap.rtl.css",
                      "~/Content/plugins/metisMenu/metisMenu.min.css",
                      "~/Content/dataTables.bootstrap.css",
                      "~/Content/rtl/sb-admin-2.css",
                      "~/Content/font-awesome.min.css"));

            BundleTable.EnableOptimizations = true;
        }
    }
}
