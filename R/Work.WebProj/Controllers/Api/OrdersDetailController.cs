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
    public class OrdersDetailController : ajaxApi<Orders_Detail, q_Orders_Detail>
    {
        public async Task<IHttpActionResult> Get(string id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Orders_Detail.FindAsync(id);
                r = new rAjaxGetData<Orders_Detail>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Orders_Detail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Orders_Detail
                             orderby x.orders_detail_id
                             select new m_Orders_Detail()
                             {
                                 orders_detail_id = x.orders_detail_id,
                                 product_sn = x.product_sn,
                                 product_name = x.product_name,
                                 price = x.price,
                                 member_detail_id = x.member_detail_id,
                                 orders_sn = x.orders_sn,
                                 member_name = x.member_name
                             });

                items = items.Where(x => x.orders_sn == q.orders_sn);
                var resultItems = items.ToList();

                return Ok(resultItems);
            }
            #endregion
        }
        public async Task<IHttpActionResult> Put([FromBody]Orders_Detail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Orders_Detail.FindAsync(md.orders_detail_id);


                item.gender = md.gender;

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
        public async Task<IHttpActionResult> Post([FromBody]Orders_Detail md)
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
                    db0.Orders_Detail.Add(md);
                    await db0.SaveChangesAsync();
                    tx.Complete();

                    rAjaxResult.result = true;
                    rAjaxResult.id = md.orders_detail_id;
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
                    item = new Orders_Detail() { orders_detail_id = id };
                    db0.Orders_Detail.Attach(item);
                    db0.Orders_Detail.Remove(item);
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
