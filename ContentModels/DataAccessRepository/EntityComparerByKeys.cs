using System;
using System.Collections.Generic;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class EntityComparerByKeys
    {
        public bool CompareEntities<TEntity>(TEntity first, TEntity second, IList<EntityKeyPropertyInfo> keyProperties)
        {
            if (first == null)
                throw new ArgumentNullException(nameof(first));
            if (second == null)
                throw new ArgumentNullException(nameof(second));
            if (keyProperties == null)
                throw new ArgumentNullException(nameof(keyProperties));
            if (keyProperties.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(keyProperties), "List contains 0 items");

            foreach (var keyProperty in FilterKeys(keyProperties))
            {
                object firstValue = keyProperty.PropertyInfo.GetValue(first);
                object secondValue = keyProperty.PropertyInfo.GetValue(second);

                // Primary key values will never be null
                bool valuesEqual = firstValue.Equals(secondValue);

                if (valuesEqual == false) return false;
                
            }
            return true;
        }

        protected virtual IEnumerable<EntityKeyPropertyInfo> FilterKeys(IList<EntityKeyPropertyInfo> keyProperties)
        {
            return keyProperties;
        }
    }
}
