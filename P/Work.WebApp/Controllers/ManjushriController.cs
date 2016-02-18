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
    public class ManjushriController : BaseController
    {
        [HttpGet]
        public string GetManjushri()
        {
            rAjaxGetItems<Manjushri> r = new rAjaxGetItems<Manjushri>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getAllManjushri = db.Manjushri.OrderBy(x => x.manjushri_id).ToList();

                r.data = getAllManjushri;
                r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
