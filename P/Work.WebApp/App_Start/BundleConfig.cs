using System.Web;
using System.Web.Optimization;

namespace Work.WebApp
{
    public class BundleConfig
    {
        // 如需 Bundling 的詳細資訊，請造訪 http://go.microsoft.com/fwlink/?LinkId=254725
        public static void RegisterBundles(BundleCollection bundles)
        {
            string[] commFile = new string[] {
                "~/ScriptsCtrl/dynScript/defData.js",
                "~/Scripts/jquery/jquery-2.1.1.js",
                "~/Scripts/typeahead.bundle.js",
                "~/Scripts/jquery/jquery.cookie.js",
                "~/Scripts/angularjs/angular.js",
                "~/Scripts/angularjs/angular-animate.js",
                "~/Scripts/angular-ui-router.js",
                "~/Scripts/angular-typeahead.js",
                "~/Scripts/ngDialog.js",
                "~/Scripts/angularjs/i18n/angular-locale_zh-tw.js",
                "~/ScriptsCtrl/commfunc.js",
                "~/Scripts/BrowerInfo.js",
            };

            bundles.Add(new ScriptBundle("~/orderCtr")
                .Include(commFile)
                .Include("~/Scripts/angularjs/angular-cookies.js")
                .Include("~/Scripts/angularjs/ng-sortable.js")
                .Include("~/ScriptsCtrl/orderCtr.js")
                );

            bundles.Add(new ScriptBundle("~/orderfortuneCtr")
                .Include(commFile)
                .Include("~/ScriptsCtrl/orderfortuneCtr.js")
                );

            bundles.Add(new ScriptBundle("~/membermarkCtr")
                .Include(commFile)
                .Include("~/ScriptsCtrl/membermarkCtr.js")
                );
            bundles.Add(new ScriptBundle("~/FortuneLightLabelCtr")
               .Include(commFile)
               .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
               .Include("~/ScriptsCtrl/FortuneLightLabelCtr.js")
               );
            bundles.Add(new ScriptBundle("~/RejectReportCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/RejectReportCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_memberCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templememberCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_accountCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templeaccountCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_memberprintCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templememberprintCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_memberprint_allCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templememberprintallCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_postprintCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templepostprintCtr.js")
                );
            bundles.Add(new ScriptBundle("~/temple_markcloseCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/templemarkcloseCtr.js")
                );
            bundles.Add(new ScriptBundle("~/memberlabelCtr")
                .Include(commFile)
                .Include("~/Scripts/ui-bootstrap-tpls-0.12.0.js")
                .Include("~/ScriptsCtrl/memberlabelCtr.js")
                );

            bundles.Add(new ScriptBundle("~/testCtr")
                .Include(commFile)
                .Include("~/Scripts/angularjs/angular-cookies.js")
                .Include("~/Scripts/angularjs/ng-sortable.js")
                );

            BundleTable.EnableOptimizations = false;
        }
    }
}