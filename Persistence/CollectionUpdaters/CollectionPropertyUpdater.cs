using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using AndrewD.EntityPlus.Reflection;

namespace AndrewD.EntityPlus.Persistence
{
    // Thread unsafe: will not work
    public class CollectionPropertyUpdater<TModel> : ICollectionPropertyUpdater<TModel>
    {
        public DbContext DbContext { get; }
        public TModel Model { get; }
        public ICollectionMerger CollectionMerger { get; }
        //public bool DisableCascadeOnDeleteBehavior // TODO
        protected EntityNavigationPropertyInfo Property;
        protected IList<EntityKeyPropertyInfo> KeyProperties;
        protected IRecursiveEntityUpdater EntityUpdater;
        
        /// <summary>
        /// Indicates whether entities of current type are to be removed from the underlying context altogether on delete
        /// </summary>
        protected bool RemoveEntitiesFromContext => Property.ReferencedEntityIsDependent &&
                Property.ReferencedEntityDeleteBehavior == System.Data.Entity.Core.Metadata.Edm.OperationAction.Cascade;

        public CollectionPropertyUpdater(DbContext dbContext, TModel model, ICollectionMerger collectionMerger)
        {
            DbContext = dbContext;
            Model = model;
            CollectionMerger = collectionMerger;
        }

        /*public void UpdateCollection(Expression<Func<TModel, IList<TCollectionEntry>>> property, object sourceModel)
        {
            MemberExpression mex = (MemberExpression)property.Body;
            UpdateCollection((PropertyInfo)mex.Member, sourceModel);
        }*/

        public void UpdateCollection<TCollectionEntry>(EntityNavigationPropertyInfo property, IList<EntityKeyPropertyInfo> keyProperties,
            object sourceModel, bool modelIsNew, IRecursiveEntityUpdater entityUpdater)
            where TCollectionEntry : class
        {
            Property = property;
            KeyProperties = keyProperties;
            EntityUpdater = entityUpdater;

            if (modelIsNew)
            {
                AddCollection((IList<TCollectionEntry>)property.PropertyInfo.GetValue(sourceModel));
            }
            else
            {
                UpdateCollection((IList<TCollectionEntry>)property.PropertyInfo.GetValue(Model),
                    (IList<TCollectionEntry>)property.PropertyInfo.GetValue(sourceModel));
            }
        }

        protected void AddCollection<TCollectionEntry>(IList<TCollectionEntry> newCollection)
            where TCollectionEntry : class
        {
            // New collection is empty, therefore nothing to add
            if (!(newCollection?.Count > 0))
                return;

            // Add new collection to the model and set appropriate state to collection entries
            Property.PropertyInfo.SetValue(Model, newCollection);

            var entityState = Property.ReferencedEntityIsDependent ? EntityState.Added : EntityState.Unchanged;

            foreach (var entry in newCollection)
            {
                DbContext.Entry(entry).State = entityState;
                /* TODO some day: in case of ReferencedEntityIsDependent == false, make sure that all entry.navigation 
                 * properties ar detached from the context. Or, even better, don't load them in the first place.
                 * In my situation, I don't have this issue, but it would be a good idea to make this thing work in all cases. */
            }
        }

        protected void UpdateCollection<TCollectionEntry>(IList<TCollectionEntry> targetCollection, IList<TCollectionEntry> newCollection)
            where TCollectionEntry : class
        {
            if (!(newCollection?.Count > 0))
            {
                // Simply remove the collection and that's it

                if (targetCollection?.Count > 0)
                {
                    RemoveFromCollection(targetCollection, targetCollection);
                }
            }
            else
            {
                if (!(targetCollection.Count > 0))
                {
                    // Add all entries to the set

                    Property.PropertyInfo.SetValue(Model, newCollection);

                    /* If referenced entity type is not dependent (can exist without this entity), Attach entries to the 
                     * context so that EF does not think they are new and does not Add them to the context */
                    if (!Property.ReferencedEntityIsDependent)
                    {
                        foreach (var entry in newCollection)
                        {
                            DbContext.Set<TCollectionEntry>().Attach(entry);
                        }
                    }
                }
                else
                {
                    // Add/Update (or Attach, if entities are not dependent upon the model) / Remove entries
                    var updatedCollection = CollectionMerger.MergeCollections(targetCollection, newCollection,
                        KeyProperties,
                        (original, newState) =>
                        {
                            if (!Property.ReferencedEntityIsDependent)
                            {
                                DbContext.Set<TCollectionEntry>().Attach(original);
                                return original;
                            }
                            else
                            {
                                // Copy foreign key property values to the new state
                                if (KeyProperties.Any(x => x.IsForeignKey == true))
                                {
                                    newState.CopyPropertyValues(original, KeyProperties.Where(x => x.IsForeignKey == true)
                                        .Select(x => x.PropertyInfo).ToArray());
                                }
                                return EntityUpdater.UpdateEntity(newState, EntityUpdater);
                            }
                        },
                        newEntry =>
                        {
                            if (!Property.ReferencedEntityIsDependent)
                            {
                                DbContext.Set<TCollectionEntry>().Attach(newEntry);
                            }
                            return newEntry;
                        },
                        entriesToRemove => RemoveFromCollection<TCollectionEntry>(targetCollection, entriesToRemove));

                    /* Set new collection to the model because the original Target collection gets modified
                     * (entries are removed) when updating entries via EntityFramework SetValues method */
                    Property.PropertyInfo.SetValue(Model, updatedCollection);
                }
            }
        }
        
        protected void RemoveFromCollection<TCollectionEntry>(IList<TCollectionEntry> targetCollection, IList<TCollectionEntry> entriesToRemove)
            where TCollectionEntry : class
        {
            if (RemoveEntitiesFromContext)
            {
                // ToArray in order to prevent collection from changing while enumerating
                foreach (TCollectionEntry entry in entriesToRemove.ToArray())
                {
                    DbContext.Entry(entry).State = EntityState.Deleted;
                }
            }
        }
    }
}
