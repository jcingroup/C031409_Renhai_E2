using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Resources;
using System.Runtime.Caching;
using System.Web.Mvc;
using System.Web.WebPages;

namespace MVCWeb.Helpers
{
    public static class LocalizationHelpe
    {
        public static String Lang(this HtmlHelper htmlHelper, String key)
        {
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, key);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            var inputName = ExpressionHelper.GetExpressionText(expression);
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, inputName);
        }

        public static String Lang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression, String prefx)
        where TModel : class
        {
            var inputName = prefx + ExpressionHelper.GetExpressionText(expression);
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, inputName);
        }

        public static String FieldLang<TModel, TProperty>(this HtmlHelper<TModel> htmlHelper, Expression<Func<TModel, TProperty>> expression)
        where TModel : class
        {
            return Lang<TModel, TProperty>(htmlHelper, expression, "f_");
        }
        public static String FLang(this HtmlHelper htmlHelper, String key)
        {
            string s = "f_" + key;
            return Lang(htmlHelper.ViewDataContainer as WebViewPage, s);
        }

        private static IEnumerable<DictionaryEntry> GetResx(string resxKey)
        {
            ObjectCache cache = MemoryCache.Default;
            IEnumerable<DictionaryEntry> resxs = null;

            if (cache.Contains(resxKey))
            {
                resxs = cache.GetCacheItem(resxKey).Value as IEnumerable<DictionaryEntry>;
            }
            else
            {
                if (File.Exists(resxKey))
                {
                    resxs = new ResXResourceReader(resxKey).Cast<DictionaryEntry>();
                    cache.Add(resxKey, resxs, new CacheItemPolicy() { Priority = CacheItemPriority.NotRemovable });
                }
            }

            return resxs;
        }

        public static String Lang(this WebPageBase page, String key)
        {
            var pagePath = page.VirtualPath;
            var pageName = pagePath.Substring(pagePath.LastIndexOf('/'), pagePath.Length - pagePath.LastIndexOf('/')).TrimStart('/');
            var filePath = page.Server.MapPath(pagePath.Substring(0, pagePath.LastIndexOf('/') + 1)) + "App_LocalResources";

            //var langs = page.Request.UserLanguages != null ?
            //page.Request.UserLanguages.Union<string>(new string[] { "" }).ToArray<string>() : new string[] { "" };

            String lang = System.Globalization.CultureInfo.CurrentCulture.Name;

            IEnumerable<DictionaryEntry> resxs = null;

            //foreach (var lang in langs)
            //{
            var resxKey =
                string.IsNullOrWhiteSpace(lang) ? string.Format(@"{0}\{1}.resx", filePath, pageName) : string.Format(@"{0}\{1}.{2}.resx", filePath, pageName, lang);

            resxs = GetResx(resxKey);

            //  if (resxs != null) { break; }
            //}

            return (string)resxs.FirstOrDefault<DictionaryEntry>(x => x.Key.ToString() == key).Value;
        }

    }
}