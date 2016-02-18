using DotWeb.Helpers;
using DotWeb.WebApp;
using ProcCore.Business;
using ProcCore.Business.DB0;
using ProcCore.NetExtension;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Web.Mvc;
using System.Threading.Tasks;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class MberDataController : BaseController, IGetMasterNewId, IGetDetailNewId
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View(new c_Member());
        }
        #endregion

        #region ajax call section
        public string aj_Init()
        {
            return defJSON(
            new
            {
                //options_Member_category = ngCodeToOption(CodeSheet.MemberCategory.MakeCodes())
            }
            );
        }

        [HttpPost]
        public string ajax_GetMasterNewId()
        {
            return defJSON(GetNewId(CodeTable.Member));
        }

        #endregion

        public string ajax_GetDetailNewId()
        {
            return defJSON(GetNewId(CodeTable.Member_Detail));
        }
    }
}