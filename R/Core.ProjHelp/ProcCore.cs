using System;

namespace ProcCore 
{ 

}
namespace ProcCore.WebCore
{
    public class FilesUpScope
    {
        public FilesUpScope()
        {
            this.LimitExtType = new String[] { ".asp", ".aspx", ".exe", ".php", ".bat" };
            this.AllowExtType = new String[] { ".jpg", ".jpeg", ".png", ".gif", ".bmp", ".doc", ".xls", "ppt", ".docx", ".xlsx", "pptx", ".pdf", ".txt", ".rar", ".zip" };
        }

        public int LimitSize { get; set; }
        /// <summary>
        /// 以副檔名為設定，此為禁止上傳類型，要加. 例.exe。
        /// </summary>
        public string[] LimitExtType { get; set; }
        public string[] AllowExtType { get; set; }
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
}
namespace ProcCore.HandleResult
{
    public class ResultInfo
    {
        public ResultInfo() {
            this.result = true;
        }
        public bool result { get; set; }
        public string message { get; set; }
        public int id { get; set; }
        public string aspnetid { get; set; }
        public bool sessionout { get; set; }
        public object json { get; set; }
    }
    public class ReturnAjaxFiles : ResultInfo
    {
        public FilesObject[] filesObject { get; set; }
        public String FileName { get; set; }
        /// <summary>
        /// 搭配Fine Uploader
        /// </summary>
        public Boolean success { get; set; }
        /// <summary>
        /// 搭配Fine Uploader
        /// </summary>
        public String error { get; set; }
    }
    public class FilesObject
    {
        public String RepresentFilePath { get; set; }
        public String OriginFilePath { get; set; }
        public String FileName { get; set; }
        public String FilesKind { get; set; }
        public Boolean IsImage { get; set; }
        public long Size { get; set; }
    }
    public class rAjaxGetData<T> : ResultInfo
    {
        public T data { get; set; }
    }
    public class rAjaxGetItems<T> : ResultInfo
    {
        public object data { get; set; }
    }
}