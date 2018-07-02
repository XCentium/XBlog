using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.XBlog.ContentSearch.ComputedFields
{
    public class ParsedXBlogCategoryName : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
            try
            {
                if (item.IsValidType<BlogPost>())
                {
                    List<Category> categoryItems = item.CreateAs<BlogPost>().Categories.ToList();
                    if (categoryItems.Any())
                    {
                        categoryItems.ForEach(tItem =>
                        {
                            if (tItem != null)
                                list.Add(tItem.CategoryName);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot created ParsedXBlogCategoryName field for {item.ID}", ex, typeof(ParsedXBlogCategoryName));
            }
           
            return list.Any() ? list : null;
        }
    }
}
