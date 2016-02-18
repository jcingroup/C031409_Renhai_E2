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
    public class OrdersController : ajaxApi<Orders, q_Orders>
    {
        public rAjaxGetData<Orders> Get(string orders_sn)
        {
            using (db0 = getDB0())
            {
                item = db0.Orders.Find(orders_sn);
                item.getOrders_Detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == orders_sn)
                    .OrderBy(x => x.orders_detail_id).ToList();
                r = new rAjaxGetData<Orders>() { data = item, result = true };
            }

            return r;
        }
        public GridInfo2<m_Orders> Get([FromUri]q_Orders q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Orders
                             orderby x.orders_sn
                             select new m_Orders()
                             {
                                 orders_sn = x.orders_sn,
                                 member_name = x.member_name,
                                 tel = x.tel,
                                 total = x.total,
                                 transation_date = x.transation_date,
                                 InsertUserName = x.Users.users_name,
                                 InsertUserId = x.InsertUserId,
                                 orders_type = x.orders_type,
                                 orders_state = x.orders_state
                             }).OrderByDescending(x => x.transation_date).AsQueryable();

                //權限控管 只能看自己的訂單
                if (this.UnitId != 1 && this.UnitId != 50000032) {
                    items = items.Where(x => x.InsertUserId == this.UserId);
                }

                if (q.InsertUserId > 0)
                    items = items.Where(x => x.InsertUserId == q.InsertUserId);

                if (q.order_sn != null)
                    items = items.Where(x => x.orders_sn == q.order_sn);

                if (q.member_name != null)
                    items = items.Where(x => x.member_name.Contains(q.member_name));

                if (q.order_type > 0)
                    items = items.Where(x => x.orders_type==q.order_type);
                
                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_Orders>()
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
        public ResultInfo Put([FromBody]Orders md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.Orders.Find(md.orders_id);
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
        public ResultInfo Post([FromBody]Orders md)
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

                db0.Orders.Add(md);
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
                    item = new Orders { orders_id = id };
                    db0.Orders.Attach(item);
                    db0.Orders.Remove(item);
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
    public class q_Orders : QueryBase
    {
        public string order_sn { get; set; }
        public int InsertUserId { get; set; }
        public string member_name { get; set; }
        public int order_type { get; set; }
       
    }
}
