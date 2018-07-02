using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Items.Blog
{
    [SitecoreItemTemplate(CategoryTemplateId)]
    public class Category : SitecoreItem
    {
        public const string CategoryTemplateId = "{A1D40A70-21BE-440B-A828-257CB81A985F}";
        public const string CategoryNameFieldId = "{0A0ACC0F-21B2-40AC-A934-E5B38E632EA0}";
        public const string CategoryNameField = "Category Name";


        [SitecoreItemField(CategoryNameFieldId)]
        public virtual string CategoryName { get; set; }
    }
}
