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
    public class AddrController : BaseApiController
    {
        public IHttpActionResult Get(string keyword)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var newKeyWord = keyword.Replace('台', '臺');

            try
            {
                db0 = getDB0();

                var getAddrs = db0.i_Adr_zh_TW_Data
                    .Select(x => new { x.id, x.zip, x.data, x.scope, x.sort })
                    .Where(x => x.data.StartsWith(newKeyWord))
                    .OrderBy(x => x.sort)
                    .Take(15).ToList();

                rAjaxResult.result = true;
                rAjaxResult.json = getAddrs;
                return Ok(rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxResult.message = ex.Message;
                rAjaxResult.result = false;
                return Ok(rAjaxResult);
            }
            finally
            {
                db0.Dispose();
            }
        }
    }
}
