using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AndrewD.EntityPlus
{
    /// <summary>
    /// Implementing classes are supposed to provide a way to retrieve Object Context entity metadata
    /// </summary>
    public interface IDbContextReflector
    {
        /// <summary>
        /// Implementing classes must return object context's specified entity type's navigation properties that refer to types that are dependent upon the specified
        /// entity type (have one-to-x relationship)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetDependentNavigationProperties(Type entityType);

        /// <summary>
        /// Implementing classes must return object context's specified entity type's navigation properties that are collections
        /// </summary>
        /// <param name="entityType">Type of entity whose collection navigation properties are to be retrieved</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetCollectionNavigationProperties(Type entityType);

        /// <summary>
        /// Implementing classes must return object context's specified entity type's all navigation properties (including collection navigation properties)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetAllNavigationProperties(Type entityType);

        /// <summary>
        /// Implementing classes must return object context's entity type's properties that serve as Primary keys
        /// </summary>
        /// <param name="entityType">Type of entity whose primary key properties are to be retrieved</param>
        EntityKeyPropertyInfo[] GetKeyProperties(Type entityType);

        /// <summary>
        /// Implementing classes must return  object context's specified entity type's all navigation properties (including collection navigation properties)
        /// </summary>
        /// <param name="entityType">Type of entity whose navigation properties are to be retrieved</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetAllNavigationProperties<TEntity>();

        /// <summary>
        /// Implementing classes must return  object context's specified entity type's navigation properties that are collections
        /// </summary>
        /// <param name="entityType">Type of entity whose collection navigation properties are to be retrieved</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetCollectionNavigationProperties<TEntity>();

        /// <summary>
        /// Implementing classes must return  object context's entity type's navigation properties that refer to types that are dependent upon the specified
        /// entity type (have one-to-x relationship)
        /// </summary>
        /// <typeparam name="TEntity">Type of entity whose navigation properties to retrieve</param>
        /// <returns></returns>
        EntityNavigationPropertyInfo[] GetDependentNavigationProperties<TEntity>();

        /// <param name="entityType"></param>
        /// <summary>
        /// Implementing classes must return object context's entity type's properties that serve as Primary keys
        /// </summary>
        /// <typeparam name="TEntity">Type of entity whose primary key properties are to be retrieved</typeparam>
        /// <returns></returns>
        EntityKeyPropertyInfo[] GetKeyProperties<TEntity>();
    }
}
