using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Data.Entity.Core.Metadata.Edm;
using System.ComponentModel.DataAnnotations.Schema;

namespace AndrewD.EntityPlus
{
    public sealed class EntityKeyPropertyInfo : EntityScalarPropertyInfo
    {
        public int? Order { get; }
        public bool IsForeignKey => RelatedNavigationProperty != null;
        public NavigationProperty RelatedNavigationProperty { get; }

        public EntityKeyPropertyInfo(EdmProperty keyProperty) : base(keyProperty)
        {
            // Check if it's indeed a primary key property
            if ((keyProperty.DeclaringType as EntityType)?.KeyProperties.Contains(keyProperty) == false)
                throw new ArgumentException("The supplied property is not a Primary Key property to its declaring type", nameof(keyProperty));

            // TODO: see if there is a way determine the Order if it's specified via FluentApi as opposed to attribute
            Order = (((List<Attribute>)Property.MetadataProperties[AttributesMetadataName].Value)
                    .SingleOrDefault(a => a.GetType() == columnAttributeType) as ColumnAttribute)?.Order;

            /* Handle nullable values. Nullable Primary Keys are automatically mapped to their
             * non-nullable counterparts in the database by the Entity Framework. */
            Type actualType = PropertyType.IsGenericType
                ? actualType = Nullable.GetUnderlyingType(PropertyType)
                : PropertyType;
   
            // Determine if this is also a Foreign Key: it is if it has an associated navigation property
            // TODO: not sure if this is guaranteed to work in all cases
            RelatedNavigationProperty = ((EntityType)Property.DeclaringType).NavigationProperties.SingleOrDefault(x => x.GetDependentProperties().Contains(Property));
        }
    }
}
