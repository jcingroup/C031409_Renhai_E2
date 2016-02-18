using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;
using System.Web.Http;

namespace DotWeb.Api
{
    public class OrdersController : ajaxApi<Orders, q_Orders>
    {
        public async Task<IHttpActionResult> Get(string sn)
        {
            using (db0 = getDB0())
            {
                item = await db0.Orders.FindAsync(sn);
                r = new rAjaxGetData<Orders>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Orders q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Orders
                             orderby x.orders_sn
                             where x.transation_date.Year > 2012
                             select new m_Orders()
                             {
                                 orders_sn = x.orders_sn,
                                 member_name = x.member_name,
                                 transation_date = x.transation_date,
                                 total = x.total,
                                 tel =x.tel
                             });

                //if (q.name != null)
                //    items = items.Where(x => x.householder.Contains(q.key));

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo2<m_Orders>()
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
        public async Task<IHttpActionResult> Put([FromBody]Orders md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Orders.FindAsync(md.orders_id);


                item.gender = md.gender;
                item.email = md.email;

                item.tel = md.tel;
                item.mobile = md.mobile;
                item.zip = md.zip;
                item.address = md.address;


                item.i_UpdateUserID = aspUserId;
                item.i_UpdateDeptID = departmentId;
                item.i_UpdateDateTime = DateTime.Now;
                item.i_Lang = getNowLnag();

                await db0.SaveChangesAsync();
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
            return Ok(rAjaxResult);
        }
        public async Task<IHttpActionResult> Post([FromBody]Orders md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            //md.i_Lang = "";
            if (!ModelState.IsValid)
            {
                rAjaxResult.message = ModelStateErrorPack();
                rAjaxResult.result = false;
                return Ok(rAjaxResult);
            }

            using (TransactionScope tx = defAsyncScope())
            {
                try
                {
                    #region working a
                    db0 = getDB0();

                    md.i_InsertUserID = aspUserId;
                    md.i_InsertDeptID = departmentId;
                    md.i_InsertDateTime = DateTime.Now;
                    md.i_Lang = getNowLnag();
                    db0.Orders.Add(md);
                    await db0.SaveChangesAsync();
                    tx.Complete();

                    rAjaxResult.result = true;
                    rAjaxResult.id = md.orders_id;
                    return Ok(rAjaxResult);
                    #endregion
                }
                catch (Exception ex)
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = ex.Message;
                    return Ok(rAjaxResult);
                }
                finally
                {
                    db0.Dispose();
                }
            }
        }
        public async Task<IHttpActionResult> Delete(int[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new Orders() { orders_id = id };
                    db0.Orders.Attach(item);
                    db0.Orders.Remove(item);
                }

                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                return Ok(rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return Ok(rAjaxResult);
            }
            finally
            {
                db0.Dispose();
            }
        }


    }
}
