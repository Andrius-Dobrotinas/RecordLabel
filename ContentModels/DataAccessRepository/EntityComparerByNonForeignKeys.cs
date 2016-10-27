using System;
using System.Collections.Generic;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class EntityComparerByNonForeignKeys : EntityComparerByKeys
    {
        protected override IEnumerable<EntityKeyPropertyInfo> FilterKeys(IList<EntityKeyPropertyInfo> keyProperties)
        {
            return keyProperties.Where(x => x.IsForeignKey == false);
        }
    }
}
