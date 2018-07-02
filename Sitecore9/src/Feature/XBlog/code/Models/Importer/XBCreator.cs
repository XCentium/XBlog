using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Sitecore.Feature.XBlog.Models.Importer
{
    public class XBCreator
    {
        public string blogName { get; set; }
        public string blogParent { get; set; }
        public string blogType { get; set; }
        public string uploadFile { get; set; }
    }
}