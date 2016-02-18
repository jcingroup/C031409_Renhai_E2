using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.WebPages;

namespace DotWeb.Helpers
{
    public static class addBundleScript
    {
        public static void AddCommScript(this HtmlHelper htmlHelper, string filePathName)
        {
            AddCommScript(htmlHelper,"~/commJS", filePathName);
        }

        public static void AddCommScript(this HtmlHelper htmlHelper, string keyName, string filePathName)
        {
            var bundle = System.Web.Optimization.BundleTable.Bundles.GetRegisteredBundles()
                .Where(x => x.Path == keyName).First();
            bundle.Include(filePathName);
        }

    }

    public static class LocalizationHelpe
    {
        public static string Lang(this HtmlHelper htmlHelper, string key)
        {
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, key);
        }

        public static string Lang<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e)
        where TModel : class
        {
            var n = ExpressionHelper.GetExpressionText(e);
            String m = n.Split('.').LastOrDefault();
            return Lang(h.ViewDataContainer as WebViewPage, m);
        }

        public static string Lang<TModel, TProperty>(this HtmlHelper<TModel> h, Expression<Func<TModel, TProperty>> e, string prefx)
        where TModel : class
        {
            var n = ExpressionHelper.GetExpressionText(e);
            String m = n.Split('.').LastOrDefault();
            var i = prefx + m;
            return Lang(h.ViewDataContainer as WebViewPage, i);
        }

        private static IEnumerable<DictionaryEntry> GetResx(string LocalResourcePath)
        {
            //System.Resources.
            ObjectCache cache = MemoryCache.Default;
            IEnumerable<DictionaryEntry> resxs = null;

            if (cache.Contains(LocalResourcePath))
                resxs = cache.GetCacheItem(LocalResourcePath).Value as IEnumerable<DictionaryEntry>;
            else
            {
                if (File.Exists(LocalResourcePath))
                {
                    resxs = new ResXResourceReader(LocalResourcePath).Cast<DictionaryEntry>();
                    cache.Add(LocalResourcePath, resxs, new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable });
                }
            }
            return resxs;
        }
        public static string Lang(this WebPageBase page, string key)
        {
            var pagePath = page.VirtualPath;
            var pageName = pagePath.Substring(pagePath.LastIndexOf('/'), pagePath.Length - pagePath.LastIndexOf('/')).TrimStart('/');
            var filePath = page.Server.MapPath(pagePath.Substring(0, pagePath.LastIndexOf('/') + 1)) + "App_LocalResources";

            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;
            String resxKey = String.Empty;
            String def_resKey = String.Format(@"{0}\{1}.resx", filePath, pageName);
            String lng_resKey = String.Format(@"{0}\{1}.{2}.resx", filePath, pageName, lang);

            resxKey = File.Exists(lng_resKey) ? lng_resKey : def_resKey;
            IEnumerable<DictionaryEntry> resxs = GetResx(resxKey);
            if (resxs != null)
                return (String)resxs.FirstOrDefault<DictionaryEntry>(x => x.Key.ToString() == key).Value;
            else
                return "";
        }
    }
    public static class CommVar
    {
        public static String ngSH(this HtmlHelper htmlHelper)
        {
            return "sd";
        }
        public static String ngGD(this HtmlHelper htmlHelper)
        {
            return "gd";
        }
        /// <summary>
        /// 明細Grid變數
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>
        public static String ngGT(this HtmlHelper htmlHelper)
        {
            return "subgd";
        }
        /// <summary>
        /// 編輯欄位變數
        /// </summary>
        /// <param name="htmlHelper"></param>
        /// <returns></returns>

        public static String ngFD(this HtmlHelper htmlHelper)
        {
            return "fd";
        }
    }
    public static class Pop
    {

        public static PopWindow popWindow(this HtmlHelper htmlHelper, String ng_show, String ng_close, String title)
        {
            PopWindow w = new PopWindow(htmlHelper.ViewContext, ng_show, ng_close, title);
            return w;
        }
    }

    public class PopWindow : IDisposable
    {
        private Boolean disposed;
        private ViewContext _vwContext;
        public PopWindow(ViewContext VCText, String ng_show, String ng_close, String title)
        {
            String tpl = String.Format("<div class=\"popup-outer ng-hide\" ng-show=\"{0}\"><div class=\"popup-window\"><h3>" + title + " " + Resources.Res.Info_Edit + "</h3><button class=\"close\" title=\"" + Resources.Res.Info_Close_Layer + "\" ng-click=\"" + ng_close + "\"><i class=\"fa-times\"></i></button><div class=\"popup\">", ng_show);
            _vwContext = VCText;
            _vwContext.Writer.Write(tpl);
        }

        public void Dispose()
        {
            _vwContext.Writer.Write("</div></div><div class=\"bg\"></div></div>");
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {

                }
                disposed = true;
            }
        }
    }
}