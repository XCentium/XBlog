using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Sitecore.Security.Accounts;
using Sitecore.Security.Authentication;
using System.Collections.Specialized;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data.Items;
using Sitecore.Security.AccessControl;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Helpers
{
    public static class Helper
    {
        #region extensions
        /// <summary>
        /// gets a slice of a list
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="startIndex"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public static IEnumerable<T> Slice<T>(this IEnumerable<T> list, int startIndex, int maxCount)
        {
            return list.Skip(startIndex).Take(maxCount);
        }

        /// <summary>
        /// gets a safe substring. 
        /// ensures that the max length is not greater than the string
        /// also ensures that the string is trimmed at the end of a word
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string text, int maxLength)
        {
            return SafeSubstring(text, maxLength, false);
        }
        /// <summary>
        ///  gets a safe substring. 
        ///  ensures that the max length is not greater than the string       
        ///  also ensures that the string is trimmed at the end of a word
        /// </summary>
        /// <param name="text"></param>
        /// <param name="maxLength"></param>
        /// <param name="dots"></param>
        /// <returns></returns>
        public static string SafeSubstring(this string text, int maxLength, bool dots)
        {
            if (string.IsNullOrEmpty(text) || text.Length < maxLength)
                return text;
            int length = Math.Min(text.LastIndexOf(' ', maxLength), maxLength);
            string substring = length <= 0 ? text.Substring(0, maxLength) : text.Substring(0, length);
            return string.Format("{0}{1}", substring, dots ? "..." : string.Empty);
        }
        #endregion
    }
}
