using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Diagnostics;

namespace Sitecore.Feature.XBlog.Areas.XBlog.ContentSearch.ComputedFields
{
    public class ParsedXBlogTagName : AbstractComputedIndexField
    {
        public override object ComputeFieldValue(IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
            try
            {
                if (item.IsValidType<BlogPost>())
                {
                    List<Tag> tagItems = item.CreateAs<BlogPost>().Tags.ToList();
                    if (tagItems.Any())
                    {
                        tagItems.ForEach(tItem =>
                        {
                            if (tItem != null)
                                list.Add(tItem.TagName);
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error($"Cannot created ParsedXBlogTagName field for {item.ID}", ex, typeof(ParsedXBlogTagName));
            }
           
            return list.Any() ? list : null;

        }
    }
}
