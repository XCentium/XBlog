using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Import
{
    public class WPAuthor
    {
        public string Creator { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        private const string WordpressNamespace = "http://wordpress.org/export/1.2/";
        public WPAuthor(XElement authorXML)
        {
            XNamespace wpContent = WordpressNamespace;

            Name = authorXML.Element(wpContent + "author_display_name").Value;
            Email = authorXML.Element(wpContent + "author_email").Value;
            Creator = authorXML.Element(wpContent + "author_login").Value;
        }
    }
}
