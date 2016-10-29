﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace AndrewD.EntityPlus.Persistence
{
    public class CollectionMerger : ICollectionMerger
    {
        public IEntityComparerByKeys EntityComparer { get; }

        public CollectionMerger(IEntityComparerByKeys entityComparer)
        {
            EntityComparer = entityComparer;
        }

        /// <summary>
        /// Adds, updates or removes entries from the Target collection based on values in the New collection using the specified primary keys
        /// </summary>
        public List<TEntry> MergeCollections<TEntry>(IList<TEntry> targetCollection, IList<TEntry> newCollection,
            IList<EntityKeyPropertyInfo> keyProperties,
            Func<TEntry, TEntry, TEntry> getUpdateEntry, Func<TEntry, TEntry> addEntry,
            Action<IList<TEntry>> removeEntriesFromCollection)
            where TEntry : class
        {
            if (targetCollection == null)
                throw new ArgumentNullException(nameof(targetCollection));
            if (newCollection == null)
                throw new ArgumentNullException(nameof(newCollection));
            if (getUpdateEntry == null)
                throw new ArgumentNullException(nameof(getUpdateEntry));
            if (addEntry == null)
                throw new ArgumentNullException(nameof(addEntry));

            var resultingCollection = new List<TEntry>(newCollection.Count);

            // Get all New collection entries that are not present in the Target collection (to add them to the Target collection)
            var newEntries = newCollection.Where(entry => targetCollection
                .SingleOrDefault(source => EntityComparer.CompareEntities<TEntry>(source, entry, keyProperties)) == null)
                .ToArray();

            // Get all Target collection entries that are not present in the New collection (to remove them from the Target collection)
            var entriesToRemove = targetCollection.Where(entry => newCollection
                .SingleOrDefault(newItem => EntityComparer.CompareEntities<TEntry>(newItem, entry, keyProperties)) == null)
                .ToArray();

            /* Get a copy of Target collection for safe iterations (in case entries get removed
             * from the collection in getUpdatedEntry */
            var target = targetCollection.Except(entriesToRemove).ToArray();

            if (entriesToRemove.Length > 0)
            {
                removeEntriesFromCollection?.Invoke(entriesToRemove);
            }
 
            // Update pre-existing entries in Target collection
            for (int i = 0; i < target.Length; i++)
            {
                TEntry newState = newCollection.Single(newItem => EntityComparer.CompareEntities(newItem, target[i], keyProperties));
                resultingCollection.Add(getUpdateEntry.Invoke(target[i], newState));
            }

            // Add entries that don't exist in the Target collection
            if (newEntries.Length > 0)
            {
                foreach (var entry in newEntries)
                {
                    resultingCollection.Add(addEntry.Invoke(entry));
                }
            }
            
            return resultingCollection;
        }
    }
}
