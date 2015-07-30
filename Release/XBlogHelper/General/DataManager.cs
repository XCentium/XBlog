using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Net;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using XBlogHelper;
using XBlogHelper.ItemMapper;
using Sitecore.ContentSearch.Utilities;
using System.Linq.Expressions;
using Sitecore;
using Sitecore.Web;
using Sitecore.StringExtensions;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.Data.Fields;
using Sitecore.Text;
using Sitecore.Resources.Media;
using System.Collections.Specialized;
using XBlogHelper.Models.Blog;

namespace XBlogHelper.General
{
    public class DataManager
    {
        #region home item
        /// <summary>
        /// gets blog home based on sitecore context
        /// </summary>
        /// <returns></returns>
        public static Item GetBlogHomeItem()
        {
            return GetBlogHomeItem(Sitecore.Context.Item);
        }
        public static Item GetBlogHomeItem(Item item)
        {
            string filter = string.Format("contains('{0}', @@templatename)", BlogHome.BlogPostParentTemplate);
            return item.Axes.SelectSingleItem("ancestor-or-self::*[" + filter + "]");
        }
        #endregion

        #region settings item
        /// <summary>
        /// gets site setting item
        /// </summary>
        /// <returns></returns>
        public static BlogSettings GetBlogSettingsItem()
        {
            return GetBlogSettingsItem(Sitecore.Context.Item);
        }
        public static BlogSettings GetBlogSettingsItem(Item item)
        {
            BlogHome homeItem = GetBlogHomeItem(item).CreateAs<BlogHome>();
            if (homeItem == null)
                return null;
            else
                return homeItem.BlogSettings.CreateAs<BlogSettings>();
        }
        #endregion

        #region url
        /// <summary>
        /// checks url for a certain pattern, returns string value if found
        /// Example - www.url.com/blog/category/apples if pattern {0}/category/{1} then returns apples
        /// </summary>
        /// <returns></returns>
        public static string GetUrlValByPattern(string url, string urlPattern)
        {
            char[] delimiterChars = { '?' };
            string[] splitUrl = url.Split(delimiterChars);
            string decodedItemName = MainUtil.DecodeName(splitUrl[0]);

            try
            {
                string pattern = urlPattern.FormatWith(@"(^.+)", @"(.+)/$");
                Match match = Regex.Match(StringUtil.EnsurePostfix('/', decodedItemName), @pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    return WebUtility.UrlDecode(match.Groups[2].Value);
                }

            }
            catch (Exception ex)
            {
                Log.Error("XBlog could not resolve url to a blog item", ex, new object());
            }

            return string.Empty;
        }
        #endregion


    }
}
