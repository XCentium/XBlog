using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sitecore.Feature.XBlog.Models.Import
{
    public class WPTag
    {
        public string TagName { get; set; }


        private const string WordpressNamespace = "http://wordpress.org/export/1.2/";
        public WPTag(XElement tagXML)
        {
            XNamespace wpContent = WordpressNamespace;
            TagName = tagXML.Element(wpContent + "tag_slug").Value;
        }
    }
}
