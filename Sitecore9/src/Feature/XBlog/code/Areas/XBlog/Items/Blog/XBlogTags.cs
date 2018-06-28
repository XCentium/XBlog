using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog
{
    [SitecoreItemTemplate(XBlogTagsTemplateId)]
    public class XBlogTags : SitecoreItem
    {
        public const string XBlogTagsTemplateId = "{42C5D698-2945-465F-936C-950AF504C559}";
        
    }
}
