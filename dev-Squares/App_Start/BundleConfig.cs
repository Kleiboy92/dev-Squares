using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Optimization;

namespace dev_Squares
{
    public class BundleConfig
    {
        // For more information on bundling, visit http://go.microsoft.com/fwlink/?LinkId=301862
        public static void RegisterBundles(BundleCollection bundles)
        {
            bundles.Add(new ScriptBundle("~/bundles/app")
               .Include("~/App/scope.js")
               .IncludeDirectory("~/App/services", "*.js")
               .IncludeDirectory("~/App/controllers", "*.js")
               );

            bundles.Add(new ScriptBundle("~/bundles/angular")
                .Include("~/Scripts/angular.js")
                .Include("~/Scripts/angular-resource.js")
                .Include("~/Scripts/angular-route.js")
                .Include("~/Scripts/angular-animate.js")
                .Include("~/Scripts/angular-sanitize.js")
                .Include("~/Scripts/ngDialog.js")
                .Include("~/Scripts/ui-bootstrap-tpls-2.5.0.js")
               );

            bundles.Add(new StyleBundle("~/Content/css")
               .IncludeDirectory("~/Content", "*.css")
               );

        }
    }
}
