using Sitecore.Data.Items;
using System;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog.General;
using Sitecore.Feature.XBlog;
using Sitecore.Feature.XBlog.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Resources.Media;
using System.ComponentModel;
using Sitecore.Feature.XBlog.Helpers;


namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class OGPostMeta : BaseSublayout
    {
        public BlogSettings settingsItem;
        public string ogURL;
        public string ogTitle;
        public string ogDescription;
        public string ogAuthorImage;
        protected void Page_Load(object sender, EventArgs e)
        {
            BlogSettings settingsItem = Sitecore.Feature.XBlog.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            BlogPost blogPost = Sitecore.Context.Item.CreateAs<BlogPost>();

            UrlOptions options = new UrlOptions();
            options.AlwaysIncludeServerUrl = true;
            ogURL = LinkManager.GetItemUrl(blogPost.InnerItem, options);

            ogTitle = blogPost.Title;

            ogDescription = "";
            if (!String.IsNullOrEmpty(blogPost.Summary))
            {
                ogDescription = Regex.Replace(blogPost.Summary, "<.*?>", String.Empty);
            }
            else
            {
                ogDescription = Regex.Replace(Sitecore.Feature.XBlog.Helpers.Helper.SafeSubstring(blogPost.Summary, settingsItem.DisplaySummaryLength), "<.*?>", String.Empty);
            }

            ogAuthorImage = "";
            if (blogPost.Authors.Any())
            {
                List<Sitecore.Data.ID> authorIds = new List<Sitecore.Data.ID>();
                foreach (Author a in blogPost.Authors)
                {
                    authorIds.Add(a.ItemId);
                }

                List<Author> authors = AuthorManager.GetAuthorsForBlogPost(authorIds);

                if (authors[0].ProfileImage != null && authors[0].ProfileImage.MediaItem != null && settingsItem.DisplayAuthorImage)
                {
                    MediaUrlOptions mediaOptions = new MediaUrlOptions();
                    mediaOptions.AlwaysIncludeServerUrl = true;
                    ogAuthorImage = MediaManager.GetMediaUrl(authors[0].ProfileImage.MediaItem, mediaOptions);
                }
            }
        }
    }
}