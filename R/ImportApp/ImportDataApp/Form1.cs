using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ImportDataApp
{
    public partial class Form1 : Form
    {
        int checkMemberCount = 0;
        int exeMemberCount = 0;

        int checkOrdersCount = 0;
        int exeOrdersCount = 0;


        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void WorkingFirst1()
        {
            var db0 = new RenHai2012Entities();
            var db1 = new Renhai_LightSiteEntities();

            try
            {
                #region 會員
                var 會員戶長資料S = db0.會員戶長資料.OrderByDescending(x => x.戶長SN);
                foreach (var item in 會員戶長資料S)
                {
                    checkMemberCount++;
                    backgroundWorker1.ReportProgress(checkMemberCount, item.戶長SN);

                    bool exist = db1.Member.Any(x => x.member_id == item.戶長SN);

                    if (!exist)
                    {
                        exeMemberCount++;
                        var md = new Member()
                        {
                            member_id = item.戶長SN,
                            householder = item.姓名,
                            address = item.地址,
                            zip = item.郵遞區號,
                            tel = item.電話,
                            i_Lang = "zh-tw"
                        };
                        db1.Member.Add(md);

                        var 會員資料表S = db0.會員資料表.Where(x => x.戶長SN == item.戶長SN);

                        foreach (var itm in 會員資料表S)
                        {
                            var mds = new Member_Detail()
                            {
                                member_detail_id = itm.序號,
                                member_id = itm.戶長SN,
                                member_name = itm.姓名,
                                l_birthday = itm.生日,
                                born_sign = itm.生肖,
                                born_time = itm.時辰,
                                gender = itm.性別 == "1" ? true : false,
                                is_holder = itm.Is戶長,
                                address = itm.地址,
                                zip = itm.郵遞區號,
                                tel = itm.電話尾碼,
                                email = itm.EMAIL,
                                mobile = itm.手機,
                                i_Lang = "zh-tw",
                                i_InsertDateTime = itm.建立日期,
                                i_Hide = false
                            };
                            db1.Member_Detail.Add(mds);
                        }

                        db1.SaveChanges();
                    }
                }
                #endregion

            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var err_Items in ex.EntityValidationErrors)
                {
                    foreach (var err_Item in err_Items.ValidationErrors)
                    {
                        sb.Append(err_Item.PropertyName + ":" + err_Item.ErrorMessage);
                    }
                }
                textBox3.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                textBox3.Text = ex.Message + ex.InnerException.ToString();
            }
        }

        private void WorkingFirst2()
        {
            var db0 = new RenHai2012Entities();
            var db1 = new Renhai_LightSiteEntities();
            try
            {
                #region 訂單
                var 訂單主檔S = db0.訂單主檔.OrderByDescending(x => x.訂單序號);
                foreach (var item in 訂單主檔S)
                {
                    checkOrdersCount++;
                    backgroundWorker2.ReportProgress(checkOrdersCount, item.訂單序號);
                    bool exist = db1.Orders.Any(x => x.orders_sn == item.訂單編號);

                    if (!exist)
                    {
                        exeOrdersCount++;
                        var md = new Orders()
                        {
                            orders_id = Convert.ToInt32(item.訂單序號),
                            orders_sn = item.訂單編號,
                            member_id = item.戶長SN,
                            member_detail_id = item.會員編號,
                            member_name = item.申請人姓名,
                            gender = item.申請人性別 == "1" ? true : false,
                            address = item.申請人地址,
                            zip = item.郵遞區號,
                            email = item.申請人EMAIL,
                            mobile = item.申請人手機,
                            tel = item.申請人電話,
                            transation_date = (DateTime)item.訂單時間,
                            total = item.總額,
                            i_Lang = "zh-tw",
                            i_InsertDateTime = item.C_InsertDateTime,
                            orders_state = "2"
                        };
                        db1.Orders.Add(md);

                        var 訂單明細檔S = db0.訂單明細檔.Where(x => x.訂單編號 == item.訂單編號);

                        foreach (var itm in 訂單明細檔S)
                        {
                            var mds = new Orders_Detail()
                            {
                                orders_detail_id = Convert.ToInt32(itm.訂單序號),
                                orders_sn = itm.訂單編號,
                                member_detail_id = itm.會員編號,
                                product_name = itm.產品名稱,
                                product_sn = itm.產品編號,
                                address = itm.申請人地址,
                                amt = itm.數量,
                                member_name = itm.申請人姓名,
                                born_sign = itm.申請人生肖,
                                born_time = itm.申請人時辰,
                                gender = itm.申請人性別 == "1" ? true : false,
                                l_birthday = itm.申請人生日,
                                Y = itm.年度,
                                price = itm.價格,
                                light_name = itm.點燈位置,
                                gold = itm.金牌,
                                race = itm.白米,
                                manjushri = itm.文疏梯次,
                                i_Hide = false,
                                i_InsertDateTime = itm.購買時間,
                                i_Lang = "zh-TW"
                            };
                            db1.Orders_Detail.Add(mds);
                        }
                        db1.SaveChanges();
                    }
                }
                #endregion
            }
            catch (DbEntityValidationException ex)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var err_Items in ex.EntityValidationErrors)
                {
                    foreach (var err_Item in err_Items.ValidationErrors)
                    {
                        sb.Append(err_Item.PropertyName + ":" + err_Item.ErrorMessage);
                    }
                }
                textBox3.Text = sb.ToString();
            }

            catch (Exception ex)
            {
                textBox3.Text = ex.Message + ex.InnerException.ToString();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.backgroundWorker1.IsBusy != true)
            {
                this.backgroundWorker1.WorkerReportsProgress = true;
                this.backgroundWorker1.RunWorkerAsync();
            }

            if (this.backgroundWorker2.IsBusy != true)
            {
                this.backgroundWorker2.WorkerReportsProgress = true;
                this.backgroundWorker2.RunWorkerAsync();
            }
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textBox1.Text = e.UserState + ":" + e.ProgressPercentage + ":" + exeMemberCount;
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker1.ReportProgress(0);
            WorkingFirst1();
        }

        private void backgroundWorker2_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            textBox2.Text = e.UserState + ":" + e.ProgressPercentage + ":" + exeOrdersCount;
        }

        private void backgroundWorker2_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundWorker2.ReportProgress(0);
            WorkingFirst2();
        }
    }
}
