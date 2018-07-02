using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Models.Blog
{
    public class BlogListingModel : RenderingModel
    {
        public BlogListingModel()
        {
            CurrentPage = 1;
            MaximumRows = 5;
            StartRowIndex = 1;
            PageResult = false;
        }
        public string SearchHeading { get; set; }

        public string BlogName { get; set; }
        public int CurrentPage { get; set; }

        public int MaximumRows { get; set; }

        public int TotalRows { get; set; }

        public Item dataSourceItem { get; set; }
        public int StartRowIndex { get; set; }

        public bool PageResult { get; set; }

        public IEnumerable<BlogPost> BlogPosts { get; set; }
    }
}