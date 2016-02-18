using ProcCore.Business.DB0;
using ProcCore.HandleResult;
using ProcCore.NetExtension;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.EntityClient;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using System.Transactions;

namespace ProcCore.Business
{
    public enum CodeTable
    {
        Base, Orders, Orders_Detail, Member, Member_Detail,Light_Site
    }
    public enum SNType
    {
        Orders
    }
}
namespace ProcCore.Business.LogicConect
{
    #region Code Replay Area
    public static class CodeSheet
    {
        public static news_category news_category = new news_category()
        {
            Active = new i_Code() { Code = "Active", Value = "最新活動", LangCode = "C_news_category_Active" },
            News = new i_Code() { Code = "News", Value = "最新消息", LangCode = "C_news_category_News" },
            Post = new i_Code() { Code = "Post", Value = "最新公告", LangCode = "C_news_category_Post" }
        };
        public static user_state user_state = new user_state()
        {
            Normal = new i_Code() { Code = "Normal", Value = "正常", LangCode = "C_user_state_Normal" },
            Stop = new i_Code() { Code = "Stop", Value = "停止", LangCode = "C_user_state_Stop" }
        };
        public static product_state product_state = new product_state()
        {
            NOR = new i_Code() { Code = "NOR", Value = "正常", LangCode = "C_product_state_NOR" },
            STP = new i_Code() { Code = "STP", Value = "停止", LangCode = "C_product_state_STP" }
        };
        public static pay_type pay_type = new pay_type()
        {
            CASH = new i_Code() { Code = "CASH", Value = "現金", LangCode = "C_pay_type_CASH" },
            CHECK = new i_Code() { Code = "CHECK", Value = "支票", LangCode = "C_pay_type_CHECK" }
        };
        public static pay_state pay_state = new pay_state()
        {
            WAIT = new i_Code() { Code = "WAIT", Value = "尚未付款", LangCode = "C_pay_state_WAIT" },
            INSUFFICIENT = new i_Code() { Code = "INSUFFICIENT", Value = "付款不足", LangCode = "C_pay_state_INSUFFICIENT" },
            FINISH = new i_Code() { Code = "FINISH", Value = "完成付款", LangCode = "C_pay_state_FINISH" }
        };
        public static tax_type tax_type = new tax_type()
        {
            FREE = new i_Code() { Code = "FREE", Value = "免稅", LangCode = "C_tax_type_FREE" },
            EXCL = new i_Code() { Code = "EXCL", Value = "未含稅", LangCode = "C_tax_type_EXCL" },
            INCL = new i_Code() { Code = "INCL", Value = "已含稅", LangCode = "C_tax_type_INCL" }
        };
        public static receiver_state receiver_state = new receiver_state()
        {
            New = new i_Code() { Code = "New", Value = "新單", LangCode = "C_receiver_state_New" },
            Done = new i_Code() { Code = "Done", Value = "完成", LangCode = "C_receiver_state_Done" }
        };
        public static receiver_source receiver_source = new receiver_source()
        {
            Outer = new i_Code() { Code = "Outer", Value = "外出收貨", LangCode = "C_receiver_source_Outer" },
            Inner = new i_Code() { Code = "Inner", Value = "廠內收貨", LangCode = "C_receiver_source_Inner" }
        };

