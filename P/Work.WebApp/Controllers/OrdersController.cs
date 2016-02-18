using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using System;
using System.Linq;
using System.Web.Mvc;
using Work.WebApp.Models;
using ProcCore.NetExtension;
using System.Data.SqlClient;
using System.Collections.Generic;
using ProcCore.Business;
using DotWeb.CommSetup;
using ProcCore;

namespace DotWeb.Controllers
{
    public class OrdersController : BaseController
    {
        public ActionResult Index(int? member_id)
        {
            return View();
        }
        public ActionResult Fortune(int? member_id)
        {
            return View();
        }
        public ActionResult sortable()
        {
            return View();
        }
        [HttpGet]
        public string GetOrders(string order_sn)
        {
            rAjaxGetData<Orders> r = new rAjaxGetData<Orders>();
            RenHai2012Entities db = null;
            try
            {
                db = getDB();
                var getOrder = db.Orders.Find(order_sn);
                var getOrderDetail = db.Orders_Detail.Where(x => x.orders_sn == order_sn).ToList();
                getOrder.getOrders_Detail = getOrderDetail.ToArray();

                r.data = getOrder;
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
        [HttpPost]
        public string AddOrders(cartMaster md)
        {
            rAjaxGetData<string> r = new rAjaxGetData<string>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "未選購任何產品";
                return defJSON(r);
            }
            else
            {
                if (cart.Item.Count == 0)
                {
                    r.result = false;
                    r.message = "未選購任何產品";
                    return defJSON(r);
                }
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                string getNewOrderSN = GetOrderNewSerial(); //取得新訂單編號

                #region 訂單主檔新增
                Orders m = new Orders()
                {
                    orders_sn = getNewOrderSN,
                    y = this.LightYear,
                    member_detail_id = md.member_detail_id,
                    member_id = md.member_id,
                    member_name = md.member_name,
                    tel = md.tel,
                    zip = md.zip,
                    address = md.address,
                    gender = md.gender,
                    新增時間 = DateTime.Now,
                    C_InsertDateTime = DateTime.Now,
                    transation_date = DateTime.Now,
                    InsertUserId = this.UserId,
                    total = cart.Item.Sum(x => x.price),
                    mobile = md.mobile,
                    orders_state = (int)Orders_State.complete,
                    orders_type = (int)Orders_Type.general
                };

                foreach (var item in cart.Item)
                {
                    Light_Site product_light = null;
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn))
                    {
                        #region 點燈類產品處理
                        if (item.light_site_id == 0) //屬於電腦選位類型
                        {
                            var getNewLight = getLightStratNum.Where(x => x.product_sn == item.product_sn).First();

                            product_light = db0.Light_Site.
                                    Where(x => x.Y == this.LightYear &&
                                        x.product_sn == item.product_sn &&
                                        x.is_sellout == "0" &&
                                        x.light_site_id >= getNewLight.start_num)
                                        .OrderBy(x => x.light_site_id)
                                        .Take(1)
                                        .First();

                            product_light.is_sellout = "1";
                            product_light.C_UpdateDateTime = DateTime.Now;
                            product_light.C_UpdateUserID = this.UserId;
                            db0.SaveChanges();
                        }
                        else
                        { //屬於手動選位類型 主副斗直接選取燈位

                        }
                        #endregion
                    }
                    else
                    {
                        #region 非點燈類產品處理

                        #endregion
                    }

                    m.Orders_Detail.Add(new Orders_Detail()
                    {
                        orders_sn = getNewOrderSN,
                        product_sn = item.product_sn,
                        product_name = item.product_name,
                        light_name = product_light == null ? null : product_light.light_name,
                        memo = product_light == null ? null : product_light.light_site_id.ToString(),
                        address = item.address,
                        l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                        Y = this.LightYear,
                        member_detail_id = item.member_detail_id,
                        member_name = item.member_name,
                        price = item.price,
                        amt = 1,
                        race = item.race,
                        gold = item.gold,
                        manjushri = item.manjushri,
                        gender = item.gender,
                        born_sign = item.born_sign,
                        born_time = item.born_time,
                        購買時間 = DateTime.Now,
                        i_InsertDateTime = DateTime.Now,
                        經手人 = this.UserName,
                        C_InsertDateTime = DateTime.Now,
                        C_InsertUserID = this.UserId,
                        i_InsertUserID = this.UserId,
                        detail_sort = item.detail_sort
                    });
                }

                db0.Orders.Add(m);
                #endregion
                db0.SaveChanges();
                tx.Complete();

                m.getOrders_Detail = m.Orders_Detail.ToArray();
                r.data = m.orders_sn;
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
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpPut]
        public string UpdateOrders(cartMaster md)
        {
            rAjaxGetData<Orders> r = new rAjaxGetData<Orders>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "購物車系統未配置";
                return defJSON(r);
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.Where(x => x.isOnOrder == false).GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial && groupOrderProductQty.Count() > 0)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                #region 訂單主檔修改
                IList<Orders_Detail> orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn).ToList();

                //標記準備異動
                foreach (var n in orders_detail)
                {
                    n.tran_mark = true;
                }

                db0.SaveChanges();

                #region Order Master
                Orders orders = db0.Orders.Find(md.orders_sn);

                orders.address = md.address;
                orders.member_name = md.member_name;
                orders.mobile = md.mobile;
                orders.tel = md.tel;
                orders.zip = md.zip;
                orders.gender = md.gender;
                orders.total = cart.Item.Sum(x => x.price);
                orders.C_UpdateDateTime = DateTime.Now;
                orders.C_UpdateUserID = this.UserId;
                //修改訂單時 新增的東西仍算原新增訂單者
                var ouser_id = orders.InsertUserId;

                #endregion

