using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Content
{
    /// <summary>
    /// Updates models of LocalStringSet type exclusively
    /// </summary>
    public static class LocalStringSetUpdater
    {
        /// <summary>
        /// Updates target model collection items with values with corresponding item values from newState
        /// and Adds new items from newState.
        /// Collection items are matched by Language property
        /// </summary>
        /// <param name="targetModel"></param>
        /// <param name="newState"></param>
        public static void UpdateModel(LocalStringSet targetModel, LocalStringSet newState)
        {
            IList<Language> newKeys = new List<Language>(); // A List containing newModel's all LocalString Languages

            foreach (LocalString newItem in newState.Collection)
            {
                LocalString targetCollectionItem = targetModel.Collection.SingleOrDefault(entity => entity.Language == newItem.Language);

                // If target collection doesn't contain this item, add it
                if (targetCollectionItem == null)
                {
                    targetModel.Collection.Add(newItem);
                }
                // Otherwise update an existing item
                else
                {
                    
                    UpdateSetItem(targetCollectionItem, newItem);
                    
                }
                // Add each to collection so that we knew which keys are contained in the new state
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

        private static void UpdateSetItem(LocalString targetModel, LocalString newState)
        {
            targetModel.Text = newState.Text;
        }
    }
}
