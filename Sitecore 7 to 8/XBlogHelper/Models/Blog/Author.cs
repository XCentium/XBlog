using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Fields;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
    [SitecoreItemTemplate(AuthorTemplateId)]
    public class Author : SitecoreItem
    {
            public const string AuthorTemplateId = "{F3FDFC91-C3D6-480D-83EB-74333EC2DE1C}";
            public const string AuthorFullNameFieldId = "{AE1B3CA1-21B0-4D6A-9327-17D5571335DF}";
            public const string AuthorTitleFieldId = "{91843C0E-FB61-494C-B949-3A8DABAD20C1}";
            public const string AuthorLocationFieldId = "{3138C2A2-0370-430D-986A-255454AFAC61}";
            public const string AuthorBioFieldId = "{F84E5DD5-3919-46EA-80F2-CFA105888C68}";
            public const string AuthorProfileImageFieldId = "{247F2F57-7523-4EDD-A340-AB03C1BEEBF1}";
            public const string AuthorFullNameField = "Full Name";
            public const string AuthorEmailAddressFieldId = "{FE4CF004-52B4-46E4-93A3-99DBA835337A}";
            public const string AuthorTitleField = "Title";
            public const string AuthorLocationField = "Location";
            public const string AuthorBioField = "Bio";
            public const string AuthorProfileImageField = "Profile Image";
            public const string CreatorFieldId = "{5C6C7A6F-2DC1-47A2-A28B-C224DF0CC0F2}";  //this is used for import only

            [SitecoreItemField(AuthorFullNameFieldId)]
            public virtual string FullName { get; set; }
            [SitecoreItemField(AuthorEmailAddressFieldId)]
            public virtual string EmailAddress { get; set; }

            [SitecoreItemField(AuthorTitleFieldId)]
            public virtual string Title { get; set; }

            [SitecoreItemField(AuthorLocationFieldId)]
            public virtual string Location { get; set; }

            [SitecoreItemField(AuthorBioFieldId)]
            public virtual string Bio { get; set; }

            [SitecoreItemField(AuthorProfileImageFieldId)]
            public virtual ImageField ProfileImage { get; set; }

            [SitecoreItemField(CreatorFieldId)]
            public virtual string Creator { get; set; }
    }

}
