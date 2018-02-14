using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog
{
    public class AuthorListModel : RenderingModel
    {
        public Dictionary<string, int> authorCount { get; set; }
        public Item dataSourceItem { get; set; }
        public IEnumerable<Author> authors { get; set; }
    }
}