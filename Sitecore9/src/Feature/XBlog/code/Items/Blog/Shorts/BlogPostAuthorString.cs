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
    public class BlogPostAuthorString : SitecoreItem
    {
        [SitecoreItemField(BlogPost.BlogPostAuthorFieldId)]
        public virtual string AuthorString { get; set; }
    }
}
