using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace RecordLabel.Data.ok
{
    // TODO: rename to :EntityNavigationPropertyInfo
    public class EntityPropertyInfo
    {
        private const string PropertyInfoMetadataName = "ClrPropertyInfo";

        public string PropertyName { get; }
        public System.Reflection.PropertyInfo PropertyInfo { get; }
        public Type ReferencedEntityType => PropertyInfo.PropertyType;
        public bool ReferencedEntityIsDependent { get; }
        public OperationAction ReferencedEntityDeleteBehavior { get; }
        public EntityRelationshipType Relationship { get; }

        public EntityPropertyInfo(NavigationProperty property, EntityRelationshipResolver relationshipDeterminator)
        {
            PropertyName = property.Name;
            PropertyInfo = (System.Reflection.PropertyInfo)property.MetadataProperties[PropertyInfoMetadataName].Value;
            ReferencedEntityDeleteBehavior = property.FromEndMember.DeleteBehavior;
            ReferencedEntityIsDependent = relationshipDeterminator.DetermineIfEndMemberIsDependent(property);
            Relationship = relationshipDeterminator.GetRelationshipType(property);
        }
    }
}
