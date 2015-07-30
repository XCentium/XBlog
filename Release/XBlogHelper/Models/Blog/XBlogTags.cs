using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
    [SitecoreItemTemplate(XBlogTagsTemplateId)]
    public class XBlogTags : SitecoreItem
    {
        public const string XBlogTagsTemplateId = "{42C5D698-2945-465F-936C-950AF504C559}";
        
    }
}
