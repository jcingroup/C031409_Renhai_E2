using ProcCore.Business;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http;
using Work.WebApp.Models;

namespace DotWeb.Api
{
    public class TempleMarkCloseController : ajaxApi<TempleMember, q_TempleMember>
    {
        public rAjaxGetData<TempleMember> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.TempleMember.Find(id);
                r = new rAjaxGetData<TempleMember>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_TempleMember> Get([FromUri]q_TempleMember q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                DateTime default_insertdate = DateTime.Parse("2004/1/1");// 預設93/1/1

                #region 先取得三年內有交易紀錄的會員(templeAccount)
                DateTime rang_year = DateTime.Now.AddYears(-3);
                List<int> haveTransactionMember = (from x in db0.TempleMember
                                                   join y in db0.TempleAccount
                                                   on x.temple_member_id equals y.temple_member_id
                                                   where y.tran_date > rang_year
                                                   & (y.product_sn == e_祈福產品.香油_契子觀摩 || y.product_sn == e_祈福產品.契子會_大會)
                                                   orderby x.temple_member_id descending
                                                   select x.temple_member_id).Distinct().ToList();
                #endregion

                #region 取得三年內有交易紀錄的會員(order_detail)
                //List<string> GodSN_psn = new List<string> { "752", "753" };//契子產品編號
                //List<int> Orders_detail = (from x in db0.Orders_Detail
                //                           where x.C_InsertDateTime > rang_year
                //                           where GodSN_psn.Contains(x.product_sn)
                //                           orderby x.member_detail_id
                //                           select x.member_detail_id).Distinct().ToList();
                #region groupby test
                //姓名沒有重複的資料
                //List<string> haveTransactionMember2 = (from x in db0.Member_Detail
                //                                       where Orders_detail.Contains(x.member_detail_id)
                //                                       group x by x.member_name into g
                //                                       orderby g.Key
                //                                       where g.Count() == 1
                //                                       select g.Key).ToList();

                //var haveTransactionMember2 = (from x in db0.Member_Detail
                //                                       where Orders_detail.Contains(x.member_detail_id)
                //                                       orderby x.member_detail_id
                //                                       select new{x.member_name,x.tel,x.mobile}).ToList();
                #endregion
                //List<string> haveTransactionMember2 = (from x in db0.Member_Detail
                //                                       where Orders_detail.Contains(x.member_detail_id)
                //                                       orderby x.member_detail_id
                //                                       select x.member_name).Distinct().ToList();

                #endregion
                var items = (from x in db0.TempleMember
                             orderby x.member_name
                             select new m_TempleMember()
                             {
                                 temple_member_id = x.temple_member_id,
                                 member_name = x.member_name,
                                 tel = x.tel,
                                 mobile = x.mobile,
                                 zip = x.zip,
                                 addr = x.addr,
                                 sno = x.sno,
                                 birthday = x.birthday,
                                 is_close = x.is_close,
                                 join_datetime = x.join_datetime == null ? default_insertdate : x.join_datetime,
                                 last_attend_datetime = x.last_attend_datetime
                             });

                #region 過濾名單
                items = items.Where(x => !haveTransactionMember.Contains(x.temple_member_id));
                //items = items.Where(x => !haveTransactionMember2.Contains(x.member_name)); 增加last_attend_datetime欄位後就不需驗證
                //items = items.Where(x => !haveTransactionMember2.Select(y=>y.member_name).Contains(x.member_name));

                items = items.Where(x => x.last_attend_datetime < rang_year || x.last_attend_datetime == null);
                #endregion

                if (q.member_name != null)
                {
                    items = items.Where(x => x.member_name.Contains(q.member_name));
                }

                if (q.tel != null)
                    items = items.Where(x => x.tel.Contains(q.tel));

                if (q.startDate != null && q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.i_insertDateTime >= start && (DateTime)x.i_insertDateTime < end);
                }

                if (q.is_close != null)
                {
                    if (q.is_close == "false")
                    {
                        items = items.Where(x => x.is_close == false);
                    }
                    else if (q.is_close == "true")
                    {
                        items = items.Where(x => x.is_close == true);
                    }

                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_TempleMember>()
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
        public ResultInfo Put([FromBody]TempleMember md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();
                item = db0.TempleMember.Find(md.temple_member_id);

                item.addr = md.addr;
                item.birthday = md.birthday;
                item.zip = md.zip;
                item.member_name = md.member_name;
                item.sno = md.sno;
                item.tel = md.tel;
                item.mobile = md.mobile;

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
        public ResultInfo Post([FromBody]TempleMember md)
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
                md.temple_member_id = GetNewId(CodeTable.TempleMember);
                md.i_insertDateTime = DateTime.Now;
                md.is_close = false;
                db0.TempleMember.Add(md);
                db0.SaveChanges();

                rAjaxResult.result = true;
                rAjaxResult.id = md.temple_member_id;
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

                var item = db0.TempleMember.Find(id);
                db0.TempleMember.Remove(item);

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
