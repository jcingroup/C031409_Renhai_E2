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
    public class TempleMemberController : ajaxApi<TempleMember, q_TempleMember>
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

                var items = (from x in db0.TempleMember
                             orderby x.join_datetime descending
                             where x.is_close == false
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
                                 join_datetime = x.join_datetime == null ? default_insertdate : x.join_datetime
                             });

                //if (q.pageclass != null)
                //{
                //    if (q.pageclass == "postprint")
                //        items = items.Where(x => x.is_close == false);
                //}

                if (q.member_name != null)
                {
                    items = items.Where(x => x.member_name.Contains(q.member_name));
                }

                if (q.tel != null)
                    items = items.Where(x => x.tel.Contains(q.tel));

                if (q.mobile != null)
                    items = items.Where(x => x.mobile.Contains(q.mobile));

                if (q.startDate != null && q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => (DateTime)x.join_datetime >= start && (DateTime)x.join_datetime < end);
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
                item.join_datetime = md.join_datetime;
                item.last_attend_datetime = md.last_attend_datetime;

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
                bool exist = db0.TempleAccount.Any(x => x.temple_member_id == id);
                if (item.is_close == false) {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "請先確認要取消該會員資料才能刪除!!";
                    return rAjaxResult;
                }
                else if (item.last_attend_datetime != null || exist)
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "有交易紀錄無法刪除!!";
                    return rAjaxResult;
                }else if (item.last_attend_datetime == null && item.is_close==true) {
                    db0.TempleMember.Remove(item);
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
