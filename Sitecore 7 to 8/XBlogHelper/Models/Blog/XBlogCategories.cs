using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
    [SitecoreItemTemplate(XBlogCategoriesTemplateId)]
    public class XBlogCategories :SitecoreItem 
    {
        public const string XBlogCategoriesTemplateId = "{EA3ABBC0-280B-4E62-9CB6-57C82B2B73D9}";
    }
}
