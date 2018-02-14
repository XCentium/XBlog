using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;
using Sitecore.Feature.XBlog.Search;
using Sitecore.Feature.XBlog;
//using Sitecore.Feature.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Links;

namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class TagList : BaseSublayout
    {
        public BlogSettings settingsItem;
        protected Dictionary<string, int> tagCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsItem = Sitecore.Feature.XBlog.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            tagCount = TagManager.GetTagCloud(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            // set title
            frTitle.FieldName = BlogSettings.TagListTitleFieldId;
            frTitle.Item = settingsItem.InnerItem;

            //Get search results
            IEnumerable<Tag> tags = null;
            if (settingsItem.OrderTagOnCount)
            {

                tags = TagManager.GetTagsOrderedByCount(tagCount);
            }
            else
            {
                tags = TagManager.GetTags(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            }

            if (tags == null || !tags.Any())
                return;

            // Bind data source
            lvTagList.DataSource = tags;
            lvTagList.DataBind();
        }


        protected void lvTagList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;

            Tag tag = e.Item.DataItem as Tag;
            if (tag == null)
                return;

            HyperLink hltag = e.Item.FindControl("hltag") as HyperLink;
            if (hltag != null)
            {
                string itemID = tag.ItemId.ToString();
                string countDisplay = "";
                if (tagCount.ContainsKey(itemID) && settingsItem.DisplayCountOnTagList)
                {
                    countDisplay = String.Format(" ({0})", tagCount[itemID].ToString());
                }

                hltag.Text = tag.TagName + countDisplay;
                hltag.NavigateUrl = TagManager.GetTagUrl(tag.TagName, DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            }
        }
    }
}