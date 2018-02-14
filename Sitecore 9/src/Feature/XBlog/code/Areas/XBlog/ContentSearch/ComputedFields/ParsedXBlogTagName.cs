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

namespace Sitecore.Feature.XBlog.Areas.XBlog.ContentSearch.ComputedFields
{
    public class ParsedXBlogTagName : IComputedIndexField
    {
        public object ComputeFieldValue(IIndexable indexable)
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

        //public object ComputeFieldValue(IIndexable indexable)
        //{
        //    throw new NotImplementedException();
        //}

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
