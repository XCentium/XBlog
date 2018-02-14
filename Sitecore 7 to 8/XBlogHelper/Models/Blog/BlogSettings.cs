using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data.Items;
using Sitecore.Data.Fields;
using XBlogHelper.ItemMapper;
using XBlogHelper.ItemMapper.Configuration.Attributes;

namespace XBlogHelper.Models.Blog {
    
    
    [SitecoreItemTemplate(BlogSettingsTemplateId)]
    public class BlogSettings : SitecoreItem
    {
        
        #region Members
        public const string BlogSettingsTemplateId = "{0951B1C7-0699-415F-9FF5-ECDFEAEC79D6}";
        
        public const string AuthorFolderFieldId = "{D0332649-962A-41D0-806B-89BD1CBFCF71}";
        
        public const string TagFolderFieldId = "{D8480E1E-8502-4B6A-BF41-DC6CE16DC177}";

        public const string AuthorViewPageFieldId = "{F30B7285-3051-4C39-A710-F3B4F91F0E0E}";
        
        public const string CategoryFolderFieldId = "{8C33AB62-E715-4369-A220-9CC0A241B41C}";

        public const string CommentsFolderFieldId = "{60004F43-1AEC-485B-A3E2-60F3C2742802}";

        public const string PrimaryCSSIDFieldId = "{4089F518-E690-4306-892E-FE9C4A66B018}";

        public const string CategoryListTitleFieldId = "{25ECBF1B-12C9-414C-8BFC-1BAD66AB8B5D}";

        public const string AuthorListTitleFieldId = "{658D3ED0-83C1-4806-91D2-43D2AAF784FA}";

        public const string AuthorViewListTitleFieldId = "{10FABF4C-1A36-47E2-BD7D-14A72D4D0621}";

        public const string TagListTitleFieldId = "{5EA82718-BFC0-4ECB-B4FA-D68BE20165D0}";

        public const string TagCloudTitleFieldId = "{F4E4E966-238F-4E0B-A83D-DE847387D77F}";

        public const string CategoryFilterTitleFieldId = "{C233523D-B0A1-41EF-B0C4-1E0454E67174}";

        public const string AuthorFilterTitleFieldId = "{FDFDCB3F-F511-4A15-BEF8-BF1699D43B25}";

        public const string TagFilterTitleFieldId = "{4BCB5A68-38B8-4CB3-A05D-F96F203452A1}";

        public const string BlogListingDateFormatFieldId = "{A297FFB7-3AD6-4F87-96EA-79329BD8B45D}";

        public const string BlogPostDateFormatFieldId = "{130FC162-9F0B-4925-9AAF-4ABFC17B0397}";

        public const string DisplayCategoryOnBlogPostListFieldId = "{8087935C-2576-419E-8399-5186A86F6EBD}";

        public const string DisplayTagsOnBlogPostListFieldId = "{27E627B2-B350-4D66-B5A3-A6652451ECEA}";

        public const string DisplayCategoryOnBlogPostFieldId = "{AEEAF42E-56B6-465C-9AF5-8E560BA10D33}";

        public const string DisplayTagsOnBlogPostFieldId = "{84275CB4-A3FD-4635-910F-CB7A580358F4}";

        public const string DisplayFilterMessageFieldId = "{CC970F6A-8469-47F4-8EE6-9AAC5A407E9D}";
        
        public const string DisplaySummaryLengthFieldId = "{7A1E7EAC-7496-486F-B902-1C20CBE6676D}";
        
        public const string PageSizeFieldId = "{48C7B32D-A72D-4C27-97FA-79495B322F6C}";

        public const string FirstPageTextFieldId = "{6C723234-3087-4A86-8C45-D80B27C01DB2}";

        public const string PreviousPageTextFieldId = "{DC6A883C-A149-42F4-AC3A-108823ADE29A}";

        public const string NextPageTextFieldId = "{FB3A03C3-95E7-4945-BD40-68CC6646FEA7}";

