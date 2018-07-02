using Sitecore.Feature.XBlog.Models.Blog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Repositories
{
    public interface IBlogRepository
    {
        BlogListingModel GetBlogListing(HttpRequestBase request);
    }
}