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
using XBlogHelper.General;
//using XBlogHelper.ItemMapper;
using XBlogHelper.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Links;

namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class TextSearch : BaseSublayout
    {
        public BlogSettings settingsItem;
        public string searchUrl;
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            Item blogHome = XBlogHelper.General.DataManager.GetBlogHomeItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            UrlOptions option = new UrlOptions();
            option.AddAspxExtension = false;
            searchUrl = String.Format("{0}/{1}/", LinkManager.GetItemUrl(blogHome, option), XBSettings.XBSearchQS);
        }
    }
}