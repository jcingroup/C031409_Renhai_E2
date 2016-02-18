using ProcCore.ReturnAjaxResult;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Work.WebApp.Models;
using System.Linq;
using ProcCore.Business.Logic;
using DotWeb.CommSetup;

namespace DotWeb.Controllers
{
    public class CartController : BaseController
    {
        [HttpPost]
        public string SetCartMaster(cartMaster md)
        {
            ResultInfo r = new ResultInfo();
            cartMaster cart;
            if (Session[this.cookieCart] == null)
            {
                cart = md;
                cart.Item = new List<cartDetail>();
                Session[this.cookieCart] = cart;
            }
            else
            {
                cart = (cartMaster)Session[this.cookieCart];
                if (cart.Item == null)
                {
                    cart.Item = new List<cartDetail>();
                }
            }
            r.result = true;
            return defJSON(r);
        }
        [HttpPost]
        public string AddCart(cartDetail cartData)
        {
            rAjaxGetItems<ResultInfo> r = new rAjaxGetItems<ResultInfo>();
            int limit_Product_Count = CommWebSetup.OrdersRecordMax; //每筆交易產品數量限制

            try
            {
                RenHai2012Entities db = getDB();

                #region Session 資料處理
                cartMaster cart;
                if (Session[this.cookieCart] == null)
                {
                    cart = new cartMaster();
                    cart.Item = new List<cartDetail>();
                    Session[this.cookieCart] = cart;
                }
                else
                {
                    cart = (cartMaster)Session[this.cookieCart];
                    if (cart.Item == null)
                    {
                        cart.Item = new List<cartDetail>();
                    }
                }
                #endregion

                #region 資料檢查

                var getProduct = db.Product.Find(cartData.product_sn);

                if (cartData.member_detail_id < 0)
                    throw new Exception("未選擇會員姓名");

                if (string.IsNullOrEmpty(cartData.product_sn))
                    throw new Exception("未選擇產品項目");

                if (cart.Item.Count >= limit_Product_Count)
                    throw new Exception(string.Format("超過訂購項目{0}", limit_Product_Count));

                if (cart.Item.Any(x => x.product_sn == cartData.product_sn && x.member_detail_id == cartData.member_detail_id))
                    throw new Exception(string.Format("{0}的'{1}'產品已在訂購清單中", cartData.member_name, cartData.product_name));

                if (string.IsNullOrEmpty(cartData.address))
                    throw new Exception("地址需輸入");

                if (cartData.product_sn == e_祈福產品.捐金牌 && cartData.gold == 0)
                    throw new Exception("捐金牌需輸入金牌數");

                if (cartData.product_sn == e_祈福產品.捐白米 && cartData.race == 0)
                    throw new Exception("捐白米需輸入白米斤數");

                if (getProduct.category == "香油" && cartData.price == 0)
                    throw new Exception("香油錢需輸入金額");

                if (cartData.product_sn == e_祈福產品.保運 && cartData.manjushri == 0)
                    throw new Exception("文殊梯次未選擇");

                if (cartData.product_sn == e_祈福產品.姻緣燈 && (cartData.LY == 0 || cartData.LM == 0 || cartData.LD == 0))
                    throw new Exception("姻緣燈其生日資料需填寫完整");

                if (cartData.product_sn == e_祈福產品.安太歲 && (cartData.LY == 0 || cartData.LM == 0 || cartData.LD == 0))
                    throw new Exception("安太歲生日不正確");

                if (cartData.product_sn == e_祈福產品.姻緣燈 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("姻緣燈其生日資料需填寫完整");

                if (cartData.product_sn == e_祈福產品.安太歲 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("安太歲生日不正確");

                if (cartData.product_sn == e_祈福產品.保運 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("保運日不正確");

                cart.Item.Add(cartData);
                cart.total = cart.Item.Sum(x => x.price);
                cart.race = cart.Item.Sum(x => x.race);
                cart.gold = cart.Item.Sum(x => x.gold);

                for (var i = 0; i < cart.Item.Count(); i++)
                {
                    cart.Item[i].detail_sort = i + 1;
                }

                #endregion

                //db = getDB();
                db.Dispose();
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

            }
        }
        [HttpPost]
        public string AddLightCart(cartDetail cartData)
        {
            rAjaxGetItems<ResultInfo> r = new rAjaxGetItems<ResultInfo>();
            int limit_Product_Count = CommWebSetup.OrdersRecordMax; //每筆交易產品數量限制

            try
            {
                RenHai2012Entities db = getDB();

                #region Session 資料處理
                cartMaster cart;
                if (Session[this.cookieCart] == null)
                {
                    cart = new cartMaster();
                    cart.Item = new List<cartDetail>();
                    Session[this.cookieCart] = cart;
                }
                else
                {
                    cart = (cartMaster)Session[this.cookieCart];
                    if (cart.Item == null)
                    {
                        cart.Item = new List<cartDetail>();
                    }
                }
                #endregion

                #region 資料檢查

                var getProduct = db.Product.Find(cartData.product_sn);

                if (cartData.member_detail_id < 0)
                    throw new Exception("未選擇會員姓名");

                if (string.IsNullOrEmpty(cartData.product_sn))
                    throw new Exception("未選擇產品項目");

                if (cart.Item.Count >= limit_Product_Count)
                    throw new Exception(string.Format("超過訂購項目{0}", limit_Product_Count));

                if (cart.Item.Any(x => x.product_sn == cartData.product_sn))
                    throw new Exception(string.Format("{0}的'{1}'產品已在訂購清單中", cartData.member_name, cartData.product_name));

                if (string.IsNullOrEmpty(cartData.address))
                    throw new Exception("地址需輸入");

                if (cartData.product_sn == e_祈福產品.捐金牌 && cartData.gold == 0)
                    throw new Exception("捐金牌需輸入金牌數");

                if (cartData.product_sn == e_祈福產品.捐白米 && cartData.race == 0)
                    throw new Exception("捐白米需輸入白米斤數");

                if (getProduct.category == "香油" && cartData.price == 0)
                    throw new Exception("香油錢需輸入金額");

                if (cartData.product_sn == e_祈福產品.保運 && cartData.manjushri == 0)
                    throw new Exception("文殊梯次未選擇");

                if (cartData.product_sn == e_祈福產品.姻緣燈 && (cartData.LY == 0 || cartData.LM == 0 || cartData.LD == 0))
                    throw new Exception("姻緣燈其生日資料需填寫完整");

                if (cartData.product_sn == e_祈福產品.安太歲 && (cartData.LY == 0 || cartData.LM == 0 || cartData.LD == 0))
                    throw new Exception("安太歲生日不正確");

                if (cartData.product_sn == e_祈福產品.姻緣燈 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("姻緣燈其生日資料需填寫完整");

                if (cartData.product_sn == e_祈福產品.安太歲 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("安太歲生日不正確");

                if (cartData.product_sn == e_祈福產品.保運 && (cartData.LY == 1 && cartData.LM == 1 && cartData.LD == 1))
                    throw new Exception("保運日不正確");

                cart.Item.Add(cartData);
                cart.total = cart.Item.Sum(x => x.price);
                cart.race = cart.Item.Sum(x => x.race);
                cart.gold = cart.Item.Sum(x => x.gold);

                for (var i = 0; i < cart.Item.Count(); i++)
                {
                    cart.Item[i].detail_sort = i + 1;
                }

                #endregion

                //db = getDB();
                db.Dispose();
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

            }
        }
        [HttpGet]
        public string ClearCart()
        {
            rAjaxGetItems<ResultInfo> r = new rAjaxGetItems<ResultInfo>();
            try
            {
                cartMaster cart = (cartMaster)Session[this.cookieCart];
                #region Session 資料處理
                if (cart != null)
                {
                    if (cart.Item != null)
                    {
                        cart.Item.Clear();
                        cart.Item = null;
                    }
                }
                Session.Remove(this.cookieCart);
                #endregion
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

            }
        }
        [HttpGet]
        public string RemoveCart(int member_detail_id, string product_sn)
        {
            rAjaxGetItems<ResultInfo> r = new rAjaxGetItems<ResultInfo>();
            try
            {
                cartMaster cart = (cartMaster)Session[this.cookieCart];
                #region Session 資料處理
                if (cart != null)
                {
                    if (cart.Item != null)
                    {
                        var removeItem = cart.Item.First(
                            x => x.member_detail_id == member_detail_id &&
                                x.product_sn == product_sn
                            );
                        cart.Item.Remove(removeItem);
                        cart.total = cart.Item.Sum(x => x.price);
                        cart.race = cart.Item.Sum(x => x.race);
                        cart.gold = cart.Item.Sum(x => x.gold);
                    }
                }

                #endregion
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

            }
        }
        [HttpGet]
        public string ViewCart(int member_detail_id, string product_sn)
        {
            rAjaxGetData<cartDetail> r = new rAjaxGetData<cartDetail>();
            try
            {
                cartMaster cart = (cartMaster)Session[this.cookieCart];
                #region Session 資料處理
                if (cart != null)
                {
                    if (cart.Item != null)
                    {
                        var viewItem = cart.Item.First(
                            x => x.member_detail_id == member_detail_id &&
                                x.product_sn == product_sn
                            );

                        r.data = viewItem;
                        r.result = true;
                    }
                }
                return defJSON(r);
                #endregion
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                return defJSON(r);
            }
            finally
            {

            }
        }
        [HttpGet]
        public string ListCartItems()
        {
            rAjaxGetData<cartMaster> r = new rAjaxGetData<cartMaster>();
            cartMaster cart;
            if (Session[this.cookieCart] == null)
            {
                cart = new cartMaster();
                cart.Item = new List<cartDetail>();
                Session[this.cookieCart] = cart;
            }
            else
            {
                cart = (cartMaster)Session[this.cookieCart];
                if (cart.Item == null)
                {
                    cart.Item = new List<cartDetail>();
                }
            }

            r.data = cart;
            r.result = true;
            return defJSON(r);
        }
        [HttpGet]
        public string OrdersToSession(string orders_sn)
        {

            rAjaxGetData<cartMaster> r = new rAjaxGetData<cartMaster>();

            try
            {
                cartMaster cart = (cartMaster)Session[this.cookieCart];
                #region Session 資料處理
                if (cart != null)
                {
                    if (cart.Item != null)
                    {
                        cart.Item.Clear();
                        cart.Item = null;
                    }
                }
                Session.Remove(this.cookieCart);

                var db0 = getDB();
                var orders = db0.Orders.Find(orders_sn);
                var orders_detail = db0.Orders_Detail.Where(x => x.orders_sn == orders_sn).OrderBy(x => x.detail_sort).ThenBy(x => x.orders_detail_id);

                cart = new cartMaster()
                {
                    orders_sn = orders.orders_sn,
                    member_id = orders.member_id,
                    member_detail_id = orders.member_detail_id,
                    member_name = orders.member_name,
                    address = orders.address,
                    gender = orders.gender,
                    tel = orders.tel,
                    zip = orders.zip,
                    total = orders.total
                };
                cart.Item = new List<cartDetail>();
                foreach (var item in orders_detail)
                {
                    string[] birthdaySplit = item.l_birthday.Split('/');
                    if (birthdaySplit.Length != 3)
                    {
                        birthdaySplit = new string[] { "1", "1", "1" };
                    }

                    cart.Item.Add(new cartDetail()
                    {
                        member_detail_id = item.member_detail_id,
                        member_name = item.member_name,
                        product_sn = item.product_sn,
                        product_name = item.product_name,
                        price = item.price,
                        race = item.race,
                        gold = item.gold,
                        isOnOrder = true,
                        born_sign = item.born_sign,
                        born_time = item.born_time,
                        l_birthday = item.l_birthday,
                        LY = int.Parse(birthdaySplit[0]),
                        LM = int.Parse(birthdaySplit[1]),
                        LD = int.Parse(birthdaySplit[2]),
                        gender = item.gender,
                        address = item.address,
                        light_name = item.light_name,
                        detail_sort = item.detail_sort
                    });
                }

                Session[this.cookieCart] = cart;
                r.result = true;
                r.data = cart;
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message + ex.StackTrace;
            }

            return defJSON(r);

                #endregion
        }
    }
}
