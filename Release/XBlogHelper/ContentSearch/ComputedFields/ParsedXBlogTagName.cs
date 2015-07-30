using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.Models.Blog;
using Sitecore.ContentSearch;
using Sitecore.ContentSearch.ComputedFields;
using Sitecore.Data.Items;

namespace XBlogHelper.ContentSearch.ComputedFields
{
    public class ParsedXBlogTagName : IComputedIndexField
    {
        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
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
            return list;

        }

        public string FieldName
        {
            get;
            set;
        }

        public string ReturnType
        {
            get;
            set;
        }
    }
}
