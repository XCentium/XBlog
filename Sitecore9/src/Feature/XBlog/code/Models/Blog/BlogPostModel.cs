using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Models.Blog
{
    public class BlogPostModel : RenderingModel
    {
        public BlogPost BlogPost { get; set; }

        public override void Initialize(Rendering rendering)
        {
            base.Initialize(rendering);
            BlogPost = Sitecore.Context.Item.CreateAs<BlogPost>();
            //if (rendering.Item != null && rendering.Item.IsValidType<BlogPost>())
            //{
            //    BlogPost = rendering.Item.CreateAs<BlogPost>();
            //}
        }
    }
}