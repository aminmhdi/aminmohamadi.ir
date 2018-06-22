using System.Collections.Generic;
using System.Web.Optimization;

namespace MyWeb
{
  public class BundleConfig
  {
    // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
    public static void RegisterBundles(BundleCollection bundles)
    {

      bundles.Add(new StyleBundle("~/Content/Css/bootstrap").Include(
        "~/Content/Css/bootstrap.css",
        "~/Content/Css/bootstrap-rtl.min.css",
        "~/Content/Css/bootstrap-reset.css"
        ));

      bundles.Add(new StyleBundle("~/Content/FontAwesome").Include(
        "~/Content/font-awesome.min.css"
      ));

      bundles.Add(new StyleBundle("~/Content/Main").Include(
        "~/Content/Main/Theme.css",
        "~/Content/Main/Responsive.css"
      ));

      bundles.Add(new ScriptBundle("~/Scripts/jquery").Include(
        "~/Scripts/jquery-1.10.2.js"
      ));

      bundles.Add(new ScriptBundle("~/Scripts/Main").Include(
        "~/Scripts/bootstrap.min.js",
        "~/Scripts/jquery.unobtrusive-ajax.min.js",
        "~/Scripts/scripts.js"
      ));

      bundles.Add(new StyleBundle("~/content/datePicker").Include(
        "~/Content/bootstrap-datepicker.min.css"
      ));

      bundles.Add(new StyleBundle("~/content/MDdatePicker").Include(
        "~/Content/MdBootstrapPersianDateTimePicker/jquery.Bootstrap-PersianDateTimePicker.css"
      ));

      bundles.Add(new StyleBundle("~/Content/MDdatePicker").Include(
        //"~/Content/bootstrap-theme.min.css",
        "~/Content/jquery.Bootstrap-PersianDateTimePicker.css"));


      bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-*",
                        "~/Scripts/jquery.unobtrusive-ajax.min.js"));

      var jqueryVal = new ScriptBundle("~/bundles/jqueryval")
          .Include("~/Scripts/jqueryval-default.min.js")
          .Include("~/Scripts/jquery.validate*");

      jqueryVal.Orderer = new NonOrderingBundleOrderer();
      bundles.Add(jqueryVal);

      bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                  "~/Scripts/modernizr-*"
                 ));

      bundles.Add(new ScriptBundle("~/bundles/datePicker").Include(
          "~/Scripts/bootstrap-datepicker.min.js",
          "~/Scripts/bootstrap-datepicker.fa.min.js"
          ));

      bundles.Add(new ScriptBundle("~/bundles/MDdatePicker").Include(
          "~/Scripts/MdBootstrapPersianDateTimePicker/jquery.Bootstrap-PersianDateTimePicker.js"
          ));

      bundles.Add(new ScriptBundle("~/bundles/MDdatePicker").Include(
          "~/Scripts/jalaali.js",
          "~/Scripts/jquery.Bootstrap-PersianDateTimePicker.js"
          ));

      BundleTable.EnableOptimizations = true;

    }

  }
  class NonOrderingBundleOrderer : IBundleOrderer
  {
    public IEnumerable<BundleFile> OrderFiles(BundleContext context, IEnumerable<BundleFile> files)
    {
      return files;
    }
  }
}
