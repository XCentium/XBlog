using System;
using System.Collections.Generic;
using Sitecore.Data.Fields;
using Sitecore.Data.Items;
using Sitecore.Links;
using Sitecore.Reflection;
using System.Linq;
using System.Reflection;
using Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper.Configuration.Attributes;
using Sitecore.ContentSearch;
using Sitecore.Data;
using System.Collections;

namespace Sitecore.Feature.XBlog.Areas.XBlog.ItemMapper
{
    internal class DataHandler
    {
        private enum DataTypes
        {
            String, DateTime, Int, Double, Bool, Item, ItemEnumerable, CustomField, SitecoreItem, SitecoreItemEnumerable, SitecoreItemFieldAttribute, SitecoreItemPropertyAttribute, SitecoreItemAttribute
        }

        private static readonly Dictionary<DataTypes, Type> _types = new Dictionary<DataTypes, Type>();

        static DataHandler()
        {
            _types.Add(DataTypes.String, typeof(string));
            _types.Add(DataTypes.DateTime, typeof(DateTime));
            _types.Add(DataTypes.Int, typeof(int));
            _types.Add(DataTypes.Double, typeof(double));
            _types.Add(DataTypes.Bool, typeof(bool));
            _types.Add(DataTypes.Item, typeof(Item));
            _types.Add(DataTypes.ItemEnumerable, typeof(IEnumerable<Item>));
            _types.Add(DataTypes.CustomField, typeof(CustomField));
            _types.Add(DataTypes.SitecoreItem, typeof(SitecoreItem));
            _types.Add(DataTypes.SitecoreItemEnumerable, typeof(IEnumerable<SitecoreItem>));
            _types.Add(DataTypes.SitecoreItemFieldAttribute, typeof(SitecoreItemFieldAttribute));
            _types.Add(DataTypes.SitecoreItemPropertyAttribute, typeof(SitecoreItemPropertyAttribute));
            _types.Add(DataTypes.SitecoreItemAttribute, typeof(SitecoreItemAttribute));
        }
        #region set property values
        public static void SetItemData(object target, Type targetType, Item item, bool isLazyLoad)
        {
            foreach (var property in targetType.GetProperties())
            {
                if (property == null || string.IsNullOrEmpty(property.Name) || !property.CanWrite)
                    continue;
                SitecoreItemFieldAttribute attribute = (SitecoreItemFieldAttribute)property.GetCustomAttribute(_types[DataTypes.SitecoreItemFieldAttribute]);
                if (attribute == null)
                {
                    SitecoreItemPropertyAttribute propAttribute = (SitecoreItemPropertyAttribute)property.GetCustomAttribute(_types[DataTypes.SitecoreItemPropertyAttribute]);
                    if (propAttribute == null)
                    {
                        SitecoreItemAttribute itemAttribute = (SitecoreItemAttribute)property.GetCustomAttribute(_types[DataTypes.SitecoreItemAttribute]);
                        if (itemAttribute != null)
                            property.SetValue(target, item);
                    }
                    else
                        property.SetValue(target, GetSitecoreItemPropertyValue(property.PropertyType, item, propAttribute));
                }
                else
                    property.SetValue(target, GetSitecoreItemFieldValue(property.PropertyType, item, attribute, isLazyLoad));
            }
        }

        public static void SetFieldValueFromDatabase(object target, bool isLazyLoad)
        {
            List<PropertyInfo> properties = target.GetType().GetProperties().ToList();
            PropertyInfo uniqueIdProperty = properties.FirstOrDefault(property =>
            {
                IndexFieldAttribute attribute = property.GetCustomAttributes<IndexFieldAttribute>().FirstOrDefault(attr => attr.IndexFieldName == BuiltinFields.UniqueId);
                return attribute != null;
            });
            if (uniqueIdProperty != null)
            {
                object uniqueIdValue = uniqueIdProperty.GetValue(target);
                if (uniqueIdValue.GetType() == typeof(ItemUri))
                {
                    ItemUri uri = (ItemUri)uniqueIdValue;
                    Item contextItem = null;
                    if (uri != null && (contextItem = Database.GetItem(uri)) != null)
                    {
                        properties.ForEach(property =>
                        {
                            if (property != null && property.GetValue(target) == null && !string.IsNullOrEmpty(property.Name) && property.CustomAttributes.Any() && property.CanWrite)
                            {
                                SitecoreItemFieldAttribute attribute = property.GetCustomAttribute<SitecoreItemFieldAttribute>();
                                SitecoreItemPropertyAttribute propAttribute = property.GetCustomAttribute<SitecoreItemPropertyAttribute>();
                                SitecoreItemAttribute itemAttribute = property.GetCustomAttribute<SitecoreItemAttribute>();
                                if (attribute != null)
                                    property.SetValue(target, GetSitecoreItemFieldValue(property.PropertyType, contextItem, attribute, isLazyLoad));
                                else if (propAttribute != null)
                                    property.SetValue(target, GetSitecoreItemPropertyValue(property.PropertyType, contextItem, propAttribute));
                                else if (itemAttribute != null)
                                    property.SetValue(target, contextItem);
                            }

                        });

                    }
                }

            }
        }
        #endregion

