using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Metadata.Edm;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Linq;

namespace AndrewD.EntityPlus
{
    /// <summary>
    /// Provides a way to retrieve Object Context entity metadata
    /// </summary>
    public class DbContextReflector
    {
        public ObjectContext ObjectContext { get; }
        private EntityContainer entityContainer;
        private Dictionary<Type, EntitySetBase> entities;
        // TODO: do something about this:
        private EntityRelationshipResolver relationshipResolver = new EntityRelationshipResolver();


        // TODO: come up with a better way to find actual entity types without asking for a namespace string

        /// <param name="objectContextAdapter">Object context that is to be interrogated</param>
        /// <param name="modelsNamespace">Namespace in which all context entities are defined (all models must be within 
        /// the same namespace. If model type is in a different namespace, an exception will be thrown when instantiating
        /// the type. This is, hopefully, a temporary workaround, until I find a good solution for this)</param>
        /// <param name="modelAssemblyName">Name of assembly which contains all context enties. Again, just like with the
        /// namespace parameter, this is a temporary workaround</param>
        public DbContextReflector(IObjectContextAdapter objectContextAdapter, string modelsNamespace, string modelAssemblyName)
            : this(objectContextAdapter.ObjectContext, modelsNamespace, modelAssemblyName)
        {

        }

        /// <param name="objectContextAdapter">Object context that is to be interrogated</param>
        /// <param name="modelsNamespace">Namespace in which all context entities are defined (all models must be within 
        /// the same namespace. If model type is in a different namespace, an exception will be thrown when instantiating
        /// the type. This is, hopefully, a temporary workaround, until I find a good solution for this)</param>
        /// <param name="modelAssemblyName">Name of assembly which contains all context enties. Again, just like with the
        /// namespace parameter, this is a temporary workaround</param>
        public DbContextReflector(ObjectContext objectContext, string modelsNamespace, string modelAssemblyName)
        {
            if (objectContext == null)
                throw new ArgumentNullException(nameof(objectContext));
            if (string.IsNullOrEmpty(modelsNamespace))
                throw new ArgumentNullException(nameof(modelsNamespace));
            if (string.IsNullOrEmpty(modelAssemblyName))
                throw new ArgumentNullException(nameof(modelAssemblyName));

            ObjectContext = objectContext;
            entityContainer = objectContext.MetadataWorkspace.GetEntityContainer(objectContext.DefaultContainerName, DataSpace.CSpace);

            entities = entityContainer.BaseEntitySets.Where(set => set.BuiltInTypeKind == BuiltInTypeKind.EntitySet)
                .ToDictionary<EntitySetBase, Type>(set =>
                Type.GetType($"{modelsNamespace}.{set.ElementType.Name},{modelAssemblyName}"));
        }

        /// <summary>
        /// Retrieves specified entity type's navigation properties that refer to types that are dependent upon the specified
        /// entity type (have one-to-x relationship)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetDependentNavigationProperties(Type entityType)
        {
            return GetAllNavigationProperties(entityType).Where(p => p.ReferencedEntityIsDependent == true).ToArray();
        }

        /// <summary>
        /// Retrieves specified entity type's navigation properties that are collections
        /// </summary>
        /// <param name="entityType">Type of entity whose collection navigation properties are to be retrieved</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetCollectionNavigationProperties(Type entityType)
        {
            return GetAllNavigationProperties(entityType).Where(p => p.Relationship == EntityRelationshipType.ManyToMany ||
                p.Relationship == EntityRelationshipType.OneToMany).ToArray();
        }

        /// <summary>
        /// Retrieves specified entity type's all navigation properties (including collection navigation properties)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetAllNavigationProperties(Type entityType)
        {
            return ((EntityType)GetEntitySet(entityType).ElementType).NavigationProperties
                .Select(prop => new EntityNavigationPropertyInfo(prop, relationshipResolver)).ToArray();
        }

        /// <summary>
        /// Retrieves entity type's properties that serve as Primary keys
        /// </summary>
        /// <param name="entityType">Type of entity whose primary key properties are to be retrieved</param>
        public EntityKeyPropertyInfo[] GetKeyProperties(Type entityType)
        {
            // TODO: probably make this thing cacheable (especially because of default values)
            return GetEntitySet(entityType).ElementType.KeyProperties.Select(x => new EntityKeyPropertyInfo(x))
                .OrderBy(x => x.Order).ToArray();
        }

        /// <summary>
        /// Retrieves specified entity type's all navigation properties (including collection navigation properties)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetAllNavigationProperties<TEntity>()
        {
            return GetAllNavigationProperties(typeof(TEntity));
        }

        /// <summary>
        /// Retrieves specified entity type's navigation properties that are collections
        /// </summary>
        /// <param name="entityType">Type of entity whose collection navigation properties are to be retrieved</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetCollectionNavigationProperties<TEntity>()
        {
            return GetCollectionNavigationProperties(typeof(TEntity));
        }

        /// <summary>
        /// Retrieves specified entity type's navigation properties that refer to types that are dependent upon the specified
        /// entity type (have one-to-x relationship)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity whose navigation properties to retrieve</param>
        /// <returns></returns>
        public EntityNavigationPropertyInfo[] GetDependentNavigationProperties<TEntity>()
        {
            return GetDependentNavigationProperties(typeof(TEntity));
        }

        /// <param name="entityType"></param>
        /// <summary>
        /// Retrieves entity type's properties that serve as Primary keys
        /// </summary>
        /// <typeparam name="TEntity">Type of entity whose primary key properties are to be retrieved</typeparam>
        /// <returns></returns>
        public EntityKeyPropertyInfo[] GetKeyProperties<TEntity>()
        {
            return GetKeyProperties(typeof(TEntity));
        }

        /// <summary>
        /// Retrieves an entity set that corresponds to the specified type
        /// <param name="entityType">Type for which to get a corresponding entity set</param>
        /// <param name="throwException">Specifies whether an exception must be thrown if an entity set corresponding
        /// to the specified type is not found</param>
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