using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    public static class LocalStringSetUpdater
    {
        public static void UpdateModel(ReleaseContext dbContext, LocalStringSet targetModel, LocalStringSet newState)
        {
            // Updates target model collection items with values with corresponding item values from newState
            // and Adds new items from newState
            // Collection items are matched by Language property

            IList<Language> newKeys = new List<Language>(); //a List containing newModel's LocalString Languages

            foreach (LocalString newItem in newState.Collection)
            {
                LocalString targetCollectionItem = targetModel.Collection.SingleOrDefault(entity => entity.Language == newItem.Language);

                // If target collection doesn't contain this item... add it
                if (targetCollectionItem == null)
                {
                    targetModel.Collection.Add(newItem);
                }
                else
                {
                    //Update an existing item
                    targetCollectionItem.UpdateModel(dbContext, newItem);
                }
                newKeys.Add(newItem.Language);
            }

            // Remove items that are not present in the newState
            for (int i = targetModel.Collection.Count - 1; i >= 0; i--)
            {
                if (!newKeys.Contains(targetModel.Collection[i].Language))
                {
                    targetModel.Collection.RemoveAt(i);
                }
            }
        }
    }
}
