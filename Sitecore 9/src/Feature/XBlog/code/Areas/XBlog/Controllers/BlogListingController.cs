using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Areas.XBlog.General;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.Search;
using Sitecore.Mvc.Presentation;
using System;
using System.Web.Mvc;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Controllers
{
    public class BlogListingController : Controller
    {
        public BlogListingController()
        {

        }

        [HttpGet]
        public ActionResult BlogListing()
        {
            var model = new BlogListingModel();
            model.Initialize(RenderingContext.Current.Rendering);
            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            BlogSettings settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);

            string categoryID = string.Empty;
            string authorID = string.Empty;
            string tagID = string.Empty;
            string searchText = string.Empty;

            string urlCategoryName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBCategoryUrlPattern);
            if (!String.IsNullOrEmpty(urlCategoryName))
            {
                Category categoryItem = CategoryManager.GetCategoryByName(Context.Item, urlCategoryName);
                if (categoryItem != null)
                {
                    categoryID = categoryItem.InnerItem.ID.ToString();
                    if (settingsItem.DisplayFilterMessage)
                        model.SearchHeading = settingsItem.CategoryFilterTitle + categoryItem.CategoryName;
                }
            }

            string urlAuthorName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBAuthorUrlPattern);
            if (!String.IsNullOrEmpty(urlAuthorName))
            {
                Author authorItem = AuthorManager.GetAuthorByName(Context.Item, urlAuthorName);
                if (authorItem != null)
                {
                    authorID = authorItem.InnerItem.ID.ToString();
                    if (settingsItem.DisplayFilterMessage)
                        model.SearchHeading = settingsItem.AuthorFilterTitle + authorItem.FullName;
                }
            }

            string urlTagName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBTagsUrlPattern);
            if (!String.IsNullOrEmpty(urlTagName))
            {
                Tag tagItem = TagManager.GetTagByName(Context.Item, urlTagName);
                if (tagItem != null)
                {
                    tagID = tagItem.InnerItem.ID.ToString();

                    if (settingsItem.DisplayFilterMessage)
                        model.SearchHeading = settingsItem.TagFilterTitle + tagItem.TagName;
                }
            }

            string urlSearchName = DataManager.GetUrlValByPattern(Request.Url.PathAndQuery, XBSettings.XBSearchURLPattern);
            if (!String.IsNullOrEmpty(urlSearchName))
            {
                searchText = urlSearchName;
                if (settingsItem.DisplayFilterMessage)
                    model.SearchHeading = settingsItem.SearchFilterTitle + searchText;
            }


            //Get search results
            int currentPage = 1;
            int maximumRows = 5;
            int startRowIndex = 1;
            bool rowResult = Int32.TryParse(settingsItem.PageSize, out maximumRows);
            model.MaximumRows = maximumRows;

            if (!rowResult)
                model.MaximumRows = 5;

            if (!String.IsNullOrEmpty(Request.QueryString[XBSettings.XBPageQS]))
                model.PageResult = Int32.TryParse(Request.QueryString[XBSettings.XBPageQS], out currentPage);
            if (!model.PageResult)
                currentPage = 1;

            startRowIndex = (currentPage - 1) * maximumRows;
            model.CurrentPage = currentPage;
            model.BlogPosts = BlogManager.GetBlogPosts(Sitecore.Context.Item, categoryID, authorID, tagID, searchText, startRowIndex, maximumRows);
            model.TotalRows = BlogManager.GetBlogsCount(Sitecore.Context.Item, categoryID, authorID, tagID, searchText);

            if (!string.IsNullOrEmpty(model.SearchHeading))
                model.SearchHeading = model.TotalRows.ToString() + " " + model.SearchHeading;

            return View("~/Areas/XBlog/Views/XBlog/BlogListing.cshtml", model);
        }
    }
}