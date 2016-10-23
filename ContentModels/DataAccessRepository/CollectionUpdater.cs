using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using RecordLabel.Data.ok;

namespace RecordLabel.Data.ok
{
    public interface ICollectionUpdater<TModel>
    {
        // Updates .... any object type that has the same property (name- and type-wise)
        void UpdateCollection<TCollectionItem>(PropertyInfo propertyInfo, EntityPropertyInfo property, object sourceModel,
            IRecursiveEntityUpdater entityUpdater, bool modelIsNew)
            where TCollectionItem : class, IHasId;
    }

    public class CollectionUpdater<TModel> : ICollectionUpdater<TModel>
    {
        protected readonly DbContext dbContext;
        protected readonly TModel Model;
        //public bool DisableCascadeOnDeleteBehavior // TODO

        public CollectionUpdater(DbContext dbContext, TModel model)
        {
            this.dbContext = dbContext;
            this.Model = model;
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
                dbContext.Entry(entry).State = entityState;
                /* TODO: in case of ReferencedEntityIsDependent == false, make sure that all entry.navigation properties ar 
                 * detached from the context. Or, even better, don't load them in the first place.
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
                    RemoveItemsCollection(targetCollection, targetCollection, removeFromContext);
                    //targetSet.Clear();
                }
            }
            else
            {
                // Add/Update/Remove entries

                if (!(targetCollection.Count > 0))
                {
                    // Add all entries to the set
                    propertyInfo.SetValue(Model, newCollection);

                    /* If referenced entity type is not dependent (can exist without this entity), Attach them to the 
                     * context so that EF does not think they are new and does not Add them to the context */
                    if (!property.ReferencedEntityIsDependent)
                    {
                        foreach (var entry in newCollection)
                        {
                            dbContext.Set<TCollectionItem>().Attach(entry);
                        }
                    }
                }
                else
                {
                    AddUpdateRemoveCollectionEntries(property, targetCollection, newCollection, entityUpdater, removeFromContext);
                }
            }
        }

        /// <summary>
        /// Adds, updates or removes entries from the Target collection based on values in the New collection using the primary key
        /// </summary>
        protected void AddUpdateRemoveCollectionEntries<TCollectionItem>(EntityPropertyInfo property, IList<TCollectionItem> targetCollection,
            IList<TCollectionItem> newCollection, IRecursiveEntityUpdater entityUpdater, bool removeFromContext)
            where TCollectionItem : class, IHasId
        {
            if (newCollection == null || newCollection.Count == 0)
            {
                if (targetCollection.Count > 0)
                {
                    RemoveItemsCollection(targetCollection, targetCollection, removeFromContext);
                    //targetCollection.Clear();
                }
            }
            else
            {
                // Get a list of entries to add to the target collection
                var newEntries = newCollection.Where(entry => targetCollection.Where(source => source.Id == entry.Id).FirstOrDefault() == null).ToArray();

                // Remove all entries that are not present in the new collection from the target collection
                var entriesToRemove = targetCollection.Where(entry => newCollection.Where(newItem => newItem.Id == entry.Id).FirstOrDefault() == null).ToArray();

                if (entriesToRemove.Length > 0)
                {
                    RemoveItemsCollection<TCollectionItem>(targetCollection, entriesToRemove, removeFromContext);
                }

                // Update each entry in target collection
                var target = targetCollection.ToArray();
                for (int i = 0; i < target.Length; i++)
                {
                    TCollectionItem newState = newCollection.Where(newItem => newItem.Id == target[i].Id).SingleOrDefault();

                    if (!property.ReferencedEntityIsDependent)
                    {
                        if (target[i].Id == default(int))
                            throw new InvalidOperationException("Entity is new (does not have an Id), therefore it cannot be attached"); // TODO: create new Ex type
                        dbContext.Set<TCollectionItem>().Attach(target[i]);
                    }
                    else
                    {
                        /* Since model containing targetCollection is a EF proxy, and the entry gets removed from the list when 
                         * SetValues is invoked on the entry in UpdateModelProperties, this is supposed to re-add the entry
                         * to the list. */ // TODO: see if there is a better way to solve this issue
                        targetCollection.Add(entityUpdater.UpdateEntity(newState, entityUpdater));
                    }
                }

                // Add entries that don't exist in the context yet
                if (newEntries.Length > 0)
                {
                    foreach (var entry in newEntries)
                    {
                        if (!property.ReferencedEntityIsDependent)
                        {
                            dbContext.Set<TCollectionItem>().Attach(entry);
                        }
                        targetCollection.Add(entry);
                    }
                }
            }
        }

        protected void RemoveItemsCollection<TCollectionItem>(IList<TCollectionItem> targetCollection, IList<TCollectionItem> entriesToRemove, bool removeFromContext)
            where TCollectionItem : class, IHasId
        {
            foreach (TCollectionItem entry in entriesToRemove.ToArray())
            {
                targetCollection.Remove(entry);
                if (removeFromContext)
                {
                    dbContext.Entry(entry).State = EntityState.Deleted;
                }
            }
        }
    }
}
