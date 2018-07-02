using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Links;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.General;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Feature.XBlog.Items.Blog.Shorts;
using Sitecore.Feature.XBlog;
using Sitecore.Feature.XBlog.ItemMapper;

namespace Sitecore.Feature.XBlog.Search
{
    public class CategoryManager
    {
        public static IEnumerable<Category> GetCategories(Item currentItem)
        {
            try
            {
                BlogSettings settingsItem = General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.CategoryFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == ID.Parse(Category.CategoryTemplateId) && item.Paths.Contains(settingsItem.CategoryFolder.ID));

                    return context.GetQueryable<SearchResultItem>().Where(predicate).CreateAs<Category>();//.OrderBy(t => t.Name)
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetCategory error", ex, new object());
            }
            return null;
        }

        public static IEnumerable<Category> GetCategoriesOrderedByCount(Dictionary<string, int> categoryCount)
        {
            IList<Category> categoryList = new List<Category>();
            // order the count descending then build list based on this order.
            var items = from pair in categoryCount
                        orderby pair.Value descending
                        select pair;

            Database context = Context.Database;
            foreach (KeyValuePair<string, int> pair in items)
            {
                if (ID.IsID(pair.Key))
                {
                    Category thisItem = context.GetItem(ID.Parse(pair.Key)).CreateAs<Category>();
                    categoryList.Add(thisItem);
                }
            }

            return categoryList;
        }

        public static Category GetCategoryByName(Item currentItem, string categoryName)
        {
            try
            {
                BlogSettings settingsItem = XBlog.General.DataManager.GetBlogSettingsItem();
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.CategoryFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == ID.Parse(Category.CategoryTemplateId) && item.Paths.Contains(settingsItem.CategoryFolder.ID));
                    predicate = predicate.And(c => c[Category.CategoryNameField] == categoryName);

                    IQueryable<SearchResultItem> results = context.GetQueryable<SearchResultItem>().Where(predicate);//.OrderBy(t => t.Name);
                    if (results.Any())
                        return results.FirstOrDefault().GetItem().CreateAs<Category>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetCategory error", ex, new object());
            }
            return null;
        }

        public static string GetCategoryUrl(string categoryName, Item currentItem)
        {
            Item blogHome = General.DataManager.GetBlogHomeItem(currentItem);
            UrlOptions option = new UrlOptions();
            option.AddAspxExtension = false;
            return String.Format("{0}/{1}/{2}", LinkManager.GetItemUrl(blogHome, option), XBSettings.XBCategoryQS, categoryName);
        }

        public static string GetCommaSeperatedCategoryList(BlogPost blogItem)
        {
            StringBuilder display = new StringBuilder();

            int totalCategories = blogItem.Categories.Count;
            int currentCount = 1;

            foreach (Category categoryItem in blogItem.Categories)
            {
                display.Append(String.Format("<a href=\"{0}\">{1}</a>", GetCategoryUrl(categoryItem.CategoryName, blogItem.InnerItem), categoryItem.CategoryName));
                if (currentCount < totalCategories)
                    display.Append(", ");

                currentCount++;
            }

            return display.ToString();
        }


        public static Dictionary<string, int> GetCategoryCount(Item currentItem)
        {
            Dictionary<string, int> categoryItems = new Dictionary<string, int>();

            try
            {
                Item repositorySearchItem = XBlog.General.DataManager.GetBlogHomeItem(currentItem);
                BlogSettings settingsItem = XBlog.General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(repositorySearchItem));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {

                    // look at the pipe seperated list.  break them out and loop through.  Build dictionary based on Ids
                    // create as blog post item
                    // could even pass a max item as a dictionary definition
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateName == BlogPost.BlogPostTemplate && item.Paths.Contains(repositorySearchItem.ID));

                    IEnumerable<BlogPostCategoryString> resultList = context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name).CreateAs<BlogPostCategoryString>();

                    foreach (BlogPostCategoryString item in resultList)
                    {
                        char[] delimiterChars = { '|' };
                        string[] itemIDs = item.CategoryString.Split(delimiterChars);

                        foreach(string itemID in itemIDs)
                        {
                            if (categoryItems.ContainsKey(itemID))
                                categoryItems[itemID] += 1;
                            else
                                categoryItems.Add(itemID, 1);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetCategoryCount error", ex, new object());
            }
            return categoryItems;
        }
    }
}
