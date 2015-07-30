using Sitecore.Data.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Sitecore.Web.UI.WebControls;
using XBlogHelper.Search;
using XBlogHelper;
using XBlogHelper.ItemMapper;
using XBlogHelper.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Links;
using Sitecore.Resources.Media;


namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class AuthorViewList : BaseSublayout
    {
        public BlogSettings settingsItem;
        protected Dictionary<string, int> authorCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            authorCount = AuthorManager.GetAuthorCount(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            // set title
            frTitle.FieldName = BlogSettings.AuthorViewListTitleFieldId;
            frTitle.Item = settingsItem.InnerItem;

            //Get search results
            IEnumerable<Author> authors = null;
            if (settingsItem.OrderAuthorOnCount)
            {

                authors = AuthorManager.GetAuthorsOrderedByCount(authorCount);
            }
            else
            {
                authors = AuthorManager.GetAuthors(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            }

            // Set max display
            authors = AuthorManager.SetAuthorDisplayLimit(settingsItem.AuthorListMaxAuthorsToDisplay, authors);

            if (authors == null || !authors.Any())
                return;

            // Bind data source
            lvAuthorList.DataSource = authors;
            lvAuthorList.DataBind();

            if (settingsItem.AuthorViewPage != null && settingsItem.AuthorViewAllLinkText != "")
            {
                ltlAuthorViewAllLink.Text = String.Format("<a href=\"{0}\" class=\"authorviewlink\">{1}</a><br class=\"clear\" />", LinkManager.GetItemUrl(settingsItem.AuthorViewPage), settingsItem.AuthorViewAllLinkText);
            }
        }


        protected void lvAuthorList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;

            Author author = e.Item.DataItem as Author;
            if (author == null)
                return;

            HyperLink hlAuthor = e.Item.FindControl("hlAuthor") as HyperLink;
            if (hlAuthor != null)
            {
                string itemID = author.ItemId.ToString();
                string countDisplay = "";
                if (authorCount.ContainsKey(itemID) && settingsItem.DisplayCountOnTagList)
                {
                    countDisplay = String.Format(" ({0})", authorCount[itemID].ToString());
                }

                hlAuthor.Text = author.FullName + countDisplay;
                hlAuthor.NavigateUrl = AuthorManager.GetAuthorViewUrl(author.FullName, settingsItem);

                Literal litProfileImage = e.Item.FindControl("litProfileImage") as Literal;

                if (litProfileImage != null && author.ProfileImage != null && author.ProfileImage.MediaItem != null && settingsItem.AuthorListDisplayImage)
                {
                    litProfileImage.Text = String.Format("<img src=\"{0}?mw={1}\" border=\"0\" />", MediaManager.GetMediaUrl(author.ProfileImage.MediaItem), settingsItem.AuthorListImageMaxWidth);
                }
            }
        }
    }
}