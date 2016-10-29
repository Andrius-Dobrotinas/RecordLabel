using System;
using System.Collections.Generic;
using System.Data.Entity.Core.Metadata.Edm;
using System.Linq;

namespace AndrewD.EntityPlus
{
    public class EntityRelationshipResolver
    {
        private const string invalidEnumValueMessage = "Invalid enum value";

        public EntityRelationshipType GetRelationshipType(NavigationProperty property)
        {
            switch (property.FromEndMember.RelationshipMultiplicity)
            {
                case RelationshipMultiplicity.ZeroOrOne:
                    switch (property.ToEndMember.RelationshipMultiplicity)
                    {
                        case RelationshipMultiplicity.Many:
                            return EntityRelationshipType.OneToMany;
                        case RelationshipMultiplicity.One:
                        case RelationshipMultiplicity.ZeroOrOne:
                            return EntityRelationshipType.OneToOne;
                        default:
                            throw new InvalidOperationException($"{invalidEnumValueMessage} ({property.ToEndMember.RelationshipMultiplicity})");
                    }
                case RelationshipMultiplicity.One:
                    switch (property.ToEndMember.RelationshipMultiplicity)
                    {
                        case RelationshipMultiplicity.Many:
                            return EntityRelationshipType.OneToMany;
                        case RelationshipMultiplicity.One:
                        case RelationshipMultiplicity.ZeroOrOne:
                            return EntityRelationshipType.OneToOne;
                        default:
                            throw new InvalidOperationException($"{invalidEnumValueMessage} ({property.ToEndMember.RelationshipMultiplicity})");
                    }
                case RelationshipMultiplicity.Many:
                    switch (property.ToEndMember.RelationshipMultiplicity)
                    {
                        case RelationshipMultiplicity.Many:
                            return EntityRelationshipType.ManyToMany;
                        case RelationshipMultiplicity.One:
                        case RelationshipMultiplicity.ZeroOrOne:
                            return EntityRelationshipType.ManyToOne;
                        default:
                            throw new InvalidOperationException($"{invalidEnumValueMessage} ({property.ToEndMember.RelationshipMultiplicity})");
                    }
                default:
                    // Just in case enum gets expanded over time
                    throw new InvalidOperationException($"{invalidEnumValueMessage} ({property.FromEndMember.RelationshipMultiplicity})");
            }
        }

        /// <summary>
        /// Returns a value which indicates whether the member that is referred to by the navigation property depends on this entity.
        /// True means that the end member cannot exist without this entity, False means that it is independent
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public bool DetermineIfEndMemberIsDependent(NavigationProperty property)
        {
            switch (property.FromEndMember.RelationshipMultiplicity)
            {
                case RelationshipMultiplicity.ZeroOrOne:
                    return false;
                case RelationshipMultiplicity.One:
                    return true;
                case RelationshipMultiplicity.Many:
                    return false;
                default:
                    // Just in case enum gets expanded over time
                    throw new InvalidOperationException($"{invalidEnumValueMessage} ({property.FromEndMember.RelationshipMultiplicity})");
            }
        }
    }
}
