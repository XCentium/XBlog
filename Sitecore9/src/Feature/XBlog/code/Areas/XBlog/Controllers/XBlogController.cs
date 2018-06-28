using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Sitecore.Feature.XBlog.Areas.XBlog.Import;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.General;
using Sitecore.Feature.XBlog.Areas.XBlog.Search;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Text.RegularExpressions;
using Sitecore.Resources.Media;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Importer;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Controllers
{
    public class XBlogController : Controller
    {
        private string _datasource;

        [HttpPost]
        [AllowAnonymous]
        public JsonResult Create(string xBlogData)
        {
            try
            {
                if (Context.User.IsAdministrator)
                {
                    xBlogData = xBlogData.Replace("[", "").Replace("]", "");

                    if (string.IsNullOrEmpty(xBlogData))
                        return Json(String.Format(""));

                    JavaScriptSerializer serializer = new JavaScriptSerializer();
                    XBCreator x = serializer.Deserialize<XBCreator>(xBlogData);

                    BlogCreator.CreateBlog(x.blogName, x.blogType, x.uploadFile, x.blogParent);

                    return Json(String.Format("success"));
                }
                else
                {
                    string message = "XBlog Error - A non administrator attempted to create a blog";
                    Log.Error(message, this);
                    return Json(message);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, this);
                return Json(String.Format(ex.Message));
            }
        }

        [HttpGet]
        public ActionResult BlogListing()
        {
            var model = new BlogListingModel();
            model.Initialize(RenderingContext.Current.Rendering);

            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            //string searchHeading = "";
            string categoryID = "";
            string authorID = "";
            string tagID = "";
            string searchText = "";

            string urlCategoryName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBCategoryUrlPattern);
            if (!String.IsNullOrEmpty(urlCategoryName))
            {
                Category categoryItem = CategoryManager.GetCategoryByName(Sitecore.Context.Item, urlCategoryName);
                if (categoryItem != null)
                {
                    categoryID = categoryItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                    {
                        model.SearchHeading = settingsItem.CategoryFilterTitle + categoryItem.CategoryName;
                    }
                }
            }

            string urlAuthorName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBAuthorUrlPattern);
            if (!String.IsNullOrEmpty(urlAuthorName))
            {
                Author authorItem = AuthorManager.GetAuthorByName(Sitecore.Context.Item, urlAuthorName);
                if (authorItem != null)
                {
                    authorID = authorItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                    {
                        model.SearchHeading = settingsItem.AuthorFilterTitle + authorItem.FullName;
                    }
                }
            }

            string urlTagName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBTagsUrlPattern);
            if (!String.IsNullOrEmpty(urlTagName))
            {
                Tag tagItem = TagManager.GetTagByName(Sitecore.Context.Item, urlTagName);
                if (tagItem != null)
                {
                    tagID = tagItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                    {
                        model.SearchHeading = settingsItem.TagFilterTitle + tagItem.TagName;
                    }
                }
            }


            string urlSearchName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBSearchURLPattern);
            if (!String.IsNullOrEmpty(urlSearchName))
            {
                if (!String.IsNullOrEmpty(urlSearchName))
                {
                    searchText = urlSearchName;

                    if (settingsItem.DisplayFilterMessage)
                    {
                        model.SearchHeading = settingsItem.SearchFilterTitle + searchText;
                    }
                }
            }


            //Get search results
            int currentPage = 1;
            int maximumRows = 5;
            int startRowIndex = 1;
            bool rowResult = Int32.TryParse(settingsItem.PageSize, out maximumRows);
            model.MaximumRows = maximumRows;
            if (!rowResult)
            {
                model.MaximumRows = 5;
            }

            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBPageQS]))
            {
                model.PageResult = Int32.TryParse(Request.QueryString[XBSettings.XBPageQS], out currentPage);
            }
            if (!model.PageResult)
            {
                currentPage = 1;
            }

            startRowIndex = (currentPage - 1) * maximumRows;

            model.CurrentPage = currentPage;

            model.BlogPosts = BlogManager.GetBlogPosts(Sitecore.Context.Item, categoryID, authorID, tagID, searchText, startRowIndex, maximumRows);
            model.TotalRows = BlogManager.GetBlogsCount(Sitecore.Context.Item, categoryID, authorID, tagID, searchText);

            if (!string.IsNullOrEmpty(model.SearchHeading))
            {
                model.SearchHeading = model.TotalRows.ToString() + " " + model.SearchHeading;
            }

            return View("~/Areas/XBlog/Views/XBlog/BlogListing.cshtml", model);
        }

        public ActionResult BlogPost()
        {
            var model = new BlogPostModel();
            model.Initialize(RenderingContext.Current.Rendering);
            
            return View("~/Areas/XBlog/Views/XBlog/BlogPost.cshtml",model);
        }

        public ActionResult AuthorView()
        {
            _datasource = GetNavigationItemPath("Author View");
            return this.View("~/Areas/XBlog/Views/XBlog/AuthorView.cshtml");
        }

        public ActionResult AuthorList()
        {
            var model = new AuthorListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.authorCount = AuthorManager.GetAuthorCount(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            IEnumerable<Author> authors = null;
            if (settingsItem.OrderAuthorOnCount)
            {

                authors = AuthorManager.GetAuthorsOrderedByCount(model.authorCount);
            }
            else
            {
                authors = AuthorManager.GetAuthors(model.dataSourceItem != null ? model.dataSourceItem : Sitecore.Context.Item);
            }
            
            // Set max display
            authors = AuthorManager.SetAuthorDisplayLimit(settingsItem.AuthorListMaxAuthorsToDisplay, authors);
            model.authors = authors;
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/AuthorList.cshtml", model);
        }

        public ActionResult AuthorViewList()
        {
            var model = new AuthorListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.authorCount = AuthorManager.GetAuthorCount(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            IEnumerable<Author> authors = null;
            if (settingsItem.OrderAuthorOnCount)
            {

                authors = AuthorManager.GetAuthorsOrderedByCount(model.authorCount);
            }
            else
            {
                authors = AuthorManager.GetAuthors(model.dataSourceItem != null ? model.dataSourceItem : Sitecore.Context.Item);
            }
            
            // Set max display
            authors = AuthorManager.SetAuthorDisplayLimit(settingsItem.AuthorListMaxAuthorsToDisplay, authors);
            model.authors = authors;
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/AuthorViewList.cshtml", model);
        }

        public ActionResult CategoryList()
        {
            var model = new CategoryListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.categoryCount = CategoryManager.GetCategoryCount(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            IEnumerable<Category> categories = null;
            if (settingsItem.OrderCategoryOnCount)
            {
                categories = CategoryManager.GetCategoriesOrderedByCount(model.categoryCount);
            }
            else
            {
                categories = CategoryManager.GetCategories(model.dataSourceItem != null ? model.dataSourceItem : Sitecore.Context.Item);
            }

            model.categories = categories;
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/CategoryList.cshtml", model);
        }

        public ActionResult OGPostMeta()
        {
            var model = new MetaModel();
            Item dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(dataSourceItem != null ? dataSourceItem : Sitecore.Context.Item);
            BlogPost blogPost = Context.Item.CreateAs<BlogPost>();
            model.title = blogPost.Title;
            UrlOptions options = new UrlOptions();
            options.AlwaysIncludeServerUrl = true;
            model.ogURL = LinkManager.GetItemUrl(blogPost.InnerItem, options);
            
            if (!String.IsNullOrEmpty(blogPost.Summary))
            {
                model.description = Regex.Replace(blogPost.Summary, "<.*?>", String.Empty);
            }
            else
            {
                model.description = Regex.Replace(Sitecore.Feature.XBlog.Areas.XBlog.Helpers.Helper.SafeSubstring(blogPost.Summary, settingsItem.DisplaySummaryLength), "<.*?>", String.Empty);
            }

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
                    model.image = MediaManager.GetMediaUrl(authors[0].ProfileImage.MediaItem, mediaOptions);
                }
            }

            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/OGPostMeta.cshtml", model);
        }

        public ActionResult RecentBlog()
        {
            _datasource = GetNavigationItemPath("Recent Blog");
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/RecentBlog.cshtml");
        }

        public ActionResult RelatedBlog()
        {
            _datasource = GetNavigationItemPath("Related Blog");
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/RelatedBlog.cshtml");
        }

        public ActionResult TagCloud()
        {
            var model = new TagListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.tagCount = TagManager.GetTagCloud(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.tags = TagManager.GetTags(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/TagCloud.cshtml", model);
        }

        public ActionResult TagList()
        {
            var model = new TagListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.tagCount = TagManager.GetTagCloud(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            if (settingsItem.OrderTagOnCount)
            {
                model.tags = TagManager.GetTagsOrderedByCount(model.tagCount);
            }
            else
            {
                model.tags = TagManager.GetTags(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            }

            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/TagList.cshtml", model);
        }

        public ActionResult TextSearch()
        {
            _datasource = GetNavigationItemPath("Text Search");
            return this.View("~/Areas/XBlog/Views/XBlog/Callouts/TextSearch.cshtml");
        }
        private string GetNavigationItemPath(string navigationTypeName)
        {
            string datasource = RenderingContext.Current.Rendering.DataSource;

            if (string.IsNullOrWhiteSpace(datasource))
            {
                return null;
            }

            return datasource;
        }

    }
}
