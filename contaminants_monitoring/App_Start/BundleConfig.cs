using System.Web;
using System.Web.Optimization;

namespace Contaminants_Monitoring
{
    public class BundleConfig
    {
        // For more information on bundling, visit https://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.validate*"));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js"));
            bundles.Add(new ScriptBundle("~/bundles/jqueryui").Include(
                "~/Scripts/jquery-ui-{version}.js"));

            bundles.Add(new ScriptBundle("~/bundles/datatables").Include(
                    "~/Scripts/Datatables/jquery.datatables.js",
                    "~/Scripts/Datatables/datatables.bootsrap.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/font-awesome.min.css",
                      "~/Content/site.css"));
            //"~/Content/DataTables/css/dataTables.bootstrap4.css"));
            bundles.Add(new StyleBundle("~/Content/jqueryui").Include("~/Content/themes/base/all.css"));
            bundles.Add(new ScriptBundle("~/bundles/chosen").Include(
                    "~/Scripts/chosen/chosen.jquery.min.js",
                    "~/Scripts/chosen/chosen.jquery.js",
                    "~/Scripts/chosen/docsupport/prism.js",
                    "~/Scripts/chosen/docsupport/init.js"));

           
        }
    }
}
