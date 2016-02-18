using DotWeb.CommSetup;
using DotWeb.Helpers;
using DotWeb.WebApp;
using ProcCore.Business;
using ProcCore.Business.DB0;
using ProcCore.NetExtension;
using ProcCore.HandleResult;
using ProcCore.WebCore;
using System;
using System.Linq;
using System.Web.Mvc;

namespace DotWeb.Areas.Sys_Active.Controllers
{
    public class FAQDataController : CtrlId<FAQ, q_FAQ>, IGetMasterNewId, IAddressTW
    {
        #region Action and function section
        public ActionResult Main()
        {
            ActionRun();
            return View(new c_FAQ());
        }
        #endregion

        #region ajax call section
        public override String aj_Init()
        {
            return defJSON(
            new
            {
                //options_Customer_category = ngCodeToOption(CodeSheet.CustomerCategory.MakeCodes())
            }
            );
        }
        [HttpPost]
        public override String aj_MasterGet(int id)
        {
            using (db0 = openLogic().getDB0)
            {
                var Async = db0.FAQ.FindAsync(id);
                Async.Wait();
                item = Async.Result;
                r = new rAjaxGetData<FAQ>() { data = item };
            }

            return defJSON(r);
        }
        [HttpPost]
        public override String aj_MasterSearch(q_FAQ q)
        {
            #region 連接BusinessLogicLibary資料庫並取得資料

            using (db0 = openLogic().getDB0)
            {
                var items = (from x in db0.FAQ
                             orderby x.FAQ_id
                             where x.i_Hide == false
                             select new m_FAQ()
                             {
                                 FAQ_id = x.FAQ_id,
                                 FAQ_name = x.FAQ_name,
                                 FAQ_title = x.FAQ_title,
                                 FAQ_content=x.FAQ_content,
                                 FAQ_email = x.FAQ_email                            
                             });

                //if (q.customer_name != null)
                //    items = items.Where(x => x.customer_name.Contains(q.customer_name));


                int page = (q.page == null ? 1 : q.page.CInt());
                int startRecord = PageCount.PageInfo(page, this.defPageSize, items.Count());

                items = items.Skip(startRecord).Take(this.defPageSize);

                return defJSON(new GridInfo2<m_FAQ>()
                {
                    rows = items,
                    total = PageCount.TotalPage,
                    page = PageCount.Page,
                    records = PageCount.RecordCount,
                    startcount = PageCount.StartCount,
                    endcount = PageCount.EndCount
                }); 
            }
            #endregion
        }
        [HttpPost]
        public override String aj_MasterUpdate(FAQ md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            try
            {
                using (db0 = openLogic().getDB0)
                {
                    item = db0.FAQ.Find(md.FAQ_id);

                    item.FAQ_id = md.FAQ_id;
                    item.FAQ_name = md.FAQ_name;

                    item.i_UpdateUserID = aspUserId;
                    item.i_UpdateDeptID = departmentId;
                    item.i_UpdateDateTime = DateTime.Now;
                    var Async = db0.SaveChangesAsync();
                    Async.Wait();
                }
                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }
            return defJSON(rAjaxResult);
        }
        [HttpPost]
        public override string aj_MasterInsert(FAQ md)
        {
            ResultInfo rAjaxResult = new ResultInfo();
            using (db0 = openLogic().getDB0)
            {
                try
                {
                    md.i_InsertUserID = aspUserId;
                    md.i_InsertDeptID = departmentId;
                    md.i_InsertDateTime = DateTime.Now;
                    md.i_Lang = getNowLnag();

                    db0.FAQ.Add(md);
                    var Async = db0.SaveChangesAsync();
                    Async.Wait();

                    rAjaxResult.result = true;
                    rAjaxResult.id = md.FAQ_id;
                }
                catch (Exception ex)
                {
                    rAjaxResult.result = false;
                    rAjaxResult.message = ex.Message;
                }
                return defJSON(rAjaxResult);
            }
        }
        [HttpPost]
        public override String aj_MasterDel(Int32[] ids)
        {
            ResultInfo rAjaxResult = new ResultInfo();

            try
            {
                db0 = openLogic().getDB0;

                foreach (var id in ids)
                {
                    item = new FAQ() { FAQ_id = id };
                    db0.FAQ.Attach(item);
                    db0.FAQ.Remove(item);
                }

                var Async = db0.SaveChangesAsync();
                Async.Wait();

                rAjaxResult.result = true;
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.message = ex.Message;
            }

            return defJSON(rAjaxResult);
        }
        [HttpPost]
        public String ajax_GetMasterNewId()
        {
            return defJSON(GetNewId(CodeTable.Customer));
        }

        public String ajax_AdrTW(String val)
        {
            db0 = openLogic().getDB0;
            var adrs = db0.i_Adr_zh_TW.Where(x => x.data.StartsWith(val)).Take(10);
            return defJSON(adrs);
        }

        [HttpPost]
        public String tyh_Customer(String val)
        {
            db0 = openLogic().getDB0;
            var items = db0.Customer
                .Where(x => x.customer_name.Contains(val) || x.index_code.StartsWith(val))
                .Select(x => new { x.customer_id, x.customer_name, x.tel })
                .Take(10);

            return defJSON(items);
        }
        #endregion

        #region ajax file section
        [HttpPost]
        public String aj_FUpload(Int32 id, String FilesKind, String hd_FileUp_EL)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();

            #region
            String tpl_File = String.Empty;
            try
            {
                //只處理圖片
                if (FilesKind == "SingleImg")
                    HandImageSave(hd_FileUp_EL, id, ImageFileUpParm.NewsBasicSingle, FilesKind);

                if (FilesKind == "DoubleImg")
                    HandImageSave(hd_FileUp_EL, id, ImageFileUpParm.NewsBasicDouble, FilesKind);

                rAjaxResult.result = true;
                rAjaxResult.success = true;
                rAjaxResult.FileName = hd_FileUp_EL.GetFileName();

            }
            catch (LogicError ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = getRecMessage(ex.Message);
            }
            catch (Exception ex)
            {
                rAjaxResult.result = false;
                rAjaxResult.success = false;
                rAjaxResult.error = ex.Message;
            }
            #endregion
            return defJSON(rAjaxResult);
        }

        [HttpPost]
        public String aj_FList(int id, String FileKind)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            rAjaxResult.filesObject = ListSysFiles(id, FileKind);
            rAjaxResult.result = true;
            return defJSON(rAjaxResult);
        }

        [HttpPost]
        public String aj_FDelete(int id, String FileKind, String FileName)
        {
            ReturnAjaxFiles rAjaxResult = new ReturnAjaxFiles();
            DeleteSysFile(id, FileKind, FileName, ImageFileUpParm.NewsBasicSingle);
            rAjaxResult.result = true;
            return defJSON(rAjaxResult);
        }
        #endregion
    }
}