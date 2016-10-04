using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    // TODO: create a class that does all the updating and make this a part of that class instead of being a IList extension method

    // TODO: TEntity should derive from EntityBase... it is currently tied to ReleaseContext, so do something about that.

    public static class DbContextHelper
    {
        /// <summary>
        /// Changes the contents of the source collection to match those of new collection by removing items not in the new collection and adding new items from the new collection and returns a list of items to remove from the database context
        /// </summary>
        /// <typeparam name="TCollectionItem"></typeparam>
        /// /// <typeparam name="TContext"></typeparam>
        /// <param name="sourceCollection">Collection to update</param>
        /// <param name="newCollection">Collection that has the set of items that the source collection must match</param>
        /// <param name="dbContext">Context in which to perform the update</param>
        /// <returns></returns>
        public static IList<TCollectionItem> UpdateCollectionByIds<TContext, TCollectionItem>(this IList<TCollectionItem> sourceCollection, IList<TCollectionItem> newCollection, TContext dbContext, bool isOneToManyRelationship = false)
            where TCollectionItem : class, IHasId, IUpdatableModel<TContext>, IValueComparable<TCollectionItem>
            where TContext : DbContext
    {
        // TODO: sort this whole List problem out because IList doesn't have AddRange that I need here and this is unsafe and wrong right now
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
                TCollectionItem newState = newCollection.Where(newItem => newItem.Id == item.Id).SingleOrDefault();

                if (isOneToManyRelationship)
                {
                    dbContext.Set<TCollectionItem>().Attach(item);
                }
                else if (item.ValuesEqual(newState) == false)
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
}
}
