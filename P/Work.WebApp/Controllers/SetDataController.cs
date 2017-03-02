using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using System.Web.Script.Serialization;

using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using Work.WebApp.Models;

namespace DotWeb.Controllers
{
    public class SetDataController : BaseController
    {
        public ActionResult Product()
        {//產品新增
            return View();
        }
        public ActionResult SwapLight()
        {//燈位交換
            return View();
        }
        public ActionResult DelOrder()
        {//刪除訂單
            return View();
        }
        public string aj_init()
        {
            return defJSON(new { });
        }
    }
}
