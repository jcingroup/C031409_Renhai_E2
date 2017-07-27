using ProcCore.Business;
using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Web.Http;
using Work.WebApp.Models;

namespace DotWeb.Api
{

    public class AssemblyBatchController : ajaxApi<AssemblyBatch, q_AssemblyBatch>
    {
        public rAjaxGetData<AssemblyBatch> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.AssemblyBatch.Find(id);
                r = new rAjaxGetData<AssemblyBatch>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_AssemblyBatch> Get([FromUri]q_AssemblyBatch q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                string[] allowedSN = new string[] { 
                    ProcCore.Business.Logic.e_祈福產品.超渡法會_祖先甲,
                    ProcCore.Business.Logic.e_祈福產品.超渡法會_祖先乙, 
                    ProcCore.Business.Logic.e_祈福產品.超渡法會_冤親債主,
                    ProcCore.Business.Logic.e_祈福產品.超渡法會_嬰靈 };
                var items = (from x in db0.AssemblyBatch
                             orderby x.batch_date, x.batch_timeperiod
                             select new m_AssemblyBatch()
                             {
                                 batch_sn = x.batch_sn,
                                 batch_title = x.batch_title,
                                 batch_date = x.batch_date,
                                 batch_timeperiod = x.batch_timeperiod,
                                 lunar_y = x.lunar_y,
                                 lunar_m = x.lunar_m,
                                 lunar_d = x.lunar_d,
                                 batch_qty = x.batch_qty,
                                 count = x.訂單明細檔.Where(y => y.is_reject != true & allowedSN.Contains(y.product_sn)).Count(),
                                 count_1401 = x.訂單明細檔.Where(y => y.is_reject != true & y.product_sn == ProcCore.Business.Logic.e_祈福產品.超渡法會_祖先甲).Count(),
                                 count_1402 = x.訂單明細檔.Where(y => y.is_reject != true & y.product_sn == ProcCore.Business.Logic.e_祈福產品.超渡法會_祖先乙).Count(),
                                 count_1403 = x.訂單明細檔.Where(y => y.is_reject != true & y.product_sn == ProcCore.Business.Logic.e_祈福產品.超渡法會_冤親債主).Count(),
                                 count_1404 = x.訂單明細檔.Where(y => y.is_reject != true & y.product_sn == ProcCore.Business.Logic.e_祈福產品.超渡法會_嬰靈).Count()
                             });

                if (q.year != null)
                {
                    items = items.Where(x => x.batch_date.Year == q.year);
                }

                //if (q.startDate != null && q.endDate != null)
                //{
                //    DateTime start = ((DateTime)q.startDate);
                //    DateTime end = ((DateTime)q.endDate).AddDays(1);
                //    items = items.Where(x => (DateTime)x.batch_date >= start && (DateTime)x.batch_date < end);
                //}

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_AssemblyBatch>()
                {
                    rows = resultItems,
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                });
            }
            #endregion
        }
        public ResultInfo Put([FromBody]AssemblyBatch md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();
                item = db0.AssemblyBatch.Find(md.batch_sn);

                item.time_sn = md.batch_date.ToString("yyyyMMdd") + md.batch_timeperiod;
                bool check_time = db0.AssemblyBatch.Any(x => x.time_sn == item.time_sn & x.batch_sn != md.batch_sn);
                if (check_time)
                {
                    rAjaxResult.message = "法會時間(國立)及時段不可重複!!";
                    rAjaxResult.result = false;
                    return rAjaxResult;
                }

                item.batch_title = md.batch_title;
                item.batch_date = md.batch_date.Date;
                item.batch_timeperiod = md.batch_timeperiod;
                item.lunar_y = md.lunar_y;
                item.lunar_m = md.lunar_m;
                item.lunar_d = md.lunar_d;
                item.batch_qty = md.batch_qty;

                db0.SaveChanges();
                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.ToString();
            }
            finally
            {
                db0.Dispose();
            }
            return rAjaxResult;
        }
        public ResultInfo Post([FromBody]AssemblyBatch md)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            if (!ModelState.IsValid)
            {
                rAjaxResult.message = ModelStateErrorPack();
                rAjaxResult.result = false;
                return rAjaxResult;
            }

            try
            {
                #region working a
                db0 = getDB0();
                //md.temple_member_id = GetNewId(CodeTable.TempleMember);

                md.batch_date = md.batch_date.Date;
                md.time_sn = md.batch_date.ToString("yyyyMMdd") + md.batch_timeperiod;
                bool check_time = db0.AssemblyBatch.Any(x => x.time_sn == md.time_sn);
                if (check_time)
                {
                    rAjaxResult.message = "法會時間(國立)及時段不可重複!!";
                    rAjaxResult.result = false;
                    return rAjaxResult;
                }


                db0.AssemblyBatch.Add(md);
                db0.SaveChanges();

                rAjaxResult.result = true;
                rAjaxResult.id = md.batch_sn;
                return rAjaxResult;
                #endregion
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return rAjaxResult;
            }
            finally
            {
                db0.Dispose();
            }
        }
        public ResultInfo Delete([FromUri]int id)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();
                bool exist = db0.Orders_Detail.Any(x => x.assembly_batch_sn == id);
                if (exist)
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "已有交易紀錄無法刪除!!";
                    return rAjaxResult;
                }

                var item = db0.AssemblyBatch.Find(id);

                db0.AssemblyBatch.Remove(item);


                db0.SaveChanges();

                rAjaxResult.result = true;
                return rAjaxResult;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return rAjaxResult;
            }
            finally
            {
                db0.Dispose();
            }
        }
    }
}
