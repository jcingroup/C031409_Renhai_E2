using ProcCore.Business.DB0;
using ProcCore.Business.LogicConect;
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
    public class MberDataDetailController : ajaxApi<Member_Detail, q_Member_Detail>
    {
        public async Task<IHttpActionResult> Get(int id)
        {
            using (db0 = getDB0())
            {
                item = await db0.Member_Detail.FindAsync(id);

                r = new rAjaxGetData<Member_Detail>() { data = item };
            }

            return Ok(r);
        }
        public IHttpActionResult Get([FromUri]q_Member_Detail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Member_Detail
                             orderby x.member_id
                             where x.i_Hide == false
                             select new m_Member_Detail()
                             {
                                 member_detail_id = x.member_detail_id,
                                 tel = x.tel,
                                 address = x.address
                             });

                //if (q.name != null)
                //    items = items.Where(x => x.householder.Contains(q.key));


                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return Ok(new GridInfo2<m_Member_Detail>()
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
        public async Task<IHttpActionResult> Put([FromBody]Member_Detail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = await db0.Member_Detail.FindAsync(md.member_detail_id);

                item.member_name = md.member_name;
                item.gender = md.gender;
                item.email = md.email;
                item.l_birthday = md.l_birthday;
                item.birthday = md.birthday;
                item.born_sign = md.born_sign;
                item.born_time = md.born_time;
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
        public async Task<IHttpActionResult> Post([FromBody]Member_Detail md)
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
                    db0 = LogicCenter.getDB0;
                    #region 查詢是否有該戶存在

                    var existHolder = db0.Member.Any(x => x.member_id == md.member_id);
                    if (!existHolder)
                    { //不存在要先將第一位當作戶長存入
                        Member mdHolder = new Member()
                        {
                            member_id = md.member_id,
                            tel = md.tel,
                            address = md.address,
                            zip = md.zip,
                            householder = md.member_name,
                            i_InsertUserID = aspUserId,
                            i_InsertDeptID = departmentId,
                            i_InsertDateTime = DateTime.Now,
                            i_Lang = getNowLnag()
                        };

                        db0.Member.Add(mdHolder);
                        await db0.SaveChangesAsync();
                    }
                    #endregion

                    md.i_InsertUserID = aspUserId;
                    md.i_InsertDeptID = departmentId;
                    md.i_InsertDateTime = DateTime.Now;
                    md.i_Lang = getNowLnag();
                    db0.Member_Detail.Add(md);
                    await db0.SaveChangesAsync();
                    tx.Complete();

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
        }
        public async Task<IHttpActionResult> Delete(int[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                foreach (var id in ids)
                {
                    item = new Member_Detail() { member_detail_id = id };
                    db0.Member_Detail.Attach(item);
                    db0.Member_Detail.Remove(item);
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
