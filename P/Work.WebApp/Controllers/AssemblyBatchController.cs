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
            ViewBag.ID = this.UserId;
            return View();
        }

        [HttpGet]
        public string GetAssemblyBatch(int? year)
        {
            rAjaxGetItems<AssemblyBatch> r = new rAjaxGetItems<AssemblyBatch>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                int y = year == null ? this.LightYear : (int)year;
                var getAssemblyBatch = db.AssemblyBatch.Where(x => x.batch_date.Year == y)
                                         .OrderBy(x => x.batch_date).ToList();

                r.data = getAssemblyBatch;
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
        #region (2017/7/13)祈福法會
        [HttpGet]
        public string CopyLastYear()
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                #region working a
                int year = DotWeb.CommSetup.CommWebSetup.WorkYear;
                int lastyear = year - 1;
                List<AssemblyBatch> mds = new List<AssemblyBatch>();
                if (!db0.AssemblyBatch.Any(x => x.batch_date.Year == year))
                {

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
                    tx.Complete();
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
                tx.Dispose();
                db0.Dispose();
            }
        }

        [HttpGet]
        public string addPlace()
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                string[] allowedSN = new string[] { e_祈福產品.超渡法會_祖先甲, e_祈福產品.超渡法會_祖先乙, e_祈福產品.超渡法會_冤親債主, e_祈福產品.超渡法會_嬰靈 };
                int year = DotWeb.CommSetup.CommWebSetup.WorkYear;

                if (this.UserId == 1000001 & !db0.Light_Site.Any(x => x.Y == year & allowedSN.Contains(x.product_sn)))
                {
                    #region working a
                    List<Light_Site> mds = new List<Light_Site>();

                    for (var i = 1; i <= 1000; i++)
                    {
                        string num = i.ToString().PadLeft(4, '0');
                        Light_Site md01 = new Light_Site()
                        {
                            light_name = "祖先甲" + num,
                            Y = year,
                            is_sellout = "0",
                            product_sn = e_祈福產品.超渡法會_祖先甲,
                            price = 1200,
                            C_Hiden = false,
                            C_LockState = false,
                            IsReject = false
                        };
                        Light_Site md02 = new Light_Site()
                        {
                            light_name = "祖先乙" + num,
                            Y = year,
                            is_sellout = "0",
                            product_sn = e_祈福產品.超渡法會_祖先乙,
                            price = 1200,
                            C_Hiden = false,
                            C_LockState = false,
                            IsReject = false
                        };
                        Light_Site md03 = new Light_Site()
                        {
                            light_name = "冤親債主" + num,
                            Y = year,
                            is_sellout = "0",
                            product_sn = e_祈福產品.超渡法會_冤親債主,
                            price = 1200,
                            C_Hiden = false,
                            C_LockState = false,
                            IsReject = false
                        };
                        Light_Site md04 = new Light_Site()
                        {
                            light_name = "嬰靈" + num,
                            Y = year,
                            is_sellout = "0",
                            product_sn = e_祈福產品.超渡法會_嬰靈,
                            price = 1200,
                            C_Hiden = false,
                            C_LockState = false,
                            IsReject = false
                        };

                        mds.Add(md01);
                        mds.Add(md02);
                        mds.Add(md03);
                        mds.Add(md04);
                    }
                    db0.Light_Site.AddRange(mds);
                    db0.SaveChanges();
                    tx.Complete();
                    #endregion
                }
                else
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "此權限無法新增法會燈位!";
                }

                return defJSON(rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return defJSON(rAjaxResult);
            }
            finally
            {
                tx.Dispose();
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
