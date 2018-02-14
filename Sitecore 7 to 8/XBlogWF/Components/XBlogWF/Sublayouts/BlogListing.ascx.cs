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
//using XBlogHelper.ItemMapper;
using XBlogHelper.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Resources.Media;
using System.ComponentModel;
using XBlogHelper.Helpers;

namespace XBlogWF.Components.XBlogWF.Sublayouts
{
    public partial class BlogListing : System.Web.UI.UserControl
    {
        public BlogSettings settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem();
        protected void Page_Load(object sender, EventArgs e)
        {

            int currentPage = 1;
            int maximumRows = 5;
            int startRowIndex = 1;
            bool rowResult = Int32.TryParse(settingsItem.PageSize, out maximumRows);
            if (!rowResult)
            {
                maximumRows = 5;   
            }

            

            bool pageResult = false;
        
            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBPageQS]))
            {
                pageResult = Int32.TryParse(Request.QueryString[XBSettings.XBPageQS], out currentPage);
            }
            if (!pageResult)
            {
                currentPage = 1;   
            }

            startRowIndex = (currentPage - 1) * maximumRows;

            string categoryID = "";
            string authorID = "";
            string tagID = "";
            string searchText = "";

            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBCategoryQS]))
            {
                Category categoryItem = CategoryManager.GetCategoryByName(Sitecore.Context.Item, Request.QueryString[XBSettings.XBCategoryQS]);
                if (categoryItem != null)
                {
                    categoryID = categoryItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                        litSearchHeading.Text = settingsItem.CategoryFilterTitle + categoryItem.CategoryName;
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBAuthorQS]))
            {
                Author authorItem = AuthorManager.GetAuthorByName(Sitecore.Context.Item, Request.QueryString[XBSettings.XBAuthorQS]);
                if (authorItem != null)
                {
                    authorID = authorItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                        litSearchHeading.Text = settingsItem.AuthorFilterTitle + authorItem.FullName;
                }
            }

            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBTagQS]))
            {
                Tag tagItem = TagManager.GetTagByName(Sitecore.Context.Item, Request.QueryString[XBSettings.XBTagQS]);
                if (tagItem != null)
                {
                    tagID = tagItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                        litSearchHeading.Text = settingsItem.TagFilterTitle + tagItem.TagName;
                }
            }

            string urlSearchName = XBlogHelper.General.DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBSearchURLPattern);
            if (!String.IsNullOrEmpty(urlSearchName))
            {
                if (!String.IsNullOrEmpty(urlSearchName))
                {
                    searchText = urlSearchName;

                    if (settingsItem.DisplayFilterMessage)
                        litSearchHeading.Text = settingsItem.SearchFilterTitle + searchText;
                }
            }

            IEnumerable<BlogPost> blogs = BlogManager.GetBlogPosts(Sitecore.Context.Item, categoryID, authorID, tagID, searchText, startRowIndex, maximumRows);

            if (blogs == null || !blogs.Any())
                return;

            int totalRows = BlogManager.GetBlogsCount(Sitecore.Context.Item, categoryID, authorID, tagID, searchText);
            litPagination.Text = BlogManager.GetBlogPagination(settingsItem, currentPage, totalRows, maximumRows);

            // Bind data source
            lvBlogPosts.DataSource = blogs;
            lvBlogPosts.DataBind();


            if (litSearchHeading.Text != "")
            {
                litSearchHeading.Text = totalRows.ToString() + " " + litSearchHeading.Text;
            }
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

            Literal scBlogPostSummary = e.Item.FindControl("scBlogPostSummary") as Literal;
            if (scBlogPostSummary != null)
            {
                if (!String.IsNullOrEmpty(blogPost.Summary))
                {
                    scBlogPostSummary.Text = blogPost.Summary;
                }
                else
                {
                    scBlogPostSummary.Text = XBlogHelper.Helpers.Helper.SafeSubstring(blogPost.Body, settingsItem.DisplaySummaryLength);
                }
                
            }

            Date dateBlogPostPublishDate = e.Item.FindControl("dateBlogPostPublishDate") as Date;
            if (dateBlogPostPublishDate != null)
            {
                dateBlogPostPublishDate.Item = blogPost.InnerItem;
                dateBlogPostPublishDate.Field = BlogPost.BlogPostPublishDateFieldId;
                dateBlogPostPublishDate.Format = settingsItem.BlogListingDateFormat;
            }

            if (settingsItem.DisplayCategoryOnBlogPostList && blogPost.Categories.Count > 0)
            {
                Literal litCategories = e.Item.FindControl("litCategories") as Literal;
                if (litCategories != null)
                {
                    litCategories.Text = "<strong>" + settingsItem.BlogListingCategoryTitle + "</strong> " + CategoryManager.GetCommaSeperatedCategoryList(blogPost);
                }
            }

            if (settingsItem.DisplayTagsOnBlogPostList && blogPost.Tags.Count > 0)
            {
                Literal litTags = e.Item.FindControl("litTags") as Literal;
                if (litTags != null)
                {
                    litTags.Text = "<strong>" + settingsItem.BlogListingTagTitle + "</strong> " + TagManager.GetCommaSeperatedTagList(blogPost);
                }
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

                Literal litProfileImage = e.Item.FindControl("litProfileImage") as Literal;

                if (litProfileImage != null && authors[0].ProfileImage != null && authors[0].ProfileImage.MediaItem != null && authors[0].ProfileImage.MediaItem != null && settingsItem.DisplayAuthorImage)
                {
                    litProfileImage.Text = String.Format("<img src=\"{0}?mw={1}\" border=\"0\" />", MediaManager.GetMediaUrl(authors[0].ProfileImage.MediaItem), settingsItem.AuthorImageMaxWidth);
                }
            }

            HyperLink hlReadMore = e.Item.FindControl("hlReadMore") as HyperLink;
            if (hlReadMore != null && settingsItem.ReadMoreLinkText != "")
            {
                hlReadMore.Text = settingsItem.ReadMoreLinkText;
                hlReadMore.ToolTip = settingsItem.ReadMoreLinkText;
                hlReadMore.NavigateUrl = Sitecore.Links.LinkManager.GetItemUrl(blogPost.InnerItem);
            }
        }
    
    }
}