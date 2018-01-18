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
                        && x.category != ProcCore.Business.Logic.e_祈福產品分類.禮斗
                        && x.category != ProcCore.Business.Logic.e_祈福產品分類.福燈
                        && x.category != ProcCore.Business.Logic.e_祈福產品分類.祈福許願燈
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
        public rAjaxGetItems<m_Product> GetProductWishLight()
        {
            rAjaxGetItems<m_Product> r = new rAjaxGetItems<m_Product>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getAllProduct = db.Product
                    .Where(x => x.i_Hide == 0 && x.category == ProcCore.Business.Logic.e_祈福產品分類.祈福許願燈 && x.isSelect == false && x.i_Hide == 0)
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

        #region (2017.7.19)取得超渡法會批次list
        public GridInfo2<BatchList> GetAssemblyList([FromUri]q_BatchList q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {
                var items = (from x in db0.Orders_Detail
                             where x.assembly_batch_sn != null & x.is_reject != true & x.Product.category == ProcCore.Business.Logic.e_祈福產品分類.超渡法會
                             orderby x.i_InsertDateTime
                             select new BatchList()
                             {
                                 batch_title = x.AssemblyBatch.batch_title,
                                 batch_date = x.AssemblyBatch.batch_date,
                                 lunar_y = x.AssemblyBatch.lunar_y,
                                 lunar_m = x.AssemblyBatch.lunar_m,
                                 lunar_d = x.AssemblyBatch.lunar_d,
                                 batch_timeperiod = x.AssemblyBatch.batch_timeperiod,
                                 assembly_batch_sn = x.assembly_batch_sn,
                                 orders_sn = x.orders_sn,
                                 member_name = x.member_name,
                                 address = x.address,
                                 Y = x.Y,
                                 product_name = x.product_name,
                                 product_sn = x.product_sn,
                                 i_InsertDateTime = x.i_InsertDateTime
                             });

                if (q.year != null)
                {
                    items = items.Where(x => x.Y == q.year);
                }

                if (q.assembly_batch_sn != null)
                {
                    items = items.Where(x => x.assembly_batch_sn == q.assembly_batch_sn);
                }

                if (q.product_sn != null)
                {
                    items = items.Where(x => x.product_sn == q.product_sn);
                }

                if (q.startDate != null & q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => x.i_InsertDateTime >= start & x.i_InsertDateTime < end);
                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<BatchList>()
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

        [HttpGet]
        public rAjaxGetItems<m_AssemblyBatch> GetBatchList(int? year)
        {
            rAjaxGetItems<m_AssemblyBatch> r = new rAjaxGetItems<m_AssemblyBatch>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                int y = year == null ? DotWeb.CommSetup.CommWebSetup.WorkYear : (int)year;
                var getAllBath = db.AssemblyBatch
                    .Where(x => x.batch_date.Year == year)
                    .Select(x => new m_AssemblyBatch()
                    {
                        batch_sn = x.batch_sn,
                        batch_date = x.batch_date,
                        batch_timeperiod = x.batch_timeperiod,
                        batch_title = x.batch_title,
                        count = x.訂單明細檔.Where(z => z.assembly_batch_sn != null & z.is_reject != true & z.Product.category == ProcCore.Business.Logic.e_祈福產品分類.超渡法會).Count()
                    }).OrderBy(x => new { x.batch_date, x.batch_timeperiod }).ToList();

                r.data = getAllBath;
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

        #region (2017.8.10)祈福許願燈
        [HttpGet]
        public rAjaxGetItems<m_WishList> GetWishList()
        {
            rAjaxGetItems<m_WishList> r = new rAjaxGetItems<m_WishList>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB0();
                var getList = db.Wish
                    .OrderBy(x => x.sort)
                    .Select(x => new m_WishList()
                    {
                        wish_id = x.wish_id,
                        wish_name = x.wish_name,
                        can_text = x.can_text,
                        wish_checked = false
                    }).ToList();

                r.data = getList;
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
        public class m_WishList : m_Wish
        {
            public bool wish_checked { get; set; }
        }
        [HttpGet]
        public GridInfo2<m_Orders> GetWishOrderList([FromUri]q_WishList q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = getDB0())
            {//訂單編號、姓名、生日、電話、手機、地址、祈求願望1、祈求願望2
                int Orders_Type = (int)ProcCore.Business.Orders_Type.wishlight;
                var items = (from x in db0.Orders
                             where x.orders_type == Orders_Type
                             orderby x.C_InsertDateTime
                             select new m_Orders()
                             {
                                 y = x.y,
                                 orders_sn = x.orders_sn,
                                 member_name = x.member_name,
                                 tel = x.tel,
                                 mobile = x.mobile,
                                 total = x.total,
                                 C_InsertDateTime = x.C_InsertDateTime,
                                 InsertUserName = x.Users.users_name,
                                 member_id = x.member_id
                             });

                if (q.year != null)
                {
                    items = items.Where(x => x.y == q.year);
                }

                if (q.startDate != null & q.endDate != null)
                {
                    DateTime start = ((DateTime)q.startDate);
                    DateTime end = ((DateTime)q.endDate).AddDays(1);
                    items = items.Where(x => x.C_InsertDateTime >= start & x.C_InsertDateTime < end);
                }

                int page = (q.page == null ? 1 : (int)q.page);
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                var resultItems = items.Skip(startRecord).Take(this.defPageSize).ToList();

                return (new GridInfo2<m_Orders>()
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

        [HttpGet]
        public rAjaxGetData<List<WishList>> GetWishOrderItem([FromUri]q_WishOrderItem q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料
            rAjaxGetData<List<WishList>> r = new rAjaxGetData<List<WishList>>();
            using (db0 = getDB0())
            {//訂單編號、姓名、生日、電話、手機、地址、祈求願望1、祈求願望2
                var items = (from x in db0.Orders_Detail
                             where x.is_reject != true & x.orders_sn == q.orders_sn
                             orderby x.i_InsertDateTime
                             select new WishList()
                             {
                                 Y = x.Y,
                                 i_InsertDateTime = x.i_InsertDateTime,
                                 product_name = x.product_name,
                                 product_sn = x.product_sn,
                                 orders_sn = x.orders_sn,
                                 orders_detail_id = x.orders_detail_id,
                                 member_detail_id = x.member_detail_id,
                                 light_name = x.light_name,
                                 member_name = x.member_name,
                                 tel = x.Member_Detail.tel,
                                 gender = x.gender,
                                 l_birthday = x.l_birthday,
                                 born_time = x.born_time,
                                 born_sign = x.born_sign,
                                 mobile = x.Member_Detail.mobile,
                                 address = x.address,
                                 wish_memo = x.wish_memo,
                                 wishs = x.Orders.Wish_Light.Where(y => y.member_detail_id == x.member_detail_id).ToList()
                             });

                //wishs = detail.Orders.Wish_Light.Where(x => x.member_detail_id == detail.member_detail_id)
                //     .Select(x => new WishText() { wish_id = x.wish_id, wish_text = x.wish_text }).ToList()
                r.result = true;
                r.data = items.ToList();
                return r;
            }
            #endregion
        }
        public class WishList : m_Orders_Detail
        {
            public List<Wish_Light> wishs { get; set; }
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
