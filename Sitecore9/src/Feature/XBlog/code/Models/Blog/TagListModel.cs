using Sitecore.Data.Items;
using Sitecore.Feature.XBlog.Items.Blog;
using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Models.Blog
{
    public class TagListModel : RenderingModel
    {
        public Dictionary<string, int> tagCount { get; set; }
        public Item dataSourceItem { get; set; }
        public IEnumerable<Tag> tags { get; set; }
    }
}