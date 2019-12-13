using ProcCore.Business.Base;
using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Work.WebApp.Models;

namespace DotWeb.Api
{
    public class DouLightLabelController : ajaxApi<Orders_Detail, q_Order_Detail>
    {
        public rAjaxGetData<Orders_Detail> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.Orders_Detail.Find(id);
                r = new rAjaxGetData<Orders_Detail>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_Orders_Detail> Get([FromUri]q_Order_Detail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                List<string> psn = new List<string> { ProcCore.Business.Logic.e_祈福產品.沉香媽祖斗燈, ProcCore.Business.Logic.e_祈福產品.藥師佛斗燈 };//斗燈產品編號

                var items = (from x in db0.Orders_Detail
                             orderby x.orders_detail_id
                             select new m_Orders_Detail()
                             {
                                 orders_detail_id = x.orders_detail_id,
                                 orders_sn = x.orders_sn,
                                 product_sn = x.product_sn,
                                 product_name = x.product_name,
                                 price = x.price,
                                 member_name = x.member_name,
                                 light_name = x.light_name,
                                 C_InsertDateTime=x.C_InsertDateTime
                             }).Where(x => psn.Contains(x.product_sn));

                if (q.psn != null)
                    items = items.Where(x => x.product_sn == q.psn);

                if (q.startDate != null && q.endDate != null) 
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.C_InsertDateTime >= start && (DateTime)x.C_InsertDateTime < end);
                }


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_Orders_Detail>()
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
        public ResultInfo Put([FromBody]Orders_Detail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.Orders_Detail.Find(md.orders_detail_id);
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
        public ResultInfo Post([FromBody]Orders_Detail md)
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

                db0.Orders_Detail.Add(md);
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
                    item = new Orders_Detail { orders_detail_id = id };
                    db0.Orders_Detail.Attach(item);
                    db0.Orders_Detail.Remove(item);
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
