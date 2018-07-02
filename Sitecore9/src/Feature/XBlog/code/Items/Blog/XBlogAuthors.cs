using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Items.Blog
{
    [SitecoreItemTemplate(XBlogAuthorsTemplateId)]
    public class XBlogAuthors : SitecoreItem
    {
        public const string XBlogAuthorsTemplateId = "{A83E5283-6C18-4E20-805D-1399D6317208}";
    }
}
