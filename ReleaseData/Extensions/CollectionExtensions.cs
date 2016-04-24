using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using RecordLabel.Content;

namespace RecordLabel
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Changes the contents of the source collection to match those of new collection by removing items not in the new collection and adding new items from the new collection and returns a list of items to remove from the database context
        /// </summary>
        /// <typeparam name="TCollectionItem"></typeparam>
        /// <param name="sourceCollection"></param>
        /// <param name="newCollection"></param>
        /// <param name="dbContext"></param>
        /// <returns></returns>
        public static IList<TCollectionItem> UpdateCollectionByIds2<TCollectionItem>(this IList<TCollectionItem> sourceCollection, IList<TCollectionItem> newCollection, ReleaseContext dbContext) where TCollectionItem : Entity, IValueComparable<TCollectionItem>
        {
            List<TCollectionItem> sourceList = (List<TCollectionItem>)sourceCollection;

            if (newCollection == null || newCollection.Count == 0)
            {
                return sourceCollection;
            }
            else
            {
                //Get a list of items to add to the collection
                List<TCollectionItem> newItems = newCollection.Where(item => sourceCollection.Where(source => source.Id == item.Id).FirstOrDefault() == null).ToList();

                //remove all items that are not present in new collection
                var itemsToRemove = sourceList.Where(item => newCollection.Where(newItem => newItem.Id == item.Id).FirstOrDefault() == null).ToArray();
                sourceList.RemoveCollection(itemsToRemove);

                //update each item in source collection
                foreach (var item in sourceList)
                {
                    TCollectionItem newState = newCollection.Where(newItem => newItem.Id == item.Id).FirstOrDefault();
                    if (item.ValuesEqual(newState) == false)
                    {
                        item.UpdateModel(dbContext, newState);
                    }
                }

                if (newItems.Count > 0)
                {
                    sourceList.AddRange(newItems);
                }

                return itemsToRemove;
            }
        }

        /// <summary>
        /// Removes an item from a collection by its ID
        /// </summary>
        public static void RemoveById<TModel>(this IList<TModel> list, int id) where TModel : IHasId
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Id == id)
                {
                    list.RemoveAt(i);
                }
            }
        }

        /// <summary>
        /// Removes all empty items from the collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        public static void RemoveEmptyItems<T>(this IList<T> collection) where T : IKnowIfImEmpty
        {
            IList<T> emptyItems = collection.Where(item => item == null || item.IsEmpty == true).ToArray();
            collection.RemoveCollection(emptyItems);
        }
    }
}