using ProcCore.Business.Base;
using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Work.WebApp.Models;

namespace DotWeb.Api
{
    public class RejectReportController : ajaxApi<Reject, q_Reject>
    {
        public rAjaxGetData<Reject> Get(int reject_id)
        {
            using (db0 = getDB0())
            {
                item = db0.Reject.Find(reject_id);
                item.getReject_Detail = db0.Reject_Detail
                    .Where(x => x.reject_id == reject_id)
                    .OrderBy(x => x.reject_detail_id).ToList();
                r = new rAjaxGetData<Reject>() { data = item, result = true };
            }

            return r;
        }
        public GridInfo2<RejectReport> Get([FromUri]q_Reject q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Reject
                             join y in db0.Reject_Detail on x.reject_id equals y.reject_id
                             join z in db0.Member_Detail on y.member_detail_id equals z.member_detail_id
                             join w in db0.Users on x.user_id equals w.users_id
                             orderby x.reject_date descending
                             select new RejectReport()
                             {
                                 orders_sn = x.orders_sn,
                                 total = x.total,
                                 reject_date = x.reject_date,
                                 InsertUserName = w.users_name,
                                 light_name = y.light_name,
                                 price = y.price,
                                 member_name=z.member_name
                             }).AsQueryable();

                //權限控管 只能看自己的訂單
                //if (this.UnitId != 1 && this.UnitId != 50000032) {
                //    items = items.Where(x => x.InsertUserId == this.UserId);
                //}


                if (q.order_sn != null)
                    items = items.Where(x => x.orders_sn == q.order_sn);

                if (q.member_name != null)
                    items = items.Where(x => x.member_name.Contains(q.member_name));

                if (q.startDate != null && q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.reject_date >= start && (DateTime)x.reject_date < end);
                }
                
                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<RejectReport>()
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
        public ResultInfo Put([FromBody]Reject md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.Reject.Find(md.reject_id);
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
        public ResultInfo Post([FromBody]Reject md)
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

                db0.Reject.Add(md);
                db0.SaveChanges();

                rAjaxResult.result = true;
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
        public ResultInfo Delete([FromUri]int[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new Reject { reject_id = id };
                    db0.Reject.Attach(item);
                    db0.Reject.Remove(item);
                }

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
