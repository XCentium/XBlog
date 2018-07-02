using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Sitecore;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.IO;
using Sitecore.Pipelines.HttpRequest;
using Sitecore.Sites;
using Sitecore.StringExtensions;
using Sitecore.Web;
using Sitecore.Feature.XBlog.General;

namespace Sitecore.Feature.XBlog.Pipelines
{
    public class BlogItemResolver : HttpRequestProcessor
    {
        private enum BlogFilterType { None = 0, Category, Tag, Author, AuthorView, Search };

        public override void Process(HttpRequestArgs args)
        {
            Assert.ArgumentNotNull(args, "args");
            if (((Context.Item == null) && (Context.Database != null)) && !string.IsNullOrWhiteSpace(args.Url.ItemPath))
            {
                Profiler.StartOperation("Resolve blog item.");
                string qsValue = string.Empty;
                string decodedItemName = MainUtil.DecodeName(args.Url.ItemPath);

                string blogItemPath = ResolveBlogItemPath(decodedItemName, XBSettings.XBCategoryUrlPattern, out qsValue);
                BlogFilterType filter = BlogFilterType.None;
                if (!string.IsNullOrEmpty(blogItemPath))
                    filter = BlogFilterType.Category;
                else
                {
                    blogItemPath = ResolveBlogItemPath(decodedItemName, XBSettings.XBAuthorUrlPattern, out qsValue);
                    if (!string.IsNullOrEmpty(blogItemPath))
                        filter = BlogFilterType.Author;
                    else
                    {
                        blogItemPath = ResolveBlogItemPath(decodedItemName, XBSettings.XBTagsUrlPattern, out qsValue);
                        if (!string.IsNullOrEmpty(blogItemPath))
                            filter = BlogFilterType.Tag;
                        else
                        {
                            blogItemPath = ResolveBlogItemPath(decodedItemName, XBSettings.XBAuthorViewUrlPattern, out qsValue);
                            if (!string.IsNullOrEmpty(blogItemPath))
                                filter = BlogFilterType.AuthorView;
                            else
                            {
                                blogItemPath = ResolveBlogItemPath(decodedItemName, XBSettings.XBSearchURLPattern, out qsValue);
                                if (!string.IsNullOrEmpty(blogItemPath))
                                    filter = BlogFilterType.Search;
                            }
                        }
                    }
                }
                if (!string.IsNullOrEmpty(blogItemPath) && filter != BlogFilterType.None)
                {
                    Item blogRoot = args.GetItem(blogItemPath);
                    if (blogRoot != null)
                    {
                        Context.Item = blogRoot;
                        NameValueCollection nv = StringUtil.ParseNameValueCollection(args.Url.QueryString, '&', '=');
                        switch (filter)
                        {
                            case BlogFilterType.Category: nv.Add(XBSettings.XBCategoryQS, qsValue);
                                break;
                            case BlogFilterType.Author: nv.Add(XBSettings.XBAuthorQS, qsValue);
                                break;
                            case BlogFilterType.AuthorView: nv.Add(XBSettings.XBAuthorViewQS, qsValue);
                                break;
                            case BlogFilterType.Tag: nv.Add(XBSettings.XBTagQS, qsValue);
                                break;
                            case BlogFilterType.Search: nv.Add(XBSettings.XBSearchQS, qsValue);
                                break;
                            case BlogFilterType.None:
                            default:
                                break;
                        }
                        args.Url.QueryString = StringUtil.NameValuesToString(nv, "&");
                    }
                }
                Profiler.EndOperation();
            }
        }

        private string ResolveBlogItemPath(string decodedItemName, string urlPattern, out string qsValue)
        {
            qsValue = string.Empty;
            try
            {
                string pattern = urlPattern.FormatWith(@"(^.+)", @"(.+)/$");
                Match match = Regex.Match(StringUtil.EnsurePostfix('/', decodedItemName), @pattern, RegexOptions.IgnoreCase);

                if (match.Success)
                {
                    qsValue = WebUtil.UrlEncode(match.Groups[2].Value);
                    return match.Groups[1].Value;
                }

            }
            catch (Exception ex)
            {
                Log.Error("XBlog could not resolve url to a blog item", ex, new object());
            }
            return string.Empty;
        }
    }
}
