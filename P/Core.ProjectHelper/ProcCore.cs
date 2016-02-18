using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace ProcCore
{
    public enum EditModeType
    {
        Insert,
        Modify
    }
    public static class EncryptString
    {
        private static string def_Key = "JcIn7386";
        private static string def_iv = "jAcKkInG";
        public static string desEncryptBase64(string source)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

            des.Key = key;
            des.IV = iv;
            string encrypt = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                encrypt = Convert.ToBase64String(ms.ToArray());
            }
            return encrypt;
        }
        public static string desDecryptBase64(string encrypt)
        {
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);
            des.Key = key;
            des.IV = iv;

            byte[] dataByteArray = Convert.FromBase64String(encrypt);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        private static string desEncrypt(string source)
        {
            StringBuilder sb = new StringBuilder();
            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);
            byte[] dataByteArray = Encoding.UTF8.GetBytes(source);

            des.Key = key;
            des.IV = iv;
            string encrypt = string.Empty;
            using (MemoryStream ms = new MemoryStream())
            using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(), CryptoStreamMode.Write))
            {
                cs.Write(dataByteArray, 0, dataByteArray.Length);
                cs.FlushFinalBlock();
                //輸出資料
                foreach (byte b in ms.ToArray())
                {
                    sb.AppendFormat("{0:X2}", b);
                }
                encrypt = sb.ToString();
            }
            return encrypt;
        }
        private static string desDecrypt(string encrypt)
        {
            byte[] dataByteArray = new byte[encrypt.Length / 2];
            for (int x = 0; x < encrypt.Length / 2; x++)
            {
                int i = (Convert.ToInt32(encrypt.Substring(x * 2, 2), 16));
                dataByteArray[x] = (byte)i;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);
            des.Key = key;
            des.IV = iv;

            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                    return Encoding.UTF8.GetString(ms.ToArray());
                }
            }
        }
        private static void desEncryptFile(string sourceFile, string encryptFile)
        {
            if (string.IsNullOrEmpty(sourceFile) || string.IsNullOrEmpty(encryptFile))
            {
                return;
            }
            if (!File.Exists(sourceFile))
            {
                return;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);

            des.Key = key;
            des.IV = iv;

            using (FileStream sourceStream = new FileStream(sourceFile, FileMode.Open, FileAccess.Read))
            using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Create, FileAccess.Write))
            {
                //檔案加密
                byte[] dataByteArray = new byte[sourceStream.Length];
                sourceStream.Read(dataByteArray, 0, dataByteArray.Length);

                using (CryptoStream cs = new CryptoStream(encryptStream, des.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                }
            }
        }
        private static void desDecryptFile(string encryptFile, string decryptFile)
        {
            if (string.IsNullOrEmpty(encryptFile) || string.IsNullOrEmpty(decryptFile))
            {
                return;
            }
            if (!File.Exists(encryptFile))
            {
                return;
            }

            DESCryptoServiceProvider des = new DESCryptoServiceProvider();
            byte[] key = Encoding.ASCII.GetBytes(def_Key);
            byte[] iv = Encoding.ASCII.GetBytes(def_iv);

            des.Key = key;
            des.IV = iv;

            using (FileStream encryptStream = new FileStream(encryptFile, FileMode.Open, FileAccess.Read))
            using (FileStream decryptStream = new FileStream(decryptFile, FileMode.Create, FileAccess.Write))
            {
                byte[] dataByteArray = new byte[encryptStream.Length];
                encryptStream.Read(dataByteArray, 0, dataByteArray.Length);
                using (CryptoStream cs = new CryptoStream(decryptStream, des.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(dataByteArray, 0, dataByteArray.Length);
                    cs.FlushFinalBlock();
                }
            }
        }
    }
}

namespace ProcCore.WebCore
{
    #region Default ENUM
    public class FilesUpScope
    {
        public int LimitSize { get; set; }
        public String[] LimitExtType { get; set; }
        public int LimitCount { get; set; }
    }
    public class ImageUpScope : FilesUpScope
    {
        public String KindName { get; set; }
        public Boolean KeepOriginImage { get; set; }
        public ImageSizeParm[] Parm { get; set; }
    }
    public class ImageSizeParm
    {
        public int SizeFolder { get; set; }
        public int heigh { get; set; }
        public int width { get; set; }

    }
    public static class PageCount
    {
        public static int PageInfo(int page, int pagesize, int recordCount)
        {
            RecordCount = recordCount;
            Page = page <= 0 ? 1 : page;
            Decimal c = Convert.ToDecimal(RecordCount) / pagesize;
            TotalPage = (RecordCount > 0 && pagesize > 0 && pagesize < RecordCount) ? Convert.ToInt32(Math.Ceiling(c)) : 1;
            Page = (Page > TotalPage) ? TotalPage : Page;
            StartCount = (Page - 1) * pagesize + 1;
            EndCount = Page * pagesize > recordCount ? recordCount : Page * pagesize;

            return (Page - 1) * pagesize;
        }

        public static int TotalPage { get; set; }
        public static int RecordCount { get; set; }
        public static int Page { get; set; }
        public static int StartCount { get; set; }
        public static int EndCount { get; set; }
    }
    #endregion
}

namespace ProcCore.ReturnAjaxResult
{
    public class ResultInfo
    {
        public ResultInfo()
        {
            this.result = true;
        }
        public bool result { get; set; }
        public string message { get; set; }
        public int id { get; set; }
        public string aspnetid { get; set; }
        public bool sessionout { get; set; }
        public string IdStr { get; set; }
    }
    public class ReturnAjaxData : ResultInfo
    {
        public Object Module { get; set; }
    }
    public class ReturnAjaxFiles : ResultInfo
    {
        public FilesObject[] filesObject { get; set; }
    }
    public class rAjaxGetData<T> : ResultInfo
    {
        public T data { get; set; }
    }
    public class rAjaxGetItems<T> : ResultInfo
    {
        public IList<T> data { get; set; }
    }
    public class FilesObject
    {
        public String RepresentFilePath { get; set; }
        public String OriginFilePath { get; set; }
        public String FileName { get; set; }
        public Boolean IsImage { get; set; }
        public long Size { get; set; }
    }
}