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
    public class MemberLabelController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
        public string aj_init()
        {
            return defJSON(new { });
        }
    }
}
