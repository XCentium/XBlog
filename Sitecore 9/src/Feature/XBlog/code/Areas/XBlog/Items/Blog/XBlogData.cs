using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog
{
    [SitecoreItemTemplate(XBlogDataTemplateId)]
    public class XBlogData : SitecoreItem
    {
        public const string XBlogDataTemplateId = "{43A42F53-3992-4CF4-AB86-E3596FDC46C2}";
    }
}
