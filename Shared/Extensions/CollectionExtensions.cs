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
        /// Cast IList to List and does RemoveRange
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void RemoveAllItems<T>(this IList<T> list)
        {
            ((List<T>)list).RemoveRange(0, list.Count);
        }

        /// <summary>
        /// Changes the contents of the source collection to match those of new collection by removing items not in the new collection and adding new items from the new collection
        /// </summary>
        /// <typeparam name="TCollectionItem"></typeparam>
        /// <param name="sourceCollection"></param>
        /// <param name="newCollection"></param>
        public static void UpdateCollection<TCollectionItem>(this IList<TCollectionItem> sourceCollection, IList<TCollectionItem> newCollection)
        {
            List<TCollectionItem> list = (List<TCollectionItem>)sourceCollection;

            if (newCollection == null || newCollection.Count == 0)
            {
                list.RemoveAllItems();
            }
            else
            {
                //Get a list of items to add to the collection
                List<TCollectionItem> newItems = newCollection.Where(item => sourceCollection.Contains(item) == false).ToList();
                list.RemoveAll(item => newCollection.Contains(item) == false);
                list.AddRange(newItems);
            }
        }
    }
}
