using System.Web;
using System.Web.Optimization;

namespace IORPromoteTool
{
    public class BundleConfig
    {
        // For more information on Bundling, visit http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/jquery")
                            .IncludeDirectory("~/Scripts/Lib/JQuery","*.js", true)
                            );

            bundles.Add(new ScriptBundle("~/bundles/bootstrap")
                            .IncludeDirectory("~/Scripts/Lib/Bootstrap", "*.js", true)
                            );

            bundles.Add(new ScriptBundle("~/bundles/d3")
                            .IncludeDirectory("~/Scripts/Lib/D3", "*.js", true)
                            );

            bundles.Add(new ScriptBundle("~/bundles/angular")
                            .Include("~/Scripts/Lib/Angular/angular.js")
                            .IncludeDirectory("~/Scripts/Lib/Angular", "*.js", true)
                            .IncludeDirectory("~/Scripts/Lib/AngularUI", "*.js", true)
                            .IncludeDirectory("~/Scripts/Lib/AngularChart", "*.js", true)
                            .IncludeDirectory("~/Scripts/Lib/GoogleChart", "*.js", true)
                            );

            bundles.Add(new ScriptBundle("~/bundles/angularScripts")
                            .IncludeDirectory("~/Scripts/App", "*.js", true)
                            .IncludeDirectory("~/Scripts/API", "*.js", true)
                            .IncludeDirectory("~/Scripts/Components", "*.js", true)
                            .IncludeDirectory("~/Scripts/Controllers", "*.js", true)
                            );

            bundles.Add(new StyleBundle("~/bundles/css")
                            .IncludeDirectory("~/Content/Bootstrap", "*.css", true)
                            .IncludeDirectory("~/Content/FontAwesome", "*.css", true)
                            .Include("~/Content/Site.css",  new CssRewriteUrlTransformWrapper())
                            .Include("~/Content/grid.css",  new CssRewriteUrlTransformWrapper())
                            );

            // Use the development version of Modernizr to develop with and learn from. Then, when you're
            // ready for production, use the build tool at http://modernizr.com to pick only the tests you need.
            bundles.Add(new ScriptBundle("~/bundles/modernizr").Include("~/Scripts/Lib/modernizr-*"));
        }
    }

    // Helen: To fix issue where the correct path is not being followed
    public class CssRewriteUrlTransformWrapper : IItemTransform
    {
        public string Process(string includedVirtualPath, string input)
        {
            return new CssRewriteUrlTransform().Process("~" + VirtualPathUtility.ToAbsolute(includedVirtualPath), input);
        }
    }
}