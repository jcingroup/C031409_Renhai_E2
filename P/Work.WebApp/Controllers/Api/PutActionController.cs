using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace DotWeb.Api
{
    public class PutActionController : BaseApiController
    {
        public ResultInfo PutMemberCheck([FromBody]q_MemberCheck q)
        {
            using (db0 = getDB0())
            {
                var item = db0.Member.Find(q.member_id);
                item.repeat_mark = q.repeat_mark;
                db0.SaveChanges();
                var r = new ResultInfo() { result = true };
                return r;
            }
        }

        public ResultInfo PutTempleMemberCheck([FromBody]q_MemberCheck q)
        {
            using (db0 = getDB0())
            {
                var item = db0.TempleMember.Find(q.member_id);
                item.is_close = q.repeat_mark;
                db0.SaveChanges();
                var r = new ResultInfo() { result = true };
                return r;
            }
        }
        public class q_MemberCheck {
            public int member_id { get; set; }
            public bool repeat_mark { get; set; }
        }
    }
}