        public const string LastPageTextFieldId = "{90880FC1-0EEA-419C-AD89-DE9EE07BA976}";

        public const string DisplayCountOnCategoryListFieldId = "{5F77BBB1-C67E-4E5D-89E8-B99816399B54}";

        public const string DisplayCountOnAuthorListFieldId = "{6884F790-3E7A-4202-8E71-5FBD32B0B25E}";

        public const string DisplayCountOnTagListFieldId = "{0D47137B-B480-4B68-9130-CBFCB1F8E9CC}";

        public const string BlogListingTagTitleFieldId = "{738E6B93-F6DD-43A0-AEC0-3B6F9FE446EE}";

        public const string BlogListingCategoryTitleFieldId = "{DEF3121F-308A-496F-915B-47E1487FB76D}";

        public const string ReadMoreLinkTextFieldId = "{FA114F5B-6F95-4BEE-8BB6-16DC1FD53364}";

        public const string DisplayAuthorImageFieldId = "{A5AB89A6-C90E-4FE9-A15D-95F76847A338}";

        public const string AuthorImageMaxWidthFieldId = "{53143BF4-E165-47D0-A8F1-B4A9EDBA8DC1}";

        public const string AuthorViewAllLinkTextFieldId = "{FFFB7FAB-D7B3-4C25-BCAA-022396A6BEB3}";

        public const string OrderCategoryOnCountFieldId = "{7101555F-7727-43C4-91C1-C096AA6B66B2}";

        public const string OrderAuthorOnCountFieldId = "{DA23A58A-1BC9-4F92-8437-BDA6C598455B}";

        public const string OrderTagOnCountFieldId = "{A9D3C4A0-C587-4E9B-9A32-3D3088CB87CD}";

        public const string AuthorListDisplayImageFieldId = "{A8BEEC09-54E0-4551-AF7D-C5EF2E657871}";

        public const string AuthorListImageMaxWidthFieldId = "{CB007726-D424-4DC3-9E10-E141A94DB6D1}";

        public const string AuthorListMaxAuthorsToDisplayFieldId = "{A8E927F5-AF7F-4B7D-8040-0BE4905DB96B}";

        public const string AuthorViewImageMaxWidthFieldId = "{F3EFD02D-DB90-4A89-834E-C013275B8FA9}";

        public const string RecentBlogTitleFieldId = "{07A26628-C3CD-4063-BC5E-B9E6CD246039}";

        public const string RecentBlogMaxPostsFieldId = "{03C54150-027E-43C0-82BC-ECA3B4E13FAF}";

        public const string RelatedBlogTitleFieldId = "{D1CE9739-3890-4B02-8A88-A75D99453473}";

        public const string RelatedBlogMaxPostsFieldId = "{EEB07890-D6F8-487F-BEB4-0D04A838DA0D}";

        public const string SearchFilterTitleFieldId = "{1DC23199-E00B-4CF7-9B71-E1833577FFA5}";

        public const string SearchButtonValueFieldId = "{DF363ED4-ADD2-417A-8937-0CAF71D520A9}";

        public const string IncludeFacebookOnBlogPostFieldId = "{6E227DFB-BBD0-4AE7-8346-360DB6A7E8B3}";

        public const string IncludeLinkedinOnBlogPostFieldId = "{CEB111FB-3D94-4FAD-BDD3-1715C0459416}";

        public const string IncludeTwitterOnBlogPostFieldId = "{178C972E-105B-4A39-8B54-E39F5ABC9738}";

        public const string IncludeGooglePlusOnBlogPostFieldId = "{617A76A3-8D9A-4AE2-A8C3-C994A6B0854F}";

        public const string IncludeEmailOnBlogPostFieldId = "{535AF7AA-06A7-4401-A9C2-94125B674056}";

        public const string CommentsTitleFieldId = "{0F5B42A4-D662-494C-86F3-D985DB48F0ED}";

        public const string CommentsRequireNameFieldId = "{AE54211A-E62E-470C-84D1-0752F5C501F4}";

