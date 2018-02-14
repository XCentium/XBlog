
#region

using System;
using System.Collections.Generic;
using System.Linq;
using Sitecore.Data;
using Sitecore.Data.Items;
using Sitecore.Data.Managers;
using Sitecore.Data.Templates;
using Sitecore.Reflection;
using Sitecore.Data.Fields;
using Sitecore;
using Sitecore.Diagnostics;
using Sitecore.Text;
using Sitecore.Web.UI.WebControls;
using System.Reflection;
using Sitecore.StringExtensions;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;
using Sitecore.ContentSearch.SearchTypes;

#endregion

namespace Sitecore.Feature.XBlog.Areas.XBlog
{
    public static class ItemExtensions
    {

        private static readonly Dictionary<string, bool> _derivedTemplatesCache = new Dictionary<string, bool>();

        private static readonly Dictionary<Type, IEnumerable<ID>> _typeTemplateMappings =
          new Dictionary<Type, IEnumerable<ID>>();

        #region create
        public static T CreateAs<T>(this Item item, bool isLazyLoad = true)
        {
            return (T)TypeMapper.CreateObject(item, typeof(T), isLazyLoad);
        }
        public static IEnumerable<T> CreateAs<T>(this IEnumerable<Item> items, bool isLazyLoad = true)
        {
            return items.Where(item => item.IsValidType<T>()).Select(itm => itm.CreateAs<T>(isLazyLoad));
        }
        /// <summary>
        /// converts a list of sitecore result item to a AR model class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public static IEnumerable<T> CreateAs<T>(this IEnumerable<SearchResultItem> results) where T : SitecoreItem
        {
            return (IEnumerable<T>)results.ToItemList().CreateAs<T>();
        }

        /// <summary>
        /// converts a list of sitecore result item to a AR model class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="results"></param>
        /// <returns></returns>
        public static IEnumerable<T> ToItemList<T>(this IEnumerable<SitecoreUISearchResultItem> results) where T : SitecoreItem
        {
            return (IEnumerable<T>)results.ToItemList().CreateAs<T>();
        }
        /// <summary>
        /// converts a sitecore result item to a AR model class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultItem"></param>
        /// <returns></returns>
        public static T CreateAs<T>(this SearchResultItem resultItem) where T : SitecoreItem
        {
            Item item = resultItem.GetItem();

            return item != null ? (T)item.CreateAs<T>() : null;
        }
        /// <summary>
        /// converts a sitecore result item to a AR model class
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="resultItem"></param>
        /// <returns></returns>
        public static T CreateAs<T>(this SitecoreUISearchResultItem resultItem) where T : SitecoreItem
        {
            Item item = resultItem.GetItem();

            return item != null ? (T)item.CreateAs<T>() : null;
        }

        public static List<Item> ToItemList(this IEnumerable<SearchResultItem> results)
        {
            return results.Select(result => result.GetItem()).OfType<Item>().ToList();
        }
        public static List<Item> ToItemList(this IEnumerable<SitecoreUISearchResultItem> results)
        {
            return results.Select(result => result.GetItem()).OfType<Item>().ToList();
        }
        #endregion

        #region item test helpers

        internal static bool IsItemValidForType(Type type, Item item)
        {
            IEnumerable<ID> templateIDs = GetTemplateIDs(type);
            if (!templateIDs.Any())
            {
                throw new InvalidOperationException("Neither the type {0} and none of its base classes are decorated with the SitecoreTemplateAttribute".FormatWith(type.FullName));
            }
            return templateIDs.All(tid => item.IsDerived(tid));
        }

        public static bool IsValidType<T>(this Item item)
        {
            bool flag = IsItemValidForType(typeof(T), item);
            if (flag)
            {
                flag = item.HasContextLanguage();
            }
            return flag;
        }
        #endregion

        #region helper methods
        private static IEnumerable<ID> GetTemplateIDs(Type type)
        {
            lock (_typeTemplateMappings)
            {
                if (_typeTemplateMappings.ContainsKey(type))
                    return _typeTemplateMappings[type];
            }
            IEnumerable<SitecoreItemTemplateAttribute> attributes = type.GetCustomAttributes<SitecoreItemTemplateAttribute>();
            IEnumerable<ID> templates = attributes.Select(t => t.TemplatesID).Distinct();
            lock (_typeTemplateMappings)
            {
                if (!_typeTemplateMappings.ContainsKey(type))
                    _typeTemplateMappings.Add(type, templates);
            }
            return templates;
        }

        public static bool HasContextLanguage(this Item item)
        {
            Item latestVersion = item.Versions.GetLatestVersion();
            return ((latestVersion != null) && (latestVersion.Versions.Count > 0));
        }

