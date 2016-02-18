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
    public class Member_DetailController : ajaxApi<Member_Detail, q_Member_Detail>
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
        public rAjaxGetItems<Member_Detail> Get([FromUri]q_Member_Detail q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            rAjaxGetItems<Member_Detail> r = new rAjaxGetItems<Member_Detail>();
            using (db0 = getDB0())
            {
                var items = db0.Member_Detail.Where(x => x.member_id == q.member_id & !x.is_delete).ToList();
                r.data = items;
                r.result = true;
                return r;
            }
            #endregion
        }
        public ResultInfo Put([FromBody]Member_Detail md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                db0 = getDB0();

                item = db0.Member_Detail.Find(md.member_id);
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

                db0.Member_Detail.Add(md);
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
                    item = new Member_Detail { member_id = id };
                    db0.Member_Detail.Attach(item);
                    db0.Member_Detail.Remove(item);
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
