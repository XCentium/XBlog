using Sitecore.Buckets.Managers;
using Sitecore.Data.Items;
using Sitecore.Diagnostics;
using Sitecore.Events;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Sitecore.Buckets.Extensions;

namespace XBlogHelper.Buckets.EventHandlers
{
    public class ItemSaved
    {
        public void Process(object sender, EventArgs args)
        {
            Assert.ArgumentNotNull(sender, "sender");
            Assert.ArgumentNotNull(args, "args");

            Item savedItem = Event.ExtractParameter(args, 0) as Item;
            Assert.IsNotNull(savedItem, "saved item can not be null");
            if (savedItem == null)
            {
                Log.Error("XBlog error creating item", this);
                return;
            }
            if (!savedItem.Database.Name.Equals("master", StringComparison.InvariantCultureIgnoreCase))
                return;
            if (!BucketManager.IsItemContainedWithinBucket(savedItem))
            {
                Log.Debug("Item {0}  is not contained in a bucket", savedItem);
                return;
            }
            Item bucketItem = savedItem.GetParentBucketItemOrParent();
            if (!BucketManager.IsBucket(bucketItem))
                return;

            BucketManager.MoveItemIntoBucket(savedItem, bucketItem);

        }
    }
}