        public static bool IsDerived(this Item item, ID templateId)
        {
            if (item == null)
            {
                return false;
            }
            if (templateId.IsNull)
            {
                return false;
            }
            string key = string.Format("[{0}, {1}]", item.TemplateID, templateId);
            lock (_derivedTemplatesCache)
            {
                if (_derivedTemplatesCache.ContainsKey(key))
                {
                    return _derivedTemplatesCache[key];
                }
            }
            TemplateItem item2 = item.Database.Templates[templateId];
            bool flag = false;
            if (item2 != null)
            {
                Template template = TemplateManager.GetTemplate(item);
                flag = ((template != null) && (template.ID == item2.ID)) || template.DescendsFrom(item2.ID);
            }
            lock (_derivedTemplatesCache)
            {
                if (!_derivedTemplatesCache.ContainsKey(key))
                {
                    _derivedTemplatesCache.Add(key, flag);
                }
            }
            return flag;
        }

        #endregion

        #region get methods
        public static bool GetBool(this Item item, string fieldID)
        {
            if (string.IsNullOrEmpty(item.GetString(fieldID)))
                return false;
            return MainUtil.GetBool(item[new ID(fieldID)], false);
        }
        public static IEnumerable<Item> GetItems(this Item item, string fieldID)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            ListString itemIds = new ListString(field.Value);
            if (!itemIds.Any())
                return Enumerable.Empty<Item>();
            return itemIds.Where(id => ID.IsID(id)).Select(id => item.Database.GetItem(id));
        }

        public static DateTime GetDateTime(this Item item, string fieldID)
        {
            if (string.IsNullOrEmpty(item.GetString(fieldID)))
                return DateTime.MinValue;
            DateField field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            return field != null ? field.DateTime : DateTime.MinValue;
        }
        public static double GetDouble(this Item item, string fieldID)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            double num;
            if (!string.IsNullOrEmpty(item.GetString(fieldID)) && double.TryParse(item.GetString(fieldID), out num))
                return num;
            return Double.NaN;
        }

        public static int? GetInt(this Item item, string fieldID)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            int num;
            if (!string.IsNullOrEmpty(item.GetString(fieldID)) && int.TryParse(item.GetString(fieldID), out num))
                return num;
            return int.MinValue;
        }

        public static Item GetItem(this Item item, string fieldID)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (ID.IsID(field.Value))
                return field.Database.GetItem(field.Value);
            return null;
        }
        public static T GetField<T>(this Item item, string fieldID) where T : CustomField
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            return (T)ReflectionUtil.CreateObject(typeof(T), new object[] { field });
        }

        public static string GetString(this Item item, string fieldID, bool returnRawValue = false)
        {

            if (returnRawValue || Context.IsBackgroundThread)
            {
                return StringUtil.GetString(item[fieldID], "");
            }
            else
            {
                try
                {
                    FieldRenderer fieldRenderer = new FieldRenderer();
                    fieldRenderer.DisableWebEditing = true;
                    fieldRenderer.DisableSecurity = true;
                    fieldRenderer.Item = item;
                    fieldRenderer.FieldName = fieldID;
                    return StringUtil.GetString(fieldRenderer.Render(), "");
                }
                catch (Exception ex)
                {
                    Log.Error("XBlog XCore field renderer", ex, new object());
                }
                return StringUtil.GetString(item[fieldID], "");
            }

        }

        #endregion

        #region set methods
        public static void SetBool(this Item item, string fieldID, bool? value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value.HasValue)
                field.Value = !value.GetValueOrDefault(false) ? "0" : "1";
            else
                item.SetString(fieldID, null);
        }
        public static void SetDateTime(this Item item, string fieldID, DateTime? value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            DateField field2 = FieldTypeManager.GetField(field) as DateField;
            if (value.HasValue)
                field2.Value = DateUtil.ToIsoDate(value.Value);
            else
                item.SetString(fieldID, null);
        }

        public static void SetDouble(this Item item, string fieldID, double? value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value.HasValue)
                field.Value = value.Value.ToString();
            else
                item.SetString(fieldID, null);
        }

        public static void SetField<T>(this Item item, string fieldID, T value) where T : CustomField
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value == null)
                item.SetString(fieldID, null);
            else
                field.Value = value.Value;
        }
        public static void SetInt(this Item item, string fieldID, int? value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value.HasValue)
                field.Value = value.Value.ToString();
            else
                item.SetString(fieldID, null);
        }
        public static void SetItem(this Item item, string fieldID, Item value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value == null)
                item.SetString(fieldID, null);
            else
                field.Value = value.ID.ToString();
        }

        public static void SetItems(this Item item, string fieldID, IEnumerable<Item> value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value == null)
                item.SetString(fieldID, null);
            else
                field.Value = StringUtil.Join(value.Select(itm => itm.ID.ToString()), "|");
        }
        public static void SetString(this Item item, string fieldID, string value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            if (value == null)
                field.Reset();
            else
                field.Value = value;
        }

        public static void SetValue<T>(this Item item, string fieldID, T value)
        {
            Field field = item.Fields[new ID(fieldID)];
            Assert.IsNotNull(field, "item does not have field: " + fieldID);
            field.Value = value.ToString();
        }
        #endregion
    }
}
