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
using Sitecore.Feature.XBlog.Areas.XBlog.General;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog.Shorts;
using Sitecore.Feature.XBlog.Areas.XBlog;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Search
{
    public class AuthorManager
    {
        public static IEnumerable<Author> GetAuthors(Item currentItem)
        {
            try
            {
                BlogSettings settingsItem = General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.AuthorFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == ID.Parse(Author.AuthorTemplateId) && item.Paths.Contains(settingsItem.AuthorFolder.ID));

                    return context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name).CreateAs<Author>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetAuthors error", ex, new object());
            }
            return null;
        }

        public static IEnumerable<Author> GetAuthorsOrderedByCount(Dictionary<string, int> authorCount)
        {
            IList<Author> authorList = new List<Author>();
            // order the count descending then build list based on this order.
            var items = from pair in authorCount
                        orderby pair.Value descending
                        select pair;

            Database context = Context.Database;
            foreach (KeyValuePair<string, int> pair in items)
            {
                if (ID.IsID(pair.Key))
                {
                    Author thisItem = context.GetItem(ID.Parse(pair.Key)).CreateAs<Author>();
                    authorList.Add(thisItem);
                }
            }

            return authorList;
        }

        public static Author GetAuthorByName(Item currentItem, string authorName)
        {
            try
            {
                BlogSettings settingsItem = General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.AuthorFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == ID.Parse(Author.AuthorTemplateId) && item.Paths.Contains(settingsItem.AuthorFolder.ID));
                    predicate = predicate.And(c => c[Author.AuthorFullNameField] == authorName);

                    IQueryable<SearchResultItem> results = context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name);
                    if (results.Any())
                        return results.FirstOrDefault().GetItem().CreateAs<Author>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetCategory error", ex, new object());
            }
            return null;
        }

        public static string GetAuthorUrl(string authorName, Item currentItem)
        {
            Item blogHome = General.DataManager.GetBlogHomeItem(currentItem);
            UrlOptions option = new UrlOptions();
            option.AddAspxExtension = false;
            return String.Format("{0}/{1}/{2}", LinkManager.GetItemUrl(blogHome, option), XBSettings.XBAuthorQS, authorName);
        }

        public static string GetAuthorViewUrl(string authorName, BlogSettings settingsItem)
        {
            UrlOptions option = new UrlOptions();
            option.AddAspxExtension = false;
            return String.Format("{0}/{1}/{2}", LinkManager.GetItemUrl(settingsItem.AuthorViewPage, option), XBSettings.XBAuthorViewQS, authorName);
        }

        public static List<Author> GetAuthorsForBlogPost(List<ID> authorIds)
        {
            List<Author> authors = new List<Author>();

            foreach (ID id in authorIds)
            { 
                authors.Add(Context.Database.GetItem(id).CreateAs<Author>());
            }

            return authors;
        }

        public static Dictionary<string, int> GetAuthorCount(Item currentItem)
        {
            Dictionary<string, int> authorItems = new Dictionary<string, int>();

            try
            {
                Item repositorySearchItem = General.DataManager.GetBlogHomeItem(currentItem);
                BlogSettings settingsItem = General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(repositorySearchItem));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {

                    // look at the pipe seperated list.  break them out and loop through.  Build dictionary based on Ids
                    // create as blog post item
                    // could even pass a max item as a dictionary definition
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateName == BlogPost.BlogPostTemplate && item.Paths.Contains(repositorySearchItem.ID));

                    IEnumerable<BlogPostAuthorString> resultList = context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name).CreateAs<BlogPostAuthorString>();

                    foreach (BlogPostAuthorString item in resultList)
                    {
                        char[] delimiterChars = { '|' };
                        string[] itemIDs = item.AuthorString.Split(delimiterChars);

                        foreach (string itemID in itemIDs)
                        {
                            if (authorItems.ContainsKey(itemID))
                                authorItems[itemID] += 1;
                            else
                                authorItems.Add(itemID, 1);

                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetAuthorCount error", ex, new object());
            }
            return authorItems;
        }

        public static IEnumerable<Author> SetAuthorDisplayLimit(string maxDisplay, IEnumerable<Author> authors)
        {
            int number;
            bool result = Int32.TryParse(maxDisplay, out number);
            if (!result)
            {
                number = 99;
            }

            if (authors.Count() < number)
            {
                number = authors.Count();
            }

            return authors.Take(number);
        }
    }
}
