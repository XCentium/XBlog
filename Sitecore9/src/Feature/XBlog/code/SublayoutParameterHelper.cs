//-------------------------------------------------------------------------------------------------
// <copyright file="SublayoutParameterHelper.cs" company="Sitecore Shared Source">
// Copyright (c) Sitecore.  All rights reserved.
// </copyright>
// <summary>Defines the SublayoutParameterHelper type.</summary>
// <license>
// http://sdn.sitecore.net/Resources/Shared%20Source/Shared%20Source%20License.aspx
// </license>
// <url>http://trac.sitecore.net/SublayoutParameterHelper/</url>
//-------------------------------------------------------------------------------------------------

namespace Sitecore.Feature.XBlog
{
    using System;
    using System.Collections.Specialized;
    using Data;
    using Data.Items;
    using Web.UI.WebControls;
    using Reflection;
    using Web;

    /// <summary>
    /// Helper class to parse parameters and data source passed to a sublayout.
    /// </summary>
    public class SublayoutParameterHelper
    {
        #region Private members
        /// <summary>
        /// Sublayout data source item.
        /// </summary>
        private Item dataSourceItem = null;
        #endregion
        #region Public constructors
        /// <summary>
        /// Initializes a new instance of the SublayoutParameterHelper class. Parses
        /// parameters and applies properties as directed.
        /// </summary>
        /// <param name="control">Pass "this" from the user control</param>
        /// <param name="applyProperties">Set user control properties corresponding to
        /// parameter names to parameter values.</param>
        public SublayoutParameterHelper(System.Web.UI.UserControl control, bool applyProperties)
        {
            this.BindingControl = control.Parent as Sublayout;
            // Parse parameters passed to the sc:sublayout control.
            if (this.BindingControl != null)
            {
                this.Parameters = WebUtil.ParseUrlParameters(this.BindingControl.Parameters);
                if (applyProperties)
                {
                    this.ApplyProperties(control);
                }
            }

            Placeholder placeholder = WebUtil.FindAncestorOfType(control, typeof(Placeholder), false) as Placeholder;
            if (placeholder != null)
            {
                this.PlaceholderParameters = WebUtil.ParseUrlParameters(placeholder.Parameters);
            }

        }
        #endregion
        #region Public properties
        /// <summary>
        /// Sets the path to the Sitecore data source item.
        /// </summary>
        public string DataSource
        {
            set
            {
                this.DataSourceItem = Context.Database.GetItem(value);
            }
        }
        /// <summary>
        /// Gets or sets the data source item.
        /// </summary>
        public Item DataSourceItem
        {
            get
            {
                // If the data source has not been set
                // and this code can access properties of the binding control
                if (this.dataSourceItem == null)
                {
                    if (this.BindingControl == null
                        || String.IsNullOrEmpty(this.BindingControl.DataSource))
                    {
                        this.dataSourceItem = Context.Item;
                    }
                    else if (Context.Database != null)
                    {
                        this.dataSourceItem =
                          Context.Database.GetItem(this.BindingControl.DataSource);
                        // when the database is not published this might still be null so a final check
                        if (this.dataSourceItem == null)
                            this.dataSourceItem = Context.Item;
                    }
                }
                return this.dataSourceItem;
            }
            set
            {
                this.dataSourceItem = value;
            }
        }
        #endregion
        #region Protected properties
        /// <summary>
        /// Gets or sets the parameters passed to the sublayout.
        /// </summary>
        protected NameValueCollection Parameters
        {
            get;
            set;
        }


        protected NameValueCollection PlaceholderParameters
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the sublayout control used when user control binds to Sitecore
        /// placeholder,  or using sc:sublayout control. Otherwiwse Null (when user control
        /// is bound using ASP.NET).
        /// </summary>
        protected Sublayout BindingControl
        {
            get;
            set;
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Return the value of a specific parameter.
        /// </summary>
        /// <param name="key">Parameter name.</param>
        /// <returns>Value of specified parameter.</returns>
        public string GetParameter(string key)
        {
            if (this.Parameters == null)
            {
                return String.Empty;
            }
            string result = this.Parameters[key];
            if (String.IsNullOrEmpty(result))
            {
                return String.Empty;
            }
            return result;
        }
        /// <summary>
        /// return the value of a specific parameter
        /// </summary>
        /// <param name="parameterItemId">sitecore item id of the parameter</param>
        /// <returns></returns>
        public string GetParameterById(ID parameterItemId)
        {
            string key = string.Empty;
            Item parameterItem = Context.Database.GetItem(parameterItemId);
            if (parameterItem != null)
                key = parameterItem.Name;
            return GetParameter(key);
        }

        /// <summary>
        /// Return the value of a specific placeholder parameter.
        /// </summary>
        /// <param name="key">Parameter name.</param>
        /// <returns>Value of specified parameter.</returns>
        public string GetPlaceholderParameter(string key)
        {
            if (this.PlaceholderParameters == null)
            {
                return String.Empty;
            }
            string result = this.PlaceholderParameters[key];
            if (String.IsNullOrEmpty(result))
            {
                return String.Empty;
            }
            return result;
        }
        #endregion
        #region Protected methods
        /// <summary>
        /// Apply parameters passed to the sublayout as properties of the user control.
        /// </summary>
        /// <param name="control">The user control.</param>
        protected void ApplyProperties(System.Web.UI.UserControl control)
        {
            foreach (string key in this.Parameters.Keys)
            {
                ReflectionUtil.SetProperty(
                  control,
                  key,
                  this.Parameters[key]);
            }
            if (String.IsNullOrEmpty(this.BindingControl.DataSource))
            {
                return;
            }
            ReflectionUtil.SetProperty(
              control,
              "datasource",
              this.DataSourceItem.Paths.FullPath);
            ReflectionUtil.SetProperty(
              control,
              "datasourceitem",
              this.DataSourceItem);
        }
        #endregion
    }
}
