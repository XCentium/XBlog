using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Import
{
    public class WPCategory
    {
        public string CategoryName { get; set; }


        private const string WordpressNamespace = "http://wordpress.org/export/1.2/";
        public WPCategory(XElement categortXML)
        {
            XNamespace wpContent = WordpressNamespace;
            CategoryName = categortXML.Element(wpContent + "category_nicename").Value;

        }
    }
}
