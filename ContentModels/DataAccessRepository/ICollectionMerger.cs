using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Data.ok
{
    public interface ICollectionMerger
    {
        List<TEntry> MergeCollections<TEntry>(IList<TEntry> targetCollection, IList<TEntry> newCollection,
            IList<EntityKeyPropertyInfo> keyProperties,
            Func<TEntry, TEntry, TEntry> getUpdateEntry, Func<TEntry, TEntry> addEntry,
            Action<IList<TEntry>> removeEntriesFromCollection = null)
            where TEntry : class;
    }
}
