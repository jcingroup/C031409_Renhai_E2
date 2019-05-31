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
        /// <summary>
        /// 取得年度批次
        /// </summary>
        /// <returns></returns>
        public ActionResult BatchCount()
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
                    //.OrderBy(x => new { x.batch_date, x.batch_timeperiod }).ToList();
                                         .OrderBy(x => x.batch_sn).ToList();

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
                        DateTime newdate = i.batch_date.AddYears(1);
                        var md = new AssemblyBatch()
                        {
                            batch_title = i.batch_title,
                            time_sn = newdate.ToString("yyyyMMdd") + i.batch_timeperiod,
                            batch_date = newdate.Date,
                            batch_timeperiod = i.batch_timeperiod,
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
        public string addPlace(int batch_sn)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                //string[] allowedSN = new string[] { e_祈福產品.超渡法會_祖先甲, e_祈福產品.超渡法會_祖先乙, e_祈福產品.超渡法會_冤親債主, e_祈福產品.超渡法會_嬰靈 };
                int year = DotWeb.CommSetup.CommWebSetup.WorkYear;

                //取得目前法會梯次資料
                var batchlist = db0.AssemblyBatch.Where(x => x.batch_date.Year == year)
                                                 .OrderBy(x => new { x.batch_date, x.batch_timeperiod }).ToList();
                var batchsIndex = batchlist.Select((x, i) => new BatchList()
                {
                    batch_sn = x.batch_sn,
                    index = i + 1,
                    batch_qty = x.batch_qty
                }).ToList();

                if (this.UserId == 1000001 & batchsIndex.Any(x => x.batch_sn == batch_sn))
                {
                    #region working a
                    List<Light_Site> mds = new List<Light_Site>();

                    var batch = batchsIndex.First(x => x.batch_sn == batch_sn);
                    int index = batch.index;

                    var check_1401 = db0.Light_Site.Any(x => x.Y == year & x.assembly_batch_sn == batch.batch_sn & x.product_sn == e_祈福產品.超渡法會_祖先甲);
                    var check_1402 = db0.Light_Site.Any(x => x.Y == year & x.assembly_batch_sn == batch.batch_sn & x.product_sn == e_祈福產品.超渡法會_祖先乙);
                    var check_1403 = db0.Light_Site.Any(x => x.Y == year & x.assembly_batch_sn == batch.batch_sn & x.product_sn == e_祈福產品.超渡法會_冤親債主);
                    var check_1404 = db0.Light_Site.Any(x => x.Y == year & x.assembly_batch_sn == batch.batch_sn & x.product_sn == e_祈福產品.超渡法會_嬰靈);

                    if (check_1401 & check_1402 & check_1403 & check_1404)
                    {
                        rAjaxResult.result = false;
                        rAjaxResult.message = "此法會梯次已有燈位!";
                    }
                    else
                    {
                        #region 加燈位
                        for (var i = 1; i <= batch.batch_qty; i++)
                        {
                            string num = i.ToString().PadLeft(4, '0');
                            Light_Site md01 = new Light_Site()
                            {
                                light_name = string.Format("{0}-祖先甲{1}", index, num),
                                Y = year,
                                is_sellout = "0",
                                product_sn = e_祈福產品.超渡法會_祖先甲,
                                price = 1200,
                                C_Hiden = false,
                                C_LockState = false,
                                IsReject = false,
                                assembly_batch_sn = batch.batch_sn
                            };
                            Light_Site md02 = new Light_Site()
                            {
                                light_name = string.Format("{0}-祖先乙{1}", index, num),
                                Y = year,
                                is_sellout = "0",
                                product_sn = e_祈福產品.超渡法會_祖先乙,
                                price = 1200,
                                C_Hiden = false,
                                C_LockState = false,
                                IsReject = false,
                                assembly_batch_sn = batch.batch_sn
                            };
                            Light_Site md03 = new Light_Site()
                            {
                                light_name = string.Format("{0}-冤親債主{1}", index, num),
                                Y = year,
                                is_sellout = "0",
                                product_sn = e_祈福產品.超渡法會_冤親債主,
                                price = 1200,
                                C_Hiden = false,
                                C_LockState = false,
                                IsReject = false,
                                assembly_batch_sn = batch.batch_sn
                            };
                            Light_Site md04 = new Light_Site()
                            {
                                light_name = string.Format("{0}-嬰靈{1}", index, num),
                                Y = year,
                                is_sellout = "0",
                                product_sn = e_祈福產品.超渡法會_嬰靈,
                                price = 1200,
                                C_Hiden = false,
                                C_LockState = false,
                                IsReject = false,
                                assembly_batch_sn = batch.batch_sn
                            };

                            if (!check_1401)
                                mds.Add(md01);
                            if (!check_1402)
                                mds.Add(md02);
                            if (!check_1403)
                                mds.Add(md03);
                            if (!check_1404)
                                mds.Add(md04);
                        }

                        db0.Light_Site.AddRange(mds);
                        db0.SaveChanges();
                        tx.Complete();
                        #endregion
                    }

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

    public class BatchList
    {
        public int batch_sn { get; set; }
        public int index { get; set; }
        public int batch_qty { get; set; }
    }
    public class ICar : Car
    {
        public int Index { get; set; }
    }
    public class Car
    {
        public string Color { get; set; }
        public int Price { get; set; }
    }

}
