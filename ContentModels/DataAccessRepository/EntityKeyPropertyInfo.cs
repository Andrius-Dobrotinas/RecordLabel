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
        private const int DefaultEnumValue = 0;

        private static Type columnAttributeType = typeof(ColumnAttribute);

        public PropertyInfo PropertyInfo { get; private set; }
        public int? Order { get; private set; }
        public object DefaultValue { get; private set; }
        public bool IsForeignKey => RelatedNavigationProperty != null;
        public NavigationProperty RelatedNavigationProperty { get; private set; }
        public Type PropertyType => PropertyInfo.PropertyType;

        public EdmProperty Property { get; }

        // TODO: check if I am able to determine the Order if it's specified in FluentApi as opposed to attribute
        public EntityKeyPropertyInfo(EdmProperty keyProperty)
        {
            if ((keyProperty.DeclaringType as EntityType)?.KeyProperties.Contains(keyProperty) == false)
            {
                throw new ArgumentException("The supplied property is not a Key property to its declaring type", nameof(keyProperty));
            }
            
            Property = keyProperty;

            PropertyInfo = (PropertyInfo)Property.MetadataProperties[PropertyInfoMetadataName].Value;

            Order = (((List<Attribute>)Property.MetadataProperties[AttributesMetadataName].Value)
                    .SingleOrDefault(a => a.GetType() == columnAttributeType) as ColumnAttribute)?.Order;

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
   
            // Determine if this is also a Foreign Key: it is if it has an associated navigation property
            // TODO: not sure if this is guaranteed to work in all cases
            RelatedNavigationProperty = ((EntityType)Property.DeclaringType).NavigationProperties.SingleOrDefault(x => x.GetDependentProperties().Contains(Property));
        }
    }
}
