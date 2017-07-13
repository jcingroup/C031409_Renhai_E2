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
    public class AssemblyBatchController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        #region (2017/7/13)祈福法會
        [HttpGet]
        public string CopyLastYear()
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var db0 = getDB();
            //var tx = defAsyncScope();
            try
            {
                #region working a
                int year = DateTime.Now.Year;
                List<AssemblyBatch> mds = new List<AssemblyBatch>();
                if (!db0.AssemblyBatch.Any(x => x.batch_date.Year == year))
                {
                    int lastyear = year - 1;
                    var list = db0.AssemblyBatch.Where(x => x.batch_date.Year == lastyear).ToList();

                    foreach (var i in list)
                    {

                        var md = new AssemblyBatch()
                        {
                            batch_title = i.batch_title,
                            batch_date = i.batch_date.AddYears(1),
                            lunar_y = (year - 1911).ToString(),
                            lunar_m = i.lunar_m,
                            lunar_d = i.lunar_d,
                            batch_qty = i.batch_qty
                        };

                        mds.Add(md);
                    }

                    db0.AssemblyBatch.AddRange(mds);
                    db0.SaveChanges();
                    rAjaxResult.result = true;
                }
                else
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "今年度已有資料無法複製!";
                }

                return defJSON(rAjaxResult);
                #endregion
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return defJSON(rAjaxResult);
            }
            finally
            {
                //tx.Dispose();
                db0.Dispose();
            }
        }
        #endregion
        public string aj_init()
        {
            return defJSON(new { });
        }
    }
}