        public static export_state export_state = new export_state()
        {
            New = new i_Code() { Code = "New", Value = "新單", LangCode = "C_export_state_New" },
            Done = new i_Code() { Code = "Done", Value = "完成", LangCode = "C_export_state_Done" }
        };
        public static import_state import_state = new import_state()
        {
            New = new i_Code() { Code = "New", Value = "新單", LangCode = "C_import_state_New" },
            Done = new i_Code() { Code = "Done", Value = "完成", LangCode = "C_import_state_Done" }
        };
        public static stockadj_state stockadj_state = new stockadj_state()
        {
            New = new i_Code() { Code = "New", Value = "新單", LangCode = "C_stockadj_state_New" },
            Done = new i_Code() { Code = "Done", Value = "完成", LangCode = "C_stockadj_state_Done" }
        };
    }
    public class news_category : BaseSheet
    {
        public i_Code Active { get; set; }
        public i_Code News { get; set; }
        public i_Code Post { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.Active, this.News, this.Post }); return base.MakeCodes();
        }
    }
    public class user_state : BaseSheet
    {
        public i_Code Normal { get; set; }
        public i_Code Stop { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.Normal, this.Stop }); return base.MakeCodes();
        }
    }
    public class product_state : BaseSheet
    {
        public i_Code NOR { get; set; }
        public i_Code STP { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.NOR, this.STP }); return base.MakeCodes();
        }
    }
    public class pay_type : BaseSheet
    {
        public i_Code CASH { get; set; }
        public i_Code CHECK { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.CASH, this.CHECK }); return base.MakeCodes();
        }
    }
    public class pay_state : BaseSheet
    {
        public i_Code WAIT { get; set; }
        public i_Code INSUFFICIENT { get; set; }
        public i_Code FINISH { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.WAIT, this.INSUFFICIENT, this.FINISH }); return base.MakeCodes();
        }
    }
    public class tax_type : BaseSheet
    {
        public i_Code FREE { get; set; }
        public i_Code EXCL { get; set; }
        public i_Code INCL { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.FREE, this.EXCL, this.INCL }); return base.MakeCodes();
        }
    }
    public class receiver_state : BaseSheet
    {
        public i_Code New { get; set; }
        public i_Code Done { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.New, this.Done }); return base.MakeCodes();
        }
    }
    public class receiver_source : BaseSheet
    {
        public i_Code Outer { get; set; }
        public i_Code Inner { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.Outer, this.Inner }); return base.MakeCodes();
        }
    }
    public class import_state : BaseSheet
    {
        public i_Code New { get; set; }
        public i_Code Done { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.New, this.Done }); return base.MakeCodes();
        }
    }
    public class export_state : BaseSheet
    {
        public i_Code New { get; set; }
        public i_Code Done { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.New, this.Done }); return base.MakeCodes();
        }
    }
    public class stockadj_state : BaseSheet
    {
        public i_Code New { get; set; }
        public i_Code Done { get; set; }
        public override List<i_Code> MakeCodes()
        {
            this.Codes = new List<i_Code>(); this.Codes.AddRange(new i_Code[] { this.New, this.Done }); return base.MakeCodes();
        }
    }
    #endregion

    public class LogicCenter
    {
        private static string db0_connectionstring;
        protected Renhai_LightSiteEntities db0;
        protected TransactionScope tx;
        public int DepartmentId { get; set; }
        public string Lang { get; set; }
        public string IP { get; set; }
        public string AspUserID { get; set; }

        public static string GetDB0EntityString(string configstring)
        {
            string[] DataConnectionInfo = configstring.Split(',');

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DataConnectionInfo[0];
            builder.UserID = DataConnectionInfo[1];
            builder.Password = DataConnectionInfo[2];
            builder.InitialCatalog = "Renhai_LightSite";
            builder.IntegratedSecurity = false;
            builder.PersistSecurityInfo = false;

            EntityConnectionStringBuilder entBuilder = new EntityConnectionStringBuilder();
            entBuilder.Provider = "System.Data.SqlClient";
            entBuilder.ProviderConnectionString = builder.ConnectionString;
            entBuilder.Metadata = String.Format("res://{0}/{1}.csdl|res://{0}/{1}.ssdl|res://{0}/{1}.msl", "Proc.BusinessLogic", "DB0.Renhai_LightSite");
            return entBuilder.ConnectionString;
        }
        public static string GetDB0ConnectionString(string configstring)
        {
            string[] DataConnectionInfo = configstring.Split(',');

            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder();
            builder.DataSource = DataConnectionInfo[0];
            builder.UserID = DataConnectionInfo[1];
            builder.Password = DataConnectionInfo[2];
            builder.InitialCatalog = "Renhai_LightSite";
            builder.MultipleActiveResultSets = true;
            builder.IntegratedSecurity = false;

            return builder.ConnectionString;
        }
        public static void SetDB0EntityString(string configstring)
        {
            db0_connectionstring = GetDB0EntityString(configstring);
        }
        public static Renhai_LightSiteEntities getDB0
        {
            get
            {
                return new Renhai_LightSiteEntities(db0_connectionstring);
            }
        }
        public LogicCenter(string db0_configstring)
        {
            db0_connectionstring = LogicCenter.GetDB0EntityString(db0_configstring);
        }
        public int GetNewId(ProcCore.Business.CodeTable tab)
        {
            int i = 0;

            using (var tx = new TransactionScope())
            {
                var get_id_db = getDB0;

                try
                {
                    string tab_name = Enum.GetName(typeof(ProcCore.Business.CodeTable), tab);
                    var item = get_id_db.i_IDX.Where(x => x.table_name == tab_name)
                        .ToList()
                        .FirstOrDefault();

                    if (item != null)
                    {
                        item.IDX++;
                        get_id_db.SaveChanges();
                        tx.Complete();
                        i = item.IDX;
                    }
                }
                catch (Exception ex)
                {
                    Log.Write(ex.ToString());
                }
                finally
                {
                    get_id_db.Dispose();
                }
                return i;
            }
        }
        private snObject GetSN(ProcCore.Business.SNType tab)
        {
            snObject sn = new snObject();

            using (var tx = new TransactionScope())
            {
                var get_sn_db = getDB0;
                try
                {
                    get_sn_db = getDB0;
                    string tab_name = Enum.GetName(typeof(ProcCore.Business.SNType), tab);
                    var items = get_sn_db.i_SN.Single(x => x.sn_type == tab_name);

                    if (items.y == DateTime.Now.Year &&
                        items.m == DateTime.Now.Month &&
                        items.d == DateTime.Now.Day
                        )
                    {
                        int now_max = items.sn_max;
                        now_max++;
                        items.sn_max = now_max;
                    }
                    else
                    {
                        items.y = DateTime.Now.Year;
                        items.m = DateTime.Now.Month;
                        items.d = DateTime.Now.Day;
                        items.sn_max = 1;
                    }

                    get_sn_db.SaveChanges();
                    tx.Complete();

                    sn.y = items.y;
                    sn.m = items.m;
                    sn.d = items.d;
                    sn.sn_max = items.sn_max;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    get_sn_db.Dispose();
                }
            }
            return sn;
        }
        public string getOrderSN() //
        {
            String tpl = "TY{0}{1:00}{2:00}-{3:00}";
            snObject sn = GetSN(ProcCore.Business.SNType.Orders);
            return string.Format(tpl, sn.y.ToString().Right(2), sn.m, sn.d, sn.sn_max);
        }
    }

    #region report def class

    public class ReportCenter : LogicCenter
    {

        public ReportCenter(string config_string)
            : base(config_string)
        {
            this.db0 = getDB0;
        }

        //public rpt_receiver[] receiver(DateTime? sdate, DateTime? edate)
        //{
        //    var items = db0.Receiver_Detail.Select(x => new rpt_receiver
        //    {
        //        receiver_doc_date = x.Receiver.receiver_doc_date,
        //        receiver_doc_sn = x.receiver_doc_sn,
        //        qty = x.qty,
        //        unit_price = x.unit_price,
        //        sub_total_price = x.sub_total_price,
        //        product_name = x.product_name
        //    });

        //    if (sdate != null)
        //        items = items.Where(x => x.receiver_doc_date >= sdate);
        //    if (edate != null)
        //        items = items.Where(x => x.receiver_doc_date <= edate);

        //    return items.ToArray();
        //}

    }
    //public class rpt_receiver
    //{
    //    public string receiver_doc_sn { get; set; }
    //    public System.DateTime receiver_doc_date { get; set; }
    //    public string product_name { get; set; }
    //    public decimal unit_price { get; set; }
    //    public decimal qty { get; set; }
    //    public string count_unit_name { get; set; }
    //    public decimal sub_total_price { get; set; }
    //}
    //public class rpt_import
    //{
    //    public string import_doc_sn { get; set; }
    //    public System.DateTime import_doc_date { get; set; }
    //    public string product_name { get; set; }
    //    public decimal unit_price { get; set; }
    //    public decimal qty { get; set; }
    //    public string count_unit_name { get; set; }
    //    public decimal sub_total_price { get; set; }
    //}
    //public class rpt_export
    //{
    //    public string export_doc_sn { get; set; }
    //    public System.DateTime export_doc_date { get; set; }
    //    public string product_name { get; set; }
    //    public decimal unit_price { get; set; }
    //    public decimal qty { get; set; }
    //    public string count_unit_name { get; set; }
    //    public decimal sub_total_price { get; set; }
    //}
    //public class rpt_stock
    //{
    //    public string product_sn { get; set; }
    //    public string product_name { get; set; }
    //    public decimal import_qty { get; set; }
    //    public decimal export_qty { get; set; }
    //    public string count_unit_name { get; set; }
    //    public decimal now_qty { get; set; }
    //}
    //public class rpt_product
    //{
    //    public int product_id { get; set; }
    //    public string product_name { get; set; }
    //    public string product_sn { get; set; }
    //    public decimal export_price_now { get; set; }
    //    public decimal import_price_average { get; set; }
    //    public decimal export_price_average { get; set; }
    //    public decimal product_stock_now { get; set; }
    //    public int count_unit_id { get; set; }
    //    public string count_unit_name { get; set; }
    //    public string currency { get; set; }
    //    public int product_category_l1_id { get; set; }
    //    public int product_category_l2_id { get; set; }
    //    public int sort { get; set; }
    //    public string introduction { get; set; }
    //    public string specifications { get; set; }
    //    public string product_state { get; set; }
    //    public decimal import_outer_price_now { get; set; }
    //    public decimal import_inner_price_now { get; set; }
    //}
    #endregion
}