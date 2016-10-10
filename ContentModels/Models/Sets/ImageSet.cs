using System;
using System.Collections.Generic;

namespace RecordLabel.Content
{
    public class ImageSet : Set<Image>
    {
        public ImageSet()
        {

        }

        public ImageSet(List<Image> images)
        {
            Collection = images;
        }

        public void RemoveAllItems()
        {
            Collection.Clear();
        }

        /// <summary>
        /// Sorts items in the set and makes order values sequential
        /// </summary>
        public void SortAndOrderSequential()
        {
            ((List<Image>)Collection).Sort((first, second) => first.Order.CompareTo(second.Order));

            //Set sequential order values
            for (int i = 0; i < Collection.Count; i++)
            {
                Collection[i].Order = i;
            }
        }

        public override void Delete(ReleaseContext dbContext)
        {
            foreach(Image item in Collection)
            {
                item.Delete(dbContext);
            }
            base.Delete(dbContext);
        }

        //Not using this yet
        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {
            throw new NotImplementedException();
        }
    }
}
