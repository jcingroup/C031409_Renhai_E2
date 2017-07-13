using ProcCore.ReturnAjaxResult;
using ProcCore.Web;
using ProcCore.WebCore;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;
using Work.WebApp.Models;
using System.Globalization;
namespace DotWeb.Api
{
    public class GetActionController : BaseApiController
    {
        public rAjaxGetItems<m_Product> GetProductAll()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0
                        && x.category != "禮斗"
                        && x.category != "福燈"
                        && x.isSelect == false
                        && x.i_Hide == 0
                    //&& x.category != "契子" // ref sa.xls No 2
                        )
                    .Select(x => new m_Product()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        category = x.category,
                        排序 = x.排序
                    }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Product> GetProductSDLight()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category == "禮斗" && x.isSelect == false && x.i_Hide == 0)
                    .Select(x => new m_Product()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        category = x.category,
                        排序 = x.排序
                    }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Product> GetProductMDLight()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category == "禮斗" && x.isSelect == true && x.i_Hide == 0)
                    .Select(x => new m_Product()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        category = x.category,
                        排序 = x.排序
                    }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Light_Site> GetLightByMD(string product_sn)
        {
            rAjaxGetItems<m_Light_Site> r = new rAjaxGetItems<m_Light_Site>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Light_Site
                    .Where(x => x.Y == CommSetup.CommWebSetup.WorkYear && x.is_sellout == "0" && x.product_sn == product_sn)
                    .Select(x => new m_Light_Site()
                    {
                        light_site_id = x.light_site_id,
                        product_sn = x.product_sn,
                        price = x.price,
                        light_name = x.light_name

                    }).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Product> GetProductFortune()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category == "福燈" && x.i_Hide == 0)
                    .Select(x => new m_Product()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        category = x.category,
                        排序 = x.排序
                    }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Users> GetUsers()
        {
            rAjaxGetItems<m_Users> r = new rAjaxGetItems<m_Users>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                if (this.UnitId == 1 || this.UnitId == 50000032)
                {
                    var g = db.Users
                            .Select(x => new m_Users()
                            {
                                account = x.account,
                                users_id = x.users_id,
                                users_name = x.users_name
                            }).OrderBy(x => x.account).ToList();

                    r.data = g;
                    r.result = true;
                }
                else
                {
                    r.data = null;
                    r.result = true;
                }

                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<m_Product> GetFortune()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category == "福燈")
                    .Select(x => new m_Product()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        category = x.category,
                        排序 = x.排序
                    }).OrderBy(x => x.排序).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        public rAjaxGetItems<int> GetFortuneLight(string orders_sn)
        {
            rAjaxGetItems<int> r = new rAjaxGetItems<int>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                IList<int> getAllProduct = db.Fortune_Light
                    .Where(x => x.order_sn == orders_sn).Select(x => x.member_detail_id).ToList();

                r.data = getAllProduct;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        /// <summary>
        /// 計算農曆時間
        /// </summary>
        /// <param name="dt">為國曆日期</param>
        /// <returns></returns>
        public LuniInfo GetLunisolar(DateTime dt)
        {
            TaiwanLunisolarCalendar tc = new TaiwanLunisolarCalendar();
            int getY = tc.GetYear(dt);
            int getM = tc.GetMonth(dt);
            int getD = tc.GetDayOfMonth(dt);

            var isLeapYear = tc.IsLeapYear(getY);
            var isLeapMonth = tc.IsLeapMonth(getY, getM);
            var getLeapMonth = tc.GetLeapMonth(getY);

            int setMonth = 0;
            if (isLeapYear && isLeapYear && getM >= getLeapMonth)
                setMonth = getM - 1;
            else
                setMonth = getM;
            LuniInfo I = new LuniInfo()
            {
                SY = getY + 1911,
                LY = getY,
                M = setMonth,
                D = getD,
                IsLeap = isLeapMonth
            };


            return I;
        }
        #region (2016.1.27)訂單明細可搜尋去年的購買紀錄
        public rAjaxGetItems<m_Orders_Detail> GetOrderDetail(int member_detail_id)
        {
            rAjaxGetItems<m_Orders_Detail> r = new rAjaxGetItems<m_Orders_Detail>();
            RenHai2012Entities db = null;
            try
            {
                List<string> product_category = new List<string>() { "福燈", "點燈", "禮斗" };
                db = getDB0();
                var getOrderDetail = db.Orders_Detail
                    .Where(x => x.Y == (DateTime.Now.Year - 1) & x.member_detail_id == member_detail_id & product_category.Contains(x.Product.category))
                    .Select(x => new m_Orders_Detail()
                    {
                        product_sn = x.product_sn,
                        product_name = x.product_name,
                        price = x.price,
                        light_name = x.light_name,
                        i_InsertDateTime = x.i_InsertDateTime
                    }).OrderBy(x => x.i_InsertDateTime).ToList();

                r.data = getOrderDetail;
                r.result = true;
                return r;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return r;
            }
            finally
            {
                db.Dispose();
            }
        }
        #endregion

    }

    //GetFortuneLight
    public class fortunelight
    {
        public int member_detail_id { get; set; }
        public int sort { get; set; }
    }

}
