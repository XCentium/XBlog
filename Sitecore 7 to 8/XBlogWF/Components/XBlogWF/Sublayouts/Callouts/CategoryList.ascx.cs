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
//using XBlogHelper.ItemMapper;
using XBlogHelper.Models.Blog;
using System.Collections;
using Sitecore.Data;
using Sitecore.Links;


namespace XBlogWF.Components.XBlogWF.Sublayouts.Callouts
{
    public partial class CategoryList : BaseSublayout
    {
        public BlogSettings settingsItem;
        protected Dictionary<string, int> categoryCount;
        protected void Page_Load(object sender, EventArgs e)
        {
            settingsItem = XBlogHelper.General.DataManager.GetBlogSettingsItem(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            categoryCount = CategoryManager.GetCategoryCount(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);

            // set title
            frTitle.FieldName = BlogSettings.CategoryListTitleFieldId;
            frTitle.Item = settingsItem.InnerItem;

            //Get search results
            IEnumerable<Category> categories = null;
            if (settingsItem.OrderCategoryOnCount)
            {

                categories = CategoryManager.GetCategoriesOrderedByCount(categoryCount);
            }
            else
            {
                categories = CategoryManager.GetCategories(DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            }

            if (categories == null || !categories.Any())
                return;

            // Bind data source
            lvCategoryList.DataSource = categories;
            lvCategoryList.DataBind();
        }


        protected void lvCategoryList_ItemDataBound(object sender, ListViewItemEventArgs e)
        {
            if (e.Item.ItemType != ListViewItemType.DataItem)
                return;

            Category category = e.Item.DataItem as Category;
            if (category == null)
                return;

            HyperLink hlCategory = e.Item.FindControl("hlCategory") as HyperLink;
            if (hlCategory != null)
            {
                string itemID = category.ItemId.ToString();
                string countDisplay = "";
                if (categoryCount.ContainsKey(itemID) && settingsItem.DisplayCountOnTagList)
                {
                    countDisplay = String.Format(" ({0})", categoryCount[itemID].ToString());
                }

                hlCategory.Text = category.CategoryName + countDisplay;
                hlCategory.NavigateUrl = CategoryManager.GetCategoryUrl(category.CategoryName, DataSourceItem != null ? DataSourceItem : Sitecore.Context.Item);
            }
        }
    }
}