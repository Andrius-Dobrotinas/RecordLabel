using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class DbContextReflector
    {
        public DbContext DbContext { get; }
        private EntityContainer entityContainer;
        private Dictionary<Type, EntitySetBase> entities;
        private EntityRelationshipResolver relationshipResolver = new EntityRelationshipResolver();

        // TODO: come up with a better way to find actual entity types without asking for a namespace string
        /// <summary>
        /// 
        /// </summary>
        /// <param name="dbContext"></param>
        /// <param name="modelsNamespace">Namespace in which all context entities are defined (all models must be within the same namespace. This is, hopefully, a temporary workaround)</param>
        public DbContextReflector(DbContext dbContext, string modelsNamespace)
        {
            DbContext = dbContext;
            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            entityContainer = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);
            
            entities = entityContainer.BaseEntitySets.Where(set => set.BuiltInTypeKind == BuiltInTypeKind.EntitySet)
                .ToDictionary<EntitySetBase, Type>(set => 
                Type.GetType($"{modelsNamespace}.{set.ElementType.Name}"));
        }
        
        public EntityPropertyInfo[] GetDependentNavigationProperties(Type entityType)
        {
            return GetAllNavigationProperties(entityType).Where(p => p.ReferencedEntityIsDependent == true).ToArray();
        }

        public EntityPropertyInfo[] GetCollectionNavigationProperties(Type entityType)
        {
            return GetAllNavigationProperties(entityType).Where(p => p.Relationship == EntityRelationshipType.ManyToMany ||
                p.Relationship == EntityRelationshipType.OneToMany).ToArray();
        }

        public EntityPropertyInfo[] GetAllNavigationProperties(Type entityType)
        {
            return ((EntityType)GetEntitySet(entityType).ElementType).NavigationProperties
                .Select(prop => new EntityPropertyInfo(prop, relationshipResolver)).ToArray();
        }

        // TODO: probably make this thing cacheable (especially because of default values)
        public EntityKeyPropertyInfo[] GetKeyProperties(Type entityType)
        {
            return GetEntitySet(entityType).ElementType.KeyProperties.Select(x => new EntityKeyPropertyInfo(x))
                .OrderBy(x => x.Order).ToArray();
        }

        public EntityPropertyInfo[] GetAllNavigationProperties<TEntity>()
        {
            return GetAllNavigationProperties(typeof(TEntity));
        }

        public EntityPropertyInfo[] GetCollectionNavigationProperties<TEntity>()
        {
            return GetCollectionNavigationProperties(typeof(TEntity));
        }

        public EntityPropertyInfo[] GetDependentNavigationProperties<TEntity>()
        {
            return GetDependentNavigationProperties(typeof(TEntity));
        }

        public EntityKeyPropertyInfo[] GetKeyProperties<TEntity>()
        {
            return GetKeyProperties(typeof(TEntity));
        }

        /// <summary>
        /// Retrieves an entity set that corresponds to the specified type (is in the inheritance chain)
        /// <param name="throwException">Specifies whethen an exception must be thrown if entity set is not found</param>
        /// </summary>
        private EntitySetBase GetEntitySet(Type entityType, bool throwException = true)
        {
            EntitySetBase entity = entities.FirstOrDefault(entry => entry.Key.IsAssignableFrom(entityType)).Value;
            if (entity == null && throwException == true)
                throw new ArgumentOutOfRangeException($"Entity type \"{entityType}\" doesn't exist in the current Context");

            return entity;
        }
    }
}