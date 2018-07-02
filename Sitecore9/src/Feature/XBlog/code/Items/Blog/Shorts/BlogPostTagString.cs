using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes;
using Sitecore.Feature.XBlog.Models.Blog;

namespace Sitecore.Feature.XBlog.Items.Blog.Shorts
{
    [SitecoreItemTemplate(BlogPost.BlogPostTemplateId)]
    public class BlogPostTagString : SitecoreItem
    {
        [SitecoreItemField(BlogPost.BlogPostTagsFieldId)]
        public virtual string TagString { get; set; }
    }
}
