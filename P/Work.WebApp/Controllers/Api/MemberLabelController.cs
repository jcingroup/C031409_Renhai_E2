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
    public class MemberLabelController : ajaxApi<Member_Detail, q_Member_Detail>
    {
        public rAjaxGetData<Member_Detail> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = db0.Member_Detail.Find(id);
                r = new rAjaxGetData<Member_Detail>() { data = item };
            }

            return r;
        }
        public GridInfo2<m_Member_Detail> Get([FromUri]q_Member_Detail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                string[] light_category = new string[] { "點燈", "福燈" };
                var items = (from x in db0.Member_Detail
                             orderby x.Member.zip, x.Member.address //地址排序
                             //orderby x.householder //姓名
                             where x.is_holder & x.Orders_Detail.Any(y => y.Y == q.year & light_category.Contains(y.Product.category)) & !x.Member.repeat_mark & !x.is_delete
                             select new m_Member_Detail()
                             {
                                 member_id = x.member_id,
                                 address = x.Member.address,
                                 zip = x.Member.zip,
                                 householder = x.Member.householder
                             }).Distinct();

                string[] zip = new string[] { "320", "324", "326" };

                if (q.zipcode != null)
                {
                    if (zip.Contains(q.zipcode))
                    {
                        items = items.Where(x => x.zip == q.zipcode).OrderBy(x => new { x.zip, x.address });
                    }
                    else
                    {
                        items = items.Where(x => !zip.Contains(x.zip)).OrderBy(x => new { x.zip, x.address });
                    }

                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_Member_Detail>()
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
        public ResultInfo Put([FromBody]Member_Detail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();
                item = db0.Member_Detail.Find(md.member_detail_id);


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
        public ResultInfo Post([FromBody]Member_Detail md)
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
                md.member_detail_id = GetNewId(CodeTable.Base);
                db0.Member_Detail.Add(md);
                db0.SaveChanges();

                rAjaxResult.result = true;
                rAjaxResult.id = md.member_detail_id;
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

                var item = db0.Member_Detail.Find(id);

                db0.Member_Detail.Remove(item);

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
