using System;

namespace Sitecore.Feature.XBlog.ItemMapper.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SitecoreItemAttribute : Attribute
    {
    }
}
