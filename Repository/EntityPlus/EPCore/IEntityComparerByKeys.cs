using System;
using System.Collections.Generic;

namespace AndrewD.EntityPlus
{
    /// <summary>
    /// Interface for classes that perform value equality comparisons on objects by comparing their
    /// primary or other key property values
    /// </summary>
    public interface IEntityComparerByKeys
    {
        /// <summary>
        /// Implementations must compare two supplied entities using values extracted from properties
        /// specified in the supplied property list (all other property values must be ignored)
        /// </summary>
        /// <param name="keyProperties">A list of properties whose values must be extracted from
        /// entities and which must be used for comparison</param>
        /// <returns></returns>
        bool CompareEntities<TEntity>(TEntity first, TEntity second, IList<EntityKeyPropertyInfo> keyProperties);
    }
}
