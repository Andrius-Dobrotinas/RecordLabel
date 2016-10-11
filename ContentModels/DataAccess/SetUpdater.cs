using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    /// <summary>
    /// Updates models of Set of T type
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public static class SetUpdater<T>
        where T : EntityBase, IValueComparable<T>
    {
        public static void UpdateSet<TModel>(TModel model, TModel copyFrom, PropertyInfo setProperty, ContextWrapper<ReleaseContext> dbContext)
            where TModel : IHasASet<T>
        {
            Set<T> targetSet = (Set<T>)setProperty.GetValue(model);
            Set<T> newSet = (Set<T>)setProperty.GetValue(copyFrom);

            if (!(newSet?.Collection?.Count > 0))
            {
                //Remove the whole set and do nothing
                if (targetSet != null)
                {
                    targetSet.Delete(dbContext.Context);
                }
            }
            else //Add/Remove items
            {
                bool isOneToOneRelationship = typeof(T).IsDefined(typeof(OneToOneRelationshipAttribute));

                if (targetSet == null)
                {
                    //Add all items to the set
                    setProperty.SetValue(model, newSet);
                    if (!isOneToOneRelationship)
                    {
                        foreach (var item in newSet.Collection)
                        {
                            dbContext.Set<T>().Attach(item);
                        }
                    }
                }
                else
                {
                    //Add/Remove items
                    UpdateSetItems(targetSet, newSet, dbContext, isOneToOneRelationship);
                }
            }
        }

        private static void UpdateSetItems(Set<T> targetModel, Set<T> sourceModel, ContextWrapper<ReleaseContext> dbContext, bool isOneToOneRelationship)
        {
            // An exception for LocalStringSet because
            if (typeof(T) == typeof(LocalString))
            {
                RecordLabel.Content.LocalStringSetUpdater.UpdateModel(targetModel as LocalStringSet, sourceModel as LocalStringSet);
            }
            else
            {
                targetModel.Collection.UpdateCollectionByIds(sourceModel.Collection,
                     (item, newState) =>
                     {
                         if (!isOneToOneRelationship)
                         {
                             dbContext.Set<T>().Attach(item);
                         }
                         else if (item.ValuesEqual(newState) == false)
                         {
                             dbContext.UpdateModel(item, newState);
                         }
                     },
                     (newItem) =>
                     {
                         if (!isOneToOneRelationship)
                         {
                             dbContext.Set<T>().Attach(newItem);
                         }
                     },
                     (itemsToRemove) =>
                     {
                         DeleteFromContext(itemsToRemove, dbContext.Context, isOneToOneRelationship);
                     });
            }
        }

        /// <summary>
        /// Permanently deletes items within the collection from the database within the supplied context if the relationship is One-to-One
        /// </summary>
        /// <param name="itemsToRemove"></param>
        /// <param name="dbContext"></param>
        private static void DeleteFromContext(IList<T> itemsToRemove, ReleaseContext dbContext, bool isOneToOneRelationship)
        {
            if (isOneToOneRelationship)
            {
                foreach (T item in itemsToRemove.ToArray())
                {
                    item.Delete(dbContext);
                }
            }
        }
    }
}