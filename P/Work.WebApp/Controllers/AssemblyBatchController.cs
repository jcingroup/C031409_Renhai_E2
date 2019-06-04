using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using System.Web.Script.Serialization;

using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using Work.WebApp.Models;
using System.Data.SqlClient;

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
                                                 .OrderBy(x => x.batch_sn).ToList();
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
        #region (2019.6.3) 更新梯次
        /// <summary>
        /// 更新梯次
        /// </summary>
        /// <param name="orders_sn">更新訂單編號</param>
        /// <param name="orders_detail_id">更新訂單明細編號</param>
        /// <param name="up_batch_sn">要更新的梯次名稱</param>
        /// <returns></returns>
        [HttpPost]
        public string chgBatch(UpBatchData p)
        {
            rAjaxGetData<object> r = new rAjaxGetData<object>();
            string orders_sn = p.orders_sn;
            int orders_detail_id = p.orders_detail_id;
            int up_batch_sn = p.up_batch_sn;
            int year = DotWeb.CommSetup.CommWebSetup.WorkYear;
            try
            {
                using (RenHai2012Entities db0 = getDB())
                {
                    using (var tx = defAsyncScope())
                    {
                        var main = db0.Orders.Where(x => x.orders_sn == orders_sn).FirstOrDefault();
                        var dtl = db0.Orders_Detail.Where(x => x.orders_sn == orders_sn & x.orders_detail_id == orders_detail_id).FirstOrDefault();
                        var batchlist = db0.AssemblyBatch.Where(x => x.batch_date.Year == year).OrderBy(x => x.batch_sn).ToList();
                        #region 驗證
                        if (dtl == null || main == null)
                        {
                            r.result = false;
                            r.message = "您搜尋的訂單已經不存,請重新整理再確認看看~!";
                            return defJSON(r);
                        }
                        if (!batchlist.Any(x => x.batch_sn == up_batch_sn))
                        {
                            r.result = false;
                            r.message = "您要更新的梯次非今年度法會梯次,無法更新!";
                            return defJSON(r);
                        }
                        if (dtl.assembly_batch_sn == up_batch_sn)
                        {
                            r.result = false;
                            r.message = "您要更新的梯次已經與原本梯次相同,無法更新!";
                            return defJSON(r);
                        }
                        #region 超渡法會梯次統計
                        var getBatchInfo = batchlist.FirstOrDefault(x => x.batch_sn == up_batch_sn);
                        int qty = db0.Orders_Detail.Where(x => x.is_reject != true & x.assembly_batch_sn == getBatchInfo.batch_sn & x.Product.category == ProcCore.Business.Logic.e_祈福產品分類.超渡法會).Count();
                        int EmptySerial = getBatchInfo.batch_qty - qty;//多餘空位
                        if (EmptySerial <= 0)
                        {
                            r.message = string.Format("{0}超渡法會梯次人數僅剩{1}位,目前選取人數已達上限,請選擇其他梯次!", getBatchInfo.batch_title, EmptySerial);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion
                        #endregion
                        string product_sn = dtl.product_sn;//產品編號
                        int old_light_sn = int.Parse(dtl.memo);//原始梯次位置
                        int? old_batch_sn = dtl.assembly_batch_sn;//原始梯次位置

                        //step1.取得舊的點燈位置
                        var old_light = db0.Light_Site.
                                Where(x => x.Y == this.LightYear &&
                                    x.product_sn == product_sn &&
                                    x.light_site_id == old_light_sn &&
                                    x.assembly_batch_sn == old_batch_sn)
                                    .FirstOrDefault();
                        old_light.is_sellout = "0";
                        old_light.C_UpdateDateTime = DateTime.Now;
                        old_light.C_UpdateUserID = this.UserId;
                        //step2.取得新的點燈位置
                        var new_light = db0.Light_Site.
                                  Where(x => x.Y == this.LightYear &&
                                      x.product_sn == product_sn &&
                                      x.is_sellout == "0" &&
                                      x.assembly_batch_sn == up_batch_sn)
                                      .OrderBy(x => x.light_site_id)
                                      .Take(1)
                                      .FirstOrDefault();

                        new_light.is_sellout = "1";
                        new_light.C_UpdateDateTime = DateTime.Now;
                        new_light.C_UpdateUserID = this.UserId;
                        //step3.更新Orders_Detail的梯次及點燈位置

                        dtl.assembly_batch_sn = up_batch_sn;//更新梯次編號
                        dtl.light_name = new_light.light_name;//更新燈位名稱
                        dtl.memo = new_light.light_site_id.ToString();//更新燈位編號

                        db0.SaveChanges();
                        tx.Complete();
                    }
                }
                //r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
        }
        #endregion
    }

    public class UpBatchData
    {
        public string orders_sn { get; set; }
        public int orders_detail_id { get; set; }
        public int up_batch_sn { get; set; }
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
