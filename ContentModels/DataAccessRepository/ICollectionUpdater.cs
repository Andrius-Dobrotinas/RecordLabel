using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Data.ok
{
    // TODO: Consider grouping EntityPropertyInfo and EntityKeyPropertyInfo into something like EntityTypeInfo or EntityInfo
    public interface ICollectionUpdater<TModel>
    {
        // Updates .... any object type that has the same property (name- and type-wise)
        void UpdateCollection<TCollectionEntry>(EntityPropertyInfo property, IList<EntityKeyPropertyInfo> keyProperties,
            object sourceModel, bool modelIsNew, IRecursiveEntityUpdater entityUpdater)
            where TCollectionEntry : class;
    }
}