        #region read data from sitecore item
        private static object GetSitecoreItemFieldValue(Type propertyType, Item item, SitecoreItemFieldAttribute attribute, bool isLazyLoad)
        {
            if (propertyType == _types[DataTypes.String])
            {
                return item.GetString(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.DateTime])
            {
                return item.GetDateTime(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.Int])
            {
                return item.GetInt(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.Double])
            {
                return item.GetDouble(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.Bool])
            {
                return item.GetBool(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.Item])
            {
                return item.GetItem(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.ItemEnumerable])
            {
                return item.GetItems(attribute.FieldId);
            }
            if (propertyType == _types[DataTypes.CustomField] || propertyType.IsSubclassOf(_types[DataTypes.CustomField]))
            {
                Field field = item.Fields[attribute.FieldId];
                if (field != null)
                    return ReflectionUtil.CreateObject(propertyType, new object[] { field });
            }
            // got to figure out a better way to do this
            if (propertyType == _types[DataTypes.SitecoreItem] || propertyType.IsSubclassOf(_types[DataTypes.SitecoreItem]))
            {
                Item fieldValueItem = item.GetItem(attribute.FieldId);
                if (fieldValueItem != null && ItemExtensions.IsItemValidForType(propertyType, fieldValueItem))
                    return TypeMapper.CreateObject(item.GetItem(attribute.FieldId), propertyType, isLazyLoad);
            }
            // got to figure out a better way to do this

            if (_types[DataTypes.SitecoreItemEnumerable].IsAssignableFrom(propertyType))
            {
                Type[] types = propertyType.GetGenericArguments();
                if (!types.Any())
                    throw new Exception("XCore needs at least one type argument");
                if (types.Count() > 1)
                    throw new Exception("XCore only supports one type argument");
                Type classType = types[0];
                Type listType = typeof(List<>).MakeGenericType(classType);
                IList list = Activator.CreateInstance(listType) as IList;
                if (list == null)
                    throw new Exception("XCore could not create a list of object type" + listType);
                foreach (Item itm in item.GetItems(attribute.FieldId))
                {
                    if (itm == null || !ItemExtensions.IsItemValidForType(classType, itm)) continue;
                    object typeObject = TypeMapper.CreateObject(itm, classType, isLazyLoad);
                    if (typeObject != null)
                        list.Add(typeObject);
                }
                return list;
            }
            return null;
        }

        private static object GetSitecoreItemPropertyValue(Type propertyType, Item item, SitecoreItemPropertyAttribute attribute)
        {
            object value = null;
            switch (attribute.ItemInfoType)
            {
                case SitecoreItemInfo.Id:
                    value = item.ID;
                    break;
                case SitecoreItemInfo.DisplayName:
                    value = item.DisplayName;
                    break;
                case SitecoreItemInfo.FullPath:
                    value = item.Paths.FullPath;
                    break;
                case SitecoreItemInfo.Language:
                    value = item.Language;
                    break;
                case SitecoreItemInfo.Name:
                    value = item.Name;
                    break;
                case SitecoreItemInfo.Path:
                    value = item.Paths.Path;
                    break;
                case SitecoreItemInfo.TemplateId:
                    value = item.TemplateID;
                    break;
                case SitecoreItemInfo.TemplateName:
                    value = item.TemplateName;
                    break;
                case SitecoreItemInfo.Version:
                    value = item.Version;
                    break;
                case SitecoreItemInfo.HasLayout:
                    value = item.Visualization.Layout != null;
                    break;
                case SitecoreItemInfo.Uri:
                    value = item.Uri;
                    break;
                case SitecoreItemInfo.Url:
                    value = LinkManager.GetItemUrl(item, LinkManager.GetDefaultUrlOptions());
                    break;
            }
            return value;
        }

        #endregion
    }
}
