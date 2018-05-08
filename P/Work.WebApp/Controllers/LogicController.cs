using ProcCore;
using ProcCore.Business.Logic;
using ProcCore.ReturnAjaxResult;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Work.WebApp.Models;
namespace DotWeb.Controllers
{
    public class LogicController : BusLogicController
    {
        // 此控製器for asp叫用
        public ActionResult Index()
        {
            if (Request.Browser.Browser == "IE")
            {
                if (
                    Request.Browser.Version == "8.0" ||
                    Request.Browser.Version == "7.0" ||
                    Request.Browser.Version == "6.0"
                    )
                {
                    return View();
                }
                else
                {
                    Response.Redirect("~/_SysAdm");
                }
            }
            else
            {
                Response.Redirect("~/_SysAdm");
            }
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_LightData_date(m_產品資料表 md)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            String returnPicturePath = String.Empty;

            a_產品資料表 ac = new a_產品資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

            if (md.EditType == EditModeType.Insert)
            {   //新增
                RunInsertEnd HResult = ac.InsertMaster(md, LoginUserID);
                rAjaxResult = HandleResultAjax(HResult, Resources.Res.Data_Insert_Success);
                //rAjaxResult.IdStr = HResult.InsertID;
            }
            else
            {
                //修改
                RunEnd HResult = ac.UpdateMaster(md, LoginUserID);
                rAjaxResult = HandleResultAjax(HResult, Resources.Res.Data_Update_Success);
                rAjaxResult.IdStr = md.產品編號;
            }

            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String btn_ChildFrm_EditProductDetail(m_產品表單處理 md)
        {
            pcar_MasterData car;

            #region MyRegion
            if (Session["ShoppingItems"] == null)
            {
                car = new pcar_MasterData();
                car.Item = new List<pcar_DetailData>();
                Session["ShoppingItems"] = car;
            }
            else
            {
                car = (pcar_MasterData)Session["ShoppingItems"];
            }
            #endregion

            ResultInfo rAjaxResult = new ResultInfo();
            a_產品資料表 ac = new a_產品資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

            try
            {
                RunOneDataEnd<m_產品資料表> r = ac.檢查訂購表單配置正確性(md, car);
                HandleRunEnd(r);

                #region Main Working
                if (car.Item == null)
                    car.Item = new List<pcar_DetailData>();

                car.戶長SN = md.MasterID;
                pcar_DetailData item = new pcar_DetailData();
                #region Detail Handle
                //主副斗燈因為要指定燈位需用到
                if (md.SF_EPD_p2_Sub != 0)
                {
                    item.點燈位置序號 = md.SF_EPD_p2_Sub;
                    a_點燈位置資料表 acLightSite = new a_點燈位置資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
                    RunOneDataEnd<m_點燈位置資料表> m_LightSite = acLightSite.g_取得可選用燈位(md.SF_EPD_p2_Sub, LoginUserID);
                    HandleRunEnd(m_LightSite);

                    item.點燈位置 = m_LightSite.SearchData.位置名稱;
                }

                item.價格 = md.SF_EPD_p16;
                item.產品編號 = md.SF_EPD_p2;
                item.產品名稱 = r.SearchData.產品名稱;
                item.會員編號 = md.SF_EPD_p1;
                item.申請人姓名 = md.SF_EPD_p1_Name;
                item.申請人地址 = md.SF_EPD_p7;
                item.申請人性別 = md.SF_EPD_p5;
                item.申請人時辰 = md.SF_EPD_p3;
                item.申請人生日 = md.SF_EPD_p6 + "/" + md.SF_EPD_p8 + "/" + md.SF_EPD_p17;
                item.申請人年齡 = "";
                item.祈福事項 = md.SF_EPD_p12;
                item.白米 = md.SF_EPD_p14;
                item.金牌 = md.SF_EPD_p15;
                item.文疏 = md.SF_EPD_p18;
                item.電話 = md.SF_EPD_p10;
                item.行動電話 = md.SF_EPD_p11;

                #endregion
                car.Item.Add(item);
                car.總額 = car.Item.Sum(x => x.價格);
                car.白米 = car.Item.Sum(x => x.白米);
                car.金牌 = car.Item.Sum(x => x.金牌);
                #endregion
                rAjaxResult.result = true;
                rAjaxResult.message = string.Empty;

            }
            catch (LogicError ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".Logic", ex);
                rAjaxLogErrHandle(ex, rAjaxResult);
            }
            catch (Exception ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name + ".System", ex);
                rAjaxSysErrHandle(ex, rAjaxResult);
            }

            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_ClearSessionOrderItems()
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            try
            {

                if (Session["ShoppingItems"] != null)
                {
                    Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Clear Car Session Start");
                    pcar_MasterData car = (pcar_MasterData)Session["ShoppingItems"];
                    if (car.Item != null)
                    {
                        car.Item.Clear();
                        car.Item = null;
                    }
                    car = null;
                    Session["ShoppingItems"] = null;
                    Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Clear Car Session End");
                }
                rAjaxResult.result = true;
                rAjaxResult.message = "";

            }
            catch (Exception ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                rAjaxResult.result = true;
                rAjaxResult.message = ex.Message;
            }

            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_RemoveSessionOrderItem(String OrderSerial, String MemberId, String ProdId)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            try
            {
                if (Session["ShoppingItems"] != null)
                {
                    pcar_MasterData car = (pcar_MasterData)Session["ShoppingItems"];
                    if (car.Item != null)
                    {
                        var item = car.Item.Find(x => x.會員編號 == MemberId && x.產品編號 == ProdId);
                        car.Item.Remove(item);

                        car.總額 = car.Item.Sum(x => x.價格);
                        car.白米 = car.Item.Sum(x => x.白米);
                        car.金牌 = car.Item.Sum(x => x.金牌);
                        Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "刪除購物車項目," + OrderSerial + "," + MemberId + "," + ProdId);
                    }
                    Session["ShoppingItems"] = car;
                }
                rAjaxResult.result = true;
                rAjaxResult.message = "";
            }
            catch (Exception ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                rAjaxResult.result = true;
                rAjaxResult.message = ex.Message;
            }

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_ViewSessionOrderItem(String MemberId, String ProdId)
        {
            pcar_DetailData item = null;
            if (Session["ShoppingItems"] != null)
            {
                pcar_MasterData car = (pcar_MasterData)Session["ShoppingItems"];
                if (car.Item != null)
                {
                    item = car.Item.Find(x => x.會員編號 == MemberId && x.產品編號 == ProdId);
                    Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, item.會員編號 + "," + item.產品編號 + "," + item.產品名稱);
                }
            }
            else
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "ShoppingItems is null!");
            }
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(item);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_UpdateOrderHandle(m_訂購人表單處理 md)
        {
            a_訂單主副 ac = new a_訂單主副() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            pcar_MasterData car = (pcar_MasterData)Session["ShoppingItems"];
            RunInsertEnd r = null;

            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            if (md.EditType == EditModeType.Insert)
            {
                r = ac.InsertData(LightYear, md, car, LoginUserID);
                if (r.Result)
                {
                    rAjaxResult.id = r.InsertId;
                    rAjaxResult.IdStr = r.CustomId.ToString();
                }
            }

            if (md.EditType == EditModeType.Modify)
            {
                r = ac.UpdateData(LightYear, md, car, LoginUserID);
                if (r.Result)
                    rAjaxResult.id = 0;
            }

            rAjaxResult.result = r.Result;
            rAjaxResult.message = r.ErrMessage;

            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// 從資料庫取得訂單資料
        /// </summary>
        /// <param name="OrderSerial"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_LoadOrderToSession(String OrderSerial)
        {
            Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "載入訂單..." + OrderSerial);

            pcar_MasterData car;

            if (Session["ShoppingItems"] != null)
            {
                car = (pcar_MasterData)Session["ShoppingItems"];
                if (car.Item != null)
                {
                    car.Item.Clear();
                    car.Item = null;
                }
                car = null;
                Session["ShoppingItems"] = null;
            }

            car = new pcar_MasterData();
            car.Item = new List<pcar_DetailData>();

            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            Boolean AllowEditProd = true;
            try
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "開始載入訂單..." + OrderSerial);
                a_訂單主檔 acM = new a_訂單主檔() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
                RunOneDataEnd<m_訂單主檔> rA = acM.GetDataMaster(OrderSerial, LoginUserID);
                if (!rA.Result)
                    throw new Exception(rA.ErrMessage);

                m_訂單主檔 mdM = rA.SearchData;

                if (mdM.新增時間 <= DateTime.Parse(DateTime.Now.AddDays(-1).ToString("yyyy/MM/dd") + " 15:00:00"))
                    AllowEditProd = false;
                else
                    AllowEditProd = true;


                a_訂單明細檔 acD = new a_訂單明細檔() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
                RunQueryPackage<m_訂單明細檔> rB = acD.SearchMaster(new q_訂單明細檔() { 訂單編號 = OrderSerial }, LoginUserID);
                if (!rB.Result)
                    throw new Exception(rB.ErrMessage);

                m_訂單明細檔[] mdDs = rB.SearchData;

                #region MyRegion
                car.戶長SN = mdM.戶長SN;
                car.姓名 = mdM.申請人姓名;
                car.性別 = mdM.申請人性別;
                car.地址 = mdM.申請人地址;
                car.電話 = mdM.申請人電話;
                car.手機 = mdM.申請人手機;
                car.Zip = mdM.郵遞區號;

                car.總額 = mdDs.Sum(x => x.價格);
                car.白米 = mdDs.Sum(x => x.白米);
                car.金牌 = mdDs.Sum(x => x.金牌);

                foreach (m_訂單明細檔 mdD in mdDs)
                {
                    pcar_DetailData ProdItems = new pcar_DetailData();

                    ProdItems.申請人姓名 = mdD.申請人姓名;
                    ProdItems.申請人地址 = mdD.申請人地址;
                    ProdItems.申請人生日 = mdD.申請人生日;
                    ProdItems.申請人年齡 = mdD.申請人年齡;
                    ProdItems.申請人性別 = mdD.申請人性別;
                    ProdItems.申請人時辰 = mdD.申請人時辰;

                    ProdItems.產品編號 = mdD.產品編號;
                    ProdItems.產品名稱 = mdD.產品名稱;
                    ProdItems.價格 = mdD.價格;
                    ProdItems.會員編號 = mdD.會員編號;
                    ProdItems.白米 = mdD.白米;
                    ProdItems.金牌 = mdD.金牌;
                    ProdItems.文疏 = mdD.文疏梯次;
                    ProdItems.點燈位置 = mdD.點燈位置;
                    car.Item.Add(ProdItems);
                }

                Session["ShoppingItems"] = car;
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "完成載入訂單..." + OrderSerial);
                #endregion

                rAjaxResult.Module = AllowEditProd;
                rAjaxResult.result = true;
                rAjaxResult.message = "";
            }
            catch (Exception ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, ex);
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }

            return js.Serialize(rAjaxResult);
        }

        /// <summary>
        /// 從Session取得目前購物資料
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_GetSessionOrderHead()
        {
            pcar_MasterData car;
            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "系統檢查記錄...Session是否有資料");
            if (Session["ShoppingItems"] != null)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Check...有資料。");
                car = (pcar_MasterData)Session["ShoppingItems"];

                a_會員資料表 acM = new a_會員資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
                RunQueryPackage<m_會員資料表> rA = acM.SearchMaster(new q_會員資料表() { 戶長SN = car.戶長SN }, LoginUserID);
                if (!rA.Result)
                    throw new Exception(rA.ErrMessage);

                m_會員資料表[] mds = rA.SearchData;
                m_會員資料表 mdM = mds.First(x => x.Is戶長 == true);
                a_會員資料表 acMembers = new a_會員資料表() { Connection = getSQLConnection() };

                rAjaxResult.result = true;
                rAjaxResult.Module = new
                {
                    car.戶長SN,
                    car.姓名,
                    car.性別,
                    car.Zip,
                    car.地址,
                    car.手機,
                    car.電話,
                    OptionMember = mds.Select(x => new { SN = x.序號, NAME = x.姓名 })
                };
                rAjaxResult.result = true;
                rAjaxResult.message = string.Empty;
            }
            else
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Check...無資料。");
                rAjaxResult.result = false;
            }
            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_GetSessionOrderItems()
        {
            Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "系統檢查記錄...Session是否有資料");
            pcar_MasterData car;
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K

            if (Session["ShoppingItems"] != null)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Check...有資料。");
                car = (pcar_MasterData)Session["ShoppingItems"];
                //因提昇網路傳輸效能，故重訂要回傳的資料結構。
                var CarItems = car.Item.Select(x => new { 姓名 = x.申請人姓名, x.產品編號, x.產品名稱, x.會員編號, x.價格, x.白米, x.金牌, 燈位 = x.點燈位置, 文殊 = x.文疏 });
                var CarData = new { car.總額, car.白米, car.金牌, car.戶長SN, Item = CarItems };
                return js.Serialize(CarData);
            }
            else
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "Check...無資料。");
                return js.Serialize("{}");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_SetupProductState(String ProdId)
        {
            a_產品資料表 ac = new a_產品資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_產品資料表> r = ac.SearchMaster(new q_產品資料表(), LoginUserID);
            m_產品資料表[] mds = r.SearchData;
            m_產品資料表 md = mds.Where(x => x.產品編號 == ProdId).FirstOrDefault();
            List<產品介面對應設定> InterfaceSetup = new List<產品介面對應設定>();

            if (md != null)
            {
                if (md.產品分類 == "香油")
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p16", AttrName = "readonly", AttrValue = false });
                else
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p16", AttrName = "readonly", AttrValue = true });

                if (md.產品編號 == e_祈福產品.捐白米)
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p14", AttrName = "disabled", AttrValue = false });
                else
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p14", AttrName = "disabled", AttrValue = true, Value = "" });

                if (md.產品編號 == e_祈福產品.捐金牌)
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p15", AttrName = "disabled", AttrValue = false });
                else
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p15", AttrName = "disabled", AttrValue = true, Value = "" });

                if (md.產品編號 == e_祈福產品.保運)
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p18", AttrName = "disabled", AttrValue = false });
                else
                    InterfaceSetup.Add(new 產品介面對應設定() { ElementId = "#SF_EPD_p18", AttrName = "disabled", AttrValue = true, Value = "" });
            }
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            return js.Serialize(InterfaceSetup);
        }

        /// <summary>
        /// 主要提供給新增訂單使用
        /// </summary>
        /// <param name="MasterId"></param>
        /// <returns></returns>
        [HttpPost]
        [ValidateInput(false)]
        public String ajax_LoadMasterId(int MasterId)
        {
            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            JavaScriptSerializer js = new JavaScriptSerializer() { MaxJsonLength = 65536 }; //64K
            try
            {
                a_會員資料表 acM = new a_會員資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
                RunQueryPackage<m_會員資料表> rA = acM.SearchMaster(new q_會員資料表() { 戶長SN = MasterId }, LoginUserID);
                if (!rA.Result)
                    throw new Exception(rA.ErrMessage);

                m_會員資料表[] mds = rA.SearchData;
                m_會員資料表 mdM = mds.First(x => x.Is戶長 == true);
                a_會員資料表 acMembers = new a_會員資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

                rAjaxResult.Module = new
                {
                    #region MyRegion
                    姓名 = mdM.姓名,
                    性別 = mdM.性別,
                    Zip = mdM.郵遞區號,
                    縣市 = mdM.縣市,
                    鄉鎮 = mdM.鄉鎮,
                    地址 = mdM.地址,
                    手機 = mdM.手機,
                    電話 = mdM.電話尾碼,
                    戶長SN = mdM.戶長SN,
                    會員編號 = mdM.序號,
                    OptionMember = mds.Select(x => new { SN = x.序號, NAME = x.姓名 })
                    #endregion
                };
                rAjaxResult.result = true;
                rAjaxResult.message = "";
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }

            return js.Serialize(rAjaxResult);
        }

        [HttpGet]
        [ValidateInput(false)]
        public String ajax_getSubProductItems(String ProdId)
        {
            a_點燈位置資料表 ac = new a_點燈位置資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RunQueryPackage<m_點燈位置資料表> r = ac.SearchMaster(new q_點燈位置資料表 { FliterLock = true, s_產品編號 = ProdId, s_空位 = "0", s_年度 = this.LightYear }, LoginUserID);

            if (!r.Result)
            {
                Response.Write(r.ErrMessage);
                Response.End();
            }

            var mds = r.SearchData.ToDictionary(x => x.序號.ToString(), x => x.位置名稱);
            return (new JavaScriptSerializer() { MaxJsonLength = 4096 }).Serialize(mds);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_getLightSiteStatistics()
        {
            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            a_點燈位置資料表 ac = new a_點燈位置資料表() { Connection = getSQLConnection(), logPlamInfo = plamInfo };

            //String[] _禮斗產品 = new String[] { e_祈福產品.主斗, e_祈福產品.副斗, e_祈福產品.大斗, e_祈福產品.中斗, e_祈福產品.小斗 };
            String[] _禮斗產品 = new String[] { };
            try
            {
                RunQueryPackage<m_燈位數統計狀態> r = ac.q_點燈數狀態統計(this.LightYear, LoginUserID);
                HandleRunEnd(r);
                rAjaxResult.Module = r.SearchData
                    .Where(x => !_禮斗產品.Contains(x.產品編號))
                    .Select(x => new { 名稱 = x.產品名稱, 總計 = x.總計, 已點 = x.已點, 剩餘 = x.剩餘 });
            }
            catch (LogicError ex)
            {
                rAjaxLogErrHandle(ex, rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxLogErrHandle(ex, rAjaxResult);
            }
            return (new JavaScriptSerializer() { MaxJsonLength = 4096 }).Serialize(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_getLiDo()
        {
            ReturnAjaxData rAjaxResult = new ReturnAjaxData();
            //a_訂單主副 ac = new a_訂單主副() { Connection = getSQLConnection(), logPlamInfo = plamInfo };
            RenHai2012Entities db = new RenHai2012Entities();
            try
            {
                //RunQueryPackage<m_訂單明細檔> r = ac.SearchMaster禮斗明細表(this.LightYear, LoginUserID);
                //HandleRunEnd(r);
                //rAjaxResult.Module = r.SearchData
                //    .Select(x => new { 名稱 = x.點燈位置, 姓名 = x.申請人姓名, 地址 = x.申請人地址 });
                rAjaxResult.Module = db.Orders_Detail
                    //.OrderByDescending(x => x.i_InsertDateTime)
                                     .OrderBy(x => new { x.product_sn, x.memo })
                                     .Where(x => x.Product.category == e_祈福產品分類.禮斗 & x.Y == this.LightYear)
                                     .Select(x => new { 燈位名稱 = x.light_name, 產品名稱 = x.product_name, 姓名 = x.member_name, 地址 = x.address, 電話 = x.Member_Detail.tel })
                                     .ToList();
            }
            catch (LogicError ex)
            {
                rAjaxLogErrHandle(ex, rAjaxResult);
            }
            catch (Exception ex)
            {
                rAjaxLogErrHandle(ex, rAjaxResult);
            }
            finally
            {
                db.Dispose();
            }
            //return (new JavaScriptSerializer() { MaxJsonLength = 1024 * 1024 }).Serialize(rAjaxResult);
            return defJSON(rAjaxResult);
        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_getOrderMemberItem(String id)
        {
            //id 指的是訂單主檔 的 訂單編號 為 String格式
            Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, "取得訂單成員開始...");
            try
            {
                RenHai2012Entities db = new RenHai2012Entities();
                var n = db.Orders_Detail.Where(x => x.orders_sn == id);
                return defJSON(n);
            }
            catch (Exception ex)
            {
                Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message);
                return "";
            }

        }

        [HttpPost]
        [ValidateInput(false)]
        public String ajax_modOrderMemberItem(OrderMemberModify obj)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                RenHai2012Entities db = new RenHai2012Entities();
                var items = db.Orders_Detail.Where(x => x.orders_sn == obj.orders_id);
                foreach (var item in obj.order_data)
                {
                    var getItem = items.Where(x => x.orders_detail_id == item.orders_detail_id).FirstOrDefault();
                    if (getItem != null)
                    {
                        getItem.member_name = item.member_name;
                        getItem.gender = item.gender;
                        getItem.l_birthday = item.l_birthday;
                        getItem.born_sign = item.born_sign;
                        getItem.born_time = item.born_time;
                        getItem.address = item.address;
                        //超渡法會修改
                        getItem.departed_name = item.departed_name;
                        getItem.departed_address = item.departed_address;
                        getItem.departed_qty = item.departed_qty;

                        getItem.manjushri = item.manjushri;
                        getItem.C_UpdateDateTime = DateTime.Now;
                    }
                }

                db.SaveChanges();
                return defJSON(rAjaxResult);
            }
            catch (Exception ex)
            {
                //Log.Write(plamInfo, this.GetType().Name + "." + System.Reflection.MethodInfo.GetCurrentMethod().Name, ex.Message);
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
                return (new JavaScriptSerializer()).Serialize(rAjaxResult);
            }
        }

        public string getString()
        {

            return "test";
        }
    }

    public class OrderMemberModify
    {
        public String orders_id { get; set; }
        public Orders_Detail[] order_data { get; set; }
    }

    public class MemberData
    {
        public String order_sn { get; set; }
        public String product_sn { get; set; }
        public String member_sn { get; set; }
        public String name { get; set; }
        public String gender { get; set; }
        public String l_birthday { get; set; }
        public String borntime { get; set; }
        public String bornsign { get; set; }
        public String address { get; set; }
        public String[] prod { get; set; }
        public String productname { get; set; }
        public String lightname { get; set; }
    }

    public class 產品介面對應設定
    {
        public String ElementId { get; set; }
        public String AttrName { get; set; }
        public Object AttrValue { get; set; }
        public String Value { get; set; }
    }
}
