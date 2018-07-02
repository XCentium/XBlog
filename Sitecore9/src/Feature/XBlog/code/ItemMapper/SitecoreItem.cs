using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Globalization;
using Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes;

namespace Sitecore.Feature.XBlog.ItemMapper
{
    [SitecoreItemTemplate(StandardTemplateId)]
    public class SitecoreItem
    {
        [SitecoreItemProperty(SitecoreItemInfo.Id)]
        public virtual ID ItemId { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Path)]
        public virtual string Path { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.DisplayName)]
        public virtual string DisplayName { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.FullPath)]
        public virtual string FullPath { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.TemplateId)]
        public virtual ID TemplateId { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.TemplateName)]
        public virtual string TemplateName { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Version)]
        public virtual Version Version { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Name)]
        public virtual string Name { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Language)]
        public virtual Language Language { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.HasLayout)]
        public virtual bool HasLayout { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Uri)]
        public virtual ItemUri Uri { get; set; }

        [SitecoreItemProperty(SitecoreItemInfo.Url)]
        public virtual string Url { get; set; }

        [SitecoreItem]
        public virtual Item InnerItem { get; set; }

        #region constants

        public const string StandardTemplateId = "{1930BBEB-7805-471A-A3BE-4858AC7CF696}";

        #endregion
    }
}
