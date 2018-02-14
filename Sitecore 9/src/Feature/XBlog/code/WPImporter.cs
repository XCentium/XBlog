using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Feature.XBlog.Import;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Models.Import;

namespace Sitecore.Feature.XBlog
{
    class WPImporter
    {
        public static List<WPPost> Import(string fileLocation, bool includeComments, bool includeCategories, bool includeTags)
        {
            var nsm = new XmlNamespaceManager(new NameTable());
            nsm.AddNamespace("atom", "http://www.w3.org/2005/Atom");

            var parseContext = new XmlParserContext(null, nsm, null, XmlSpace.Default);
            using (var reader = XmlReader.Create(fileLocation, null, parseContext))
            {
                var doc = XDocument.Load(reader);

                var posts = (from item in doc.Descendants("item")
                             select new WPPost(item, includeComments, includeCategories, includeTags)).ToList();

                return posts;
            }
        }
        private void ImportBlog()
        {
            string fileLocation = @"C:\Users\Kloss\Documents\Projects\XBlog\test.xml";
            List<WPPost> listWordpressPosts = ImportManager.Import(fileLocation, false, false, false);

            BlogHome bh = new BlogHome();
            bh.InnerItem.Editing.BeginEdit();
            bh.BlogName = "meh";
            bh.Name = "meh";
            bh.InnerItem.Editing.EndEdit();

            

           // ImportManager.ImportPosts(bh, listWordpressPosts);
        }
    }
}
