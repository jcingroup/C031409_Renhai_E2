using ProcCore.WebCore;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DotWeb.CommSetup
{
    public static class CommWebSetup
    {
        private static string GetKeyValue(string key)
        {
            return System.Configuration.ConfigurationManager.AppSettings[key];
        }
        public static string AutoLoginUser
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DEV_USER"];
            }
        }
        public static string AutoLoginPassword
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DEV_PWD"];
            }
        }
        public static string WebCookiesId
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["WebCookiesId"];
            }
        }
        public static string ManageDefCTR
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["ManageDefCTR"];
            }
        }
        public static string UserLoginSource
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["UserLoginSource"];
            }
        }
        public static DateTime Expire
        {
            get
            {
                return DateTime.Parse(System.Configuration.ConfigurationManager.AppSettings["Expire"]);
            }
        }
        public static int MasterGridDefPageSize
        {
            get
            {
                return int.Parse(System.Configuration.ConfigurationManager.AppSettings["PageSize"]);
            }
        }
        public static string Cookie_UserName
        {
            get
            {
                return "Cookie_UserName";
            }
        }
        public static string Cookie_LastLogin
        {
            get
            {
                return "Cookie_LastLogin";
            }
        }
        public static string Cookie_DepartmentId
        {
            get
            {
                return "Cookie_DepartmentId";
            }
        }
        public static string Cookie_DepartmentName
        {
            get
            {
                return "Cookie_DepartmentName";
            }
        }
        public static string CacheVer
        {
            get
            {
                return GetKeyValue("CacheVer");
            }
        }
        public static int WorkYear
        {
            get
            {
                return int.Parse(GetKeyValue("WorkYear"));
            }
        }
        public static string DB0_CodeString
        {
            get
            {
                return System.Configuration.ConfigurationManager.AppSettings["DB0"];
            }
        }
        public static string Cookie_user_name
        {
            get
            {
                return "user_name";
            }
        }
        public static string Cookie_user_id
        {
            get
            {
                return "user_id";
            }
        }
        public static string Cookie_unit_id
        {
            get
            {
                return "unit_id";
            }
        }

        public static int[] AllowRejectUnit
        {
            get
            {
                var g = GetKeyValue("AllowRejectUnit").Split(',');
                IList<int> m = new List<int>();
                foreach (var n in g) {
                    m.Add(int.Parse(n));
                }
                return m.ToArray();
            }
        }

        public static int OrdersRecordMax
        {
            get
            {
                return int.Parse(GetKeyValue("OrdersRecordMax"));
            }
        }
        public static int FortuneLimit
        {
            get
            {
                return int.Parse(GetKeyValue("FortuneLimit"));
            }
        }
    }

    #region Image UpLoad Parma
    public static class ImageFileUpParm
    {
        public static ImageUpScope NewsBasicSingle
        {
            get
            {
                ImageUpScope imUp = new ImageUpScope() { KeepOriginImage = true, LimitCount = 50, LimitSize = 1024 * 1024 * 2 };
                imUp.Parm = new ImageSizeParm[] {
                    new ImageSizeParm(){ SizeFolder=700, width=700}
                };
                return imUp;
            }
        }
    }
    public static class SysFileUpParm
    {
        public static FilesUpScope BaseLimit
        {
            get
            {
                FilesUpScope FiUp = new FilesUpScope() { LimitCount = 5, LimitSize = 1024 * 1024 * 256 };
                return FiUp;
            }
        }
    }
    #endregion
}