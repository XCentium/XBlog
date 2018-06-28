using Sitecore.Mvc.Presentation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog
{
    public class MetaModel : RenderingModel
    {
        public string ogURL { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string image { get; set; }
    }
}