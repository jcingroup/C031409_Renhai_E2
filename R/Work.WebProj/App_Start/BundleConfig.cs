using System.Collections.Generic;
using System.Web;
using System.Web.Optimization;

namespace DotWeb.AppStart
{
    public class BundleConfig
    {
        public static void RegisterBundles(BundleCollection bundles)
        {
            #region JScript
            
            string[] commFile = new string[] {
                "~/Scripts/BrowerInfo.js",
                "~/Scripts/dynScript/defData.js",
                "~/Scripts/jquery/jquery-2.1.1.js",
                "~/Scripts/angular/angular.js",
                "~/Scripts/angular/angular-animate.js",
                "~/Scripts/angular/angular-route.js",
                "~/Scripts/angular/i18n/angular-locale_zh-tw.js",
                "~/Scripts/angular-plugging/signalr-hub.js",
                "~/Scripts/angular-plugging/toaster.js",
                "~/Scripts/angular-plugging/ui-bootstrap-tpls-0.11.0.js",
                "~/_Code/Scripts/commfunc.js"
            };
            //bundles.Add(new ScriptBundle("~/commJS").Include(CommFile));

            List<string> lis_handlejs = new List<string>();
            lis_handlejs.AddRange(commFile);
            lis_handlejs.Add("~/ScriptsCtrl/login.js");
            bundles.Add(new ScriptBundle("~/loginController").Include(lis_handlejs.ToArray()));
            lis_handlejs.Clear();

            lis_handlejs.AddRange(commFile);
            lis_handlejs.Add("~/ScriptsCtrl/mberController.js");
            bundles.Add(new ScriptBundle("~/mberController").Include(lis_handlejs.ToArray()));
            lis_handlejs.Clear();


            lis_handlejs.AddRange(commFile);
            lis_handlejs.Add("~/ScriptsCtrl/ordersController.js");
            bundles.Add(new ScriptBundle("~/ordersController").Include(lis_handlejs.ToArray()));
            lis_handlejs.Clear();

            lis_handlejs.AddRange(commFile);
            lis_handlejs.Add("~/ScriptsCtrl/ordersdetailController.js");
            bundles.Add(new ScriptBundle("~/ordersdetailController").Include(lis_handlejs.ToArray()));
            lis_handlejs.Clear();


            bundles.Add(new ScriptBundle("~/upfile")
            .Include(
            "~/Scripts/angular-plugging/angular-file-upload/angular-file-upload-html5-shim.js",
            "~/Scripts/angular-plugging/angular-file-upload/angular-file-upload.js",
            "~/Scripts/shadowbox/shadowbox.js",
            "~/_Code/Scripts/commupload.js"
            ));
            #endregion

            #region CSS
            bundles.Add(new StyleBundle("~/_Code/Style")
            .Include(
            "~/_Code/CSS/css/sys_page.css",
            "~/_Code/CSS/toaster.css"
            ));
            #endregion

            BundleTable.EnableOptimizations = true;
        }
    }
}
