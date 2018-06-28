using Sitecore.Configuration;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using Sitecore.Feature.XBlog.Areas.XBlog.Import;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Blog;
using Sitecore.Feature.XBlog.Areas.XBlog.Models.Import;
using Sitecore.Data;
using Sitecore.SecurityModel;
using Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Import
{
    public class BlogCreator
    {
        public static bool CreateBlog(string blogName, string blogType, string importFileName, string blogStartId)
        {
            try
            {
                BlogType.Tech currentBlogType;

                if (blogType.ToLower() == "mvc")
                    currentBlogType = BlogType.Tech.MVC;
                else
                    currentBlogType = BlogType.Tech.WebForm;

                bool hasImportFile = false;
                string fileLocation = string.Empty;

                if (!string.IsNullOrEmpty(importFileName))
                {
                    hasImportFile = true;
                    fileLocation = Settings.DataFolder.ToString() + @"\" + importFileName;
                }

                string creationPath = Context.Database.GetItem(blogStartId).Paths.FullPath;

                if (string.IsNullOrEmpty(creationPath))
                    creationPath = "/sitecore/content/home";


                if (blogName == null)
                    blogName = "Blog";

                blogName = ItemUtil.ProposeValidItemName(blogName);

                if (CreateBlogStructure(blogName, currentBlogType, hasImportFile, creationPath, fileLocation))
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, "BlogCreator");
                return false;
            }
        }

        public static bool CreateBlogStructure(string blogName, BlogType.Tech currentBlogType, bool hasImportFile, string blogHomePath, string fileLocation)
        {

            Database master = Database.GetDatabase("master");

            using (new SecurityDisabler())
            {
                #region Create high level blog structure

                TemplateItem folder = master.GetItem("{A87A00B1-E6DB-45AB-8B54-636FEC3B5523}");

                //Check to see if a Data folder already exists, if not create one.
                Item blogDataParentItem = master.GetItem("/sitecore/content/Data");

                if (blogDataParentItem == null)
                {
                    Item contentHome = master.GetItem("/sitecore/content");
                    blogDataParentItem = contentHome.Add("Data", folder);
                }

                //Where all new templates should be created
                Item pageTemplateHome = master.GetItem("/sitecore/templates/XBlog/Page");
                if (pageTemplateHome == null)
                    return false;

                //Add an instance of this blog to pageTemplateHome
                Item blogPageTemplateHomeFolder = pageTemplateHome.Add(blogName, folder);
                #endregion

                #region Create Templates (Blog Home, Author Listing, and Blog Post) specific to blog type technology

                Item userCreatedBlogBaseTemplate = null;
                Item userCreatedAuthorTemplate = null;
                Item userBlogPostTemplate = null;

                try
                {
                    if (currentBlogType == BlogType.Tech.MVC)
                    {
                        //Creates template for MVC Author Home
                        Item mvcAuthorHome = master.GetItem("/sitecore/templates/XBlog/Page/MVC/Blog Author");
                        Item userMvcAuthorHome = mvcAuthorHome.CopyTo(blogPageTemplateHomeFolder, "Blog Author");
                        userCreatedAuthorTemplate = userMvcAuthorHome;

                        //TODO if I could get the standard values and set the datasource location of the author listing to the imported authors.... would be pretty sweet.

                        //Creates template for MVC Blog Post
                        Item mvcBlogPost = master.GetItem("/sitecore/templates/XBlog/Page/MVC/Blog Post");
                        Item userMvcBlogPost = mvcBlogPost.CopyTo(blogPageTemplateHomeFolder, "Blog Post");
                        userBlogPostTemplate = userMvcBlogPost;

                        //Creates template for MVC Blog Home
                        Item mvcBlogHome = master.GetItem("/sitecore/templates/XBlog/Page/MVC/Blog Home");
                        Item userMvcBlogHome = mvcBlogHome.CopyTo(blogPageTemplateHomeFolder, "Blog Home");
                        userCreatedBlogBaseTemplate = userMvcBlogHome;

                        //Get the standard values for the newly created Blog Home and update its insert options to include the newly created Blog Post
                        Item userMvcBlogHomeStandarValues = master.GetItem(userMvcBlogHome.Paths.FullPath + "/__Standard Values");

                        //Change the insert options to use the newly created templates instead of the "shipped" MVC / WF
                        using (new EditContext(userMvcBlogHomeStandarValues))
                        {
                            userMvcBlogHomeStandarValues.SetValue("{1172F251-DAD4-4EFB-A329-0C63500E4F1E}", userMvcBlogPost.ID);
                        }
                    }
                    else if (currentBlogType == BlogType.Tech.WebForm)
                    {
                        //Creates template for WebForms Author Home
                        Item wfAuthorHome = master.GetItem("/sitecore/templates/XBlog/Page/WF/Blog Author");
                        Item userWfAuthorHome = wfAuthorHome.CopyTo(blogPageTemplateHomeFolder, "Blog Author");
                        userCreatedAuthorTemplate = userWfAuthorHome;

                        //Creates template for WebForms Blog Post
                        Item wfBlogPost = master.GetItem("/sitecore/templates/XBlog/Page/WF/Blog Post");
                        Item userWfBlogPost = wfBlogPost.CopyTo(blogPageTemplateHomeFolder, "Blog Post");
                        userBlogPostTemplate = userWfBlogPost;

                        //Creates template for MVC Blog Home
                        Item wfBlogHome = master.GetItem("/sitecore/templates/XBlog/Page/WF/Blog Home");
                        Item userWfBlogHome = wfBlogHome.CopyTo(blogPageTemplateHomeFolder, "Blog Home");
                        userCreatedBlogBaseTemplate = userWfBlogHome;

                        //Get the standard values for the newly created Blog Home and update its insert options to include the newly created Blog Post
                        Item userWfBlogHomeStandarValues = master.GetItem(userWfBlogHome.Paths.FullPath + "/__Standard Values");

                        //Change the insert options to use the newly created templates instead of the "shipped" MVC / WF
                        using (new EditContext(userWfBlogHomeStandarValues))
                        {
                            userWfBlogHomeStandarValues.SetValue("{1172F251-DAD4-4EFB-A329-0C63500E4F1E}", userWfBlogPost.ID);
                        }
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("Failed to create Blog Templates: " + ex.Message, "BlogCreator");
                    return false;
                }

                #endregion

                #region Create Data and Settings
                //create new XBlog Data node
                TemplateItem blogDataTemplate = master.GetItem(XBlogData.XBlogDataTemplateId);
                XBlogData xdata = blogDataParentItem.Add(blogName, blogDataTemplate).CreateAs<XBlogData>();

                //Create new Settings node under the XBlog Data node
                TemplateItem settingsTemplate = master.GetItem(BlogSettings.BlogSettingsTemplateId);
                BlogSettings blogSettings = xdata.InnerItem.Add("Settings", settingsTemplate).CreateAs<BlogSettings>();
                #endregion

                #region Create Author Structure and import authors if needed
                //Create new Authors node under the new XBlog Data node
                TemplateItem blogAuthorsTemplate = master.GetItem(XBlogAuthors.XBlogAuthorsTemplateId);
                XBlogAuthors xbAuthors = xdata.InnerItem.Add("Authors", blogAuthorsTemplate).CreateAs<XBlogAuthors>();

                //Links author Parent in settings
                using (new EditContext(blogSettings.InnerItem))
                {
                    blogSettings.InnerItem.SetValue(BlogSettings.AuthorFolderFieldId, xbAuthors.InnerItem.ID);
                }

                //Import authors
                if (hasImportFile && !string.IsNullOrEmpty(fileLocation))
                {
                    List<WPAuthor> authors = ImportManager.ImportAuthors(fileLocation);
                    if (authors.Any())
                        CreateImportedAuthorsInSitecore(master, authors, xbAuthors);
                }

                #endregion

                #region Create Tags Structure and import tags if needed

                //Create new Tags node under the XBlog Data node
                TemplateItem blogTagsTemplate = master.GetItem(XBlogTags.XBlogTagsTemplateId);
                XBlogTags xbTags = xdata.InnerItem.Add("Tags", blogTagsTemplate).CreateAs<XBlogTags>();

                //Links tag Parent in settings
                using (new EditContext(blogSettings.InnerItem))
                {
                    blogSettings.InnerItem.SetValue(BlogSettings.TagFolderFieldId, xbTags.InnerItem.ID);
                }

                //Import tags
                if (hasImportFile && !string.IsNullOrEmpty(fileLocation))
                {
                    List<WPTag> tags = ImportManager.ImportTags(fileLocation);
                    if (tags.Any())
                        CreateImportedTagsInSitecore(master, tags, xbTags);
                }


                #endregion

                #region Create Categories Structure and import categories if needed

                //Create new Categories node under the XBlog Data node
                TemplateItem blogCategoriesTemplate = master.GetItem(XBlogCategories.XBlogCategoriesTemplateId);
                XBlogCategories xbCategories = xdata.InnerItem.Add("Categories", blogCategoriesTemplate).CreateAs<XBlogCategories>();

                //Links category Parent in settings
                using (new EditContext(blogSettings.InnerItem))
                {
                    blogSettings.InnerItem.SetValue(BlogSettings.CategoryFolderFieldId, xbCategories.InnerItem.ID);
                }

                //Import categories
                if (hasImportFile && !string.IsNullOrEmpty(fileLocation))
                {
                    List<WPCategory> categories = ImportManager.ImportCategories(fileLocation);
                    if (categories.Any())
                        CreateImportedCategoriesInSitecore(master, categories, xbCategories);
                }

                #endregion

                #region Create Author Home, Blog Home and import blog posts if needed
                //Use the template created based on the base template - containes correct insert options
                TemplateItem blogHomeTemplate = master.GetItem(userCreatedBlogBaseTemplate.ID);//(BlogHome.BlogPostParentTemplateId);
                TemplateItem authorTemplate = master.GetItem(userCreatedAuthorTemplate.ID);

                //TODO give the user the option of where the Blog Home should be created
                Item blogPostParentItem = master.GetItem(blogHomePath);
                Item bh = blogPostParentItem.Add(blogName, blogHomeTemplate);
                Item authorPage = blogPostParentItem.Add("Authors of " + blogName, authorTemplate);

                if (currentBlogType == BlogType.Tech.MVC)
                {
                    //Add the datasource to the AuthorView
                    var authorViewRenderingRef = authorPage.Visualization.GetRenderings(Context.Device, true).FirstOrDefault();
                    if (authorViewRenderingRef != null)
                    {
                        authorPage.SetRenderingDatasource(new ID(authorViewRenderingRef.UniqueId).Guid, bh.Paths.FullPath);
                    }
                }

                //Links author Parent in settings
                using (new EditContext(blogSettings.InnerItem))
                {
                    blogSettings.InnerItem.SetValue(BlogSettings.AuthorViewPageFieldId, authorPage.ID);
                }

                using (new EditContext(bh))
                {
                    bh.SetValue(BlogHome.BlogSettingsFieldId, blogSettings.InnerItem.ID);
                    bh.SetString(BlogHome.BlogNameFieldId, blogName);
                }

                if (hasImportFile)
                {
                    //IMPORTS BLOG POSTS
                    List<WPPost> listWordpressPosts = ImportManager.Import(fileLocation, false, false, false);
                    if (listWordpressPosts.Any())
                        ImportManager.ImportPosts(bh, listWordpressPosts.ToList(), blogName, userBlogPostTemplate);
                }

                return true;

                #endregion
            }
        }

        private static void CreateImportedTagsInSitecore(Database master, List<WPTag> tags, XBlogTags xbTags)
        {
            TemplateItem blogTagTemplate = master.GetItem(Tag.TagTemplateId);
            foreach (WPTag wpt in tags)
            {
                try
                {
                    string tagName = ItemUtil.ProposeValidItemName(wpt.TagName);

                    Tag t = xbTags.InnerItem.Add(tagName, blogTagTemplate).CreateAs<Tag>();

                    using (new EditContext(t.InnerItem))
                    {
                        t.InnerItem.SetString(Tag.TagNameFieldId, wpt.TagName);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("failed to create tag: " + wpt.TagName + ex.Message, "XB Creator");
                }
            }
        
        }

        private static void CreateImportedCategoriesInSitecore(Database master, List<WPCategory> categories, XBlogCategories xbCategory)
        {
            TemplateItem blogCategoryTemplate = master.GetItem(Category.CategoryTemplateId);
            foreach (WPCategory wpc in categories)
            {
                try
                {
                    string categoryName = ItemUtil.ProposeValidItemName(wpc.CategoryName);

                    Category c = xbCategory.InnerItem.Add(categoryName, blogCategoryTemplate).CreateAs<Category>();

                    using (new EditContext(c.InnerItem))
                    {
                        c.InnerItem.SetString(Category.CategoryNameFieldId, wpc.CategoryName);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("failed to create categor: " + wpc.CategoryName + ex.Message, "XB Creator");
                }
            }

        }

        private static void CreateImportedAuthorsInSitecore(Database master, List<WPAuthor> authors, XBlogAuthors xbAuthors)
        {
            TemplateItem blogAuthorTemplate = master.GetItem(Author.AuthorTemplateId);
            foreach (WPAuthor wpa in authors)
            {
                try
                {
                    string authorsName = ItemUtil.ProposeValidItemName(wpa.Name);

                    Author a = xbAuthors.InnerItem.Add(authorsName, blogAuthorTemplate).CreateAs<Author>();

                    using (new EditContext(a.InnerItem))
                    {
                        a.InnerItem.SetString(Author.AuthorFullNameFieldId, wpa.Name.Replace(".", " "));
                        a.InnerItem.SetString(Author.AuthorEmailAddressFieldId, wpa.Email);
                        a.InnerItem.SetString(Author.CreatorFieldId, wpa.Creator);
                    }
                }
                catch (Exception ex)
                {
                    Log.Error("failed to create author: " + wpa.Name + ex.Message, "XB Creator");
                }
            }
        }
    }
}
