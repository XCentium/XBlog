using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Feature.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Models.Import;
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Diagnostics;
using Sitecore.SecurityModel;
using Sitecore.Feature.XBlog.Items.Blog;

namespace Sitecore.Feature.XBlog.Import
{
    public class ImportManager
    {
        public static List<WPPost> Import(string fileLocation, bool includeComments, bool includeCategories, bool includeTags)
        {
            try
            {
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("atom", "http://www.w3.org/2005/Atom");

                var parseContext = new XmlParserContext(null, nsm, null, XmlSpace.Default);
                using (var reader = XmlReader.Create(fileLocation, null, parseContext))
                {
                    var doc = XDocument.Load(reader);

                    var posts = (from item in doc.Descendants("item")
                                 select new WPPost(item, false, true, true)).ToList();

                    return posts;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to read xml: " + ex.Message, "importer");
                return new List<WPPost>();
            }
        }

        public static List<WPTag> ImportTags(string fileLocation)
        {
            try
            {
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("wp", "http://wordpress.org/export/1.2/excerpt/");
                var parseContext = new XmlParserContext(null, nsm, null, XmlSpace.Default);

                using (var reader = XmlReader.Create(fileLocation, null, parseContext))
                {
                    var doc = XDocument.Load(reader);
                    string WordpressNamespace = "http://wordpress.org/export/1.2/";

                    XNamespace wpContent = WordpressNamespace;
                    List<WPTag> blogTags = new List<WPTag>();

                    foreach (XElement element in doc.Descendants("channel"))
                    {
                        foreach (XElement e in element.Descendants(wpContent + "tag"))
                        {
                            WPTag tag = new WPTag(e);
                            blogTags.Add(tag);
                        }
                    }

                    return blogTags;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to import tags: " + ex.Message, "WP Importer");
                return new List<WPTag>();
            }
        }

        public static List<WPCategory> ImportCategories(string fileLocation)
        {
            try
            {
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("wp", "http://wordpress.org/export/1.2/excerpt/");
                var parseContext = new XmlParserContext(null, nsm, null, XmlSpace.Default);

                using (var reader = XmlReader.Create(fileLocation, null, parseContext))
                {
                    var doc = XDocument.Load(reader);
                    string WordpressNamespace = "http://wordpress.org/export/1.2/";

                    XNamespace wpContent = WordpressNamespace;
                    List<WPCategory> blogCategories = new List<WPCategory>();

                    foreach (XElement element in doc.Descendants("channel"))
                    {
                        foreach (XElement e in element.Descendants(wpContent + "category"))
                        {
                            WPCategory cat = new WPCategory(e);
                            blogCategories.Add(cat);
                        }
                    }

                    return blogCategories;
                }
            }
            catch (Exception ex)
            { 
                Log.Error("Failed to import categories: " + ex.Message, "WP Importer");
                return new List<WPCategory>();
            }
        }


        public static List<WPAuthor> ImportAuthors(string fileLocation)
        {
            try
            {
                var nsm = new XmlNamespaceManager(new NameTable());
                nsm.AddNamespace("wp", "http://wordpress.org/export/1.2/excerpt/");
                var parseContext = new XmlParserContext(null, nsm, null, XmlSpace.Default);

                using (var reader = XmlReader.Create(fileLocation, null, parseContext))
                {
                    var doc = XDocument.Load(reader);
                    string WordpressNamespace = "http://wordpress.org/export/1.2/";

                    XNamespace wpContent = WordpressNamespace;
                    List<WPAuthor> blogAuthors = new List<WPAuthor>();

                    foreach (XElement element in doc.Descendants("channel"))
                    {
                        foreach (XElement e in element.Descendants(wpContent + "author"))
                        {
                            WPAuthor auth = new WPAuthor(e);
                            blogAuthors.Add(auth);
                        }
                    }

                    return blogAuthors;
                }
            }
            catch (Exception ex)
            {
                Log.Error("Failed to import authors: " + ex.Message, "WP Importer");
                return new List<WPAuthor>();
            }
       }

        public static void ImportPosts(Item bh, List<WPPost> listWordpressPosts, string blogName, Item userBlogPostTemplate)
        {
            Database master = Database.GetDatabase("master");

            foreach (WPPost post in listWordpressPosts)
            {
                try
                {
                    using (new SecurityDisabler())
                    {
                        if (string.IsNullOrEmpty(post.Content) || string.IsNullOrEmpty(post.Title))
                            continue;

                        TemplateItem blogPostTemplate = master.GetItem(userBlogPostTemplate.ID);

                        string validItemName = ItemUtil.ProposeValidItemName(post.Title);
                        if (string.IsNullOrEmpty(validItemName))
                            continue;


                        BlogPost bp = ItemManager.CreateItem(validItemName, bh, userBlogPostTemplate.ID).CreateAs<BlogPost>();

                        if (bp == null)
                            continue;

                        using (new EditContext(bp.InnerItem))
                        {
                            bp.InnerItem.SetString(BlogPost.BlogPostTitleFieldId, post.Title.Replace("-", " "));
                            bp.InnerItem.SetString(BlogPost.BlogPostBodyFieldId, post.Content);
                            bp.PublishDate.Value = DateUtil.ToIsoDate(post.PublishDate);

                            if (!string.IsNullOrEmpty(post.WPAuthor))
                            {
                                if (post.Tags.Any())
                                {
                                    foreach (string tag in post.Tags)
                                    {
                                        Item t = master.SelectItems("fast://sitecore/content/Data/" + blogName + "/Tags/*[@Tag Name= '" + tag + "']").FirstOrDefault();
                                        if (t == null)
                                            continue;
                                        bp.InnerItem.SetValue(BlogPost.BlogPostTagsFieldId, t.ID);
                                    }
                                }

                                if (post.Categories.Any())
                                {
                                    foreach (string cat in post.Categories)
                                    {
                                        Item c = master.SelectItems("fast://sitecore/content/Data/" + blogName + "/Categories/*[@Category Name= '" + cat + "']").FirstOrDefault();
                                        if (c == null)
                                            continue;
                                        bp.InnerItem.SetValue(BlogPost.BlogPostCategoryFieldId, c.ID);
                                    }
                                }                              

                                Item auth = master.SelectItems("fast://sitecore/content/Data/" + blogName + "/Authors/*[@Creator= '" + post.WPAuthor + "']").FirstOrDefault();
                                if (auth != null)
                                    bp.InnerItem.SetValue(BlogPost.BlogPostAuthorFieldId, auth.ID);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Cannot create entry for: " + post.Title + " " + ex.Message, "WPImporter");
                }
            }
        }
    }
}
