using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
    [SitecoreItemTemplate(XBlogDataTemplateId)]
    public class XBlogData : SitecoreItem
    {
        public const string XBlogDataTemplateId = "{43A42F53-3992-4CF4-AB86-E3596FDC46C2}";
    }
}
