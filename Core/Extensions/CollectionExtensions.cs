using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace RecordLabel
{
    public static class CollectionExtensions
    {
        /// <summary>
        /// Performs an action on each item in collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <param name="action"></param>
        public static void ForEach<T>(this IList<T> collection, Action<T> action)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                action(collection[i]);
            }
        }

        public static void ForEach<T>(this IList<T> collection, Func<T, Action<T>> action)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                action(collection[i]);
            }
        }

        /// <summary>
        /// Concatenates string representations of collection items using a supplied separator
        /// </summary>
        /// <param name="separator">Character to use as item separator</param>
        /// <returns></returns>
        public static string JoinValues<T>(this IList<T> collection, Func<T, string> itemStringValue, char separator)
        {
            StringBuilder builder = new StringBuilder();

            for (int i = 0; i < collection.Count - 1; i++)
            {
                builder.Append(itemStringValue(collection[i]));
                builder.Append(separator);
            }
            builder.Append(itemStringValue(collection[collection.Count - 1]));

            return builder.ToString();
        }

        /// <summary>
        /// Removes a collection of items from the source collection
        /// </summary>
        /// <param name="itemsToRemove">A collection of items to remove</param>
        /// <returns></returns>
        public static int RemoveCollection<T>(this IList<T> sourceList, IList<T> itemsToRemove)
        {
            int result = 0;
            foreach (T item in itemsToRemove)
            {
                if (sourceList.Remove(item) == true) result++;
            }
            return result;
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

        /// <summary>
        /// Checks for differences between both collections using item Id to compare item equality and invokes the supplied actions to update the target collection
        /// </summary>
        /// <typeparam name="TCollectionItem"></typeparam>
        /// <param name="targetCollection">Collection to update</param>
        /// <param name="newCollection">Collection that has a set of items that the target collection must match</param>
        /// <param name="mergeChanges">Action that merges target collection item values with those from the corresponding item from the new collection</param>
        /// <param name="beforeAddingNewItem">Action to be invoked on each item before adding it to the target collection</param>
        /// <param name="beforeRemovingItems">Action to be invoked on items that will be removed from the target collection</param>
        /// <returns></returns>
        public static void UpdateCollectionByIds<TCollectionItem>(this IList<TCollectionItem> targetCollection, IList<TCollectionItem> newCollection, Action<TCollectionItem, TCollectionItem> mergeChanges, Action<TCollectionItem> beforeAddingNewItem, Action<IList<TCollectionItem>> beforeRemovingItems)
            where TCollectionItem : IHasId
        {
            if (newCollection == null || newCollection.Count == 0)
            {
                if (targetCollection.Count > 0)
                {
                    beforeRemovingItems?.Invoke(targetCollection);
                    targetCollection.Clear();
                }
            }
            else
            {
                // Get a list of items to add to the collection
                IList<TCollectionItem> newItems = newCollection.Where(item => targetCollection.Where(source => source.Id == item.Id).FirstOrDefault() == null).ToArray();

                // Remove all items that are not present in new collection
                var itemsToRemove = targetCollection.Where(item => newCollection.Where(newItem => newItem.Id == item.Id).FirstOrDefault() == null).ToArray();

                if (itemsToRemove.Length > 0)
                {
                    beforeRemovingItems?.Invoke(itemsToRemove);
                    targetCollection.RemoveCollection(itemsToRemove);
                }

                // Update each item in source collection
                foreach (var item in targetCollection)
                {
                    TCollectionItem newState = newCollection.Where(newItem => newItem.Id == item.Id).SingleOrDefault();

                    mergeChanges?.Invoke(item, newState);
                }

                if (newItems.Count > 0)
                {
                    foreach (var item in newItems)
                    {
                        beforeAddingNewItem?.Invoke(item);
                        targetCollection.Add(item);
                    }
                }
            }
        }
    }
}
