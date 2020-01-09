using System.Web;
using System.Web.Optimization;

namespace MVC5WebApplication
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/scripts").IncludeDirectory("~/Scripts/plugins", "*.js", true));

            //  bundles.Add(new StyleBundle("~/bundles/styles").IncludeDirectory("~/Content/Global/css", "*.css", true));

            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/modernizr-2.6.2.js"));

            bundles.Add(new StyleBundle("~/bundles/base/css").Include(
              "~/Content/bootstrap/css/bootstrap.min.css",
              "~/Content/MyCss.css",                            //new css by me
              "~/Content/assets/css/main.css",
              "~/Content/assets/css/plugins.css",
              "~/Content/assets/css/responsive.css",
              "~/Content/assets/css/icons.css",
              "~/Content/assets/css/fontawesome/font-awesome.min.css",
              "~/Content/Custom_menu.css",
             "~/Content/font-awesome.css",

              "~/Content/themes/base/jquery.ui.all.css",
              "~/Content/error-notifier.css",

              "~/Content/Timepicker/jquery.timepicker.css"

          ));


            bundles.Add(new ScriptBundle("~/bundles/plugins").Include(
                "~/Content/assets/js/libs/lodash.compat.min.js",
                "~/plugins/touchpunch/jquery.ui.touch-punch.min.js",
                "~/plugins/event.swipe/jquery.event.move.js",
                "~/plugins/event.swipe/jquery.event.swipe.js",
                "~/Content/assets/js/libs/breakpoints.js",
                "~/plugins/respond/respond.min.js",
                "~/plugins/slimscroll/jquery.slimscroll.min.js",
                "~/plugins/slimscroll/jquery.slimscroll.horizontal.min.js",
                "~/plugins/sparkline/jquery.sparkline.min.js",
                "~/plugins/daterangepicker/moment.min.js",

                "~/plugins/daterangepicker/daterangepicker.js",

                "~/plugins/uniform/jquery.uniform.min.js",
                "~/plugins/select2/select2.min.js",
                "~/plugins/datatables/jquery.dataTables.min.js",
                "~/plugins/datatables/tabletools/TableTools.min.js",
                "~/plugins/datatables/colvis/ColVis.min.js",
                "~/plugins/datatables/columnfilter/jquery.dataTables.columnFilter.js",
                "~/plugins/datatables/DT_bootstrap.js",
                "~/Content/assets/js/app.js",
                "~/Content/assets/js/plugins.js",
                "~/Content/assets/js/plugins.form-components.js",
                "~/Content/assets/js/custom.js",
                "~/Content/Timepicker/jquery.timepicker.min.js"
                ));


            bundles.Add(new ScriptBundle("~/bundles/Master").Include("~/Scripts/Master.js"));

        }
    }
}
