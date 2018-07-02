using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.Items.Blog
{
    [SitecoreItemTemplate(BlogPostParentTemplateId)]
    public class BlogHome : SitecoreItem
    {
        public const string BlogPostParentTemplateId = "{69F0EB70-7F15-4356-B036-7786332AC92F}";
        public const string BlogPostParentTemplate = "Blog Home";

        public const string BlogSettingsFieldId = "{8DA365F3-8504-42D8-9407-99A48D6EC424}";

        public const string BlogNameFieldId = "{522642CF-7B22-4879-B75C-8295A849BE76}";


        [SitecoreItemField(BlogSettingsFieldId)]
        public virtual Item BlogSettings { get; set; }

        [SitecoreItemField(BlogNameFieldId)]
        public virtual string BlogName { get; set; } 
    }
}
