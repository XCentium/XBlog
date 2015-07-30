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
    public class ParsedXBlogCategoryName : IComputedIndexField
    {
        public object ComputeFieldValue(Sitecore.ContentSearch.IIndexable indexable)
        {
            Item item = (Item)(indexable as SitecoreIndexableItem);
            List<string> list = new List<string>();
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
