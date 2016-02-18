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
    public class TempleAccountController : ajaxApi<TempleAccount, q_TempleAccount>
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
        public GridInfo2<m_TempleAccount> Get([FromUri]q_TempleAccount q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.TempleAccount
                             join y in db0.TempleMember
                             on x.temple_member_id equals y.temple_member_id
                             orderby x.i_InsertDateTime descending
                             select new m_TempleAccount()
                             {
                                 temple_account_id = x.temple_account_id,
                                 temple_member_id=x.temple_member_id,
                                 price = x.price,
                                 tran_date=x.tran_date,
                                 product_sn = x.product_sn,
                                 member_name = y.member_name,
                                 birthday = y.birthday,
                                 tel = y.tel,
                                 mobile = y.mobile,
                                 member_insertDateTime = y.i_insertDateTime,
                                 i_InsertDateTime=x.i_InsertDateTime,
                                 i_InsertUserName=x.i_InsertUserName,
                                 i_InsertUserID=x.i_InsertUserID
                             });

                //權限控管 只能看自己的訂單
                if (this.UnitId != 1 && this.UnitId != 50000032)
                {
                    items = items.Where(x => x.i_InsertUserID == this.UserId);
                }
                if (q.InsertUserId > 0)
                    items = items.Where(x => x.i_InsertUserID == q.InsertUserId);

                if (q.account_sn != null)
                    items = items.Where(x => x.temple_account_id == q.account_sn);//契子訂單編號查詢

                if (q.product_sn != null)
                    items = items.Where(x => x.product_sn == q.product_sn);

                if (q.member_name != null)
                    items = items.Where(x => x.member_name.Contains(q.member_name));

                if (q.startDate != null && q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.tran_date >= start && (DateTime)x.tran_date < end);
                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize);

                if (q.temple_member_id != null)
                {
                    resultItems = resultItems.Where(x => x.temple_member_id == q.temple_member_id);
                }

                return (new GridInfo2<m_TempleAccount>()
                {
                    rows = resultItems.ToList(),
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
                item.i_UpdateUserName = this.UserName;

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
