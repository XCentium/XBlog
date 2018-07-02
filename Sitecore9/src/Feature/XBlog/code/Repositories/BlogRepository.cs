using System;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog.General;
using Sitecore.Mvc.Presentation;
using System.Web;
using Sitecore.Feature.XBlog.Items.Blog;

namespace Sitecore.Feature.XBlog.Repositories
{
    public class BlogRepository : IBlogRepository
    {
        public BlogListingModel GetBlogListing(HttpRequestBase request)
        {
            var model = new BlogListingModel();
            model.Initialize(RenderingContext.Current.Rendering);

            model.dataSourceItem = Context.Database.GetItem(RenderingContext.Current.Rendering.DataSource);
            var settingsItem = DataManager.GetBlogSettingsItem(model.dataSourceItem != null ? model.dataSourceItem : Context.Item);
            //string searchHeading = "";
            string categoryID = "";
            string authorID = "";
            string tagID = "";
            string searchText = "";

            string urlCategoryName = DataManager.GetUrlValByPattern(request.Url.PathAndQuery, XBSettings.XBCategoryUrlPattern);
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

            string urlAuthorName = DataManager.GetUrlValByPattern(request.Url.PathAndQuery, XBSettings.XBAuthorUrlPattern);
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

            string urlTagName = DataManager.GetUrlValByPattern(request.Url.PathAndQuery, XBSettings.XBTagsUrlPattern);
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


            string urlSearchName = DataManager.GetUrlValByPattern(request.Url.PathAndQuery, XBSettings.XBSearchURLPattern);
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

            if (!String.IsNullOrEmpty(request.QueryString[XBSettings.XBPageQS]))
            {
                model.PageResult = Int32.TryParse(request.QueryString[XBSettings.XBPageQS], out currentPage);
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

            return model;
        }
    }
}