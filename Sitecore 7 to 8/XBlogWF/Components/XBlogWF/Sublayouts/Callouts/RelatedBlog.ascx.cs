using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;
using XBlogHelper.Search;
using XBlogHelper.General;
using XBlogHelper;
using XBlogHelper.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Resources.Media;
using System.ComponentModel;
using XBlogHelper.Helpers;

namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class RelatedBlog : BaseSublayout
    {
        public BlogSettings settingsItem;
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            BlogPost currentBlogPost = Sitecore.Context.Item.CreateAs<BlogPost>();

            // set title
            frTitle.FieldName = BlogSettings.RelatedBlogTitleFieldId;
            frTitle.Item = settingsItem.InnerItem;

            //Get search results
            IEnumerable<BlogPost> blogs = BlogManager.GetBlogRelatedValues(currentBlogPost);
            blogs = BlogManager.SetBlogDisplayLimit(settingsItem.RelatedBlogMaxPosts, blogs);

            if (blogs == null || !blogs.Any())
            {
                return;
            }

            // Bind data source
            lvBlogPosts.DataSource = blogs;
            lvBlogPosts.DataBind();
        }

        protected void lvBlogPosts_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;

            BlogPost blogPost = e.Item.DataItem as BlogPost;
            if (blogPost == null)
                return;

            Text scBlogPostTitle = e.Item.FindControl("scBlogPostTitle") as Text;
            if (scBlogPostTitle != null)
            {
                scBlogPostTitle.Item = blogPost.InnerItem;
            }

            HyperLink hlTitle = e.Item.FindControl("hlTitle") as HyperLink;
            if (hlTitle != null)
            {
                hlTitle.Text = blogPost.Title;
                hlTitle.ToolTip = blogPost.Title;
                hlTitle.NavigateUrl = Sitecore.Links.LinkManager.GetItemUrl(blogPost.InnerItem);
            }

            Date dateBlogPostPublishDate = e.Item.FindControl("dateBlogPostPublishDate") as Date;
            if (dateBlogPostPublishDate != null)
            {
                dateBlogPostPublishDate.Item = blogPost.InnerItem;
                dateBlogPostPublishDate.Field = BlogPost.BlogPostPublishDateFieldId;
                dateBlogPostPublishDate.Format = settingsItem.BlogListingDateFormat;
            }

            Literal litAuthor = e.Item.FindControl("litAuthor") as Literal;
            if (litAuthor != null && blogPost.Authors.Any())
            {
                List<ID> authorIds = new List<Sitecore.Data.ID>();
                foreach (Author a in blogPost.Authors)
                {
                    authorIds.Add(a.ItemId);
                }

                List<Author> authors = AuthorManager.GetAuthorsForBlogPost(authorIds);

                string authorNames = string.Empty;

                foreach (Author a in authors)
                {
                    authorNames += a.FullName;
                }

                litAuthor.Text = authors[0].FullName;

            }
        }
    }
}