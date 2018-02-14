using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog;

namespace XBlogWF.Components.XBlogWF.Sublayouts
{
    public partial class BlogPostSL : System.Web.UI.UserControl
    {
        public BlogSettings settingsItem = Sitecore.Feature.XBlog.General.DataManager.GetBlogSettingsItem();
        public string currentUrl;
        public string title;
        protected void Page_Load(object sender, EventArgs e)
        {
            LoadBlogPost();
        }

        private void LoadBlogPost()
        {
            //Context item - gets Blog Post infomration
            BlogPost blogPost = Sitecore.Context.Item.CreateAs<BlogPost>();
            scTitle.Item = Sitecore.Context.Item;
            scBody.Item = Sitecore.Context.Item;
            dateBlogPostPublishDate.Item = Sitecore.Context.Item;
            
            UrlOptions option = new UrlOptions();
            option.AlwaysIncludeServerUrl = true;
            //currentUrl = HttpUtility.UrlEncode(LinkManager.GetItemUrl(blogPost.InnerItem, option));
            currentUrl = LinkManager.GetItemUrl(blogPost.InnerItem, option);
            title = HttpUtility.UrlEncode(blogPost.Title);

            if (settingsItem.IncludeEmailOnBlogPost)
                pnlEmail.Visible = true;
            if (settingsItem.IncludeFacebookOnBlogPost)
                pnlFacebook.Visible = true;
            if (settingsItem.IncludeGooglePlusOnBlogPost)
                pnlGooglePlus.Visible = true;
            if (settingsItem.IncludeLinkedinOnBlogPost)
                pnlLinkedin.Visible = true;
            if (settingsItem.IncludeTwitterOnBlogPost)
                pnlTwitter.Visible = true;
            
            if (dateBlogPostPublishDate != null)
            {
                dateBlogPostPublishDate.Field = BlogPost.BlogPostPublishDateFieldId;
                dateBlogPostPublishDate.Format = settingsItem.BlogPostDateFormat;
            }


            if (blogPost.Authors.Any())
            {
                List<Sitecore.Data.ID> authorIds = new List<Sitecore.Data.ID>();
                foreach (Author a in blogPost.Authors)
                {
                    authorIds.Add(a.ItemId);
                }

                List<Author> authors = AuthorManager.GetAuthorsForBlogPost(authorIds);


                litAuthor.Text = authors[0].FullName;
            }

            litCategories.Text = CategoryManager.GetCommaSeperatedCategoryList(blogPost);

            litTags.Text = TagManager.GetCommaSeperatedTagList(blogPost);
        }
    }
}