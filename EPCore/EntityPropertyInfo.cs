using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace AndrewD.EntityPlus
{
    public class EntityNavigationPropertyInfo
    {
        private const string PropertyInfoMetadataName = "ClrPropertyInfo";

        public string PropertyName { get; }
        public System.Reflection.PropertyInfo PropertyInfo { get; }
        public Type ReferencedEntityType => PropertyInfo.PropertyType;
        public bool ReferencedEntityIsDependent { get; }
        public OperationAction ReferencedEntityDeleteBehavior { get; }
        public EntityRelationshipType Relationship { get; }

        public EntityNavigationPropertyInfo(NavigationProperty property, EntityRelationshipResolver relationshipDeterminator)
        {
            PropertyName = property.Name;
            PropertyInfo = (System.Reflection.PropertyInfo)property.MetadataProperties[PropertyInfoMetadataName].Value;
            ReferencedEntityDeleteBehavior = property.FromEndMember.DeleteBehavior;
            ReferencedEntityIsDependent = relationshipDeterminator.DetermineIfEndMemberIsDependent(property);
            Relationship = relationshipDeterminator.GetRelationshipType(property);
        }
    }
}
