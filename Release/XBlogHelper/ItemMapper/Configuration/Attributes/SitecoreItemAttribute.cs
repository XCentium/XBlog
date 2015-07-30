using System;

namespace XBlogHelper.ItemMapper.Configuration.Attributes
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = true)]
    public sealed class SitecoreItemAttribute : Attribute
    {
    }
}
