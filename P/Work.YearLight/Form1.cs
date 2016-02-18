using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Work.YearLight
{
    public partial class Form1 : Form
    {
        int 年度 = 2015;
        RenHai2012Entities db = null;
        List<Light> ls_Lisgths = null;
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ls_Lisgths = new List<Light>();

            ls_Lisgths.Add(new Light() { Id = "3", lightKint = LightKind.壁, TagName = "媽壁", 面S = new String[] { "甲", "乙" }, 排S = 36, 人S = 96, 柱S = new string[] { "" } });
            ls_Lisgths.Add(new Light() { Id = "3", lightKint = LightKind.壁, TagName = "媽座", 面S = new String[] { "甲", "乙" }, 排S = 42, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "3", lightKint = LightKind.壁, TagName = "媽座", 面S = new String[] { "丙" }, 排S = 30, 人S = 20, 柱S = new string[] { "A", "B", "C", "D" } });

            ls_Lisgths.Add(new Light() { Id = "31", lightKint = LightKind.頭, TagName = "媽座頭", 面S = new String[] { "甲", "乙", "丙" }, Row1 = 23, Row2 = 27 });

            ls_Lisgths.Add(new Light() { Id = "5", lightKint = LightKind.壁, TagName = "關壁", 面S = new String[] { "甲" }, 排S = 30, 人S = 122, 柱S = new string[] { "" } });
            ls_Lisgths.Add(new Light() { Id = "5", lightKint = LightKind.壁, TagName = "關座", 面S = new String[] { "甲", "乙" }, 排S = 36, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "51", lightKint = LightKind.頭, TagName = "關座頭", 面S = new String[] { "甲", "乙" }, Row1 = 23, Row2 = 27 });


            ls_Lisgths.Add(new Light() { Id = "2", lightKint = LightKind.壁, TagName = "文壁", 面S = new String[] { "丙" }, 排S = 30, 人S = 122, 柱S = new string[] { "" } });
            ls_Lisgths.Add(new Light() { Id = "2", lightKint = LightKind.壁, TagName = "文座", 面S = new String[] { "丙", "丁" }, 排S = 36, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "21", lightKint = LightKind.頭, TagName = "文座頭", 面S = new String[] { "丙", "丁" }, Row1 = 23, Row2 = 27 });

            ls_Lisgths.Add(new Light() { Id = "12", lightKint = LightKind.壁, TagName = "財座", 面S = new String[] { "甲" }, 排S = 40, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "12", lightKint = LightKind.壁, TagName = "財座", 面S = new String[] { "乙" }, 排S = 40, 人S = 20, 柱S = new string[] { "A", "B", "C" } });
            ls_Lisgths.Add(new Light() { Id = "121", lightKint = LightKind.頭, TagName = "財座頭", 面S = new String[] { "甲" }, Row1 = 23, Row2 = 27 });

            ls_Lisgths.Add(new Light() { Id = "13", lightKint = LightKind.壁, TagName = "月座", 面S = new String[] { "甲" }, 排S = 40, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "13", lightKint = LightKind.壁, TagName = "月座", 面S = new String[] { "乙" }, 排S = 40, 人S = 20, 柱S = new string[] { "A", "B", "C" } });
            ls_Lisgths.Add(new Light() { Id = "131", lightKint = LightKind.頭, TagName = "月座頭", 面S = new String[] { "甲" }, Row1 = 23, Row2 = 27 });

            ls_Lisgths.Add(new Light() { Id = "4", lightKint = LightKind.壁, TagName = "觀座", 面S = new String[] { "甲", "乙", "丙", "丁", "戊", "己" }, 排S = 36, 人S = 20, 柱S = new string[] { "A", "B", "C", "D", "E", "F" } });
            ls_Lisgths.Add(new Light() { Id = "41", lightKint = LightKind.頭, TagName = "觀座頭", 面S = new String[] { "甲", "乙", "丙", "丁", "戊", "己" }, Row1 = 23, Row2 = 27 });


        }

        private void button1_Click(object sender, EventArgs e)
        {
            db = new RenHai2012Entities();

            foreach (Light lg in ls_Lisgths)
            {
                var md = db.產品資料表.Where(x => x.產品編號 == lg.Id).Single();
                if (lg.lightKint == LightKind.壁)
                {
                    GeneralLightInsert(md.產品編號, md.價格, lg.TagName, lg.面S, lg.柱S, lg.排S, lg.人S, 1, 1, 2013, 0);
                }
                else
                {
                    HeadLightInsert(md.產品編號, md.價格, lg.TagName, lg.面S, new int[] { lg.Row1, lg.Row2 }, 2013, 0);
                }
            }

            db.SaveChanges();
        }

        private void GeneralLightInsert(string product_sn, int price,
            string LightBeforeLabel,string[] 面s,
            string[] 柱s, int 排數, int 每排人數, int 排起始數, int 人啟始數, int 年度, int accountId)
        {
            try
            {
                #region Main working
                string LightTPL = "{0}{1} {2}{3:00}-{4:00}";

                foreach (string 面 in 面s)
                {
                    foreach (string 柱 in 柱s)
                    {
                        for (int i = 排起始數; i <= 排數; i++)
                        {
                            for (int j = 人啟始數; j <= 每排人數; j++)
                            {
                                #region Main Working
                                string SiteName = string.Format(LightTPL, LightBeforeLabel, 面, 柱, i, j);

                                點燈位置資料表 tab = new 點燈位置資料表()
                                {
                                    年度 = 年度,
                                    位置名稱 = SiteName,
                                    空位 = "0",
                                    產品編號 = product_sn,
                                    價格 = price,
                                    IsReject = false
                                };
                                #endregion
                                db.點燈位置資料表.Add(tab);
                            }
                        }
                    }
                }

                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public void HeadLightInsert(String product_sn, int price,
            string LightBeforeLabel,
            string[] 面s,
              int[] 每排人數, int 年度, int accountId)
        {
            try
            {
                #region Main working
                string LightTPL = "{0}{1} {2:00}-{3:00}";

                foreach (string 面 in 面s)
                {
                    int k = 1;
                    foreach (int 人數 in 每排人數)
                    {
                        for (int i = 1; i <= 人數; i++)
                        {
                            #region Main Working
                            string SiteName = string.Format(LightTPL, LightBeforeLabel, 面, k, i);
                            點燈位置資料表 tab = new 點燈位置資料表()
                            {
                                年度 = 年度,
                                位置名稱 = SiteName,
                                空位 = "0",
                                產品編號 = product_sn,
                                價格 = price,
                                IsReject = false
                            };
                            db.點燈位置資料表.Add(tab);
                            #endregion
                        }
                        k++;
                    }
                }
                #endregion
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }


        private enum LightKind
        {
            壁, 頭
        }
        private class Light
        {
            public String Id { get; set; }
            public LightKind lightKint { get; set; }
            public String TagName { get; set; }
            public String[] 面S { get; set; }
            public String[] 柱S { get; set; }
            public int 排S { get; set; }
            public int 人S { get; set; }

            public int Row1 { get; set; }
            public int Row2 { get; set; }
        }
    }

}
