using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog
{
    [SitecoreItemTemplate(BlogPostTemplateId)]
    public class BlogPost : SitecoreItem
    {
        public const string BlogPostTemplateId = "{E5697002-1F90-48EC-B378-E6461475F611}";
        public const string MVCBlogPostTemplateid = "{8FED7E54-AEC9-4432-B593-6229AAD779EC}";
        public const string WFBlogPostTemplateId = "{79E476C3-9DBB-4C1B-9A64-F7F4B4A2EC41}";
        public const string BlogPostTemplate = "Blog Post";
        public const string BlogPostTitleFieldId = "{9821DF39-A4BC-476B-BAEF-2A2922C7A8C3}";
        public const string BlogPostSummaryFieldId = "{2779A63D-DD7F-4E0F-96C0-4066F90B6A8D}";
        public const string BlogPostBodyFieldId = "{F148B76B-8BC2-4A5B-89F1-9E5E42ED7F85}";
        public const string BlogPostPublishDateFieldId = "{9608E520-EF16-49FC-BD92-75CC101E5FEC}";
        public const string BlogPostCategoryFieldId = "{0E02704D-0AFD-4102-8D99-9241A8B03C89}";
        public const string BlogPostAuthorFieldId = "{06B84F39-CF71-446A-81AA-30E76FEBE4A5}";
        public const string BlogPostTagsFieldId = "{FE842158-1C63-48AC-91CD-85BF89F473C9}";
        public const string BlogPostTitleField = "Title";
        public const string BlogPostSummaryField = "Summary";
        public const string BlogPostBodyField = "Body";
        public const string BlogPostPublishDateField = "Published Date";
        public const string BlogPostCategoryField = "Category";
        public const string BlogPostAuthorField = "Author";
        public const string BlogPostTagsField = "Tags";


        [SitecoreItemField(BlogPostTitleFieldId)]
        public virtual string Title{get; set; }

        [SitecoreItemField(BlogPostSummaryFieldId)]
        public virtual string Summary { get; set; }

        [SitecoreItemField(BlogPostBodyFieldId)]
        public virtual string Body { get; set; }

        [SitecoreItemField(BlogPostPublishDateFieldId)]
        public virtual Sitecore.Data.Fields.DateField PublishDate { get; set; }

        [SitecoreItemField(BlogPostCategoryFieldId)]
        public virtual List<Category> Categories { get; set; }

        [SitecoreItemField(BlogPostAuthorFieldId)]
        public virtual List<Author> Authors { get; set; }

        [SitecoreItemField(BlogPostTagsFieldId)]
        public virtual List<Tag> Tags { get; set; }




    }
}
