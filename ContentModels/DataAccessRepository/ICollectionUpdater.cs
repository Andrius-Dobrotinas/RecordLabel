using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Data.ok
{
    public interface ICollectionUpdater<TModel>
    {
        // Updates .... any object type that has the same property (name- and type-wise)
        void UpdateCollection<TCollectionItem>(PropertyInfo propertyInfo, EntityPropertyInfo property, object sourceModel,
            IRecursiveEntityUpdater entityUpdater, bool modelIsNew)
            where TCollectionItem : class, IHasId;
    }
}
