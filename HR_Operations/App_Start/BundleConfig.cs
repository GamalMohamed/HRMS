using System.Web;
using System.Web.Optimization;

namespace CoEX_HRMS
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

            bundles.Add(new ScriptBundle("~/bundles/datatable-plugins").Include(
                        "~/assets/global/scripts/datatable.js",
                        "~/assets/global/plugins/datatables/datatables.min.js",
                        "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.js"));

            bundles.Add(new ScriptBundle("~/bundles/spinner-button-plugins").Include(
                        "~/assets/global/plugins/ladda/spin.min.js",
                        "~/assets/global/plugins/ladda/ladda.min.js"));

            bundles.Add(new ScriptBundle("~/bundles/multiple-select-autocomplete").Include(
                "~/assets/global/plugins/select2/js/select2.full.min.js",
                "~/assets/pages/scripts/components-select2.js",
                "~/assets/global/plugins/typeahead/typeahead.bundle.min.js"
                ));

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at https://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include(
                      "~/Scripts/bootstrap.js",
                      "~/Scripts/respond.js"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                      "~/Content/bootstrap.css",
                      "~/Content/site.css",
                      "~/Content/toastr.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/multiple-select-autocomplete").Include(
                      "~/assets/global/plugins/typeahead/typeahead.css",
                      "~/assets/global/plugins/select2/css/select2.min.css",
                      "~/assets/global/plugins/select2/css/select2-bootstrap.min.css"
                      ));

            bundles.Add(new StyleBundle("~/Content/datatables-css").Include(
                      "~/assets/global/plugins/datatables/datatables.min.css",
                      "~/assets/global/plugins/datatables/plugins/bootstrap/datatables.bootstrap.css"
                      ));
        }
    }
}
