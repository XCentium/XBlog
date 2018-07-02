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
    public class ParsedXBlogAuthorName : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
            try
            {
                if (item.IsValidType<BlogPost>())
                {
                    List<Author> authorItems = item.CreateAs<BlogPost>().Authors.ToList();
                    if (authorItems.Any())
                    {
                        authorItems.ForEach(tItem =>
                        {
                            if (tItem != null)
                                list.Add(tItem.InnerItem.Name);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot created ParsedXBlogAuthorName field for {item.ID}", ex, typeof(ParsedXBlogAuthorName));
            }
            
            return list.Any() ? list : null;

        }
    }
}
