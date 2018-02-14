using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;


namespace Sitecore.Feature.XBlog.Areas.XBlog.Items.Blog
{
    [SitecoreItemTemplate(TagTemplateId)]
    public class Tag : SitecoreItem
    {
        public const string TagTemplateId = "{8B11616F-F9F1-4DC0-BF8A-80D6026E8AD3}";
        public const string TagNameFieldId = "{6957AE02-3C36-46B9-8E6B-F4A279A63B13}";
        //public const string TagNameField = "Tag Name";
        public const string TagNameField = "tag_name_t";


        [SitecoreItemField(TagNameFieldId)]
        public virtual string TagName { get; set; }
    }
}
