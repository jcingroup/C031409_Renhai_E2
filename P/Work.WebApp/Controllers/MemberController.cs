using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;

using System.Web.Script.Serialization;

using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using Work.WebApp.Models;

namespace DotWeb.Controllers
{
    public class MemberController : BaseController
    {
        [HttpGet]
        public string GetMemberAll(int member_id)
        {
            rAjaxGetData<Member> r = new rAjaxGetData<Member>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getMember = db.Member.Find(member_id);
                var getMemberDetail = db.Member_Detail.Where(x => x.member_id == member_id & !x.is_delete)
                    .OrderByDescending(x => x.is_holder).ThenBy(x => x.lbirthday).ToList();
                foreach (var md in getMemberDetail)
                {
                    if (md.sno != null && md.sno != "")
                    {
                        md.sno = md.sno.ToUpper();
                        bool isTemple = db.TempleMember.Any(x => x.sno == md.sno);
                        if (isTemple) {
                            DateTime? getJoinDate = db.TempleMember.Where(x => x.sno == md.sno).FirstOrDefault().join_datetime;
                            md.member_name += "(契子)";
                            if (getJoinDate != null)
                            {
                                md.join_date = DateTime.Parse(getJoinDate.ToString()).ToString("yyyy/MM/dd");
                            }
                        }
                    }
                }
                getMember.getMember_Detail = getMemberDetail;

                r.data = getMember;
                r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpGet]
        public string GetMember(int member_id)
        {
            rAjaxGetData<Member> r = new rAjaxGetData<Member>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getMember = db.Member.Find(member_id);
                r.data = getMember;
                r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpGet]
        public string GetMemberDetail(int member_detail_id)
        {
            rAjaxGetData<Member_Detail> r = new rAjaxGetData<Member_Detail>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getMemberDetail = db.Member_Detail.Find(member_detail_id);
                r.data = getMemberDetail;
                r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {
                db.Dispose();
            }
        }
        [HttpGet]
        public string GetMemberByDetail(int member_id)
        {
            rAjaxGetData<Member_Detail> r = new rAjaxGetData<Member_Detail>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getMemberDetail = db.Member_Detail.Where(x => x.member_id == member_id && x.is_holder == true).First();
                r.data = getMemberDetail;
                r.result = true;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {
                db.Dispose();
            }
        }
    }
}
