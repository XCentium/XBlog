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
    public class ParsedXBlogAuthorName : IComputedIndexField
    {
        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
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
