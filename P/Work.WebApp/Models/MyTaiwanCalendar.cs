using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Work.WebApp.Models
{
    public class MyTaiwanCalendar
    {
        public class PlayTCL
        {
            static string TeanGean = "甲乙丙丁戊己庚辛壬癸";
            static string DeGe = "子丑寅卯辰巳午未申酉戌亥";
            static string CAnimal = "鼠牛虎兔龍蛇馬羊猴雞狗豬";
            /// <summary>
            /// 取得農曆日期及生肖
            /// </summary>
            /// <param name="param"></param>
            /// <returns></returns>
            public string getTaiwanLC(DateTime? p_day = null)
            {
                DateTime day = p_day != null ? (DateTime)p_day : DateTime.Now;//取得日期
                TaiwanLunisolarCalendar Tlc = new TaiwanLunisolarCalendar();

                int lun60Year = Tlc.GetSexagenaryYear(day.AddYears(0));
                int TeanGeanYear = Tlc.GetCelestialStem(lun60Year) - 1;
                int DeGeYear = Tlc.GetTerrestrialBranch(lun60Year) - 1;

                int lunMonth = Tlc.GetMonth(day.AddYears(0));
                int leapMonth = Tlc.GetLeapMonth(Tlc.GetYear(day.AddYears(0)));
                if (leapMonth > 0 && lunMonth >= leapMonth)
                {
                    //lunMonth = lunMonth - 1;
                    lunMonth -= 1;
                }
                int lunDay = Tlc.GetDayOfMonth(day.AddYears(0));
                return String.Format("農曆:{0}年{1}月{2}日 今年的生肖: {3}", TeanGean[TeanGeanYear].ToString() + DeGe[DeGeYear].ToString(), lunMonth, lunDay, CAnimal[DeGeYear].ToString());
            }
            public LunDate getTaiwanLCDate(DateTime? p_day = null)
            {
                DateTime day = p_day != null ? (DateTime)p_day : DateTime.Now;//取得日期
                LunDate res = new LunDate();
                TaiwanLunisolarCalendar Tlc = new TaiwanLunisolarCalendar();

                int lun60Year = Tlc.GetSexagenaryYear(day.AddYears(0));
                int TeanGeanYear = Tlc.GetCelestialStem(lun60Year) - 1;
                int DeGeYear = Tlc.GetTerrestrialBranch(lun60Year) - 1;

                int lunMonth = Tlc.GetMonth(day.AddYears(0));
                int leapMonth = Tlc.GetLeapMonth(Tlc.GetYear(day.AddYears(0)));
                if (leapMonth > 0 && lunMonth >= leapMonth)
                {
                    //lunMonth = lunMonth - 1;
                    lunMonth -= 1;
                }
                int lunDay = Tlc.GetDayOfMonth(day.AddYears(0));

                res.year = TeanGean[TeanGeanYear].ToString() + DeGe[DeGeYear].ToString();
                res.month = TurnNum(lunMonth.ToString());
                res.day = TurnNum(lunDay.ToString());
                return res;
            }
            public string TurnNum(string sInput)
            {
                //數字第一位(不顯示零)
                string[] chineseNumber = { "", "一", "二", "三", "四", "五", "六", "七", "八", "九" };
                //數字第二位(不顯示零、一僅顯示單位unit)
                string[] chineseNumberTwo = { "", "", "二", "三", "四", "五", "六", "七", "八", "九" };
                string[] unit = { "", "十", "百", "千", "萬", "十萬", "百萬", "千萬", "億", "十億", "百億", "千億", "兆", "十兆", "百兆", "千兆" };
                StringBuilder ret = new StringBuilder();
                string inputNumber = sInput;
                int idx = inputNumber.Length;
                bool needAppendZero = false;
                foreach (char c in inputNumber)
                {
                    idx--;
                    if (c > '0')
                    {
                        if (needAppendZero)
                        {
                            ret.Append(chineseNumber[0]);
                            needAppendZero = false;
                        }
                        if (idx == 1)
                        {//數字第二位(陣列1)
                            ret.Append(chineseNumberTwo[(int)(c - '0')] + unit[idx]);
                        }
                        else
                        {
                            ret.Append(chineseNumber[(int)(c - '0')] + unit[idx]);
                        }

                    }
                    else
                        needAppendZero = true;
                }

                string retStr = ret.ToString();
                Regex reg = new Regex("二十.");//二十幾才要取代
                if (reg.IsMatch(retStr))
                {
                    retStr = Regex.Replace(retStr, "二十", "廿");//二十取代為廿
                }
                return ret.Length == 0 ? chineseNumber[0] : retStr;
            }
        }
        public class LunDate
        {
            public string year { get; set; }
            public string month { get; set; }
            public string day { get; set; }
        }
    }
}