                foreach (var item in cart.Item)
                {


                    if (orders_detail.Any(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn))
                    {
                        var get_detail_item = orders_detail.First(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn);
                        get_detail_item.tran_mark = false;
                        get_detail_item.member_detail_id = item.member_detail_id;
                        get_detail_item.member_name = item.member_name;
                        get_detail_item.price = item.price;
                        get_detail_item.race = item.race;
                        get_detail_item.gold = item.gold;
                        get_detail_item.C_UpdateDateTime = DateTime.Now;
                        get_detail_item.l_birthday = item.LY + "/" + item.LM + "/" + item.LD;
                        get_detail_item.born_sign = item.born_sign;
                        get_detail_item.born_time = item.born_time;
                        get_detail_item.gender = item.gender;
                        get_detail_item.detail_sort = item.detail_sort;
                    }
                    else
                    {
                        Light_Site product_light = null;
                        if (getLightProduct.Any(x => x.product_sn == item.product_sn)) //如果是點燈類產品
                        {
                            #region 點燈類產品處理
                            if (item.light_site_id == 0 && item.isOnOrder == false) //屬於電腦選位類型
                            {
                                var getNewLight = getLightStratNum.Where(x => x.product_sn == item.product_sn).FirstOrDefault();
                                if (getNewLight != null)
                                {
                                    product_light = db0.Light_Site.
                                    Where(x => x.Y == this.LightYear &&
                                        x.product_sn == item.product_sn &&
                                        x.is_sellout == "0" &&
                                        x.light_site_id >= getNewLight.start_num)
                                        .OrderBy(x => x.light_site_id)
                                        .Take(1)
                                        .First();

                                    product_light.is_sellout = "1";
                                    product_light.C_UpdateDateTime = DateTime.Now;
                                    product_light.C_UpdateUserID = this.UserId;
                                    db0.SaveChanges();
                                }
                            }
                            else
                            { //屬於手動選位類型 主副斗直接選取燈位

                            }
                            #endregion
                        }

                        orders.Orders_Detail.Add(new Orders_Detail()
                        {
                            orders_sn = md.orders_sn,
                            product_sn = item.product_sn,
                            product_name = item.product_name,
                            light_name = product_light == null ? null : product_light.light_name,
                            memo = product_light == null ? null : product_light.light_site_id.ToString(),
                            address = item.address,
                            l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            member_name = item.member_name,
                            price = item.price,
                            amt = 1,
                            race = item.race,
                            gold = item.gold,
                            manjushri = item.manjushri,
                            gender = item.gender,
                            born_sign = item.born_sign,
                            born_time = item.born_time,
                            購買時間 = DateTime.Now,
                            i_InsertDateTime = DateTime.Now,
                            經手人 = this.UserName,
                            C_InsertDateTime = DateTime.Now,
                            C_InsertUserID = ouser_id,
                            i_InsertUserID = (int)ouser_id,
                            C_UpdateUserID = this.UserId,
                            C_UpdateDateTime = DateTime.Now,
                            tran_mark = false,
                            detail_sort = item.detail_sort
                        });
                    }
                }

                #endregion
                db0.SaveChanges();

