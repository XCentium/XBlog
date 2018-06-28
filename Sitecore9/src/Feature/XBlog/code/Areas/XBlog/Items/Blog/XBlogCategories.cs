using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog
{
    [SitecoreItemTemplate(XBlogCategoriesTemplateId)]
    public class XBlogCategories :SitecoreItem 
    {
        public const string XBlogCategoriesTemplateId = "{EA3ABBC0-280B-4E62-9CB6-57C82B2B73D9}";
    }
}
