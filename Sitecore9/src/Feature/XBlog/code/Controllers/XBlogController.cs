using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Sitecore.Feature.XBlog.Import;
using Sitecore.Mvc.Presentation;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.General;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Data.Items;
using Sitecore.Links;
using System.Text.RegularExpressions;
using Sitecore.Resources.Media;
using Sitecore.Feature.XBlog.Models.Importer;
using Sitecore.Feature.XBlog.Repositories;
using Sitecore.Mvc.Controllers;
using System.Web;

namespace Sitecore.Feature.XBlog.Controllers
{
    public class XBlogController : SitecoreController
    {
        private string _datasource;
        private readonly IBlogRepository _blogRepository;

        public XBlogController(IBlogRepository blogRepository)
        {
            _blogRepository = blogRepository;
        }

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
            //TODO:  Need to figure out another way instead of using base.Request being passed to the repo.  This won't be easy to unit test.
            var model = _blogRepository.GetBlogListing(Request);
            return View("~/Views/Feature/XBlog/BlogListing.cshtml", model);
        }

        public ActionResult BlogPost()
        {
            var model = new BlogPostModel();
            model.Initialize(RenderingContext.Current.Rendering);
            
            return View("~/Views/Feature/XBlog/BlogPost.cshtml", model);
        }

        public ActionResult AuthorView()
        {
            _datasource = GetNavigationItemPath("Author View");
            return this.View("~/Views/Feature/XBlog/AuthorView.cshtml");
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
            return this.View("~/Views/Feature/XBlog/Callouts/AuthorList.cshtml", model);
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
            return this.View("~/Views/Feature/XBlog/Callouts/AuthorViewList.cshtml", model);
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
            return this.View("~/Views/Feature/XBlog/Callouts/CategoryList.cshtml", model);
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
                model.description = Regex.Replace(Sitecore.Feature.XBlog.Helpers.Helper.SafeSubstring(blogPost.Summary, settingsItem.DisplaySummaryLength), "<.*?>", String.Empty);
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

            return this.View("~/Views/Feature/XBlog/Callouts/OGPostMeta.cshtml", model);
        }

        public ActionResult RecentBlog()
        {
            _datasource = GetNavigationItemPath("Recent Blog");
            return this.View("~/Views/Feature/XBlog/Callouts/RecentBlog.cshtml");
        }

        public ActionResult RelatedBlog()
        {
            _datasource = GetNavigationItemPath("Related Blog");
            return this.View("~/Views/Feature/XBlog/Callouts/RelatedBlog.cshtml");
        }

        public ActionResult TagCloud()
        {
            var model = new TagListModel();
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.tagCount = TagManager.GetTagCloud(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            model.tags = TagManager.GetTags(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            return this.View("~/Views/Feature/XBlog/Callouts/TagCloud.cshtml", model);
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

            return this.View("~/Views/Feature/XBlog/Callouts/TagList.cshtml", model);
        }

        public ActionResult TextSearch()
        {
            _datasource = GetNavigationItemPath("Text Search");
            return this.View("~/Views/Feature/XBlog/Callouts/TextSearch.cshtml");
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
