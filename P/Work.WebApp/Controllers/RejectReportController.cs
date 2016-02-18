using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using System;
using System.Linq;
using System.Web.Mvc;
using Work.WebApp.Models;
using ProcCore.NetExtension;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProcCore.Business;
using DotWeb.CommSetup;
using ProcCore;

namespace DotWeb.Controllers
{
    public class RejectReportController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
