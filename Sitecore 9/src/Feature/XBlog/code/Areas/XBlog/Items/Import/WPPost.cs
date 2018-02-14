using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Models.Import
{
    public class WPPost
    {
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string WPAuthor { get; set; }
        public string Content { get; set; }
        public List<string> Categories { get; set; }
        public List<string> Tags { get; set; }
        public List<WPComment> Comments { get; set; }

        private const string ContentNamespace = "http://purl.org/dc/elements/1.1/";
        private const string CommentNamespace = "http://wellformedweb.org/CommentAPI/";
        private const string ElementsNamespace = "http://purl.org/rss/1.0/modules/content/";
        private const string WordpressNamespace = "http://wordpress.org/export/1.2/";
        public WPPost(XElement postXml, bool includeComments, bool includeCategories, bool includeTags)
        {
            GetPost(postXml, includeComments, includeCategories, includeTags);
        }

        private void GetPost(XElement postXml, bool includeComments, bool includeCategories, bool includeTags)
        {
            XNamespace nsContent = ContentNamespace;
            XNamespace wpContent = WordpressNamespace;
            XNamespace dc = ContentNamespace;
            XNamespace content = ElementsNamespace;

            if (postXml.Element("title") != null)
                Title = postXml.Element("title").Value;

            if (postXml.Element(content + "encoded") != null)
                Content = postXml.Element(content + "encoded").Value;

            if (postXml.Element("pubDate") != null)
                PublishDate = DateTime.Parse(postXml.Element("pubDate").Value);

            if (postXml.Element(dc + "creator") != null)
                WPAuthor = postXml.Element(dc + "creator").Value;

            Categories = new List<string>();
            Tags = new List<string>();
            Comments = new List<WPComment>();

            if (includeCategories)
            {
                Categories = (from category in postXml.Elements("category")
                              where category.Attribute("domain").Value == "category"
                              select category.Attribute("nicename").Value.ToString()).ToList();
                             
            }

            if (includeTags)
            {
                Tags = (from category in postXml.Elements("category")
                        where category.Attribute("domain").Value == "post_tag"
                        select category.Attribute("nicename").Value.ToString()).ToList();
            }

            if (includeComments)
            {
                Comments = (from comment in postXml.Elements(wpContent + "comment")
                            orderby comment.Value
                            select new WPComment(comment)).ToList();
            }
        }
    }
}
