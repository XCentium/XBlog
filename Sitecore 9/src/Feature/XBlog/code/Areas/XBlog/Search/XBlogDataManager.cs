using Sitecore.ContentSearch;
using Sitecore.ContentSearch.Linq.Utilities;
using Sitecore.ContentSearch.SearchTypes;
using Sitecore.ContentSearch.Utilities;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Search
{
    public class XBlogDataManager
    {
        public static List<SearchResultItem> GetItemsByTaxonomy(Item contentRoot, IEnumerable<string> taxonomyItemNames, int count, bool matchAll, bool descending)
        {
            List<ID> taxonomyItemIds = GetTagItemIds(taxonomyItemNames);
            if (taxonomyItemIds.Any())
                return GetItemsByTaxonomy(contentRoot, taxonomyItemIds, count, matchAll, descending);

            return new List<SearchResultItem>(0);
        }

        public static List<ID> GetTagItemIds(IEnumerable<string> taxonomyItemNames)
        {
            try
            {
                Tag tagRepository = GetTagRepositoryItem();
                using (IProviderSearchContext context = ContentSearchManager.GetIndex(new SitecoreIndexableItem(tagRepository.InnerItem)).CreateSearchContext())
                {
                    Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                    predicate = predicate.And(item => item.TemplateId == ID.Parse(Tag.TagTemplateId));
                    predicate = predicate.And(item => item.Language == Context.Language.Name);
                    Expression<Func<SearchResultItem, bool>> predicate1 = PredicateBuilder.True<SearchResultItem>();
                    taxonomyItemNames.ForEach(name =>
                    {

                        predicate1 = predicate1.Or(p => p.Name == name);
                    });
                    predicate = predicate.And(predicate1);

                    List<ID> returnList = context.GetQueryable<SearchResultItem>().Where(predicate).Select(result => result.ItemId).ToList();

                    return returnList;

                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetTaxonomyItemIds error", ex, new object());
            }

            return new List<ID>(0);
        }

        public static Tag GetTagRepositoryItem()
        {
            return new Tag();
            //return CacheUtil.GetObject<Tag>(BusinessConsts.CacheKeys.TaxonomyRepositoryItemKey, () =>
            //{
            //    Item repositoryItem = Sitecore.Context.Database.GetItem(ARSettings.TaxonomyRepositoryItemId);
            //    Assert.IsNotNull(repositoryItem, "could not locate taxonomy repository");
            //    return repositoryItem.CreateAs<TaxonomyRepository>();
            //}, TimeSpan.FromHours(4));
        }

        public static List<SearchResultItem> GetItemsByTaxonomy(Item contentRoot, IEnumerable<ID> taxonomyItemIds, int count, bool matchAll, bool descending)
        {
            try
            {
                using (IProviderSearchContext context = ContentSearchManager.GetIndex(new SitecoreIndexableItem(contentRoot)).CreateSearchContext())
                {
                    IEnumerable<string> normalizedIds = taxonomyItemIds.Select(id => IdHelper.NormalizeGuid(id));
                    if (normalizedIds.Any())
                    {
                        #region root item filter
                        Expression<Func<SearchResultItem, bool>> predicate = PredicateBuilder.True<SearchResultItem>();
                        predicate = predicate.And(p => p.Paths.Contains(contentRoot.ID));

                        #endregion

                        #region taxonomy predicate
                        if (normalizedIds.Any())
                        {
                            Expression<Func<SearchResultItem, bool>> taxonomyPredicate = PredicateBuilder.True<SearchResultItem>();
                            if (!matchAll)
                            {
                                normalizedIds.ForEach(id =>
                                {
                                    taxonomyPredicate = taxonomyPredicate.Or(p => p["TODO REPLACE THIS WITH FIELDNAME FOR TAG ITEM"].Contains(id));
                                });
                            }
                            else
                            {
                                normalizedIds.ForEach(id =>
                                {
                                    taxonomyPredicate = taxonomyPredicate.And(p => p["TODO REPLACE THIS WITH FIELDNAME FOR TAG ITEM"].Contains(id));
                                });
                            }
                            predicate = predicate.And(taxonomyPredicate);
                        }
                        #endregion

                        #region language filter
                        predicate = predicate.And(p => p.Language == Context.Language.Name);
                        #endregion

                        if (descending)
                            return context.GetQueryable<SearchResultItem>().Where(predicate).OrderByDescending(r => r["TODO REPLACE THIS WITH FIELDNAME FOR PUBLICATION DATE"]).Take(count).ToList();
                        return context.GetQueryable<SearchResultItem>().Where(predicate).OrderBy(r => r["TODO REPLACE THIS WITH FIELDNAME FOR PUBLICATION DATE"]).Take(count).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error("XBlog GetItemsByTaxonomy error", ex, new object());
            }
            return new List<SearchResultItem>(0);
        }

    }
}
