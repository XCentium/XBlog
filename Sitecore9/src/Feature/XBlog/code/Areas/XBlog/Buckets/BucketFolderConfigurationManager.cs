using Sitecore.Collections;
using Sitecore.Configuration;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Xml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using Sitecore.Feature.XBlog.Areas.XBlog.General;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Buckets
{
    public static class BucketFolderConfigurationManager
    {
        private static SafeDictionary<ID, string> _templateDateFieldCollection;

        private static SafeDictionary<ID, string> TemplateDateFieldCollection
        {
            get
            {
                return _templateDateFieldCollection;
            }
            set
            {
                Assert.ArgumentNotNull(value, "value");
                _templateDateFieldCollection = value;
            }
        }


        static BucketFolderConfigurationManager()
        {
            Initialize();
        }
        private static void Initialize()
        {

            TemplateDateFieldCollection = new SafeDictionary<ID, string>();
            Database masterDB = Context.ContentDatabase ?? Factory.GetDatabase("master");
            Assert.IsNotNull(masterDB, "content database is not defined");

            Item[] blogPostTemplates = masterDB.SelectItems("fast:/sitecore/templates/Feature//*[@@name = 'Blog Post']");

            foreach (Item blogItem in blogPostTemplates)
            {
                if (blogItem != null)
                {
                    TemplateDateFieldCollection.Add(blogItem.ID, "Publish Date");
                }
            }

        }

        public static string GetDateFieldName(TemplateItem template)
        {
            return TemplateDateFieldCollection.ContainsKey(template.ID) ? TemplateDateFieldCollection[template.ID] : null;
        }
    }
}
