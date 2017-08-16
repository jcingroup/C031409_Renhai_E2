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
    public class WishLightController : BaseController
    {
        public ActionResult Index()
        {
            ViewBag.ID = this.UserId;
            return View();
        }
        #region 祈福許願燈位
        [HttpGet]
        public string addPlace(int row)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                int year = DotWeb.CommSetup.CommWebSetup.WorkYear;

                //取得以新增的登為數量
                int count = db0.Light_Site.Where(x => x.Y == year & x.product_sn == e_祈福產品.祈福許願燈).Count();

                string[] StandA = { "甲", "乙", "丙", "丁", "戊", "己" };//第一座六個面編號
                string[] StandB = { "A", "B", "C", "D", "E", "F" };//第二座六個面編號
                string[] SideRow = { "A", "B", "C", "D", "E", "F", "G",
                                   "H", "I", "J", "K", "L", "M", "N",
                                   "O", "P", "Q", "R", "S", "T", "U"};//每面有21排
                string frm = "祈願{0}{1}{2}-{3}";//ex:甲A1-1;0:stand,1:siderow,2:row,3:sidecol
                if (this.UserId == 1000001 & count <= 5040)
                {
                    #region working a
                    #region 第一座
                    foreach (var i in StandA)
                    {
                        List<Light_Site> mds = new List<Light_Site>();
                        foreach (var j in SideRow)
                        {                 
                            for (int k = 1; k <= 4; k++)
                            {
                                //一面四列
                                Light_Site md = new Light_Site()
                                {
                                    light_name = string.Format(frm, i, j, row, k),
                                    Y = year,
                                    is_sellout = "0",
                                    product_sn = e_祈福產品.祈福許願燈,
                                    price = 1200,
                                    C_Hiden = false,
                                    C_LockState = false,
                                    IsReject = false,
                                    assembly_batch_sn = null
                                };
                                mds.Add(md);
                            }
                        }
                        db0.Light_Site.AddRange(mds);
                        db0.SaveChanges();
                    }
                    #endregion

                    #region 第二座
                    foreach (var i in StandB)
                    {
                        List<Light_Site> mds = new List<Light_Site>();
                        foreach (var j in SideRow)
                        {                      
                            for (int k = 1; k <= 4; k++)
                            {
                                //一面四列
                                Light_Site md = new Light_Site()
                                {
                                    light_name = string.Format(frm, i, j, row, k),
                                    Y = year,
                                    is_sellout = "0",
                                    product_sn = e_祈福產品.祈福許願燈,
                                    price = 1200,
                                    C_Hiden = false,
                                    C_LockState = false,
                                    IsReject = false,
                                    assembly_batch_sn = null
                                };
                                mds.Add(md);
                            }
                        }
                        db0.Light_Site.AddRange(mds);
                        db0.SaveChanges();
                    }
                    #endregion

                    tx.Complete();
                    #endregion
                }
                else
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = "此權限無法新增燈位!";
                }

                return defJSON(rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return defJSON(rAjaxResult);
            }
            finally
            {
                tx.Dispose();
                db0.Dispose();
            }
        }
        #endregion
        public string aj_init()
        {
            return defJSON(new { });
        }
    }

}
