using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using RecordLabel.Data.Models;
using System.Reflection;

namespace RecordLabel.Data
{
    public abstract class Repository<TContext, TModel>
        where TContext : DbContext
        where TModel : EntityBase
    {
        protected TContext DbContext { get; set; }

        protected Repository(TContext context)
        {
            DbContext = context;
        }

        public virtual TModel GetModel(int id)
        {
            return DbContext.Set<TModel>().Find(id);
        }

        public virtual void SaveModel(TModel model)
        {
            Updater.UpdateModel<TModel>(DbContext, model);
        }

        

        public void SaveChanges()
        {
            DbContext.SaveChanges();
        }


        
    }

    // 1. Each collection that holds items that cannot exist without the model, must be marked with an attribute (so that these collection items are marked deleted because for some reason the Cascade delete thing doesn't work on the Track collection)
    // 2. TODO: Remove that contextModel. It was supposed to prevent from collection item update calling back to the calling model via collection (backreference)
    // 3. TODO: string collections handle separately (because of the composite key)
    public static class Updater
    {
        public static void UpdateModel<TModel>(DbContext context, TModel model)
            where TModel : EntityBase
        {
            var originalModel = context.Set<TModel>().Find(model.Id);

            // If adding new entity
            if (originalModel == null)
            {
                context.Set<TModel>().Add(model);

                // TODO: if we add new entity but assign, like, an existing metadata... do that else if anyway.
            }
            else// if (!ReferenceEquals(originalModel, model))
            {
                context.Entry(originalModel).CurrentValues.SetValues(model);
                

                var collectionProperties = typeof(TModel).GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IList<>)).ToArray();
                
                foreach (var property in collectionProperties)
                {
                    var genericArguments = property.PropertyType.GetGenericArguments();

                    InvokeGenericMethod(typeof(CollectionUpdater), "UpdateCollection", genericArguments, BindingFlags.Public | BindingFlags.Static,
                        new object[] { property, model, originalModel, context }, null);
                }
            }
        }

        public static object InvokeGenericMethod(Type classType, Type[] classGenericTypeArguments, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            // I could probably implement cache so as not to invoke this every time
            var genericType = classType.MakeGenericType(classGenericTypeArguments);

            return InvokeGenericMethod(genericType, methodName, methodGenericTypeArguments, methodBindingFlags, methodArguments, targetObject);
        }

        public static object InvokeGenericMethod(Type classType, string methodName, Type[] methodGenericTypeArguments, BindingFlags methodBindingFlags, object[] methodArguments, object targetObject)
        {
            var method = classType.GetMethod(methodName, methodBindingFlags);
            if (methodGenericTypeArguments?.Length > 0)
                method = method.MakeGenericMethod(methodGenericTypeArguments);
            return method.Invoke(targetObject, methodArguments);
        }
    }

    public static class CollectionUpdater
    {
        public static void UpdateCollection<T>(PropertyInfo property, object sourceModel, object targetModel, DbContext context)
            where T : EntityBase
        {
            IList<T> targetSet = (IList<T>)property.GetValue(targetModel);
            IList<T> newSet = (IList<T>)property.GetValue(sourceModel);

            var itemsAreNotShared = property.IsDefined(typeof(CascadeOnDeleteAttribute));

            if (!(newSet?.Count > 0))
            {
                //Remove the whole set and do nothing
                if (targetSet?.Count > 0)
                {
                    DeleteFromContext<T>(targetSet, context, itemsAreNotShared);

                    targetSet.Clear();
                }
            }
            else //Add/Remove items
            {
                
                if (!(targetSet.Count > 0))
                {
                    //Add all items to the set
                    property.SetValue(targetModel, newSet);

                    // If collection items are "singletons", attach them to the context so that they do not get added as new items
                    if (!itemsAreNotShared)
                    {
                        foreach (var item in newSet)
                        {
                            context.Set<T>().Attach(item);
                        }
                    }
                }
                else
                {
                    //Add/Remove items
                    UpdateSetItems(targetSet, newSet, context, itemsAreNotShared);
                }
            }
        }

        private static void UpdateSetItems<T>(IList<T> targetModel, IList<T> sourceModel, DbContext dbContext, bool itemsAreNotShared) 
            where T : EntityBase
        {
            // An exception for LocalStringSet because
           /* if (typeof(T) == typeof(LocalString))
            {
                RecordLabel.Content.LocalStringSetUpdater.UpdateModel(targetModel as LocalStringSet, sourceModel as LocalStringSet);
            }
            else*/
            {
                targetModel.UpdateCollectionByIds(sourceModel,
                     (item, newState) =>
                     {
                         if (!itemsAreNotShared)
                         {
                             dbContext.Set<T>().Attach(item);
                         }
                         else //if (item.ValuesEqual(newState) == false) // TODO: implement it someday for better performance?
                         {
                             Updater.UpdateModel<T>(dbContext, newState); //dbContext.UpdateModel(item, newState);
                         }
                     },
                     (newItem) =>
                     {
                         if (!itemsAreNotShared)
                         {
                             dbContext.Set<T>().Attach(newItem);
                         }
                     },
                     (itemsToRemove) =>
                     {
                         DeleteFromContext(itemsToRemove, dbContext, itemsAreNotShared);
                     });
            }
        }

        /// <summary>
        /// Permanently deletes items within the collection from the database within the supplied context if the relationship is One-to-One
        /// </summary>
        /// <param name="itemsToRemove"></param>
        /// <param name="dbContext"></param>
        private static void DeleteFromContext<T>(IList<T> itemsToRemove, DbContext context, bool deleteFromContext)
            where T: EntityBase
        {
            if (deleteFromContext)
            {
                foreach (T item in itemsToRemove.ToArray())
                {
                    //context.DeleteModel(item);
                    context.Entry(item).State = EntityState.Deleted;
                }
            }
        }
    }

    public class ReleaseRepository : Repository<ReleaseContext, Release>
    {
        public ReleaseRepository(ReleaseContext context) : base(context)
        {

        }
    }
}
