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
    public class MemberMailController : ajaxApi<Member, q_Member>
    {
        public rAjaxGetData<Member> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.Member.Find(id);
                r = new rAjaxGetData<Member>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_Member> Get([FromUri]q_Member q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Member
                             orderby x.householder
                             select new m_Member()
                             {
                                 member_detail_count = 0,
                                 member_id = x.member_id,
                                 householder = x.householder,
                                 zip = x.zip,
                                 address = x.address,
                                 tel = x.tel,
                                 repeat_mark = x.repeat_mark
                             });

                //var items = (from x in db0.Member
                //             join y in db0.Orders on x.member_id equals y.member_id
                //             select new m_Member()
                //             {
                //                 member_detail_count = 0,
                //                 member_id = x.member_id,
                //                 householder = x.householder,
                //                 zip = x.zip,
                //                 address = x.address,
                //                 tel = x.tel,
                //                 repeat_mark = x.repeat_mark,
                //                 year=y.y
                //             }).Where(x => (x.zip == "320" || x.zip == "324" || x.zip == "326") & x.year == 2014 & x.repeat_mark == false).Distinct().OrderBy(x => x.householder);

                if ((q.householder == null && q.member_tel == null)||(q.member_tel==null && q.householder!=null && q.householder.Length<2))
                    items = null;

                if (q.householder != null && q.householder.Length>=2)
                    items = items.Where(x => x.householder.Contains(q.householder));

                if (q.member_tel != null)
                    items = items.Where(x => x.tel == q.member_tel);

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_Member>()
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
        public ResultInfo Put([FromBody]Member md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.Member.Find(md.member_id);
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
        public ResultInfo Post([FromBody]Member md)
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

                db0.Member.Add(md);
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
                    item = new Member { member_id = id };
                    db0.Member.Attach(item);
                    db0.Member.Remove(item);
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
