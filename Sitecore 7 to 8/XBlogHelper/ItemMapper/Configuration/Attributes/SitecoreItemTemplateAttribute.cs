using System;
using Sitecore.Data;

namespace XBlogHelper.ItemMapper.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)] // interfaces are currently not supported | AttributeTargets.Interface.
    public sealed class SitecoreItemTemplateAttribute : Attribute
    {
        public SitecoreItemTemplateAttribute(ID templatesID)
        {
            TemplatesID = templatesID;
        }

        public SitecoreItemTemplateAttribute(string templatesID)
        {
            TemplatesID = ID.Parse(templatesID);
        }

        public ID TemplatesID { get; set; }
    }
}
