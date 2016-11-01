using System;
using System.Collections.Generic;
using System.Linq;

namespace AndrewD.EntityPlus
{
    /// <summary>
    /// Determines entity value equality by comparing values of their key properties except those
    /// that also serve as Foreign keys
    /// </summary>
    public class EntityComparerByNonForeignKeys : EntityComparerByKeys
    {
        protected override IEnumerable<EntityKeyPropertyInfo> FilterKeys(IList<EntityKeyPropertyInfo> keyProperties)
        {
            return keyProperties.Where(x => x.IsForeignKey == false);
        }
    }
}
