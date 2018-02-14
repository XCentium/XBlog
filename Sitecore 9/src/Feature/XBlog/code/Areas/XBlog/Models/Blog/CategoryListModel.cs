using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog
{
    public class CategoryListModel : RenderingModel
    {
        public Dictionary<string, int> categoryCount { get; set; }
        public Item dataSourceItem { get; set; }
        public IEnumerable<Category> categories { get; set; }
    }
}