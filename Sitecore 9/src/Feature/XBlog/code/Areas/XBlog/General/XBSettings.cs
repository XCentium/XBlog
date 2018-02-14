using System;
using System.Collections.Generic;
using Sitecore.Configuration;
using Sitecore.Text;

namespace Sitecore.Feature.XBlog.Areas.XBlog.General
{
    public sealed class XBSettings
    {
        /// <summary>
        /// url pattern for blogs by categoryName
        /// e.g. blog/categoryName/government
        /// </summary>
        public static string XBCategoryUrlPattern
        {
            get
            {
                return Settings.GetSetting("XBCategoryUrlPattern", "{0}/category/{1}");
            }
        }
        public static string XBAuthorUrlPattern
        {
            get
            {
                return Settings.GetSetting("XBAuthorUrlPattern", "{0}/author/{1}");
            }
        }
        public static string XBAuthorViewUrlPattern
        {
            get
            {
                return Settings.GetSetting("XBAuthorViewUrlPattern", "{0}/authorview/{1}");
            }
        }
        public static string XBTagsUrlPattern
        {
            get
            {
                return Settings.GetSetting("XBTagsUrlPattern", "{0}/tag/{1}");
            }
        }
        public static string XBSearchURLPattern
        {
            get
            {
                return Settings.GetSetting("XBSearchURLPattern", "{0}/searchterm/{1}");
            }
        }

        /// <summary>
        /// query string values
        /// e.g. ?category=something
        /// </summary>
        public static string XBCategoryQS
        {
            get
            {
                return Settings.GetSetting("XBCategoryQS", "category");
            }
        }
        public static string XBAuthorQS
        {
            get
            {
                return Settings.GetSetting("XBAuthorQS", "author");
            }
        }
        public static string XBAuthorViewQS
        {
            get
            {
                return Settings.GetSetting("XBAuthorViewQS", "authorview");
            }
        }
        public static string XBTagQS
        {
            get
            {
                return Settings.GetSetting("XBTagQS", "tag");
            }
        }
        public static string XBPageQS
        {
            get
            {
                return Settings.GetSetting("XBPageQS", "page");
            }
        }
        public static string XBSearchQS
        {
            get
            {
                return Settings.GetSetting("XBSearchQS", "searchterm");
            }
        }

        /// <summary>
        /// Computed fields
        /// 
        /// </summary>
        public static string XBParsedCategoryName
        {
            get
            {
                return Settings.GetSetting("XBParsedCategoryName", "parsedxblogcategoryname");
            }
        }
        public static string XBParsedAuthorName
        {
            get
            {
                return Settings.GetSetting("XBParsedAuthorName", "parsedxblogauthorname");
            }
        }
        public static string XBParsedTagName
        {
            get
            {
                return Settings.GetSetting("XBParsedTagName", "parsedxblogtagname");
            }
        }

        /// <summary>
        /// Specific Search index fields
        /// 
        /// </summary>
        public static string XBSearchPublishDate
        {
            get
            {
                return Settings.GetSetting("XBSearchPublishDate", "publish_date");
            }
        }

        public static string XBCommentSubmissionDate
        {
            get
            {
                return Settings.GetSetting("XBCommentSubmissionDate", "submission_date");
            }
        }

        /// <summary>
        /// Tag Cloud Range
        /// Percent as number, number is equal to or greater than
        /// Weight One should be the highest number while weight five is lowest
        /// If Weight five is not 0, then anything below its range will not be included in cloud
        /// </summary>
        public static int XBTagCloudWeightOne
        {
            get
            {
                string settingVal = Settings.GetSetting("XBTagCloudWeightOne", "99");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 99;

                return number;
            }
        }
        public static int XBTagCloudWeightTwo
        {
            get
            {
                string settingVal = Settings.GetSetting("XBTagCloudWeightTwo", "70");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 70;

                return number;
            }
        }
        public static int XBTagCloudWeightThree
        {
            get
            {
                string settingVal = Settings.GetSetting("XBTagCloudWeightThree", "40");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 40;

                return number;
            }
        }
        public static int XBTagCloudWeightFour
        {
            get
            {
                string settingVal = Settings.GetSetting("XBTagCloudWeightFour", "20");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 20;

                return number;
            }
        }
        public static int XBTagCloudWeightFive
        {
            get
            {
                string settingVal = Settings.GetSetting("XBTagCloudWeightFive", "3");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 3;

                return number;
            }
        }


        /// <summary>
        /// Tag Cloud Range Class
        /// Class names for each weight
        /// </summary>
        public static string XBTagCloudClassOne
        {
            get
            {
                return Settings.GetSetting("XBTagCloudClassOne", "weight1");
            }
        }
        public static string XBTagCloudClassTwo
        {
            get
            {
                return Settings.GetSetting("XBTagCloudClassTwo", "weight2");
            }
        }
        public static string XBTagCloudClassThree
        {
            get
            {
                return Settings.GetSetting("XBTagCloudClassThree", "weight3");
            }
        }
        public static string XBTagCloudClassFour
        {
            get
            {
                return Settings.GetSetting("XBTagCloudClassFour", "weight4");
            }
        }
        public static string XBTagCloudClassFive
        {
            get
            {
                return Settings.GetSetting("XBTagCloudClassFive", "weight5");
            }
        }

        /// <summary>
        /// Related Blog
        /// Weight value of each.
        /// For example if two blogs have the same 3 categories
        /// and RelatedCategoryValue = 2, then categories weight at a value of 6 (3 x 2)
        /// </summary>
        public static int RelatedCategoryValue
        {
            get
            {
                string settingVal = Settings.GetSetting("RelatedCategoryValue", "2");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 2;

                return number;
            }
        }

        public static int RelatedAuthorValue
        {
            get
            {
                string settingVal = Settings.GetSetting("RelatedAuthorValue", "1");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 1;

                return number;
            }
        }

        public static int RelatedTagValue
        {
            get
            {
                string settingVal = Settings.GetSetting("RelatedTagValue", "1");
                int number;
                bool result = Int32.TryParse(settingVal, out number);
                if (!result)
                    number = 1;

                return number;
            }
        }

        /// <summary>
        /// 
        /// Paths
        /// 
        /// </summary>
        public static string XBPageTemplatePath
        {
            get
            {
                return Settings.GetSetting("XBPageTemplatePath", "/sitecore/templates/Feature/XBlog/Page");
            }
        }
    }
}
