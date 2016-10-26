using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Data.ok
{
    public class EntityKeyPropertyInfo
    {
        private const string PropertyInfoMetadataName = "ClrPropertyInfo";
        private const string AttributesMetadataName = "ClrAttributes";

        private static Type columnAttributeType = typeof(ColumnAttribute);

        public PropertyInfo PropertyInfo { get; private set; }
        public int? Order { get; private set; }
        public object DefaultValue { get; private set; }

        private EdmProperty Property { get; }

        // TODO: would be nice to find a way to check if this is indeed a Key property
        // TODO: check if I am able to determine the Order if it's specified in FluentApi as opposed to attribute
        public EntityKeyPropertyInfo(EdmProperty property)
        {
            Property = property;

            PropertyInfo = (PropertyInfo)Property.MetadataProperties[PropertyInfoMetadataName].Value;

            Order = (((List<Attribute>)Property.MetadataProperties[AttributesMetadataName].Value)
                    .SingleOrDefault(a => a.GetType() == columnAttributeType) as ColumnAttribute)?.Order;

            DefaultValue = PropertyInfo.PropertyType.IsEnum ? null
                        : Activator.CreateInstance(PropertyInfo.PropertyType);
            /* Skip enums because they always have a valid value
             * TODO: maybe think of a better way in order to include some enums that have "default" unusable values? */
        }
    }
}