                //異動仍為True代表要做刪除
                IList<Orders_Detail> tran_orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn && x.tran_mark == true).ToList();

                Reject rjt = null;

                foreach (var item in tran_orders_detail)
                {
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn)) //如果是燈類產品要做燈位復原動作
                    {
                        var n = db0.Light_Site.First(x => x.Y == this.LightYear && x.light_name == item.light_name);
                        n.is_sellout = "0";
                        n.IsReject = true;
                        n.C_UpdateDateTime = DateTime.Now;

                        if (rjt == null) //退燈位記錄
                        {
                            rjt = new Reject()
                            {
                                reject_id = GetNewId(ProcCore.Business.CodeTable.Reject),
                                orders_sn = md.orders_sn,
                                reject_date = DateTime.Now,
                                user_id = this.UserId,
                                total = tran_orders_detail.Sum(x => x.price),
                                y = this.LightYear
                            };

                            db0.Reject.Add(rjt);
                        }

                        Reject_Detail rjts = new Reject_Detail()
                        {
                            reject_detail_id = GetNewId(ProcCore.Business.CodeTable.Reject_Detail),
                            reject_id = rjt.reject_id,
                            light_site_id = n.light_site_id,
                            light_name = n.light_name,
                            price = (int)n.price,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            orders_detail_id = item.orders_detail_id
                        };

                        rjt.Reject_Detail.Add(rjts);
                    }
                    db0.Orders_Detail.Remove(item);
                }

                db0.SaveChanges();
                tx.Complete();

                db0.Dispose();
                tx.Dispose();

                var dbN = getDB();
                var new_orders = dbN.Orders.Find(md.orders_sn);
                new_orders.getOrders_Detail = dbN.Orders_Detail.Where(x => x.orders_sn == md.orders_sn).ToList();
                r.data = new_orders;
                dbN.Dispose();
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
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpPost]
        public string AddOrdersFortune(cartMaster master, string product_sn, int[] member_detail_ids)
        {
            rAjaxGetData<string> r = new rAjaxGetData<string>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (member_detail_ids == null)
            {
                r.result = false;
                r.message = "尚未選擇會員";
                return defJSON(r);
            }

            if (member_detail_ids.Length > CommWebSetup.FortuneLimit)
            {
                r.result = false;
                r.message = string.Format("只能選擇{0}位成員", CommWebSetup.FortuneLimit);
                return defJSON(r);
            }

            if (string.IsNullOrEmpty(product_sn))
            {
                r.result = false;
                r.message = "尚未選擇福燈";
                return defJSON(r);
            }

            if (cart == null)
            {
                r.result = false;
                r.message = "購物車Session無法取得";
                return defJSON(r);
            }
            var db0 = getDB();
            var tx = RepeatableReadAsyncScope();
            try
            {
                var cartProduct = db0.Product.Find(product_sn);

                cartDetail cart_detail = new cartDetail()
                {
                    member_detail_id = cart.member_detail_id,
                    member_name = cart.member_name,
                    product_sn = product_sn,
                    product_name = cartProduct.product_name,
                    light_name = cartProduct.product_name,
                    tel = cart.tel,
                    price = cartProduct.price,
                    address = cart.address,
                    gender = cart.gender,
                    race = 0,
                    gold = 0,
                    light_site_id = master.Item.FirstOrDefault().light_site_id
                };

                cart.Item.Clear();
                cart.Item.Add(cart_detail);

                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (master.is_light_serial)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (master.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                string getNewOrderSN = GetOrderNewSerial(); //取得新訂單編號

                #region 訂單主檔 明細 福燈
                Orders m = new Orders()
                {
                    orders_sn = getNewOrderSN,
                    y = this.LightYear,
                    member_detail_id = master.member_detail_id,
                    member_id = master.member_id,
                    member_name = master.member_name,
                    tel = master.tel,
                    zip = master.zip,
                    address = master.address,
                    gender = master.gender,
                    新增時間 = DateTime.Now,
                    C_InsertDateTime = DateTime.Now,
                    transation_date = DateTime.Now,
                    InsertUserId = this.UserId,
                    total = cart.Item.Sum(x => x.price),
                    mobile = master.mobile,
                    orders_state = (int)Orders_State.complete,
                    orders_type = (int)Orders_Type.fortune_order
                };

                foreach (var item in cart.Item)
                {
                    Light_Site product_light = null;
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn))
                    {
                        #region 點燈類產品處理
                        if (item.light_site_id == 0) //屬於電腦選位類型
                        {
                            var getNewLight = getLightStratNum.Where(x => x.product_sn == item.product_sn).First();

                            product_light = db0.Light_Site.
                                    Where(x => x.Y == this.LightYear &&
                                        x.product_sn == item.product_sn &&
                                        x.is_sellout == "0" &&
                                        x.light_site_id >= getNewLight.start_num)
                                        .OrderBy(x => x.light_site_id)
                                        .Take(1)
                                        .First();

                            product_light.is_sellout = "1";
                            product_light.C_UpdateDateTime = DateTime.Now;
                            db0.SaveChanges();
                        }
                        else
                        { //屬於手動選位類型 主副斗直接選取燈位
                            string sqlLight = @"Select 序號 as 'light_site_id',位置名稱 as 'light_name',年度 as 'Y',空位 as 'is_sellout',產品編號 as 'product_sn',價格 as 'price',
                                                       _Hiden as 'C_Hiden',_InsertUserID as 'C_InsertUserID',_InsertDateTime as 'C_InsertDateTime',_UpdateUserID as 'C_UpdateUserID',
                                                       _UpdateDateTime as 'C_UpdateDateTime',_LockUserID as 'C_LockUserID',_LockDateTime as 'C_LockDateTime',_LockState as 'C_LockState',IsReject
                                                FROM 點燈位置資料表 WITH (ROWLOCK, UPDLOCK)  Where 序號 = @id";
                            product_light = db0.Database.SqlQuery<Light_Site>(sqlLight, new SqlParameter("@id", item.light_site_id)).FirstOrDefault();
                            //product_light = db0.Light_Site.Where(x => x.light_site_id == item.light_site_id).FirstOrDefault();
                            if (product_light == null)
                            {
                                throw new Exception("無法查詢福燈燈位:" + item.light_site_id);
                            }
                            if (product_light.is_sellout != "0")
                            {
                                throw new Exception("此福燈燈位:" + product_light.light_name + ",已被使用!");
                            }
                            product_light = db0.Light_Site.Where(x => x.light_site_id == item.light_site_id).FirstOrDefault();
                            product_light.is_sellout = "1";
                            product_light.C_UpdateDateTime = DateTime.Now;
                            db0.SaveChanges();
                        }
                        #endregion
                    }

                    m.Orders_Detail.Add(new Orders_Detail()
                    {
                        orders_sn = getNewOrderSN,
                        product_sn = item.product_sn,
                        product_name = item.product_name,
                        light_name = product_light == null ? null : product_light.light_name,
                        memo = product_light == null ? null : product_light.light_site_id.ToString(),
                        address = item.address,
                        l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                        Y = this.LightYear,
                        member_detail_id = item.member_detail_id,
                        member_name = item.member_name,
                        price = item.price,
                        amt = 1,
                        race = item.race,
                        gold = item.gold,
                        manjushri = item.manjushri,
                        gender = item.gender,
                        born_sign = item.born_sign,
                        born_time = item.born_time,
                        購買時間 = DateTime.Now,
                        i_InsertDateTime = DateTime.Now,
                        經手人 = this.UserName,
                        C_InsertDateTime = DateTime.Now,
                        C_InsertUserID = this.UserId,
                        i_InsertUserID = this.UserId,
                        detail_sort = 1
                    });
                }

                db0.Orders.Add(m);

                #region 福燈處理
                foreach (var member_detail_id in member_detail_ids)
                {
                    var get_member = db0.Member_Detail.Find(member_detail_id);

                    Fortune_Light f = new Fortune_Light()
                    {
                        Y = this.LightYear,
                        fortune_light_id = GetNewId(ProcCore.Business.CodeTable.Fortune_Light),
                        member_detail_id = member_detail_id,
                        member_name = get_member.member_name,
                        order_sn = getNewOrderSN
                    };
                    db0.Fortune_Light.Add(f);
                }
                #endregion
                #endregion
                db0.SaveChanges();
                tx.Complete();

                m.getOrders_Detail = m.Orders_Detail.ToArray();
                r.data = m.orders_sn;
                return defJSON(r);
            }
            catch (Exception ex)
            {
                r.result = false;
                r.message = ex.Message;
                Log.Write(ex.Message + ex.StackTrace);
                return defJSON(r);
            }
            finally
            {
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpPut]
        public string UpdateOrdersFortune(cartMaster master, int[] member_detail_ids)
        {
            rAjaxGetData<Orders> r = new rAjaxGetData<Orders>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (member_detail_ids == null)
            {
                r.result = false;
                r.message = "尚未選擇會員";
                return defJSON(r);
            }

            if (member_detail_ids.Length > CommWebSetup.FortuneLimit)
            {
                r.result = false;
                r.message = string.Format("只能選擇{0}位成員", CommWebSetup.FortuneLimit);
                return defJSON(r);
            }
            if (cart == null)
            {
                r.result = false;
                r.message = "購物車系統未配置";
                return defJSON(r);
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getNowFortune = db0.Fortune_Light.Where(x => x.order_sn == master.orders_sn);

                foreach (var item in getNowFortune)
                {
                    item.tran_mark = true;
                }
                db0.SaveChanges();

                foreach (var item in member_detail_ids)
                {
                    var md = getNowFortune.Where(x => x.member_detail_id == item).FirstOrDefault();
                    if (md == null)
                    {
                        var mb = db0.Member_Detail.Find(item);
                        var m = new Fortune_Light()
                        {
                            fortune_light_id = GetNewId(CodeTable.Fortune_Light),
                            order_sn = master.orders_sn,
                            Y = this.LightYear,
                            member_detail_id = item,
                            member_name = mb.member_name,
                            tran_mark = false
                        };

                        db0.Fortune_Light.Add(m);
                    }
                    else
                    {
                        md.tran_mark = false;
                    }
                    db0.SaveChanges();
                }

                var del_item = getNowFortune.Where(x => x.tran_mark == true);
                db0.Fortune_Light.RemoveRange(del_item);

                db0.SaveChanges();
                tx.Complete();

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
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpGet]
        public string DeleteOrdersFortune(string orders_sn)
        {
            ResultInfo r = new ResultInfo();

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var item_orders_detail = db0.Orders_Detail.Where(x => x.orders_sn == orders_sn).First();
                var item_light_site = db0.Light_Site.Where(x => x.Y == this.LightYear && x.light_name == item_orders_detail.light_name).First();
                item_light_site.IsReject = true;
                item_light_site.is_sellout = "0";
                item_light_site.C_UpdateDateTime = DateTime.Now;

                #region 退訂燈位
                Reject rjt = new Reject()
                {
                    reject_id = GetNewId(ProcCore.Business.CodeTable.Reject),
                    orders_sn = orders_sn,
                    reject_date = DateTime.Now,
                    user_id = this.UserId,
                    total = item_orders_detail.price,
                    y = this.LightYear
                };

                db0.Reject.Add(rjt);

                Reject_Detail rjts = new Reject_Detail()
                {
                    reject_detail_id = GetNewId(ProcCore.Business.CodeTable.Reject_Detail),
                    reject_id = rjt.reject_id,
                    light_site_id = item_light_site.light_site_id,
                    light_name = item_light_site.light_name,
                    price = (int)item_light_site.price,
                    Y = this.LightYear,
                    member_detail_id = item_orders_detail.member_detail_id,
                    orders_detail_id = item_orders_detail.orders_detail_id
                };
                rjt.Reject_Detail.Add(rjts);
                #endregion

                db0.Orders_Detail.Remove(item_orders_detail);
                var item_fortune = db0.Fortune_Light.Where(x => x.order_sn == orders_sn);
                db0.Fortune_Light.RemoveRange(item_fortune);

                var o = db0.Orders.Find(orders_sn);
                o.orders_state = (int)Orders_State.reject;
                o.total = 0;
                db0.SaveChanges();
                tx.Complete();
                r.result = true;
            }
            catch (Exception ex)
            {
                r.message = ex.Message;
                r.result = false;
            }
            finally
            {
                tx.Dispose();
                db0.Dispose();
            }
            return defJSON(r);
        }

        //大中小斗
        [HttpPost]
        public string AddSDLight(cartMaster md)
        {
            rAjaxGetData<string> r = new rAjaxGetData<string>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "未選購任何產品";
                return defJSON(r);
            }
            else
            {
                if (cart.Item.Count == 0)
                {
                    r.result = false;
                    r.message = "未選購任何產品";
                    return defJSON(r);
                }
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                string getNewOrderSN = GetOrderNewSerial(); //取得新訂單編號

                #region 訂單主檔新增
                Orders m = new Orders()
                {
                    orders_sn = getNewOrderSN,
                    y = this.LightYear,
                    member_detail_id = md.member_detail_id,
                    member_id = md.member_id,
                    member_name = md.member_name,
                    tel = md.tel,
                    zip = md.zip,
                    address = md.address,
                    gender = md.gender,
                    新增時間 = DateTime.Now,
                    C_InsertDateTime = DateTime.Now,
                    transation_date = DateTime.Now,
                    InsertUserId = this.UserId,
                    total = cart.Item.Sum(x => x.price),
                    mobile = md.mobile,
                    orders_state = (int)Orders_State.complete,
                    orders_type = (int)Orders_Type.sdlight
                };

                foreach (var item in cart.Item)
                {
                    Light_Site product_light = null;
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn))
                    {
                        #region 點燈類產品處理
                        if (item.light_site_id == 0) //屬於電腦選位類型
                        {
                            var getNewLight = getLightStratNum.Where(x => x.product_sn == item.product_sn).First();

                            product_light = db0.Light_Site.
                                    Where(x => x.Y == this.LightYear &&
                                        x.product_sn == item.product_sn &&
                                        x.is_sellout == "0" &&
                                        x.light_site_id >= getNewLight.start_num)
                                        .OrderBy(x => x.light_site_id)
                                        .Take(1)
                                        .First();

                            product_light.is_sellout = "1";
                            product_light.C_UpdateDateTime = DateTime.Now;
                            product_light.C_UpdateUserID = this.UserId;
                            db0.SaveChanges();
                        }
                        else
                        { //屬於手動選位類型 主副斗直接選取燈位

                        }
                        #endregion
                    }
                    else
                    {
                        #region 非點燈類產品處理

                        #endregion
                    }

                    m.Orders_Detail.Add(new Orders_Detail()
                    {
                        orders_sn = getNewOrderSN,
                        product_sn = item.product_sn,
                        product_name = item.product_name,
                        light_name = product_light == null ? null : product_light.light_name,
                        memo = product_light == null ? null : product_light.light_site_id.ToString(),
                        address = item.address,
                        l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                        Y = this.LightYear,
                        member_detail_id = item.member_detail_id,
                        member_name = item.member_name,
                        price = item.price,
                        amt = 1,
                        race = item.race,
                        gold = item.gold,
                        manjushri = item.manjushri,
                        gender = item.gender,
                        born_sign = item.born_sign,
                        born_time = item.born_time,
                        購買時間 = DateTime.Now,
                        i_InsertDateTime = DateTime.Now,
                        經手人 = this.UserName,
                        C_InsertDateTime = DateTime.Now,
                        C_InsertUserID = this.UserId,
                        i_InsertUserID = this.UserId,
                        detail_sort = item.detail_sort
                    });
                }

                db0.Orders.Add(m);
                #endregion
                db0.SaveChanges();
                tx.Complete();

                m.getOrders_Detail = m.Orders_Detail.ToArray();
                r.data = m.orders_sn;
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
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpPut]
        public string UpdateSDLight(cartMaster md)
        {
            rAjaxGetData<Orders> r = new rAjaxGetData<Orders>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "購物車系統未配置";
                return defJSON(r);
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.Where(x => x.isOnOrder == false).GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial && groupOrderProductQty.Count() > 0)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                #region 訂單主檔修改
                IList<Orders_Detail> orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn).ToList();

                //標記準備異動
                foreach (var n in orders_detail)
                {
                    n.tran_mark = true;
                }

                db0.SaveChanges();

                #region Order Master
                Orders orders = db0.Orders.Find(md.orders_sn);

                orders.address = md.address;
                orders.member_name = md.member_name;
                orders.mobile = md.mobile;
                orders.tel = md.tel;
                orders.zip = md.zip;
                orders.gender = md.gender;
                orders.total = cart.Item.Sum(x => x.price);
                orders.C_UpdateDateTime = DateTime.Now;
                orders.C_UpdateUserID = this.UserId;
                //修改訂單時 新增的東西仍算原新增訂單者
                var ouser_id = orders.InsertUserId;

                #endregion

                foreach (var item in cart.Item)
                {


                    if (orders_detail.Any(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn))
                    {
                        var get_detail_item = orders_detail.First(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn);
                        get_detail_item.tran_mark = false;
                        get_detail_item.member_detail_id = item.member_detail_id;
                        get_detail_item.member_name = item.member_name;
                        get_detail_item.price = item.price;
                        get_detail_item.race = item.race;
                        get_detail_item.gold = item.gold;
                        get_detail_item.C_UpdateDateTime = DateTime.Now;
                        get_detail_item.l_birthday = item.LY + "/" + item.LM + "/" + item.LD;
                        get_detail_item.born_sign = item.born_sign;
                        get_detail_item.born_time = item.born_time;
                        get_detail_item.gender = item.gender;
                        get_detail_item.detail_sort = item.detail_sort;
                    }
                    else
                    {
                        Light_Site product_light = null;
                        if (getLightProduct.Any(x => x.product_sn == item.product_sn)) //如果是點燈類產品
                        {
                            #region 點燈類產品處理
                            if (item.light_site_id == 0 && item.isOnOrder == false) //屬於電腦選位類型
                            {
                                var getNewLight = getLightStratNum.Where(x => x.product_sn == item.product_sn).FirstOrDefault();
                                if (getNewLight != null)
                                {
                                    product_light = db0.Light_Site.
                                    Where(x => x.Y == this.LightYear &&
                                        x.product_sn == item.product_sn &&
                                        x.is_sellout == "0" &&
                                        x.light_site_id >= getNewLight.start_num)
                                        .OrderBy(x => x.light_site_id)
                                        .Take(1)
                                        .First();

                                    product_light.is_sellout = "1";
                                    product_light.C_UpdateDateTime = DateTime.Now;
                                    product_light.C_UpdateUserID = this.UserId;
                                    db0.SaveChanges();
                                }
                            }
                            else
                            { //屬於手動選位類型 主副斗直接選取燈位

                            }
                            #endregion
                        }

                        orders.Orders_Detail.Add(new Orders_Detail()
                        {
                            orders_sn = md.orders_sn,
                            product_sn = item.product_sn,
                            product_name = item.product_name,
                            light_name = product_light == null ? null : product_light.light_name,
                            memo = product_light == null ? null : product_light.light_site_id.ToString(),
                            address = item.address,
                            l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            member_name = item.member_name,
                            price = item.price,
                            amt = 1,
                            race = item.race,
                            gold = item.gold,
                            manjushri = item.manjushri,
                            gender = item.gender,
                            born_sign = item.born_sign,
                            born_time = item.born_time,
                            購買時間 = DateTime.Now,
                            i_InsertDateTime = DateTime.Now,
                            經手人 = this.UserName,
                            C_InsertDateTime = DateTime.Now,
                            C_InsertUserID = ouser_id,
                            i_InsertUserID = (int)ouser_id,
                            C_UpdateUserID = this.UserId,
                            C_UpdateDateTime = DateTime.Now,
                            tran_mark = false,
                            detail_sort = item.detail_sort
                        });
                    }
                }

                #endregion
                db0.SaveChanges();

                //異動仍為True代表要做刪除
                IList<Orders_Detail> tran_orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn && x.tran_mark == true).ToList();

                Reject rjt = null;

                foreach (var item in tran_orders_detail)
                {
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn)) //如果是燈類產品要做燈位復原動作
                    {
                        var n = db0.Light_Site.First(x => x.Y == this.LightYear && x.light_name == item.light_name);
                        n.is_sellout = "0";
                        n.IsReject = true;
                        n.C_UpdateDateTime = DateTime.Now;

                        if (rjt == null) //退燈位記錄
                        {
                            rjt = new Reject()
                            {
                                reject_id = GetNewId(ProcCore.Business.CodeTable.Reject),
                                orders_sn = md.orders_sn,
                                reject_date = DateTime.Now,
                                user_id = this.UserId,
                                total = tran_orders_detail.Sum(x => x.price),
                                y = this.LightYear
                            };

                            db0.Reject.Add(rjt);
                        }

                        Reject_Detail rjts = new Reject_Detail()
                        {
                            reject_detail_id = GetNewId(ProcCore.Business.CodeTable.Reject_Detail),
                            reject_id = rjt.reject_id,
                            light_site_id = n.light_site_id,
                            light_name = n.light_name,
                            price = (int)n.price,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            orders_detail_id = item.orders_detail_id
                        };

                        rjt.Reject_Detail.Add(rjts);
                    }
                    db0.Orders_Detail.Remove(item);
                }

                db0.SaveChanges();
                tx.Complete();

                db0.Dispose();
                tx.Dispose();

                var dbN = getDB();
                var new_orders = dbN.Orders.Find(md.orders_sn);
                new_orders.getOrders_Detail = dbN.Orders_Detail.Where(x => x.orders_sn == md.orders_sn).ToList();
                r.data = new_orders;
                dbN.Dispose();
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
                db0.Dispose();
                tx.Dispose();
            }
        }

        //主副斗
        [HttpPost]
        public string AddMDLight(cartMaster md)
        {
            rAjaxGetData<string> r = new rAjaxGetData<string>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "未選購任何產品";
                return defJSON(r);
            }
            else
            {
                if (cart.Item.Count == 0)
                {
                    r.result = false;
                    r.message = "未選購任何產品";
                    return defJSON(r);
                }
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                string getNewOrderSN = GetOrderNewSerial(); //取得新訂單編號

                #region 訂單主檔新增
                Orders m = new Orders()
                {
                    orders_sn = getNewOrderSN,
                    y = this.LightYear,
                    member_detail_id = md.member_detail_id,
                    member_id = md.member_id,
                    member_name = md.member_name,
                    tel = md.tel,
                    zip = md.zip,
                    address = md.address,
                    gender = md.gender,
                    新增時間 = DateTime.Now,
                    C_InsertDateTime = DateTime.Now,
                    transation_date = DateTime.Now,
                    InsertUserId = this.UserId,
                    total = cart.Item.Sum(x => x.price),
                    mobile = md.mobile,
                    orders_state = (int)Orders_State.complete,
                    orders_type = (int)Orders_Type.mdlight
                };

                foreach (var item in cart.Item)
                {
                    Light_Site product_light = db0.Light_Site.Where(x => x.light_site_id == item.light_site_id).FirstOrDefault();

                    product_light.is_sellout = "1";

                    if (product_light == null)
                    {
                        throw new Exception("無法查詢主副斗燈位:" + item.light_site_id);
                    }

                    m.Orders_Detail.Add(new Orders_Detail()
                    {
                        orders_sn = getNewOrderSN,
                        product_sn = item.product_sn,
                        product_name = item.product_name,
                        light_name = product_light == null ? null : product_light.light_name,
                        memo = product_light == null ? null : product_light.light_site_id.ToString(),
                        address = item.address,
                        l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                        Y = this.LightYear,
                        member_detail_id = item.member_detail_id,
                        member_name = item.member_name,
                        price = item.price,
                        amt = 1,
                        race = item.race,
                        gold = item.gold,
                        manjushri = item.manjushri,
                        gender = item.gender,
                        born_sign = item.born_sign,
                        born_time = item.born_time,
                        購買時間 = DateTime.Now,
                        i_InsertDateTime = DateTime.Now,
                        經手人 = this.UserName,
                        C_InsertDateTime = DateTime.Now,
                        C_InsertUserID = this.UserId,
                        i_InsertUserID = this.UserId,
                        detail_sort = item.detail_sort
                    });
                }

                db0.Orders.Add(m);
                #endregion
                db0.SaveChanges();
                tx.Complete();

                m.getOrders_Detail = m.Orders_Detail.ToArray();
                r.data = m.orders_sn;
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
                db0.Dispose();
                tx.Dispose();
            }
        }
        [HttpPut]
        public string UpdateMDLight(cartMaster md)
        {
            rAjaxGetData<Orders> r = new rAjaxGetData<Orders>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];

            if (cart == null)
            {
                r.result = false;
                r.message = "購物車系統未配置";
                return defJSON(r);
            }

            var db0 = getDB();
            var tx = defAsyncScope();
            try
            {
                var getLightProduct = db0.Product.Where(x => x.isLight); //取得點燈類產品資訊

                #region 燈位產品類別數量及連續配位檢查

                //先群組各項產品的訂購量
                var groupOrderProductQty = cart.Item.Where(x => x.isOnOrder == false).GroupBy(x => x.product_sn,
                (key, Num) => new
                {
                    product_sn = key,
                    needQty = Num.Count()
                });

                //查詢連續配位SQL語法
                string sql = string.Empty;
                if (md.is_light_serial && groupOrderProductQty.Count() > 0)
                {
                    sql = @"select start_num=min(序號),end_num=max(序號),nums= max(序號)-min(序號) + 1 from 
                    (select row_number()over (order by 序號) as rid,* from 點燈位置資料表 Where 年度 = @Y And 產品編號=@P And 空位='0') A 
	                group by 序號-RID";
                }

                //記錄取燈位時的始啟燈位編號
                IList<GetLightStratNum> getLightStratNum = new List<GetLightStratNum>();

                foreach (var getOrderQty in groupOrderProductQty) //點燈類產品數量檢查是否足夠
                {
                    var getProductInfo = getLightProduct.Where(x => x.product_sn == getOrderQty.product_sn).FirstOrDefault();

                    if (getProductInfo != null) //屬於燈位產品
                    {
                        #region 數量檢查是否足夠
                        int statisticsCount = db0.Light_Site.Where(x => x.Y == this.LightYear
                                        && x.product_sn == getOrderQty.product_sn && x.is_sellout == "0").Count(); //統計燈位目前還剩的數量

                        if (statisticsCount < getOrderQty.needQty)
                        {
                            r.message = string.Format("{0}燈位位置不足或已用完，剩餘數量:{1}", getProductInfo.product_name, statisticsCount);
                            r.result = false;
                            return defJSON(r);
                        }
                        #endregion

                        #region 連續配位資料處理
                        if (md.is_light_serial) //採用連續配位
                        {
                            SqlParameter[] sps = new SqlParameter[] { new SqlParameter("@Y", this.LightYear), new SqlParameter("@P", getOrderQty.product_sn) };
                            var getLightSerial = db0.Database.SqlQuery<LightSerial>(sql, sps).ToList();
                            var isHaveSerial = getLightSerial.Any(x => x.nums >= getOrderQty.needQty);
                            if (!isHaveSerial)
                            {
                                r.message = string.Format("{0}燈位位置已無連續配位可用", getProductInfo.product_name);
                                r.result = false;
                                return defJSON(r);
                            }

                            var light_serial_ok_start = getLightSerial.Where(x => x.nums >= getOrderQty.needQty)
                                .OrderBy(x => x.start_num)
                                .First();

                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = light_serial_ok_start.start_num
                            });
                        }
                        else
                        {
                            //不採用連續配位
                            getLightStratNum.Add(new GetLightStratNum()
                            {
                                product_sn = getOrderQty.product_sn,
                                start_num = 0
                            });
                        }
                        #endregion
                    }
                }
                #endregion

                #region 訂單主檔修改
                IList<Orders_Detail> orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn).ToList();

                //標記準備異動
                foreach (var n in orders_detail)
                {
                    n.tran_mark = true;
                }

                db0.SaveChanges();

                #region Order Master
                Orders orders = db0.Orders.Find(md.orders_sn);

                orders.address = md.address;
                orders.member_name = md.member_name;
                orders.mobile = md.mobile;
                orders.tel = md.tel;
                orders.zip = md.zip;
                orders.gender = md.gender;
                orders.total = cart.Item.Sum(x => x.price);
                orders.C_UpdateDateTime = DateTime.Now;
                orders.C_UpdateUserID = this.UserId;
                //修改訂單時 新增的東西仍算原新增訂單者
                var ouser_id = orders.InsertUserId;

                #endregion

                foreach (var item in cart.Item)
                {


                    if (orders_detail.Any(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn))
                    {
                        var get_detail_item = orders_detail.First(x => x.member_detail_id == item.member_detail_id && x.product_sn == item.product_sn);
                        get_detail_item.tran_mark = false;
                        get_detail_item.member_detail_id = item.member_detail_id;
                        get_detail_item.member_name = item.member_name;
                        get_detail_item.price = item.price;
                        get_detail_item.race = item.race;
                        get_detail_item.gold = item.gold;
                        get_detail_item.C_UpdateDateTime = DateTime.Now;
                        get_detail_item.l_birthday = item.LY + "/" + item.LM + "/" + item.LD;
                        get_detail_item.born_sign = item.born_sign;
                        get_detail_item.born_time = item.born_time;
                        get_detail_item.gender = item.gender;
                        get_detail_item.detail_sort = item.detail_sort;
                    }
                    else
                    {
                        Light_Site product_light = db0.Light_Site.Where(x => x.light_site_id == item.light_site_id).FirstOrDefault();

                        orders.Orders_Detail.Add(new Orders_Detail()
                        {
                            orders_sn = md.orders_sn,
                            product_sn = item.product_sn,
                            product_name = item.product_name,
                            light_name = product_light == null ? null : product_light.light_name,
                            memo = product_light == null ? null : product_light.light_site_id.ToString(),
                            address = item.address,
                            l_birthday = item.LY + "/" + item.LM + "/" + item.LD,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            member_name = item.member_name,
                            price = item.price,
                            amt = 1,
                            race = item.race,
                            gold = item.gold,
                            manjushri = item.manjushri,
                            gender = item.gender,
                            born_sign = item.born_sign,
                            born_time = item.born_time,
                            購買時間 = DateTime.Now,
                            i_InsertDateTime = DateTime.Now,
                            經手人 = this.UserName,
                            C_InsertDateTime = DateTime.Now,
                            C_InsertUserID = ouser_id,
                            i_InsertUserID = (int)ouser_id,
                            C_UpdateUserID = this.UserId,
                            C_UpdateDateTime = DateTime.Now,
                            tran_mark = false,
                            detail_sort = item.detail_sort
                        });
                    }
                }

                #endregion
                db0.SaveChanges();

                //異動仍為True代表要做刪除
                IList<Orders_Detail> tran_orders_detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == md.orders_sn && x.tran_mark == true).ToList();

                Reject rjt = null;

                foreach (var item in tran_orders_detail)
                {
                    if (getLightProduct.Any(x => x.product_sn == item.product_sn)) //如果是燈類產品要做燈位復原動作
                    {
                        var n = db0.Light_Site.First(x => x.Y == this.LightYear && x.light_name == item.light_name);
                        n.is_sellout = "0";
                        n.IsReject = true;
                        n.C_UpdateDateTime = DateTime.Now;

                        if (rjt == null) //退燈位記錄
                        {
                            rjt = new Reject()
                            {
                                reject_id = GetNewId(ProcCore.Business.CodeTable.Reject),
                                orders_sn = md.orders_sn,
                                reject_date = DateTime.Now,
                                user_id = this.UserId,
                                total = tran_orders_detail.Sum(x => x.price),
                                y = this.LightYear
                            };

                            db0.Reject.Add(rjt);
                        }

                        Reject_Detail rjts = new Reject_Detail()
                        {
                            reject_detail_id = GetNewId(ProcCore.Business.CodeTable.Reject_Detail),
                            reject_id = rjt.reject_id,
                            light_site_id = n.light_site_id,
                            light_name = n.light_name,
                            price = (int)n.price,
                            Y = this.LightYear,
                            member_detail_id = item.member_detail_id,
                            orders_detail_id = item.orders_detail_id
                        };

                        rjt.Reject_Detail.Add(rjts);
                    }
                    db0.Orders_Detail.Remove(item);
                }

                db0.SaveChanges();
                tx.Complete();

                db0.Dispose();
                tx.Dispose();

                var dbN = getDB();
                var new_orders = dbN.Orders.Find(md.orders_sn);
                new_orders.getOrders_Detail = dbN.Orders_Detail.Where(x => x.orders_sn == md.orders_sn).ToList();
                r.data = new_orders;
                dbN.Dispose();
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
                db0.Dispose();
                tx.Dispose();
            }
        }


        [HttpGet]
        public string GetToSession(string orders_sn)
        {
            rAjaxGetData<cartMaster> r = new rAjaxGetData<cartMaster>();
            using (var db0 = getDB())
            {
                var order = db0.Orders.Find(orders_sn);
                order.getOrders_Detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == orders_sn)
                    .OrderBy(x => x.detail_sort).ThenBy(x => x.orders_detail_id).ToList();

                if (order.getOrders_Detail.Any(x => x.product_sn == "380" && x.product_sn == "381"))
                {
                    r.message = Url.Content("~/_SysAdm/OnlineService/Order_MD/upt.asp?p0=" + orders_sn);
                    r.result = true;
                    return defJSON(r);
                }

                if (order.getOrders_Detail.Any(x => x.product_sn == "382" && x.product_sn == "383" && x.product_sn == "384"))
                {
                    r.message = Url.Content("~/_SysAdm/OnlineService/Order_MBMS/upt.asp?p0=" + orders_sn);
                    r.result = true;
                    return defJSON(r);
                }

                cartMaster cart = new cartMaster()
                {
                    member_id = order.member_id,
                    member_detail_id = order.member_detail_id,
                    member_name = order.member_name,
                    address = order.address,
                    zip = order.zip,
                    gender = order.gender,
                    tel = order.tel,
                    mobile = order.mobile,
                    total = order.total,
                    orders_sn = order.orders_sn,
                    transation_date = order.transation_date
                };

                cart.Item = new List<cartDetail>();

                foreach (var detail in order.getOrders_Detail)
                {
                    string[] ss = null;
                    if (detail.l_birthday != null)
                    {
                        ss = detail.l_birthday.Split('/');
                    }

                    var cartitem = new cartDetail()
                    {
                        member_detail_id = detail.member_detail_id,
                        member_name = detail.member_name,
                        product_sn = detail.product_sn,
                        product_name = detail.product_name,
                        price = detail.price,
                        address = detail.address,
                        born_sign = detail.born_sign,
                        born_time = detail.born_time,
                        gender = detail.gender,
                        race = detail.race,
                        gold = detail.gold,
                        manjushri = detail.manjushri,
                        l_birthday = detail.l_birthday,
                        light_name = detail.light_name,
                        isOnOrder = true,
                        detail_sort = detail.detail_sort
                    };

                    if (ss != null && ss.Length == 3)
                    {
                        cartitem.LY = int.Parse(ss[0]);
                        cartitem.LM = int.Parse(ss[1]);
                        cartitem.LD = int.Parse(ss[2]);
                    }
                    cart.Item.Add(cartitem);

                }
                cart.total = cart.Item.Sum(x => x.price);
                cart.race = cart.Item.Sum(x => x.race);
                cart.gold = cart.Item.Sum(x => x.gold);

                Session[this.sessionCart] = cart;
                r = new rAjaxGetData<cartMaster>() { data = cart, result = true };
            }

            return defJSON(r);
        }
        [HttpGet]
        public string GetCartDetailFromSession()
        {
            rAjaxGetItems<cartDetail> r = new rAjaxGetItems<cartDetail>();
            cartMaster cart = (cartMaster)Session[this.sessionCart];
            r.data = cart.Item;
            return defJSON(r);
        }
        [HttpGet]
        public string CheckOrderType(string orders_sn)
        {
            ResultInfo r = new ResultInfo();
            r.message = "";
            using (var db0 = getDB())
            {
                var getOrders_Detail = db0.Orders_Detail
                    .Where(x => x.orders_sn == orders_sn)
                    .OrderBy(x => x.orders_detail_id).ToList();

                //if (getOrders_Detail.Any(x => x.product_sn == "380" || x.product_sn == "381"))
                //{
                //    r.message = Url.Content("~/_SysAdm/OnlineService/Order_MD/upt.asp?p0=" + orders_sn);
                //    r.result = true;
                //    return defJSON(r);
                //}

                //if (getOrders_Detail.Any(x => x.product_sn == "382" || x.product_sn == "383" && x.product_sn == "384"))
                //{
                //    r.message = Url.Content("~/_SysAdm/OnlineService/Order_MBMS/upt.asp?p0=" + orders_sn);
                //    r.result = true;
                //    return defJSON(r);
                //}

                //if (getOrders_Detail.First().Product.category == "福燈")
                //{
                //    r.message = Url.Content("~/Orders/Fortune?p0=" + orders_sn);
                //    r.result = true;
                //    return defJSON(r);
                //}
            }

            return defJSON(r);
        }
        /// <summary>
        /// 取得訂單編號
        /// </summary>
        /// <returns></returns>
        private string GetOrderNewSerial()
        {
            #region 處理取得訂單編號
            var db0 = getDB();

            string setDateFormat = DateTime.Now.ToString("yyMMdd");
            //string getLastOrderSN = db0.Orders
            //    .Where(x => x.orders_sn.StartsWith(setDateFormat))
            //    .Max(x => x.orders_sn);

            string getLastOrderSN = db0.Database.SqlQuery<string>("Select Max(訂單編號) From 訂單主檔 (UPDLOCK) Where 訂單編號 like '" + setDateFormat + "%'").FirstOrDefault();

            if (getLastOrderSN == null)
            {
                return setDateFormat + "0001";
            }
            else
            {
                int getLastNumber = int.Parse(getLastOrderSN.Right(4));
                int setNewNumber = getLastNumber + 1;
                return setDateFormat + setNewNumber.ToString().PadLeft(4, '0');
            }
            #endregion
        }
    }
    public class LightSerial
    {
        public int start_num { get; set; }
        public int end_num { get; set; }
        public int nums { get; set; }
    }
    public class GetLightStratNum
    {
        public string product_sn { get; set; }
        public int start_num { get; set; }
    }
}
