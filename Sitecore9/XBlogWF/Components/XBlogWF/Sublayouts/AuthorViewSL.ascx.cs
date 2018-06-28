using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog.General;
using Sitecore.Feature.XBlog;
using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Resources.Media;

namespace XBlogWF.Components.XBlogWF.Sublayouts
{
    public partial class AuthorViewSL : BaseSublayout
    {
        public BlogSettings settingsItem;
        protected void Page_Load(object sender, EventArgs e)
        {
            // set settings item to datasource or context

            settingsItem = Sitecore.Feature.XBlog.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            bool authorFound = false;
            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBAuthorViewQS]))
            {
                Author authorItem = AuthorManager.GetAuthorByName(DataSourceItem, Request.QueryString[XBSettings.XBAuthorViewQS]);
                if (authorItem != null)
                {
                    authorFound = true;
                    lvAuthorList.Visible = false;
                    pnlAuthor.Visible = true;

                    frTitle.Item = authorItem.InnerItem;
                    frTitle.FieldName = Author.AuthorFullNameFieldId;

                    frAuthorTitle.Item = authorItem.InnerItem;
                    frAuthorTitle.FieldName = Author.AuthorTitleFieldId;

                    frLocation.Item = authorItem.InnerItem;
                    frLocation.FieldName = Author.AuthorLocationFieldId;

                    frBio.Item = authorItem.InnerItem;
                    frBio.FieldName = Author.AuthorBioFieldId;

                    frProfileImage.Item = authorItem.InnerItem;
                    frProfileImage.FieldName = Author.AuthorProfileImageFieldId;
                    frProfileImage.CssClass = "authorImage";


                    string searchHeading = "";
                    string categoryID = "";
                    string authorID = "";
                    string tagID = "";
                    string searchText = "";

                    
                    authorID = authorItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                    {
                        searchHeading = settingsItem.AuthorFilterTitle + authorItem.FullName;
                    }
                   

                    //Get search results
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


                    IEnumerable<BlogPost> blogs = BlogManager.GetBlogPosts(DataSourceItem, categoryID, authorID, tagID, searchText, startRowIndex, maximumRows);
                    int totalRows = BlogManager.GetBlogsCount(DataSourceItem, categoryID, authorID, tagID, searchText);

                    if (searchHeading != "")
                    {
                        searchHeading = totalRows.ToString() + " " + searchHeading;
                    }

                    litSearchHeading.Text = searchHeading;

                    litPagination.Text = BlogManager.GetBlogPagination(settingsItem, currentPage, totalRows, maximumRows);

                    if (blogs == null || !blogs.Any())
                        return;

                    // Bind data source
                    lvBlogPosts.DataSource = blogs;
                    lvBlogPosts.DataBind();
                }
            }

            if (!authorFound)
            {
                lvAuthorList.Visible = true;
                pnlAuthor.Visible = false;

                // no author found, get list
                AuthorView authorItem = Sitecore.Context.Item.CreateAs<AuthorView>();

                frTitle.Item = authorItem.InnerItem;
                frTitle.FieldName = AuthorView.AuthorDefaultTitleFieldId;

                //Get search results
                IEnumerable<Author> authors = AuthorManager.GetAuthors(DataSourceItem);

                if (authors == null || !authors.Any())
                    return;

                // Bind data source
                lvAuthorList.DataSource = authors;
                lvAuthorList.DataBind();

            }
        }

        protected void lvAuthorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;

            Author author = e.Item.DataItem as Author;
            if (author == null)
                return;

            HyperLink hlAuthor = e.Item.FindControl("hlAuthor") as HyperLink;
            if (hlAuthor != null)
            {
                hlAuthor.Text = author.FullName;
                hlAuthor.NavigateUrl = AuthorManager.GetAuthorViewUrl(author.FullName, settingsItem);

                Literal litProfileImage = e.Item.FindControl("litProfileImage") as Literal;

                if (litProfileImage != null && author.ProfileImage != null && author.ProfileImage.MediaItem != null && settingsItem.AuthorListDisplayImage)
                {
                    litProfileImage.Text = String.Format("<img src=\"{0}?mw={1}\" border=\"0\" />", MediaManager.GetMediaUrl(author.ProfileImage.MediaItem), settingsItem.AuthorViewImageMaxWidth);
                }
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
                    scBlogPostSummary.Text = Sitecore.Feature.XBlog.Helpers.Helper.SafeSubstring(blogPost.Body, settingsItem.DisplaySummaryLength);
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

                if (litProfileImage != null && authors[0].ProfileImage != null && authors[0].ProfileImage.MediaItem != null && settingsItem.DisplayAuthorImage)
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