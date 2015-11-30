using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
     [SitecoreItemTemplate(XBlogCommentsTemplateId)]
    public class XBlogComments : SitecoreItem 
    {
         public const string XBlogCommentsTemplateId = "{FD58F23F-9E45-415A-AA1F-1C74425C0734}";
    }
}
