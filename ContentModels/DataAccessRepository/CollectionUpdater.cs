using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RecordLabel.Data.ok;

namespace RecordLabel.Data.ok
{
    // TODO: refactor methods that require both PropertyInfo and EntityPropertyInfo because the latter contains the former

    public class CollectionUpdater<TModel> : ICollectionUpdater<TModel>
    {
        protected readonly DbContext DbContext;
        protected readonly TModel Model;
        protected ICollectionMerger CollectionMerger { get; }
        //public bool DisableCascadeOnDeleteBehavior // TODO

        public CollectionUpdater(DbContext dbContext, TModel model, ICollectionMerger collectionMerger)
        {
            this.DbContext = dbContext;
            this.Model = model;
            this.CollectionMerger = collectionMerger;
        }

        /*public void UpdateCollection(Expression<Func<TModel, IList<TCollectionItem>>> property, object sourceModel)
        {
            MemberExpression mex = (MemberExpression)property.Body;
            UpdateCollection((PropertyInfo)mex.Member, sourceModel);
        }*/

        public void UpdateCollection<TCollectionItem>(PropertyInfo propertyInfo, EntityPropertyInfo property, object sourceModel,
            IRecursiveEntityUpdater entityUpdater, bool modelIsNew)
            where TCollectionItem : class, IHasId
        {
            if (modelIsNew)
            {
                AddCollection(propertyInfo, property, (IList<TCollectionItem>)propertyInfo.GetValue(sourceModel));
            }
            else
            {
                UpdateCollection(propertyInfo, property,
                    (IList<TCollectionItem>)propertyInfo.GetValue(Model),
                    (IList<TCollectionItem>)propertyInfo.GetValue(sourceModel), entityUpdater);
            }
        }

        protected void AddCollection<TCollectionItem>(PropertyInfo propertyInfo, EntityPropertyInfo property, IList<TCollectionItem> newCollection)
            where TCollectionItem : class, IHasId
        {
            // New collection is empty, therefore nothing to add
            if (!(newCollection?.Count > 0))
                return;

            // Add new collection to the model and set appropriate state to collection entries
            propertyInfo.SetValue(Model, newCollection);

            var entityState = property.ReferencedEntityIsDependent ? EntityState.Added : EntityState.Unchanged;

            foreach (var entry in newCollection)
            {
                DbContext.Entry(entry).State = entityState;
                /* TODO some day: in case of ReferencedEntityIsDependent == false, make sure that all entry.navigation 
                 * properties ar detached from the context. Or, even better, don't load them in the first place.
                 * In my situation, I don't have this issue, but it would be a good idea to make this thing work in all cases. */
            }
        }

        protected void UpdateCollection<TCollectionItem>(PropertyInfo propertyInfo, EntityPropertyInfo property, 
            IList<TCollectionItem> targetCollection, IList<TCollectionItem> newCollection, IRecursiveEntityUpdater entityUpdater)
            where TCollectionItem : class, IHasId
        {
            bool removeFromContext = property.ReferencedEntityIsDependent && 
                property.ReferencedEntityDeleteBehavior == System.Data.Entity.Core.Metadata.Edm.OperationAction.Cascade;

            if (!(newCollection?.Count > 0))
            {
                // Simply remove the whole collection and that's it

                if (targetCollection?.Count > 0)
                {
                    RemoveFromCollection(targetCollection, targetCollection, removeFromContext);
                }
            }
            else
            {
                if (!(targetCollection.Count > 0))
                {
                    // Add all entries to the set

                    propertyInfo.SetValue(Model, newCollection);

                    /* If referenced entity type is not dependent (can exist without this entity), Attach entries to the 
                     * context so that EF does not think they are new and does not Add them to the context */
                    if (!property.ReferencedEntityIsDependent)
                    {
                        foreach (var entry in newCollection)
                        {
                            DbContext.Set<TCollectionItem>().Attach(entry);
                        }
                    }
                }
                else
                {
                    // Add/Update (or Attach, if entities are not dependent upon the model) / Remove entries

                    var updatedCollection = new CollectionMerger().MergeCollections(targetCollection, newCollection,
                        (original, newState) =>
                        {
                            if (!property.ReferencedEntityIsDependent)
                            {
                                DbContext.Set<TCollectionItem>().Attach(original);
                                return original;
                            }
                            else
                            {
                                return entityUpdater.UpdateEntity(newState, entityUpdater);
                            }
                        },
                        newEntry =>
                        {
                            if (!property.ReferencedEntityIsDependent)
                            {
                                DbContext.Set<TCollectionItem>().Attach(newEntry);
                            }
                            return newEntry;
                        },
                        entriesToRemove => RemoveFromCollection<TCollectionItem>(targetCollection, entriesToRemove, removeFromContext));

                    /* Set new collection to the model because the original Target collection gets modified
                     * (entries are removed) when updating entries via EntityFramework SetValues method */
                    property.PropertyInfo.SetValue(Model, updatedCollection);
                }
            }
        }
        
        protected void RemoveFromCollection<TCollectionItem>(IList<TCollectionItem> targetCollection, IList<TCollectionItem> entriesToRemove, bool removeFromContext)
            where TCollectionItem : class, IHasId
        {
            if (removeFromContext)
            {
                // ToArray in order to prevent collection from changing while enumerating
                foreach (TCollectionItem entry in entriesToRemove.ToArray())
                {
                    DbContext.Entry(entry).State = EntityState.Deleted;
                }
            }
        }
    }
}
