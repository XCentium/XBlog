//-------------------------------------------------------------------------------------------------
// <copyright file="SublayoutBase.cs" company="Sitecore Shared Source">
// Copyright (c) Sitecore.  All rights reserved.
// </copyright>
// <summary>Defines the SublayoutBase type.</summary>
// <license>
// http://sdn.sitecore.net/Resources/Shared%20Source/Shared%20Source%20License.aspx
// </license>
// <url>http://trac.sitecore.net/SublayoutParameterHelper/</url>
//-------------------------------------------------------------------------------------------------
using Sitecore;
using Sitecore.Data;
using Sitecore.Data.Items;

namespace Sitecore.Feature.XBlog
{
    /// <summary>
    /// Base class for sublayouts that accept parameters and expose properties.
    /// </summary>
    public class BaseSublayout : System.Web.UI.UserControl
    {
        #region Private members
        /// <summary>
        /// The data source item for the sublayout (defaults to the context item).
        /// </summary>
        private Item dataSourceItem;
        #endregion
        #region Public properties
        /// <summary>
        /// Gets or sets the data source item for the sublayout.
        /// </summary>
        public Item DataSourceItem
        {
            get
            {
                if (this.dataSourceItem == null)
                {
                    this.dataSourceItem = Sitecore.Context.Item;
                }
                return this.dataSourceItem;
            }
            set
            {
                this.dataSourceItem = value;
            }
        }
        /// <summary>
        /// Gets or sets the data source for the sublayout.
        /// Reads or updates <see cref="DataSourceItem" />.
        /// </summary>
        public string DataSource
        {
            get
            {
                if (this.DataSourceItem == null)
                {
                    return string.Empty;
                }
                return this.DataSourceItem.Paths.FullPath;
            }
            set
            {
                this.dataSourceItem = Context.Database.GetItem(value);
            }
        }
        #endregion

        #region Protected properties
        /// <summary>
        /// Gets or sets an object that helps to parse sublayout parameters.
        /// </summary>
        protected SublayoutParameterHelper Helper
        {
            get;
            set;
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Return the value of a parameter passed to the sublayout.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value of the parameter, or an empty string.</returns>
        public string GetParameter(string name)
        {
            return this.Helper.GetParameter(name);
        }
        /// <summary>
        /// return the value of a specific parameter
        /// </summary>
        /// <param name="parameterItemId">sitecore item id of the parameter</param>
        /// <returns></returns>
        public string GetParameterById(ID parameterItemId)
        {
            return this.Helper.GetParameterById(parameterItemId);
        }
        /// <summary>
        /// Return the value of a placeholder parameter passed to the sublayout.
        /// </summary>
        /// <param name="name">The name of the parameter.</param>
        /// <returns>The value of the parameter, or an empty string.</returns>
        public string GetPlaceholderParameter(string name)
        {
            return this.Helper.GetPlaceholderParameter(name);
        }
        #endregion
        #region Protected methods
        /// <summary>
        /// Applies properties including data source.
        /// </summary>
        /// <param name="e">Event arguments.</param>
        protected override void OnInit(System.EventArgs e)
        {
            this.Helper =
              new SublayoutParameterHelper(this, true);
        }
        #endregion
    }
}
