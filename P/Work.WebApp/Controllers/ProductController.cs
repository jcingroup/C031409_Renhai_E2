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
    public class ProductController : BaseController
    {
        [HttpGet]
        public string GetProductAll()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category != "禮斗" && x.isSelect == false)
                    .Select(x => new m_Product()
                {
                    product_sn = x.product_sn,
                    product_name = x.product_name,
                    排序 = x.排序
                }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
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
