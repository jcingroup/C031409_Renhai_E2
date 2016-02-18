using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Data;
using System.Reflection;
using System.Text;

namespace ProcCore.NetExtension
{
    public static class ExtensionDateTime
    {
        public static string ToStandardDate(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd");
        }
        public static string ToStandardTime(this DateTime dt)
        {
            return dt.ToString("HH:mm:ss");
        }
        public static string ToStandardDateTime(this DateTime dt)
        {
            return dt.ToString("yyyy/MM/dd HH:mm:ss");
        }
        public static string To民國年(this DateTime dt)
        {
            return (dt.Year - 1911).ToString() + dt.ToString("/MM/dd");
        }
    }
    public static class ExtensionInt
    {
        public static string[] ToStringArray(this int[] s)
        {

            List<String> r = new List<String>();
            foreach (int i in s)
            {
                r.Add(i.ToString());
            }
            return r.ToArray();
        }
        public static string GetChineseNumber(this int number)
        {
            string[] chineseNumber = { "零", "壹", "貳", "參", "肆", "伍", "陸", "柒", "捌", "玖" };
            string[] unit = { "", "拾", "佰", "仟", "萬", "拾", "佰", "仟", "億", "拾", "佰", "仟", "兆", "拾", "佰", "仟" };
            StringBuilder ret = new StringBuilder();
            string inputNumber = number.ToString();
            int idx = inputNumber.Length;
            bool needAppendZero = false;
            foreach (char c in inputNumber)
            {
                idx--; string un = unit[idx];
                if (c > '0')
                {
                    if (needAppendZero)
                    {
                        if (idx % 4 != 3) ret.Append(chineseNumber[0]);
                        needAppendZero = false;
                    }
                    ret.Append(chineseNumber[(int)(c - '0')] + un);
                }
                else
                {
                    needAppendZero = true;
                    if (idx % 4 == 0) ret.Append(un);
                }
            }
            return ret.Length == 0 ? chineseNumber[0] : ret.ToString();
        }
    }
    public static class ExtensionString
    {
        public static string NullValue(this String s, String v)
        {
            return s == null ? v : s;
        }
        public static string JoinArray(this String[] s, String JoinChar, String beforedot, String afterdot)
        {
            String r = String.Join(afterdot + JoinChar + beforedot, s);

            if (r != "")
            {
                return beforedot + r + afterdot;
            }
            else
            {
                return "";
            }
        }
        public static string JoinArray(this String[] s, String JoinChar, String dot)
        {
            String r = String.Join(dot + JoinChar + dot, s); // ","

            if (r != "")
            {
                return dot + r + dot; //整個字串的前後
            }
            else
            {
                return "";
            }
        }
        public static string Right(this String s, int n)
        {
            return s.Substring(n > s.Length ? 0 : s.Length - n);
        }
        public static string Left(this String s, int n)
        {
            return s.Substring(0, n > s.Length ? s.Length : n);
        }
    }
    public static class ExtensionBoolean
    {
        public static String BooleanValue(this Boolean s, string TrueString, string FalseString)
        {
            if (s) return TrueString; else return FalseString;
        }

    }
    public static class ExtensionObject
    {
        public static int? ToInt(this object o)
        {
            if (o != null)
            { return (int)o; }
            else
            { return null; }
        }
        public static DateTime? ToDateTime(this object o)
        {
            if (o != null)
            { return (DateTime)o; }
            else
            { return null; }
        }
        public static Boolean? ToBoolean(this object o)
        {
            if (o != null)
            { return (Boolean)o; }
            else
            { return null; }
        }
        public static int CInt(this object o)
        {
        return int.Parse(o.ToString());
        }
        public static DateTime CDateTime(this object o)
        {
            return (DateTime)o;
        }
        public static Boolean CBoolean(this object o)
        {
            return (Boolean)o;
        }
    }
    public static class ExtensionCollect
    {
        public static List<String> ToKeyValueList(this Dictionary<String, String> s)
        {
            List<String> r = new List<String>();
            foreach (KeyValuePair<String, String> kv in s)
            {
                r.Add(kv.Key + ":" + kv.Value);
            }
            return r;
        }
    }
    public static class ExtensionData
    {
        public static void LoadDataToModule(this DataRow drData, Object mdObject)
        {
            //http://www.codeproject.com/Articles/11914/Using-Reflection-to-convert-DataRows-to-objects-or
            DataColumnCollection GetTableColumnsObj = drData.Table.Columns;

            Type t = mdObject.GetType();
            PropertyInfo[] f = t.GetProperties();
            String GetColName;
            for (Int32 i = 0; i <= GetTableColumnsObj.Count - 1; i++)
            {
                try
                {
                    GetColName = GetTableColumnsObj[i].ColumnName;
                    var gf = f.AsEnumerable().Where(x => x.Name == GetColName);

                    if (gf.Count() > 0)
                    {
                        PropertyInfo mf = gf.FirstOrDefault();
                        if (drData[GetColName] != DBNull.Value)
                        {
                            mf.SetValue(mdObject, drData[GetColName], null);
                        }

                        //t.InvokeMember(GetColName,BindingFlags.SetProperty, null, mdObject, new Object[] { drData[GetColName] });
                    }
                }
                catch (Exception ex)
                {
                    if (ex.ToString() != null) { }
                }
            };
        }
        public static Dictionary<String, String> dicMakeKeyValue(this DataTable s, int KeyIndex, int ValueIndex)
        {
            Dictionary<String, String> dic = new Dictionary<String, String>();
            foreach (DataRow dr in s.Rows)
            {
                dic.Add(dr[KeyIndex].ToString().TrimEnd(), dr[ValueIndex].ToString().TrimEnd());
            }
            return dic;
        }
        public static String TableCodeValue(this int intValue, Dictionary<int, String> dic)
        {
            var query = dic.Where(x => x.Key == intValue);

            if (query.Count() > 0)
            {
                return query.FirstOrDefault().Value;
            }
            else
            {
                return "";
            }
        }
        public static String TableCodeValue(this String StringValue, Dictionary<String, String> dic)
        {
            var query = dic.Where(x => x.Key == StringValue);

            if (query.Count() > 0)
            {
                return query.FirstOrDefault().Value;
            }
            else
            {
                return "";
            }
        }
    }
}