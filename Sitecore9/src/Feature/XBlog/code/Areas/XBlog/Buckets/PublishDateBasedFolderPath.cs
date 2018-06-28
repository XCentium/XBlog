using Sitecore;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Buckets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Data;
using Sitecore.Buckets.Util;

namespace Sitecore.Feature.XBlog.Areas.XBlog.Buckets
{
    public class PublishDateBasedFolderPath :IDynamicBucketFolderPath
    {
        private Database _contentDatabase;
        protected Database ContentDatabase
        {
            get
            {
                if (_contentDatabase == null)
                {
                    _contentDatabase = Context.ContentDatabase ?? Configuration.Factory.GetDatabase("master");
                    
                }
                return _contentDatabase;
            }
        }

        protected DateTime EnsureAndGetDate(Item item, string dateFieldName, DateTime defaultDateTime)
        {
            DateField dateField = item.Fields[dateFieldName];
            if (dateField == null)
                return defaultDateTime;
            DateTime result = dateField.DateTime;
            if (!string.IsNullOrEmpty(dateField.InnerField.Value))
            {
                return result;
            }
            using (new EditContext(item))
            {
                dateField.Value = DateUtil.ToIsoDate(defaultDateTime);
                return DateUtil.IsoDateToDateTime(dateField.InnerField.Value);
            }
        }

        public string GetFolderPath(Database ContentDatabase, string name, ID templateId, ID newItemId, ID parentItemId, DateTime creationDateOfNewItem)
        {
            Item newItem = this.ContentDatabase != null ? this.ContentDatabase.GetItem(newItemId) : null;
            if (newItem != null)
            {
                string fieldName = BucketFolderConfigurationManager.GetDateFieldName(newItem.Template);
                if (!string.IsNullOrEmpty(fieldName))
                {
                    DateTime creationDateTime = EnsureAndGetDate(newItem, fieldName, creationDateOfNewItem);
                    return creationDateTime.ToString(BucketConfigurationSettings.BucketFolderPath, Context.Culture);
                }
                
            }

            return creationDateOfNewItem.ToString(BucketConfigurationSettings.BucketFolderPath, Context.Culture);
        }
    }
}
