using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Utilities;
using Sitecore.ContentSearch.Linq;
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
using XBlogHelper.General;
using XBlogHelper.Models.Blog;
using XBlogHelper.Models.Blog.Shorts;
using XBlogHelper;
using XBlogHelper.ItemMapper;

namespace XBlogHelper.Search
{
    public class TagManager
    {
        public static IEnumerable<Tag> GetTags(Item currentItem)
        {
            try
            {
                BlogSettings settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.TagFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == Sitecore.Data.ID.Parse(Tag.TagTemplateId) && item.Paths.Contains(settingsItem.TagFolder.ID));

                    return context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name).CreateAs<Tag>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetTags error", ex, new object());
            }
            return null;
        }

        public static IEnumerable<Tag> GetTagsOrderedByCount(Dictionary<string, int> tagCount)
        {
            IList<Tag> tagList = new List<Tag>();
            // order the count descending then build list based on this order.
            var items = from pair in tagCount
                        orderby pair.Value descending
                        select pair;

            Sitecore.Data.Database context = Sitecore.Context.Database;
            foreach (KeyValuePair<string, int> pair in items)
            {
                if (Sitecore.Data.ID.IsID(pair.Key))
                {
                    Tag thisItem = context.GetItem(Sitecore.Data.ID.Parse(pair.Key)).CreateAs<Tag>();
                    tagList.Add(thisItem);
                }
            }

            return tagList;
        }

        public static Tag GetTagByName(Item currentItem, string tagName)
        {
            try
            {
                BlogSettings settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem();
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.AuthorFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == Sitecore.Data.ID.Parse(Tag.TagTemplateId) && item.Paths.Contains(settingsItem.TagFolder.ID));
                    predicate = predicate.And(c => c[Tag.TagNameField] == tagName);

                    IQueryable<SearchResultItem> results = context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name);
                    
                    if (results.Any())
                        return results.FirstOrDefault().GetItem().CreateAs<Tag>();
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetTagByname error", ex, new object());
            }
            return null;
        }

        public static Dictionary<string, int> GetTagCloud(Item currentItem)
        {
            Dictionary<string, int> tagItems = new Dictionary<string, int>();

            try
            {
                Item repositorySearchItem = XBlogHelper.General.DataManager.GetBlogHomeItem(currentItem);
                BlogSettings settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(currentItem);
                ISearchIndex index = ContentSearchManager.GetIndex(new SitecoreIndexableItem(settingsItem.AuthorFolder));

                using (IProviderSearchContext context = index.CreateSearchContext())
                {

                    // look at the pipe seperated list.  break them out and loop through.  Build dictionary based on Ids
                    // create as blog post item
                    // could even pass a max item as a dictionary definition
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateName == BlogPost.BlogPostTemplate && item.Paths.Contains(repositorySearchItem.ID));

                    IEnumerable<BlogPostTagString> resultList = context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(t => t.Name).CreateAs<BlogPostTagString>();

                    List<ID> listIDs = context.GetQueryable<SearchResultItem>().Where(predicate).Select(result => result.ItemId).ToList();

                    tagItems.Add("max", 0);
                    foreach (BlogPostTagString item in resultList)
                    {
                        char[] delimiterChars = { '|' };
                        string[] itemIDs = item.TagString.Split(delimiterChars);

                        foreach(string itemID in itemIDs)
                        {
                            if (tagItems.ContainsKey(itemID))
                                tagItems[itemID] += 1;
                            else
                                tagItems.Add(itemID, 1);

                            if (tagItems[itemID] > tagItems["max"])
                                tagItems["max"] = tagItems[itemID];
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetTagCloud error", ex, new object());
            }
            return tagItems;
        }

        public static string GetTagCloudClass(int itemTotal, int max)
        {
            string cssClass = "none";
            double weightPercent = (Convert.ToDouble(itemTotal) / Convert.ToDouble(max)) * 100;

            if (weightPercent >= XBSettings.XBTagCloudWeightOne)
                cssClass = XBSettings.XBTagCloudClassOne;
            else if (weightPercent >= XBSettings.XBTagCloudWeightTwo)
                cssClass = XBSettings.XBTagCloudClassTwo;
            else if (weightPercent >= XBSettings.XBTagCloudWeightThree)
                cssClass = XBSettings.XBTagCloudClassThree;
            else if (weightPercent >= XBSettings.XBTagCloudWeightFour)
                cssClass = XBSettings.XBTagCloudClassFour;
            else if (weightPercent >= XBSettings.XBTagCloudWeightFive)
                cssClass = XBSettings.XBTagCloudClassFive;


            return cssClass;
        }

        public static string GetTagUrl(string tagName, Item currentItem)
        {
            Item blogHome = General.DataManager.GetBlogHomeItem(currentItem);
            UrlOptions option = new UrlOptions();
            option.AddAspxExtension = false;
            return String.Format("{0}/{1}/{2}", LinkManager.GetItemUrl(blogHome, option), XBSettings.XBTagQS, tagName);
        }

        public static string GetCommaSeperatedTagList(BlogPost blogItem)
        {
            StringBuilder display = new StringBuilder();

            int totalTags = blogItem.Tags.Count;
            int currentCount = 1;

            foreach (Tag tagItem in blogItem.Tags)
            {
                display.Append(String.Format("<a href=\"{0}\">{1}</a>", GetTagUrl(tagItem.TagName, blogItem.InnerItem), tagItem.TagName));
                if (currentCount < totalTags)
                    display.Append(", ");

                currentCount++;
            }

            return display.ToString();
        }

        public static List<Tag> GetTagsForBlogPost(List<ID> authorIds)
        {
            List<Tag> tags = new List<Tag>();

            foreach (ID id in authorIds)
            {
                tags.Add(Sitecore.Context.Database.GetItem(id).CreateAs<Tag>());
            }

            return tags;
        }
    }
}