        public const string CommentsRequireEmailFieldId = "{5F56CD35-8925-43EE-999E-4C3B95E7067C}";

        public const string CommentsPerPageFieldId = "{D9DD2645-40EC-4271-96D4-C4B20C30B01A}";


        #endregion
        
        #region Properties
        [SitecoreItemField(AuthorFolderFieldId)] 
         public virtual Item AuthorFolder { get; set; } 

        [SitecoreItemField(TagFolderFieldId)] 
         public virtual Item TagFolder { get; set; } 

        [SitecoreItemField(CategoryFolderFieldId)] 
         public virtual Item CategoryFolder { get; set; }

        [SitecoreItemField(AuthorViewPageFieldId)]
        public virtual Item AuthorViewPage { get; set; }

        [SitecoreItemField(CommentsFolderFieldId)]
        public virtual Item CommentsFolder { get; set; } 

        [SitecoreItemField(PrimaryCSSIDFieldId)]
        public virtual string PrimaryCSSID { get; set; }

        [SitecoreItemField(CategoryListTitleFieldId)]
        public virtual string CategoryListTitle { get; set; }

        [SitecoreItemField(AuthorListTitleFieldId)]
        public virtual string AuthorListTitle { get; set; }

        [SitecoreItemField(AuthorViewListTitleFieldId)]
        public virtual string AuthorViewListTitle { get; set; }

        [SitecoreItemField(TagListTitleFieldId)]
        public virtual string TagListTitle { get; set; }

        [SitecoreItemField(TagCloudTitleFieldId)]
        public virtual string TagCloudTitle { get; set; }

        [SitecoreItemField(CategoryFilterTitleFieldId)]
        public virtual string CategoryFilterTitle { get; set; }

        [SitecoreItemField(AuthorFilterTitleFieldId)]
        public virtual string AuthorFilterTitle { get; set; }

        [SitecoreItemField(TagFilterTitleFieldId)]
        public virtual string TagFilterTitle { get; set; }

        [SitecoreItemField(BlogListingDateFormatFieldId)]
        public virtual string BlogListingDateFormat { get; set; }

        [SitecoreItemField(BlogPostDateFormatFieldId)]
        public virtual string BlogPostDateFormat { get; set; }

        [SitecoreItemField(DisplayCategoryOnBlogPostListFieldId)]
        public virtual bool DisplayCategoryOnBlogPostList { get; set; }

        [SitecoreItemField(DisplayTagsOnBlogPostListFieldId)]
        public virtual bool DisplayTagsOnBlogPostList { get; set; }

        [SitecoreItemField(DisplayCategoryOnBlogPostFieldId)]
        public virtual bool DisplayCategoryOnBlogPost { get; set; }

        [SitecoreItemField(DisplayTagsOnBlogPostFieldId)]
        public virtual bool DisplayTagsOnBlogPost { get; set; }

        [SitecoreItemField(DisplayFilterMessageFieldId)]
        public virtual bool DisplayFilterMessage { get; set; }

        [SitecoreItemField(DisplaySummaryLengthFieldId)]
        public virtual int DisplaySummaryLength { get; set; }

        [SitecoreItemField(PageSizeFieldId)]
        public virtual string PageSize { get; set; }

        [SitecoreItemField(FirstPageTextFieldId)]
        public virtual string FirstPageText { get; set; }

        [SitecoreItemField(PreviousPageTextFieldId)]
        public virtual string PreviousPageText { get; set; }
        
        [SitecoreItemField(NextPageTextFieldId)]
        public virtual string NextPageText { get; set; }
        
        [SitecoreItemField(LastPageTextFieldId)]
        public virtual string LastPageText { get; set; }

        [SitecoreItemField(DisplayCountOnCategoryListFieldId)]
        public virtual bool DisplayCountOnCategoryList { get; set; }

        [SitecoreItemField(DisplayCountOnAuthorListFieldId)]
        public virtual bool DisplayCountOnAuthorList { get; set; }

