using System;
using System.Collections.Generic;

namespace AndrewD.EntityPlus
{
    public interface IEntityComparerByKeys
    {
        bool CompareEntities<TEntity>(TEntity first, TEntity second, IList<EntityKeyPropertyInfo> keyProperties);
    }
}
