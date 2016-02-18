using ProcCore.Business;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Web.Http;
using Work.WebApp.Models;

namespace DotWeb.Api
{
    public class TempleAccountPrintController : ajaxApi<TempleAccount, q_TempleAccount>
    {
        public rAjaxGetData<TempleAccount> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.TempleAccount.Find(id);
                r = new rAjaxGetData<TempleAccount>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_Orders_Detail> Get([FromUri]q_TempleAccount q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Orders_Detail
                             join y in db0.Member_Detail
                             on x.member_detail_id equals y.member_detail_id
                             orderby y.member_name
                             where x.product_sn == e_祈福產品.香油_契子觀摩
                             select new m_Orders_Detail()
                             {
                                 orders_detail_id = x.orders_detail_id,
                                 member_detail_id = x.member_detail_id,
                                 orders_sn = x.orders_sn,//訂單編號
                                 member_name = y.member_name,//會員姓名
                                 price = x.price,//產品價格
                                 product_sn = x.product_sn,//產品編號
                                 l_birthday = y.lbirthday,//生日
                                 i_InsertDateTime = x.i_InsertDateTime,
                                 memo=y.Memo,
                                 sno=y.sno,
                                 insure_birthday=y.insure_birthday
                             });
                if (q.product_sn != null)
                    items = items.Where(x => x.product_sn == q.product_sn);

                if (q.startDate != null && q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.i_InsertDateTime >= start && (DateTime)x.i_InsertDateTime < end);
                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                foreach (var i in resultItems)
                {
                    //i.l_birthday = getTaiwanCalendarDate(DateTime.Parse(i.l_birthday));
                    i.l_insertDateTime = getTaiwanCalendarDate(i.i_InsertDateTime);
                }


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
        public ResultInfo Put([FromBody]TempleAccount md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.TempleAccount.Find(md.temple_account_id);
                item.price = md.price;
                item.product_sn = md.product_sn;
                item.product_name = db0.Product.Where(x => x.product_sn == md.product_sn).SingleOrDefault().product_name;

                item.i_UpdateDateTime = DateTime.Now;
                item.i_UpdateUserID = this.UserId;
                item.i_InsertUserName = this.UserName;

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
        public ResultInfo Post([FromBody]TempleAccount md)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            md.i_InsertDateTime = DateTime.Now;
            md.i_InsertUserID = this.UserId;
            md.i_InsertUserName = this.UserName;


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

                #region 取得最後一次交易紀錄
                var temple_member = db0.TempleMember.Find(md.temple_member_id);
                if (md.product_sn == e_祈福產品.契子會_大會 || md.product_sn == e_祈福產品.香油_契子觀摩)
                {
                    if (temple_member.last_attend_datetime == null || temple_member.last_attend_datetime < md.tran_date)
                    {
                        temple_member.last_attend_datetime = md.tran_date;
                    }
                }
                #endregion

                md.product_name = db0.Product.Where(x => x.product_sn == md.product_sn).SingleOrDefault().product_name;//取得產品名稱

                md.temple_account_id = GetNewId(CodeTable.TempleAccount);
                db0.TempleAccount.Add(md);
                db0.SaveChanges();

                rAjaxResult.result = true;
                rAjaxResult.id = md.temple_account_id;
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
                    item = new TempleAccount { temple_account_id = id };
                    db0.TempleAccount.Attach(item);
                    db0.TempleAccount.Remove(item);
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
