using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using System.Threading.Tasks;
using ProcCore.Business.DB0;
using ProcCore.Business;
using DotWeb.WebApp;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class OrdersController : BaseController, IGetMasterNewId, IGetDetailNewId
    {
        public ActionResult Main()
        {
            ActionRun();
            return View(new c_Orders());
        }
        [HttpPost]
        public string aj_Init()
        {
            return defJSON(
            new
            {
                //options_Member_category = ngCodeToOption(CodeSheet.MemberCategory.MakeCodes())
            }
            );
        }
        public string ajax_GetMasterNewId()
        {
            return defJSON(GetNewId(CodeTable.Orders));
        }
        public string ajax_GetDetailNewId()
        {
            return defJSON(GetNewId(CodeTable.Orders_Detail));
        }
    }
}
