using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;
using XBlogHelper.Models.Blog;

namespace XBlogHelper.Models.Blog.Shorts
{
    [SitecoreItemTemplate(BlogPost.BlogPostTemplateId)]
    public class BlogPostTagString : SitecoreItem
    {
        [SitecoreItemField(BlogPost.BlogPostTagsFieldId)]
        public virtual string TagString { get; set; }
    }
}