        [SitecoreItemField(DisplayCountOnTagListFieldId)]
        public virtual bool DisplayCountOnTagList { get; set; }

        [SitecoreItemField(BlogListingCategoryTitleFieldId)]
        public virtual string BlogListingCategoryTitle { get; set; }

        [SitecoreItemField(BlogListingTagTitleFieldId)]
        public virtual string BlogListingTagTitle { get; set; }

        [SitecoreItemField(ReadMoreLinkTextFieldId)]
        public virtual string ReadMoreLinkText { get; set; }

        [SitecoreItemField(DisplayAuthorImageFieldId)]
        public virtual bool DisplayAuthorImage { get; set; }

        [SitecoreItemField(AuthorImageMaxWidthFieldId)]
        public virtual string AuthorImageMaxWidth { get; set; }

        [SitecoreItemField(AuthorViewAllLinkTextFieldId)]
        public virtual string AuthorViewAllLinkText { get; set; }

        [SitecoreItemField(OrderCategoryOnCountFieldId)]
        public virtual bool OrderCategoryOnCount { get; set; }

        [SitecoreItemField(OrderAuthorOnCountFieldId)]
        public virtual bool OrderAuthorOnCount { get; set; }

        [SitecoreItemField(OrderTagOnCountFieldId)]
        public virtual bool OrderTagOnCount { get; set; }

        [SitecoreItemField(AuthorListDisplayImageFieldId)]
        public virtual bool AuthorListDisplayImage { get; set; }

        [SitecoreItemField(AuthorListImageMaxWidthFieldId)]
        public virtual string AuthorListImageMaxWidth { get; set; }

        [SitecoreItemField(AuthorListMaxAuthorsToDisplayFieldId)]
        public virtual string AuthorListMaxAuthorsToDisplay { get; set; }

        [SitecoreItemField(AuthorViewImageMaxWidthFieldId)]
        public virtual string AuthorViewImageMaxWidth { get; set; }

        [SitecoreItemField(RecentBlogTitleFieldId)]
        public virtual string RecentBlogTitle { get; set; }

        [SitecoreItemField(RecentBlogMaxPostsFieldId)]
        public virtual string RecentBlogMaxPosts { get; set; }

        [SitecoreItemField(RelatedBlogTitleFieldId)]
        public virtual string RelatedBlogTitle { get; set; }

        [SitecoreItemField(RelatedBlogMaxPostsFieldId)]
        public virtual string RelatedBlogMaxPosts { get; set; }

        [SitecoreItemField(SearchFilterTitleFieldId)]
        public virtual string SearchFilterTitle { get; set; }

        [SitecoreItemField(SearchButtonValueFieldId)]
        public virtual string SearchButtonValue { get; set; }

        [SitecoreItemField(IncludeEmailOnBlogPostFieldId)]
        public virtual bool IncludeEmailOnBlogPost { get; set; }

        [SitecoreItemField(IncludeFacebookOnBlogPostFieldId)]
        public virtual bool IncludeFacebookOnBlogPost { get; set; }

        [SitecoreItemField(IncludeGooglePlusOnBlogPostFieldId)]
        public virtual bool IncludeGooglePlusOnBlogPost { get; set; }

        [SitecoreItemField(IncludeLinkedinOnBlogPostFieldId)]
        public virtual bool IncludeLinkedinOnBlogPost { get; set; }

        [SitecoreItemField(IncludeTwitterOnBlogPostFieldId)]
        public virtual bool IncludeTwitterOnBlogPost { get; set; }

        [SitecoreItemField(CommentsTitleFieldId)]
        public virtual string CommentsTitle { get; set; }

        [SitecoreItemField(CommentsRequireEmailFieldId)]
        public virtual bool CommentsRequireEmail { get; set; }

        [SitecoreItemField(CommentsRequireNameFieldId)]
        public virtual bool CommentsRequireName { get; set; }

        [SitecoreItemField(CommentsPerPageFieldId)]
        public virtual string CommentsPerPage { get; set; }

        #endregion
        
    }
}
