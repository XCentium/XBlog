using System;
using Sitecore.Links;

namespace Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SitecoreItemPropertyAttribute : Attribute
    {
        public SitecoreItemPropertyAttribute(SitecoreItemInfo itemInfoType)
        {
            ItemInfoType = itemInfoType;
        }


        public SitecoreItemInfo ItemInfoType { get; set; }
    }

    /// <summary>
    /// Id:             use it for Item ID, set the property type to Sitecore.Data.ID
    /// Path:           use it for item path, set the property type to System.String
    /// DisplayName:    use it for item's display name, set the property type to System.String
    /// FullPath:       use this for item's full path, set the property type to System.String
    /// TemplateId:     use it for Item's Template ID, set the property type to Sitecore.Data.ID
    /// TemplateName:   use it for Item's template name, set the property type to System.String
    /// Version:        use it for item version, set the property type to Sitecore.Data.Version
    /// Name:           use it for item's name, set the property type to System.String
    /// Language:       use it for item's name, set the property type to Sitecore.Globalization.Language
    /// HasLayout:      boolean value to determine if an item has layout, set the property type to Susyem.Boolean
    /// Uri:            use it for item's Uri. set the the property type to Sitecore.Data.ItemUri
    /// Url:            use it to get item url, set the property type to System.String. Uses LinkManager.GetDefaultUrlOptions()
    /// </summary>
    public enum SitecoreItemInfo
    {
        Id, Path, DisplayName, FullPath, TemplateId, TemplateName, Version, Name, Language, HasLayout, Uri, Url
    }
}
