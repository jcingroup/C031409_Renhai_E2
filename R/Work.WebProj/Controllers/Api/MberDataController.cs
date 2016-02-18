using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotWeb.Api
{
    public class MberDataController : ajaxApi<Member, q_Member>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Member.FindAsync(id);
                item.getMemberDetail = item.Member_Detail.ToArray();
                r = new rAjaxGetData<Member>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Member q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Member
                             orderby x.member_id
                             where x.i_Hide == false
                             select new m_Member()
                             {
                                 member_id = x.member_id,
                                 householder = x.householder,
                                 tel = x.tel,
                                 address = x.address
                             });

                if (q.key != null)
                    items = items.Where(x => x.householder.Contains(q.key));


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo2<m_Member>()
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
        public async Task<IHttpActionResult> Put([FromBody]Member md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Member.FindAsync(md.member_id);

                item.householder = md.householder;
                item.tel = md.tel;
                item.address = md.address;
                item.zip = md.zip;

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
        public async Task<IHttpActionResult> Post([FromBody]Member md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            if (!ModelState.IsValid)
            {
                rAjaxResult.message = ModelStateErrorPack();
                rAjaxResult.result = false;
                return Ok(rAjaxResult);
            }

            try
            {
                #region working a
                db0 = getDB0();

                md.i_InsertUserID = aspUserId;
                md.i_InsertDeptID = departmentId;
                md.i_InsertDateTime = DateTime.Now;
                md.i_Lang = getNowLnag();
                db0.Member.Add(md);
                await db0.SaveChangesAsync();

                rAjaxResult.result = true;
                rAjaxResult.id = md.member_id;
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
        public async Task<IHttpActionResult> Delete(int[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new Member() { member_id = id };
                    db0.Member.Attach(item);
                    db0.Member.Remove(item);
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
