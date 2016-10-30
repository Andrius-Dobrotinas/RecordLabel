using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndrewD.EntityPlus
{
    public class EntityScalarPropertyInfo
    {
        internal const string PropertyInfoMetadataName = "ClrPropertyInfo";
        internal const string AttributesMetadataName = "ClrAttributes";
        internal const int DefaultEnumValue = 0;

        protected static Type columnAttributeType = typeof(ColumnAttribute);

        public EdmProperty Property { get; }
        public PropertyInfo PropertyInfo { get; private set; }
        public Type PropertyType => PropertyInfo.PropertyType;
        public object DefaultValue { get; private set; }

        // TODO: check if I am able to determine the Order if it's specified in FluentApi as opposed to attribute
        public EntityScalarPropertyInfo(EdmProperty property)
        {
            if (property == null)
                throw new ArgumentNullException(nameof(property));

            Property = property;

            PropertyInfo = (PropertyInfo)Property.MetadataProperties[PropertyInfoMetadataName].Value;

            /* Handle nullable values. Nullable Primary Keys are automatically mapped to their
             * non-nullable counterparts in the database by the Entity Framework. */
            Type actualType = PropertyType.IsGenericType
                ? actualType = Nullable.GetUnderlyingType(PropertyType)
                : PropertyType;

            /* Skip bool because they always have a valid value.
             * Skip those enum types that don't have a default enum value defined.
             * All values defined in enum types are considered to valid (non-default) */
            /* TODO: see if it's worth covering scenarios where an enum has aactually a "default" value
             * (like "NotSelected = 0") defined. I could use an attribute for that enum type then. */
            DefaultValue = (actualType == typeof(bool)
                || (actualType.IsEnum && actualType.IsEnumDefined(DefaultEnumValue)))
                    ? null
                    : Activator.CreateInstance(PropertyType);
        }
    }
}