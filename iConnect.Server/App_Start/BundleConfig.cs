using System.Web.Optimization;

namespace iConnect.Server
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery").Include(
                        "~/Scripts/jquery-{version}.js",
                        "~/Scripts/jquery.signalR-2.1.1.js",
                         "~/Scripts/shared/global.js",
                         "~/Scripts/amplify.js"
                       
                        ));

            bundles.Add(new ScriptBundle("~/bundles/chathub").Include(
                        "~/Scripts/app/chat.js",
                        "~/Scripts/noty/packaged/jquery.noty.packaged.js",
                        "~/Scripts/shared/emoticonParser.js"
                        ));

            bundles.Add(new ScriptBundle("~/bundles/jqueryval").Include(
                        "~/Scripts/jquery.unobtrusive*",
                        "~/Scripts/jquery.validate*"));

            bundles.Add(new ScriptBundle("~/bundles/bootstrap").Include("~/Scripts/bootstrap.js"));

            
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include(
                        "~/Scripts/modernizr-*"));

            bundles.Add(new StyleBundle("~/Content/css").Include(
                "~/Content/bootstrap.css",
                "~/Content/site.css"));
          
        }
    }
}