using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.Style;
using ProcCore;
using ProcCore.Business;
using ProcCore.Business.Logic;
using ProcCore.NetExtension;
using ProcCore.ReturnAjaxResult;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using Work.WebApp.Models;
namespace DotWeb.Controllers
{
    public class ExcelReportController : BaseController
    {
        // 此控製器for asp叫用
        public ActionResult Index()
        {
            return View();
        }

        //收款統計表 輸出為Excel
        public FileResult ajax_MakeExcel(q_Excel_統計表 qr)
        {
            //qr.Date1 = DateTime.Parse("2014/12/1");
            //qr.Time1 = 8;

            //qr.Date2 = DateTime.Parse("2014/12/1");
            //qr.Time2 = 23;

            //qr.People = 2000027;

            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            ExcelPackage excel = null;
            FileStream fs = null;
            m_統計表 md = null;
            try
            {
                #region 資料庫運算
                var db0 = getDB();
                var qItem = db0.Orders_Detail.AsQueryable();

                if (qr.People > 0)
                    qItem = qItem.Where(x => x.C_InsertUserID == qr.People);

                if (qr.Date1 != null)
                {
                    var d = qr.Date1.Value.AddHours(qr.Time1);
                    qItem = qItem.Where(x => x.i_InsertDateTime >= d);
                }

                if (qr.Date2 != null)
                {
                    var d = qr.Date2.Value.AddHours(qr.Time2);
                    qItem = qItem.Where(x => x.i_InsertDateTime <= d);
                }


                var gItem = db0.TempleAccount.AsQueryable();

                if (qr.People > 0)
                    gItem = gItem.Where(x => x.i_InsertUserID == qr.People);

                if (qr.Date1 != null)
                {
                    var d = qr.Date1.Value.AddHours(qr.Time1);
                    gItem = gItem.Where(x => x.i_InsertDateTime >= d);
                }

                if (qr.Date2 != null)
                {
                    var d = qr.Date2.Value.AddHours(qr.Time2);
                    gItem = gItem.Where(x => x.i_InsertDateTime <= d);
                }
                gItem = gItem.AsQueryable();

                var mds = qItem.Select(x => new { x.product_sn, x.product_name, x.price, x.race, x.gold })
                    .GroupBy(x => new { x.product_sn, x.product_name }, (key, total) => new
                    {
                        product_sn = key.product_sn,
                        product_name = key.product_name,
                        price = total.Sum(x => x.price),
                        race = total.Sum(x => x.race),
                        gold = total.Sum(x => x.gold)
                    }).ToList();
                var user = db0.Users.Find(qr.People);
                md = new m_統計表();
                #region 統計計算
                md.負責人 = user.users_name;
                md.民國 = ((DateTime)qr.Date1).To民國年() + "~" + ((DateTime)qr.Date2).To民國年();
                md.時間 = String.Format("{0:00}:00", qr.Time1) + "~" + String.Format("{0:00}:00", qr.Time2);

                md.文昌燈 = mds.Any(x => x.product_sn == e_祈福產品.文昌燈) ? mds.First(x => x.product_sn == e_祈福產品.文昌燈).price : 0;
                md.文昌頭燈 = mds.Any(x => x.product_sn == e_祈福產品.文昌頭燈) ? mds.First(x => x.product_sn == e_祈福產品.文昌頭燈).price : 0;

                md.姻緣燈 = mds.Any(x => x.product_sn == e_祈福產品.姻緣燈) ? mds.First(x => x.product_sn == e_祈福產品.姻緣燈).price : 0;
                md.姻緣頭燈 = mds.Any(x => x.product_sn == e_祈福產品.月老頭燈) ? mds.First(x => x.product_sn == e_祈福產品.月老頭燈).price : 0;

                md.財神燈 = mds.Any(x => x.product_sn == e_祈福產品.財神燈) ? mds.First(x => x.product_sn == e_祈福產品.財神燈).price : 0;
                md.財神頭燈 = mds.Any(x => x.product_sn == e_祈福產品.財神頭燈) ? mds.First(x => x.product_sn == e_祈福產品.財神頭燈).price : 0;

                md.媽祖燈 = mds.Any(x => x.product_sn == e_祈福產品.媽祖燈) ? mds.First(x => x.product_sn == e_祈福產品.媽祖燈).price : 0;
                md.媽祖頭燈 = mds.Any(x => x.product_sn == e_祈福產品.媽祖頭燈) ? mds.First(x => x.product_sn == e_祈福產品.媽祖頭燈).price : 0;

                md.關聖燈 = mds.Any(x => x.product_sn == e_祈福產品.關聖燈) ? mds.First(x => x.product_sn == e_祈福產品.關聖燈).price : 0;
                md.關聖頭燈 = mds.Any(x => x.product_sn == e_祈福產品.關聖頭燈) ? mds.First(x => x.product_sn == e_祈福產品.關聖頭燈).price : 0;

                md.觀音燈 = mds.Any(x => x.product_sn == e_祈福產品.觀音燈) ? mds.First(x => x.product_sn == e_祈福產品.觀音燈).price : 0;
                md.觀音頭燈 = mds.Any(x => x.product_sn == e_祈福產品.觀音頭燈) ? mds.First(x => x.product_sn == e_祈福產品.觀音頭燈).price : 0;

                //媽祖殿燈籠頭燈
                md.頭燈前排福燈 = mds.Any(x => x.product_sn == e_祈福產品.頭燈前排福燈) ? mds.First(x => x.product_sn == e_祈福產品.頭燈前排福燈).price : 0;
                md.頭燈後排福燈 = mds.Any(x => x.product_sn == e_祈福產品.頭燈後排福燈) ? mds.First(x => x.product_sn == e_祈福產品.頭燈後排福燈).price : 0;
                md.前排福燈 = mds.Any(x => x.product_sn == e_祈福產品.前排福燈) ? mds.First(x => x.product_sn == e_祈福產品.前排福燈).price : 0;
                md.後排福燈 = mds.Any(x => x.product_sn == e_祈福產品.後排福燈) ? mds.First(x => x.product_sn == e_祈福產品.後排福燈).price : 0;
                md.左排福燈 = mds.Any(x => x.product_sn == e_祈福產品.左排福燈) ? mds.First(x => x.product_sn == e_祈福產品.左排福燈).price : 0;
                md.右排福燈 = mds.Any(x => x.product_sn == e_祈福產品.右排福燈) ? mds.First(x => x.product_sn == e_祈福產品.右排福燈).price : 0;
                md.上排福燈 = mds.Any(x => x.product_sn == e_祈福產品.上排福燈) ? mds.First(x => x.product_sn == e_祈福產品.上排福燈).price : 0;
                //加總
                md.媽祖殿燈籠頭燈加總 = md.頭燈前排福燈 + md.頭燈後排福燈 + md.前排福燈 + md.後排福燈 + md.左排福燈 + md.右排福燈 + md.上排福燈;

                md.香油錢 = mds.Any(x => x.product_sn == e_祈福產品.香油錢) ? mds.First(x => x.product_sn == e_祈福產品.香油錢).price : 0;
                md.香油_媽祖聖誕 = mds.Any(x => x.product_sn == e_祈福產品.香油_媽祖聖誕) ? mds.First(x => x.product_sn == e_祈福產品.香油_媽祖聖誕).price : 0;
                md.香油_媽祖回鑾 = mds.Any(x => x.product_sn == e_祈福產品.香油_媽祖回鑾) ? mds.First(x => x.product_sn == e_祈福產品.香油_媽祖回鑾).price : 0;
                md.香油_祈願卡 = mds.Any(x => x.product_sn == e_祈福產品.香油_祈願卡) ? mds.First(x => x.product_sn == e_祈福產品.香油_祈願卡).price : 0;

                md.香油_信徒觀摩 = mds.Any(x => x.product_sn == e_祈福產品.香油_信徒觀摩) ? mds.First(x => x.product_sn == e_祈福產品.香油_信徒觀摩).price : 0;
                md.香油_媽祖聖誕典禮 = mds.Any(x => x.product_sn == e_祈福產品.香油_媽祖聖誕典禮) ? mds.First(x => x.product_sn == e_祈福產品.香油_媽祖聖誕典禮).price : 0;
                md.香油_專案專款 = mds.Any(x => x.product_sn == e_祈福產品.香油_專案專款) ? mds.First(x => x.product_sn == e_祈福產品.香油_專案專款).price : 0;
                md.香油_契子觀摩 = mds.Any(x => x.product_sn == e_祈福產品.香油_契子觀摩) ? mds.Where(x => x.product_sn == e_祈福產品.香油_契子觀摩).Sum(x => x.price) : 0;
                //香油其它
                md.香油_牛軋糖 = mds.Any(x => x.product_sn == e_祈福產品.香油_牛軋糖) ? mds.Where(x => x.product_sn == e_祈福產品.香油_牛軋糖).Sum(x => x.price) : 0;
                //2016/12/7 下架 md.香油_農民曆廣告 = mds.Any(x => x.product_sn == e_祈福產品.香油_農民曆廣告) ? mds.Where(x => x.product_sn == e_祈福產品.香油_農民曆廣告).Sum(x => x.price) : 0;
                md.香油_衣服 = mds.Any(x => x.product_sn == e_祈福產品.香油_衣服) ? mds.Where(x => x.product_sn == e_祈福產品.香油_衣服).Sum(x => x.price) : 0;
                md.香油_薦拔祖先 = mds.Any(x => x.product_sn == e_祈福產品.香油_薦拔祖先) ? mds.Where(x => x.product_sn == e_祈福產品.香油_薦拔祖先).Sum(x => x.price) : 0;
                md.香油_冤親債主 = mds.Any(x => x.product_sn == e_祈福產品.香油_冤親債主) ? mds.Where(x => x.product_sn == e_祈福產品.香油_冤親債主).Sum(x => x.price) : 0;
                md.香油_嬰靈 = mds.Any(x => x.product_sn == e_祈福產品.香油_嬰靈) ? mds.Where(x => x.product_sn == e_祈福產品.香油_嬰靈).Sum(x => x.price) : 0;
                //2016/12/7 加入
                md.香油_屋頂整修費 = mds.Any(x => x.product_sn == e_祈福產品.香油_屋頂整修費) ? mds.Where(x => x.product_sn == e_祈福產品.香油_屋頂整修費).Sum(x => x.price) : 0;
                //2016/12/30 加入
                md.香油_祈福玉珮 = mds.Any(x => x.product_sn == e_祈福產品.香油_祈福玉珮) ? mds.Where(x => x.product_sn == e_祈福產品.香油_祈福玉珮).Sum(x => x.price) : 0;

                md.太歲 = mds.Any(x => x.product_sn == e_祈福產品.安太歲) ? mds.First(x => x.product_sn == e_祈福產品.安太歲).price : 0;
                md.法會_入斗 = mds.Any(x => x.product_sn == e_祈福產品.入斗) ? mds.First(x => x.product_sn == e_祈福產品.入斗).price : 0;
                md.保運 = mds.Any(x => x.product_sn == e_祈福產品.保運) ? mds.First(x => x.product_sn == e_祈福產品.保運).price : 0;
                md.租金 = mds.Any(x => x.product_sn == e_祈福產品.租金) ? mds.First(x => x.product_sn == e_祈福產品.租金).price : 0;

                md.白米 = mds.Any(x => x.product_sn == e_祈福產品.捐白米) ? mds.First(x => x.product_sn == e_祈福產品.捐白米).race : 0;
                md.金牌 = mds.Any(x => x.product_sn == e_祈福產品.捐金牌) ? mds.First(x => x.product_sn == e_祈福產品.捐金牌).gold : 0;

                md.大斗 = mds.Any(x => x.product_sn == e_祈福產品.大斗) ? mds.First(x => x.product_sn == e_祈福產品.大斗).price : 0;
                md.中斗 = mds.Any(x => x.product_sn == e_祈福產品.中斗) ? mds.First(x => x.product_sn == e_祈福產品.中斗).price : 0;
                md.小斗 = mds.Any(x => x.product_sn == e_祈福產品.小斗) ? mds.First(x => x.product_sn == e_祈福產品.小斗).price : 0;
                md.主斗 = mds.Any(x => x.product_sn == e_祈福產品.主斗) ? mds.First(x => x.product_sn == e_祈福產品.主斗).price : 0;
                md.副斗 = mds.Any(x => x.product_sn == e_祈福產品.副斗) ? mds.First(x => x.product_sn == e_祈福產品.副斗).price : 0;

                md.合計禮斗 = md.大斗 + md.中斗 + md.小斗 + md.主斗 + md.副斗;

                //md.合計金額 = mds.Where(x => x.product_sn != e_祈福產品.捐白米 && x.product_sn != e_祈福產品.捐金牌).Select(x=>x.price).DefaultIfEmpty().Sum();

                md.合計燈類金額 = md.文昌燈 + md.文昌頭燈;

                md.契子會_入會 = gItem.Any(x => x.product_sn == e_祈福產品.契子會_入會) ? gItem.Where(x => x.product_sn == e_祈福產品.契子會_入會).Sum(x => x.price) : 0;
                md.契子會_大會 = gItem.Any(x => x.product_sn == e_祈福產品.契子會_大會) ? gItem.Where(x => x.product_sn == e_祈福產品.契子會_大會).Sum(x => x.price) : 0;
                //md.香油_契子觀摩 = gItem.Any(x => x.product_sn == e_祈福產品.香油_契子觀摩) ? gItem.Where(x => x.product_sn == e_祈福產品.香油_契子觀摩).Sum(x => x.price) : 0;
                //香油_契子觀摩 改放在一般會員資料管理
                #endregion
                #endregion

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/統計表.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);
                ExcelWorksheet sheet = excel.Workbook.Worksheets["SheetPrint"];

                #region Report Making
                sheet.Cells["C5"].Value = md.民國;
                sheet.Cells["C6"].Value = md.時間;

                sheet.Cells["C8"].Value = md.太歲;
                sheet.Cells["E8"].Value = md.法會_入斗;

                sheet.Cells["C9"].Value = md.文昌燈;
                sheet.Cells["E9"].Value = md.文昌頭燈;

                sheet.Cells["C10"].Value = md.媽祖燈;
                sheet.Cells["E10"].Value = md.媽祖頭燈;

                sheet.Cells["C11"].Value = md.觀音燈;
                sheet.Cells["E11"].Value = md.觀音頭燈;

                sheet.Cells["C12"].Value = md.關聖燈;
                sheet.Cells["E12"].Value = md.關聖頭燈;

                sheet.Cells["C13"].Value = md.財神燈;
                sheet.Cells["E13"].Value = md.財神頭燈;

                sheet.Cells["C14"].Value = md.姻緣燈;
                sheet.Cells["E14"].Value = md.姻緣頭燈;

                sheet.Cells["C15"].Value = md.媽祖殿燈籠頭燈加總;
                sheet.Cells["E15"].Value = md.文昌燈 + md.文昌頭燈 + md.媽祖燈 + md.媽祖頭燈 + md.觀音燈 + md.觀音頭燈 + md.關聖燈 + md.關聖頭燈 + md.財神燈 + md.財神頭燈 + md.姻緣燈 + md.姻緣頭燈 + md.媽祖殿燈籠頭燈加總;

                sheet.Cells["C16"].Value = md.香油錢;
                sheet.Cells["E16"].Value = md.香油_信徒觀摩;

                sheet.Cells["C17"].Value = md.香油_祈願卡;
                sheet.Cells["E17"].Value = md.合計禮斗;

                sheet.Cells["C18"].Value = md.香油_媽祖回鑾;
                sheet.Cells["E18"].Value = md.契子會_大會;

                sheet.Cells["C19"].Value = md.契子會_入會;
                sheet.Cells["E19"].Value = md.香油_媽祖聖誕典禮 + md.香油_媽祖聖誕;

                sheet.Cells["C20"].Value = md.香油_契子觀摩;
                sheet.Cells["E20"].Value = md.香油_專案專款;

                sheet.Cells["C21"].Value = md.香油_牛軋糖;
                sheet.Cells["E21"].Value = md.香油_屋頂整修費;

                sheet.Cells["C22"].Value = md.香油_衣服;
                sheet.Cells["E22"].Value = md.香油_薦拔祖先;

                sheet.Cells["C23"].Value = md.香油_冤親債主;
                sheet.Cells["E23"].Value = md.香油_嬰靈;

                sheet.Cells["C24"].Value = md.香油_祈福玉珮;

                sheet.Cells["C25"].Value = md.租金;
                sheet.Cells["E25"].Value = md.保運;

                sheet.Cells["C26"].Value = md.金牌 + "面";
                sheet.Cells["E26"].Value = md.白米 + "斤";

                sheet.Cells["D27"].Formula = "C8+E8+E15+SUM(C16:C25) + SUM(E16:E25)";
                sheet.Cells["D27"].Calculate();
                sheet.Cells["D28"].Value = md.負責人;
                sheet.Cells["D31"].Value = DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss");
                #endregion

                rAjaxResult.result = true;

                string filename = "統計表[" + md.負責人 + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);

            }
            catch (Exception ex)
            {
                //    rAjaxResult.result = false;
                //    rAjaxResult.message = "[" + ex.Message + "]" + "[" + ex.StackTrace + "]";
                Log.Write("[" + ex.Message + "]" + "[" + ex.StackTrace + "]");
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
            }
        }
        public FileResult downloadExcel_Thanks(string orders_sn)
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {
                var orders = db0.Orders.Find(orders_sn);
                var orders_detail = db0.Orders_Detail.Where(x => x.orders_sn == orders_sn).OrderBy(x => x.detail_sort).ThenBy(x => x.orders_detail_id).Take(CommSetup.CommWebSetup.OrdersRecordMax);
                var user = db0.Users.Find(this.UserId);
                int detail_count = orders_detail.Count();
                int fortune_conut = 0;
                if (orders.orders_type == (int)Orders_Type.fortune_order)//判斷福燈人數
                {
                    var item = orders_detail.FirstOrDefault();
                    var items = db0.Fortune_Light.Where(x => x.order_sn == orders_sn).OrderBy(x => x.fortune_light_id);
                    fortune_conut = items.Count();
                }

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/感謝狀.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);

                ExcelWorksheet sheet = null;
                if (detail_count <= 8 && fortune_conut <= 8)
                    sheet = excel.Workbook.Worksheets["SheetPrint"];
                else if ((detail_count > 8 && detail_count <= 16) || fortune_conut > 8)
                    sheet = excel.Workbook.Worksheets["SheetPrint2"];
                else if (detail_count > 16)
                    sheet = excel.Workbook.Worksheets["SheetPrint3"];

                sheet.View.TabSelected = true;
                #region 地址顯示過濾
                //列印感謝狀時地址不要顯示"空白"兩個字
                if (orders.address.StartsWith("空白空白"))
                {
                    if (orders.address.Equals("空白空白0") || orders.address.Equals("空白空白1"))
                    {
                        orders.address = "";
                    }
                    else
                    {
                        orders.address = orders.address.Replace("空白空白", "");
                    }
                }
                //沒有郵遞區號時就不顯示不要顯示"0"
                if (orders.zip.Equals("0"))
                    orders.zip = "";
                #endregion

                #region Excel Handle

                int detail_row = 2;
                #region Page 1
                #region 主檔
                sheet.Cells["G9"].Value = orders.total.GetChineseNumber(); //總金額
                sheet.Cells["B13"].Value = orders.member_name; //戶長姓名
                sheet.Cells["D15"].Value = orders.zip + orders.address; //戶長地址

                var race = orders_detail.Sum(x => x.race);
                var gold = orders_detail.Sum(x => x.gold);

                IList<string> memo = new List<string>();
                if (gold > 0)
                    memo.Add("金牌" + gold + "面");
                if (race > 0)
                    memo.Add("白米" + race + "斤");

                sheet.Cells["D18"].Value = string.Join(" ", memo); //備註欄 註記金牌及捐米

                DateTime reportdate = orders.transation_date == null ? DateTime.Now : (DateTime)orders.transation_date;
                sheet.Cells["C21"].Value = reportdate.Year - 1911; //訂單日期年
                sheet.Cells["F21"].Value = reportdate.Month; //訂單日期月
                sheet.Cells["H21"].Value = reportdate.Day; //訂單日期日
                sheet.Cells["L21"].Value = orders.Users.users_name; //承辦人員
                sheet.Cells["I23"].Value = orders.orders_sn; //訂單編號 
                #endregion

                #region 明細

                if (orders.orders_type == (int)Orders_Type.fortune_order)
                {

                    var item = orders_detail.FirstOrDefault();
                    var items = db0.Fortune_Light.Where(x => x.order_sn == orders_sn).OrderBy(x => x.fortune_light_id);
                    sheet.Cells["D18"].Value = item.light_name;//列印福燈時將燈位註記在備註裡面
                    for (var i = 0; i < 8; i++)
                    {
                        var itm = items.Skip(i).Take(1).FirstOrDefault();
                        if (itm != null)
                        {
                            sheet.Cells[detail_row, 15].Value = item.product_name;
                            sheet.Cells[detail_row, 16].Value = "";
                            sheet.Cells[detail_row + 1, 16].Value = itm.member_name;
                            sheet.Cells[detail_row, 17].Value = "";
                        }
                        else
                        {
                            sheet.Cells[detail_row, 15].Value = "";
                            sheet.Cells[detail_row, 16].Value = "";
                            sheet.Cells[detail_row, 17].Value = "";
                        }
                        detail_row = detail_row + 2;
                    }
                }
                else
                {
                    for (var i = 0; i < 8; i++)
                    {
                        var item = orders_detail.Skip(i).Take(1).FirstOrDefault();
                        if (item != null)
                        {
                            sheet.Cells[detail_row, 15].Value = item.member_name;
                            sheet.Cells[detail_row, 16].Value = item.product_name;
                            sheet.Cells[detail_row, 17].Value = item.price;

                            if (item.light_name != null)
                            {
                                sheet.Cells[detail_row + 1, 16].Value = item.light_name;
                            }
                            else if (item.product_sn == "6")
                            {
                                sheet.Cells[detail_row + 1, 16].Value = item.l_birthday + " " + item.born_time + "時";
                            }

                        }
                        else
                        {
                            sheet.Cells[detail_row, 15].Value = "";
                            sheet.Cells[detail_row, 16].Value = "";
                            sheet.Cells[detail_row, 17].Value = "";
                        }
                        detail_row = detail_row + 2;
                    }
                }
                #endregion
                #endregion

                #region Page 2
                if (detail_count > 8 || fortune_conut > 8)
                {


                    #region 主檔
                    sheet.Cells["G55"].Value = orders.total.GetChineseNumber(); //總金額
                    sheet.Cells["B59"].Value = orders.member_name; //戶長姓名
                    sheet.Cells["D61"].Value = orders.zip + orders.address; //戶長地址

                    if (gold > 0)
                        memo.Add("金牌" + gold + "面");
                    if (race > 0)
                        memo.Add("白米" + race + "斤");

                    sheet.Cells["D64"].Value = string.Join(" ", memo); //備註欄 註記金牌及捐米

                    sheet.Cells["C67"].Value = reportdate.Year - 1911; //訂單日期年
                    sheet.Cells["F67"].Value = reportdate.Month; //訂單日期月
                    sheet.Cells["H67"].Value = reportdate.Day; //訂單日期日
                    sheet.Cells["L67"].Value = orders.Users.users_name; //承辦人員
                    sheet.Cells["I69"].Value = orders.orders_sn; //訂單編號 
                    #endregion

                    detail_row = 48;

                    #region 明細
                    if (orders.orders_type == (int)Orders_Type.fortune_order)
                    {
                        var item = orders_detail.FirstOrDefault();
                        var items = db0.Fortune_Light.Where(x => x.order_sn == orders_sn).OrderBy(x => x.fortune_light_id);
                        sheet.Cells["D64"].Value = item.light_name;//列印福燈時將燈位註記在備註裡面
                        for (var i = 0; i < 8; i++)
                        {
                            var itm = items.Skip(i + 8).Take(1).FirstOrDefault();
                            if (itm != null)
                            {
                                sheet.Cells[detail_row, 15].Value = item.product_name;
                                sheet.Cells[detail_row, 16].Value = "";
                                sheet.Cells[detail_row + 1, 16].Value = itm.member_name;
                                sheet.Cells[detail_row, 17].Value = "";
                            }
                            else
                            {
                                sheet.Cells[detail_row, 15].Value = "";
                                sheet.Cells[detail_row, 16].Value = "";
                                sheet.Cells[detail_row, 17].Value = "";
                            }
                            detail_row = detail_row + 2;
                        }
                    }
                    else
                    {
                        for (var i = 0; i < 8; i++)
                        {
                            var item = orders_detail.Skip(i + 8).Take(1).FirstOrDefault();
                            if (item != null)
                            {
                                sheet.Cells[detail_row, 15].Value = item.member_name;
                                sheet.Cells[detail_row, 16].Value = item.product_name;
                                sheet.Cells[detail_row, 17].Value = item.price;
                                if (item.light_name != null)
                                {
                                    sheet.Cells[detail_row + 1, 16].Value = item.light_name;
                                }
                                else if (item.product_sn == "6")
                                {
                                    sheet.Cells[detail_row + 1, 16].Value = item.l_birthday + " " + item.born_time + "時";
                                }
                            }
                            else
                            {
                                sheet.Cells[detail_row, 15].Value = "";
                                sheet.Cells[detail_row, 16].Value = "";
                                sheet.Cells[detail_row, 17].Value = "";
                            }
                            detail_row = detail_row + 2;
                        }
                    }


                    #endregion
                }
                #endregion

                #region Page 3
                if (detail_count > 16)
                {
                    #region 主檔
                    sheet.Cells["G101"].Value = orders.total.GetChineseNumber(); //總金額
                    sheet.Cells["B105"].Value = orders.member_name; //戶長姓名
                    sheet.Cells["D107"].Value = orders.zip + orders.address; //戶長地址

                    if (gold > 0)
                        memo.Add("金牌" + gold + "面");
                    if (race > 0)
                        memo.Add("白米" + race + "斤");

                    sheet.Cells["D110"].Value = string.Join(" ", memo); //備註欄 註記金牌及捐米

                    sheet.Cells["C113"].Value = reportdate.Year - 1911; //訂單日期年
                    sheet.Cells["F113"].Value = reportdate.Month; //訂單日期月
                    sheet.Cells["H113"].Value = reportdate.Day; //訂單日期日
                    sheet.Cells["L113"].Value = orders.Users.users_name; //承辦人員
                    sheet.Cells["I115"].Value = orders.orders_sn; //訂單編號 
                    #endregion

                    detail_row = 94;

                    #region 明細
                    for (var i = 0; i < 8; i++)
                    {
                        var item = orders_detail.Skip(i + 16).Take(1).FirstOrDefault();
                        if (item != null)
                        {
                            sheet.Cells[detail_row, 15].Value = item.member_name;
                            sheet.Cells[detail_row, 16].Value = item.product_name;
                            sheet.Cells[detail_row, 17].Value = item.price;
                            if (item.light_name != null)
                            {
                                sheet.Cells[detail_row + 1, 16].Value = item.light_name;
                            }
                            else if (item.product_sn == "6")
                            {
                                sheet.Cells[detail_row + 1, 16].Value = item.l_birthday + " " + item.born_time + "時";
                            }
                        }
                        else
                        {
                            sheet.Cells[detail_row, 15].Value = "";
                            sheet.Cells[detail_row, 16].Value = "";
                            sheet.Cells[detail_row, 17].Value = "";
                        }
                        detail_row = detail_row + 2;
                    }
                    #endregion
                }
                #endregion

                sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "感謝狀[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_FortuneLabel(string fortune_psn, string startDate, string endDate)
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/福燈標籤8.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);
                ExcelWorksheet sheet = excel.Workbook.Worksheets["SheetPrint"];

                sheet.View.TabSelected = true;
                #region 取得福燈訂單明細
                List<string> fortuneLight_psn = new List<string> { "390", "391", "392", "393", "394", "395", "396" };//福燈產品編號

                var Orders_detail = (from x in db0.Orders_Detail
                                     orderby x.C_InsertDateTime
                                     select new m_Orders_Detail()
                                     {
                                         orders_sn = x.orders_sn,
                                         product_sn = x.product_sn,
                                         member_name = x.member_name,
                                         light_name = x.light_name,
                                         C_InsertDateTime = x.C_InsertDateTime
                                     }).Where(x => fortuneLight_psn.Contains(x.product_sn));

                if (fortune_psn != null && fortune_psn != "")
                    Orders_detail = Orders_detail.Where(x => x.product_sn == fortune_psn);

                if (startDate != null && endDate != null)
                {
                    DateTime start = (DateTime.Parse(startDate));
                    DateTime end = (DateTime.Parse(endDate)).AddDays(1);
                    Orders_detail = Orders_detail.Where(x => (DateTime)x.C_InsertDateTime >= start && (DateTime)x.C_InsertDateTime < end);
                }
                #endregion

                #region 複製格式

                //var test = sheet.Drawings.AddShape("test", eShapeStyle.RoundRect);

                #region 複製列高
                int page = Convert.ToInt16(Math.Ceiling((Double)Orders_detail.Count() / 8));//一頁8筆資料
                if (page > 1)
                {
                    for (var i = 1; i < page; i++)
                    {//從第二頁開始複製列高
                        for (var j = 1; j <= 29; j++)
                        {//一頁29列
                            sheet.Row(j + (i * 29)).Height = sheet.Row(j).Height;
                        }
                    }
                }
                #endregion

                #endregion

                #region Excel Handle

                int detail_row = 2;
                #region 主檔

                int index = 1;
                foreach (var item in Orders_detail)
                {
                    //var members = db0.Fortune_Light.Where(x => x.order_sn == item.orders_sn).OrderByDescending(x => x.sort);
                    var members = (from x in db0.Fortune_Light
                                   join y in db0.Member_Detail
                                   on x.member_detail_id equals y.member_detail_id
                                   where x.order_sn == item.orders_sn
                                   select new { x.member_detail_id, x.member_name, x.order_sn, y.lbirthday, y.is_holder })
                                   .OrderByDescending(x => x.is_holder).ThenBy(x => x.lbirthday);

                    #region copy戶長格式(第一頁以後)
                    if (index > 8)
                    {
                        if (index % 2 != 0)
                        {
                            sheet.Cells["H2"].Copy(sheet.Cells[detail_row, 17]);//戶長姓名
                            sheet.Cells["H5"].Copy(sheet.Cells[detail_row + 3, 17]);//闔府
                        }
                        else
                        {
                            sheet.Cells["H2"].Copy(sheet.Cells[detail_row, 8]);//戶長姓名
                            sheet.Cells["H5"].Copy(sheet.Cells[detail_row + 3, 8]);//闔府
                        }
                    }
                    else if (index > 2)
                    {
                        sheet.Cells[detail_row + 3, 17].Value = "闔府";
                        sheet.Cells[detail_row + 3, 8].Value = "闔府";
                    }
                    #endregion
                    #region 內容
                    if (index % 2 != 0)
                    {
                        sheet.Cells[detail_row, 17].Value = item.member_name;//戶長姓名
                        sheet.Cells[detail_row + 5, 11].Value = item.light_name;//燈位名稱
                        if (item.member_name.Length > 4)
                            length4HouseHold(sheet, detail_row, 17);

                        light_name(sheet, detail_row + 5, 11);
                        #region 家族成員
                        int column = 16, index2 = 1;
                        foreach (var mb in members)
                        {
                            if (index2 % 2 != 0)
                            {
                                if (index > 8)
                                {
                                    sheet.Cells["P2"].Copy(sheet.Cells[detail_row, column]);
                                }
                                sheet.Cells[detail_row, column].Value = mb.member_name;
                                if (mb.member_name.Length > 4)
                                    length4Member(sheet, detail_row, column);
                            }
                            else
                            {
                                if (index > 8)
                                {
                                    sheet.Cells["P2"].Copy(sheet.Cells[detail_row + 2, column]);
                                }

                                sheet.Cells[detail_row + 2, column].Value = mb.member_name;
                                if (mb.member_name.Length > 4)
                                    length4Member(sheet, detail_row + 2, column);

                                column--;//偶數換行
                            }
                            index2++;
                        }
                        #endregion
                    }
                    else
                    {
                        sheet.Cells[detail_row, 8].Value = item.member_name;//戶長姓名
                        sheet.Cells[detail_row + 5, 2].Value = item.light_name;//燈位名稱
                        if (item.member_name.Length > 4)
                            length4HouseHold(sheet, detail_row, 8);

                        light_name(sheet, detail_row + 5, 2);

                        #region 家族成員
                        int column = 7, index2 = 1;
                        foreach (var mb in members)
                        {
                            if (index2 % 2 != 0)
                            {
                                if (index > 8)
                                {
                                    sheet.Cells["P2"].Copy(sheet.Cells[detail_row, column]);
                                }
                                sheet.Cells[detail_row, column].Value = mb.member_name;
                                if (mb.member_name.Length > 4)
                                    length4Member(sheet, detail_row, column);
                            }
                            else
                            {
                                if (index > 8)
                                {
                                    sheet.Cells["P2"].Copy(sheet.Cells[detail_row + 2, column]);
                                }
                                sheet.Cells[detail_row + 2, column].Value = mb.member_name;
                                if (mb.member_name.Length > 4)
                                    length4Member(sheet, detail_row + 2, column);
                                column--;//偶數換行
                            }
                            index2++;
                        }
                        #endregion
                    }
                    #endregion
                    #region 合併儲存格(copy時相鄰儲存格不能被合併)
                    if (index > 8)
                    {
                        if (index % 2 != 0)
                        {
                            sheet.Cells[detail_row, 17, detail_row + 2, 17].Merge = true;//戶長合併儲存格
                            sheet.Cells[detail_row + 3, 17, detail_row + 3 + 2, 17].Merge = true;//合併儲存格

                            int column = 16, index2 = 1;
                            foreach (var mb in members)
                            {
                                if (index2 % 2 == 0)
                                {
                                    sheet.Cells[detail_row + 2, column, detail_row + 2 + 1, column].Merge = true;
                                    column--;//偶數換行
                                }
                                index2++;
                            }
                        }
                        else
                        {
                            sheet.Cells[detail_row, 8, detail_row + 2, 8].Merge = true;//戶長合併儲存格
                            sheet.Cells[detail_row + 3, 8, detail_row + 3 + 2, 8].Merge = true;//合併儲存格

                            int column = 7, index2 = 1;
                            foreach (var mb in members)
                            {
                                if (index2 % 2 == 0)
                                {
                                    sheet.Cells[detail_row + 2, column, detail_row + 2 + 1, column].Merge = true;
                                    column--;//偶數換行
                                }
                                index2++;
                            }
                        }
                    }
                    #endregion
                    if (index % 8 == 0)
                    {
                        detail_row += 8;//換頁
                    }
                    else if (index % 2 == 0)
                    {
                        detail_row += 7;//偶數換列
                    }
                    index++;
                }

                #endregion

                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "福燈標籤[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.ToString());
                //Console.Write(ex.Message);
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_RejectReport(string order_sn, string startDate, string endDate, string member_name)
        {
            ExcelPackage excel = null;
            MemoryStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                fs = new MemoryStream();
                excel = new ExcelPackage(fs);
                excel.Workbook.Worksheets.Add("RejectData");
                ExcelWorksheet sheet = excel.Workbook.Worksheets["RejectData"];

                sheet.View.TabSelected = true;
                #region 取得福燈訂單明細
                var items = (from x in db0.Reject
                             join y in db0.Reject_Detail on x.reject_id equals y.reject_id
                             join z in db0.Member_Detail on y.member_detail_id equals z.member_detail_id
                             join w in db0.Users on x.user_id equals w.users_id
                             orderby x.reject_date descending
                             select new RejectReport()
                             {
                                 orders_sn = x.orders_sn,
                                 total = x.total,
                                 reject_date = x.reject_date,
                                 InsertUserName = w.users_name,
                                 light_name = y.light_name,
                                 price = y.price,
                                 member_name = z.member_name
                             }).AsQueryable();

                if (order_sn != "")
                    items = items.Where(x => x.orders_sn == order_sn);

                if (member_name != "")
                    items = items.Where(x => x.member_name.Contains(member_name));

                if (startDate != null && endDate != null)
                {
                    DateTime start = (DateTime.Parse(startDate));
                    DateTime end = (DateTime.Parse(endDate)).AddDays(1);
                    items = items.Where(x => (DateTime)x.reject_date >= start && (DateTime)x.reject_date < end);
                }
                var count = items.Count();
                #endregion


                #region Excel Handle

                int detail_row = 3;

                #region 標題
                sheet.Cells[1, 1].Value = "點燈燈位退訂報表";

                sheet.Cells[2, 1].Value = "訂單編號";
                sheet.Cells[2, 2].Value = "退訂總額";
                sheet.Cells[2, 3].Value = "燈位名稱";
                sheet.Cells[2, 4].Value = "金額";
                sheet.Cells[2, 5].Value = "會員名稱";
                sheet.Cells[2, 6].Value = "退訂時間";
                sheet.Cells[2, 7].Value = "承辦人";
                #endregion

                #region 內容
                foreach (var item in items)
                {
                    sheet.Cells[detail_row, 1].Value = item.orders_sn;
                    sheet.Cells[detail_row, 2].Value = item.total;
                    sheet.Cells[detail_row, 3].Value = item.light_name;
                    sheet.Cells[detail_row, 4].Value = item.price;
                    sheet.Cells[detail_row, 5].Value = item.member_name;
                    sheet.Cells[detail_row, 6].Value = ((DateTime)item.reject_date).ToString("yyyy-MM-dd HH:mm");
                    sheet.Cells[detail_row, 7].Value = item.InsertUserName;
                    detail_row++;
                }
                #endregion

                #region excel排版
                int startColumn = sheet.Dimension.Start.Column;
                int endColumn = sheet.Dimension.End.Column;
                for (int j = startColumn; j <= endColumn; j++)
                {
                    sheet.Column(j).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//靠左對齊
                    // sheet1.Column(i).Width = 80;//固定寬度寫法
                    sheet.Column(j).AutoFit();//依內容fit寬度
                }//End for
                #endregion
                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "燈位退訂報表[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                excel.Save();
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.ToString());
                //Console.Write(ex.Message);
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_GodSON(int temple_account_id)
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {
                var temple_account = db0.TempleAccount.Find(temple_account_id);
                var member = temple_account.TempleMember;
                var user = db0.Users.Find(temple_account.i_InsertUserID);
                var product = db0.Product.Find(temple_account.product_sn);

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/契子會.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);

                ExcelWorksheet sheet = null;
                sheet = excel.Workbook.Worksheets["SheetPrint"];

                sheet.View.TabSelected = true;

                int detail_row = 2;

                #region 主檔
                sheet.Cells["G9"].Value = temple_account.price.GetChineseNumber(); //總金額
                sheet.Cells["B13"].Value = member.member_name; //戶長姓名
                sheet.Cells["D15"].Value = member.zip + member.addr; //戶長地址

                //var race = 0;
                //var gold = 0;

                IList<string> memo = new List<string>();
                //if (gold > 0)
                //    memo.Add("金牌" + gold + "面");
                //if (race > 0)
                //    memo.Add("白米" + race + "斤");

                sheet.Cells["D18"].Value = string.Join(" ", memo); //備註欄 註記金牌及捐米

                DateTime reportdate = temple_account.tran_date == null ? DateTime.Now : (DateTime)temple_account.tran_date;
                sheet.Cells["C21"].Value = reportdate.Year - 1911; //訂單日期年
                sheet.Cells["F21"].Value = reportdate.Month; //訂單日期月
                sheet.Cells["H21"].Value = reportdate.Day; //訂單日期日
                sheet.Cells["L21"].Value = user.users_name; //承辦人員
                sheet.Cells["I23"].Value = temple_account.temple_account_id; //訂單編號 
                #endregion

                #region 明細

                sheet.Cells[detail_row, 15].Value = member.member_name;
                sheet.Cells[detail_row, 16].Value = product.product_name;
                sheet.Cells[detail_row, 17].Value = temple_account.price;

                sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "感謝狀[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            finally
            {
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_MemberPrint(string product_sn, string startDate, string endDate)
        {
            ExcelPackage excel = null;
            MemoryStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                fs = new MemoryStream();
                excel = new ExcelPackage(fs);
                excel.Workbook.Worksheets.Add("GodSONData");
                ExcelWorksheet sheet = excel.Workbook.Worksheets["GodSONData"];

                sheet.View.TabSelected = true;
                #region 取得契子會員名單
                DateTime default_insertdate = DateTime.Parse("2004/1/1");// 預設93/1/1

                var items = (from x in db0.Orders_Detail
                             join y in db0.Member_Detail
                             on x.member_detail_id equals y.member_detail_id
                             orderby y.member_name
                             where x.product_sn == e_祈福產品.香油_契子觀摩
                             select new m_Orders_Detail()
                             {
                                 orders_detail_id = x.orders_detail_id,
                                 member_detail_id = x.member_detail_id,
                                 orders_sn = x.orders_sn,//訂單編號
                                 member_name = y.member_name,//會員姓名
                                 price = x.price,//產品價格
                                 product_sn = x.product_sn,//產品編號
                                 l_birthday = y.lbirthday,//生日
                                 i_InsertDateTime = x.i_InsertDateTime,
                                 address = y.address,
                                 tel = y.tel,
                                 mobile = y.mobile,
                                 member_insertDateTime = y.i_InsertDateTime,
                                 memo = y.Memo,
                                 sno = y.sno,
                                 insure_birthday = y.insure_birthday
                             }).AsQueryable();



                //var items = (from x in db0.TempleAccount
                //             join y in db0.TempleMember
                //             on x.temple_member_id equals y.temple_member_id
                //             orderby y.member_name
                //             select new m_TempleAccount()
                //             {
                //                 temple_account_id = x.temple_account_id,
                //                 tran_date = x.tran_date,
                //                 price = x.price,
                //                 product_sn = x.product_sn,
                //                 temple_member_id = x.temple_member_id,
                //                 member_name = y.member_name,
                //                 sno = y.sno,
                //                 birthday = y.birthday,
                //                 tel = y.tel,
                //                 mobile = y.mobile,
                //                 zip = y.zip,
                //                 addr = y.addr,
                //                 member_insertDateTime = y.join_datetime == null ? default_insertdate : y.join_datetime
                //             }).AsQueryable();

                if (product_sn != "")
                    items = items.Where(x => x.product_sn == product_sn);

                if (startDate != null && endDate != null)
                {
                    DateTime start = (DateTime.Parse(startDate));
                    DateTime end = (DateTime.Parse(endDate)).AddDays(1);
                    items = items.Where(x => (DateTime)x.i_InsertDateTime >= start && (DateTime)x.i_InsertDateTime < end);
                }
                var count = items.Count();
                #endregion


                #region Excel Handle

                int detail_row = 3;

                #region 標題
                sheet.Cells[1, 1].Value = "契子觀摩會員名冊";

                sheet.Cells[2, 1].Value = "會員姓名";
                sheet.Cells[2, 2].Value = "電話";
                sheet.Cells[2, 3].Value = "手機";
                sheet.Cells[2, 4].Value = "身分證字號";
                sheet.Cells[2, 5].Value = "國曆投保生日";
                //sheet.Cells[2, 6].Value = "產品名稱";
                sheet.Cells[2, 6].Value = "繳款金額";
                sheet.Cells[2, 7].Value = "繳款日期";
                //sheet.Cells[2, 8].Value = "會員備註";
                #endregion

                #region 內容
                List<GodSONProduct> psn = new List<GodSONProduct>() { new GodSONProduct { product_sn = "751", product_name = "契子會(入會)" },
                                                                      new GodSONProduct{ product_sn = "752", product_name = "契子會(大會)"},
                                                                      new GodSONProduct{ product_sn = "753", product_name = "香油(契子觀摩)"}};//契子產品名稱
                foreach (var item in items)
                {
                    string product_name = string.Empty;
                    foreach (var i in psn)
                    {
                        if (i.product_sn == item.product_sn)
                        {
                            product_name = i.product_name;
                            break;
                        }
                    };
                    sheet.Cells[detail_row, 1].Value = item.member_name;
                    sheet.Cells[detail_row, 2].Value = item.tel;
                    sheet.Cells[detail_row, 3].Value = item.mobile;
                    sheet.Cells[detail_row, 4].Value = item.sno;
                    sheet.Cells[detail_row, 5].Value = item.insure_birthday;
                    //sheet.Cells[detail_row, 6].Value = product_name;
                    sheet.Cells[detail_row, 6].Value = item.price;
                    sheet.Cells[detail_row, 7].Value = getTaiwanCalendarDate(item.i_InsertDateTime);
                    //sheet.Cells[detail_row, 8].Value = item.memo;
                    detail_row++;
                }
                #endregion

                #region excel排版
                int startColumn = sheet.Dimension.Start.Column;
                int endColumn = sheet.Dimension.End.Column;
                for (int j = startColumn; j <= endColumn; j++)
                {
                    sheet.Column(j).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//靠左對齊
                    // sheet1.Column(i).Width = 80;//固定寬度寫法
                    sheet.Column(j).AutoFit();//依內容fit寬度
                }//End for
                #endregion
                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "契子觀摩會會員名冊[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                excel.Save();
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            finally
            {
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_PostPrint(string startDate, string endDate)
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/契子郵寄標籤.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);
                ExcelWorksheet sheet = excel.Workbook.Worksheets["SheetPrint"];

                sheet.View.TabSelected = true;
                #region 取得契子會員名單
                DateTime default_insertdate = DateTime.Parse("2004/1/1");// 預設93/1/1

                var getItems = (from x in db0.TempleMember
                                orderby x.zip, x.addr
                                where x.is_close == false
                                select new m_TempleMember()
                                {
                                    temple_member_id = x.temple_member_id,
                                    member_name = x.member_name,
                                    tel = x.tel,
                                    mobile = x.mobile,
                                    zip = x.zip,
                                    addr = x.addr,
                                    sno = x.sno,
                                    birthday = x.birthday,
                                    join_datetime = x.join_datetime == null ? default_insertdate : x.join_datetime
                                });


                if (startDate != null && endDate != null)
                {
                    DateTime start = (DateTime.Parse(startDate));
                    DateTime end = (DateTime.Parse(endDate)).AddDays(1);
                    getItems = getItems.Where(x => (DateTime)x.join_datetime >= start && (DateTime)x.join_datetime < end);
                }
                #endregion

                #region 複製列高
                int page = Convert.ToInt16(Math.Ceiling((Double)getItems.Count() / 22));//一頁22筆資料
                if (page > 1)
                {
                    for (var i = 1; i < page; i++)
                    {//從第二頁開始複製列高
                        for (var j = 1; j <= 6; j++)
                        {//一頁6列
                            sheet.Row(j + (i * 6)).Height = sheet.Row(j).Height;
                        }
                    }
                }
                #endregion

                #region Excel Handle

                int detail_row = 2;
                #region 主檔
                for (int i = 0; i < page; i++)
                { //此迴圈跑的事頁數(起始值0方便乘倍數)

                    var getMember = getItems.Skip(i * 22).Take(22);//取22筆會員(一頁22筆)

                    for (int j = 0; j < 2; j++)
                    {//此迴圈跑的是每頁行數(一頁分兩行)

                        var getRowItem = getMember.Skip(j * 11).Take(11);//來放每行資料

                        int index_halfpage = 1;
                        foreach (var item in getRowItem)
                        {
                            int column = index_halfpage * 2;//一筆資料佔兩行

                            #region 複製格式
                            if (i > 0)//一頁以後才開始複製
                            {
                                sheet.Cells["B2"].Copy(sheet.Cells[detail_row, column]);//姓名
                                sheet.Cells["C2"].Copy(sheet.Cells[detail_row, column + 1]);//郵遞區號
                                sheet.Cells["C3"].Copy(sheet.Cells[detail_row + 1, column + 1]);//地址
                            }
                            #endregion
                            sheet.Cells[detail_row, column].Value = "契子  " + item.member_name;
                            sheet.Cells[detail_row, column + 1].Value = item.zip;
                            sheet.Cells[detail_row + 1, column + 1].Value = item.addr;
                            #region 合併儲存格(合併要在copy格式之後做否則會出錯)
                            if (i > 0)
                                sheet.Cells[detail_row, column, detail_row + 1, column].Merge = true;//合併姓名儲存格
                            #endregion
                            index_halfpage++;
                        }

                        detail_row += 3;
                    }
                }



                #endregion

                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "契子郵寄標籤[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.ToString());
                //Console.Write(ex.Message);
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_MemberPrintAll(string member_name, string startDate, string endDate)
        {
            ExcelPackage excel = null;
            MemoryStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                fs = new MemoryStream();
                excel = new ExcelPackage(fs);
                excel.Workbook.Worksheets.Add("GodSONData");
                ExcelWorksheet sheet = excel.Workbook.Worksheets["GodSONData"];

                sheet.View.TabSelected = true;
                #region 取得契子會員名單
                DateTime default_insertdate = DateTime.Parse("2004/1/1");// 預設93/1/1

                var items = (from x in db0.TempleMember
                             where x.is_close == false
                             orderby x.member_name
                             select new m_TempleMember()
                             {
                                 temple_member_id = x.temple_member_id,
                                 member_name = x.member_name,
                                 sno = x.sno,
                                 birthday = x.birthday,
                                 tel = x.tel,
                                 mobile = x.mobile,
                                 zip = x.zip,
                                 addr = x.addr,
                                 join_datetime = x.join_datetime == null ? default_insertdate : x.join_datetime
                             }).AsQueryable();

                if (member_name != "")
                    items = items.Where(x => x.member_name.Contains(member_name));

                if (startDate != null && endDate != null)
                {
                    DateTime start = (DateTime.Parse(startDate));
                    DateTime end = (DateTime.Parse(endDate)).AddDays(1);
                    items = items.Where(x => (DateTime)x.join_datetime >= start && (DateTime)x.join_datetime < end);
                }
                var count = items.Count();
                #endregion


                #region Excel Handle

                int detail_row = 3;

                #region 標題
                sheet.Cells[1, 1].Value = "契子會會員名冊";

                sheet.Cells[2, 1].Value = "會員姓名";
                sheet.Cells[2, 2].Value = "郵遞區號";
                sheet.Cells[2, 3].Value = "地址";
                sheet.Cells[2, 4].Value = "電話";
                sheet.Cells[2, 5].Value = "手機";
                sheet.Cells[2, 6].Value = "加入日期";
                sheet.Cells[2, 7].Value = "身分證字號";
                sheet.Cells[2, 8].Value = "生日";
                ;
                #endregion

                #region 內容

                foreach (var item in items)
                {
                    sheet.Cells[detail_row, 1].Value = item.member_name;
                    sheet.Cells[detail_row, 2].Value = item.zip;
                    sheet.Cells[detail_row, 3].Value = item.addr;
                    sheet.Cells[detail_row, 4].Value = item.tel;
                    sheet.Cells[detail_row, 5].Value = item.mobile;
                    sheet.Cells[detail_row, 6].Value = getTaiwanCalendarDate((DateTime)item.join_datetime);
                    sheet.Cells[detail_row, 7].Value = item.sno;
                    sheet.Cells[detail_row, 8].Value = item.birthday;

                    detail_row++;
                }
                #endregion

                #region excel排版
                int startColumn = sheet.Dimension.Start.Column;
                int endColumn = sheet.Dimension.End.Column;
                for (int j = startColumn; j <= endColumn; j++)
                {
                    sheet.Column(j).Style.HorizontalAlignment = ExcelHorizontalAlignment.Left;//靠左對齊
                    // sheet1.Column(i).Width = 80;//固定寬度寫法
                    sheet.Column(j).AutoFit();//依內容fit寬度
                }//End for
                #endregion
                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "契子會會員名冊[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                excel.Save();
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                return null;
            }
            finally
            {
                db0.Dispose();
            }
        }
        public FileResult downloadExcel_PostMember(string zipcode, int year)
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/郵寄標籤.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);
                ExcelWorksheet sheet = excel.Workbook.Worksheets["label"];

                sheet.View.TabSelected = true;
                #region 取得會員戶長資料
                string[] light_category = new string[] { "點燈", "福燈" };

                var getItems = (from x in db0.Member_Detail
                                orderby x.Member.zip, x.Member.address //地址排序
                                //orderby x.householder //姓名
                                where x.is_holder & x.Orders_Detail.Any(y => y.Y == year & light_category.Contains(y.Product.category)) & !x.Member.repeat_mark & !x.is_delete
                                select new m_Member()
                                {
                                    member_id = x.member_id,
                                    address = x.Member.address,
                                    zip = x.Member.zip,
                                    householder = x.Member.householder
                                }).Distinct();

                string[] zip = new string[] { "320", "324", "326" };

                if (zipcode != null)
                {
                    if (zip.Contains(zipcode))
                    {
                        getItems = getItems.Where(x => x.zip == zipcode).OrderBy(x => new { x.zip, x.address });
                    }
                    else
                    {
                        getItems = getItems.Where(x => !zip.Contains(x.zip)).OrderBy(x => new { x.zip, x.address });
                    }

                }
                //地址不重複
                //var GetUnique = getItems.GroupBy(x => x.address).Select(g => g.First()).ToList();



                #endregion

                #region 複製列高
                int page = getItems.Count();//一頁1筆資料
                if (page > 1)
                {
                    for (var i = 1; i < page; i++)
                    {//從第二頁開始複製列高
                        for (var j = 1; j <= 6; j++)
                        {//一頁6列
                            sheet.Row(j + (i * 6)).Height = sheet.Row(j).Height;
                        }
                    }
                }
                #endregion

                #region Excel Handle

                int detail_row = 2;
                int name_col = 2;
                int add_col = 4;

                #region 主檔

                foreach (var item in getItems)
                {
                    #region 複製格式
                    if (detail_row > 2)//一頁以後才開始複製
                    {
                        sheet.Cells["B3"].Copy(sheet.Cells[detail_row + 1, name_col]);//姓名
                        sheet.Cells["D2"].Copy(sheet.Cells[detail_row, add_col]);//郵遞區號
                        sheet.Cells["D3"].Copy(sheet.Cells[detail_row + 1, add_col]);//地址
                    }
                    #endregion
                    sheet.Cells[detail_row + 1, name_col].Value = item.householder;//姓名
                    sheet.Cells[detail_row, add_col].Value = item.zip;//郵遞區號
                    sheet.Cells[detail_row + 1, add_col].Value = item.address;//地址
                    #region 合併儲存格(合併要在copy格式之後做否則會出錯)
                    if (detail_row > 2)
                        sheet.Cells[detail_row + 1, add_col, detail_row + 2, add_col].Merge = true;//合併地址儲存格
                    #endregion

                    #region 換頁(分頁breaks)設定
                    //sheet.Row(detail_row + 4).PageBreak = true;
                    #endregion
                    detail_row += 6;
                }

                #endregion

                //sheet.Cells.Calculate(); //要對所以Cell做公計計算 否則樣版中的公式值是不會變的

                #endregion

                string filename = "郵寄標籤[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Log.Write(ex.Message);
                Log.Write(ex.ToString());
                //Console.Write(ex.Message);
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }
        public FileResult TEST()
        {
            ExcelPackage excel = null;
            FileStream fs = null;
            var db0 = getDB();
            try
            {

                var user = db0.Users.Find(this.UserId);

                string ExcelTemplateFile = Server.MapPath("~/_Code/Excel/test.xlsx");
                FileInfo finfo = new FileInfo(ExcelTemplateFile);
                excel = new ExcelPackage(finfo, true);
                ExcelWorksheet sheet = excel.Workbook.Worksheets.First();

                sheet.View.TabSelected = true;

                //sheet.Cells["a3:j4"].Copy(sheet.Cells["a6:j7"]);
                //Merging cells and create a center heading for out table
                //sheet.Cells[1, 1].Value = "你好測試"; // Heading Name
                //household(sheet, 1, 1, 3, 1);
                //sheet.Cells["A1"].Copy(sheet.Cells["C3"]);
                //sheet.Cells["C3:D4"].Merge = true;
                //sheet.Cells["A1:R10"].Copy(sheet.Cells["A31:R40"]);

                DateTime default_insertdate = DateTime.Parse("2004/1/1");// 預設93/1/1

                List<string> GodSN_psn = new List<string> { "752", "753" };//契子產品編號
                var Orders_detail = (from x in db0.Orders_Detail
                                     where x.C_InsertDateTime > default_insertdate
                                     where GodSN_psn.Contains(x.product_sn)
                                     orderby x.member_detail_id
                                     select x.member_detail_id).Distinct().ToList();

                List<string> haveTransactionMember2 = (from x in db0.Member_Detail
                                                       where Orders_detail.Contains(x.member_detail_id)
                                                       group x by x.member_name into g
                                                       orderby g.Key
                                                       where g.Count() == 1
                                                       select g.Key).ToList();

                List<string> haveTransactionMember3 = (from x in db0.Member_Detail
                                                       where Orders_detail.Contains(x.member_detail_id)
                                                       group x by x.member_name into g
                                                       orderby g.Key
                                                       where g.Count() > 1
                                                       select g.Key).ToList();

                int detail_row = 1;
                foreach (var item in Orders_detail)
                {
                    sheet.Cells[detail_row, 1].Value = item;
                    detail_row++;
                }

                detail_row = 1;
                foreach (var item in haveTransactionMember2)
                {
                    sheet.Cells[detail_row, 2].Value = item;
                    detail_row++;
                }
                detail_row = 1;
                foreach (var item in haveTransactionMember3)
                {
                    sheet.Cells[detail_row, 3].Value = item;
                    detail_row++;
                }

                string filename = "TEST[" + user.users_name + "][" + DateTime.Now.ToString("yyyyMMddHHmm") + "].xlsx";
                string filepath = Server.MapPath("~/_Code/Log/" + filename);
                fs = new FileStream(filepath, FileMode.Create, FileAccess.ReadWrite);
                excel.SaveAs(fs);
                fs.Position = 0;
                return File(fs, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", filename);
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message);
                Session["CurrentError"] = ex.Message;
                return null;
            }
            finally
            {
                //fs.Close();
                //fs.Dispose();
                db0.Dispose();
            }
        }

        public void light_name(ExcelWorksheet sheet, int row, int column)
        {

            sheet.Cells[row, column].Style.Font.Size = 10; //Font 字型大小20
        }
        public void length4HouseHold(ExcelWorksheet sheet, int row, int column)
        {   //福燈標籤戶長姓名超過四個字以上就套用的格式
            sheet.Cells[row, column].Style.VerticalAlignment = ExcelVerticalAlignment.Top;//靠上對齊
            sheet.Cells[row, column].Style.WrapText = true;//自動換行
            sheet.Cells[row, column].Style.Font.Size = 16;//字型大小16px
        }
        public void length4Member(ExcelWorksheet sheet, int row, int column)
        {   //福燈標籤成員姓名超過四個字以上就套用的格式
            sheet.Cells[row, column].Style.WrapText = true;//自動換行
            sheet.Cells[row, column].Style.Font.Size = 8;//字型大小8px
        }
        public void setShapeStlye(ExcelShape shape, int row, int column, int rowoff, int columnoff)
        {
            shape.Fill.Style = eFillStyle.NoFill;//無填滿
            shape.Border.Fill.Color = Color.Black;//黑色外框
            //位置設定
            shape.From.Row = row;
            shape.From.Column = column;
            shape.From.RowOff = Pixel2MTU(rowoff);
            shape.From.ColumnOff = Pixel2MTU(columnoff);
            //位置設定
            shape.SetSize(555, 302);//大小
        }
        public int Pixel2MTU(int pixels)
        {
            int mtus = pixels * 9525;
            return mtus;
        }
        protected string getTaiwanCalendarDate(DateTime date)
        {
            System.Globalization.TaiwanCalendar TC = new System.Globalization.TaiwanCalendar();
            return string.Format("{0}/{1}/{2}", TC.GetYear(date), TC.GetMonth(date), TC.GetDayOfMonth(date));
        }
    }
}
public class GodSONProduct
{
    public string product_sn { get; set; }
    public string product_name { get; set; }
}
