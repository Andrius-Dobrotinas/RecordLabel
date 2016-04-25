using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace RecordLabel.Content
{
    public abstract class Set<T> : FirstBase, IKnowIfImEmpty where T : EntityBase, IValueComparable<T>
    {
        public virtual IList<T> Collection {
            get
            {
                return collection ?? (collection = new List<T>());
            }
            set
            {
                collection = value;
            }
        }
        private IList<T> collection { get; set; }

        /// <summary>
        /// Updates Set-typed property of a given model. Takes care of null values
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <param name="model"></param>
        /// <param name="property"></param>
        /// <param name="newSet"></param>
        /// <param name="dbContext"></param>
        public static void UpdateSet<TModel>(TModel model, Expression<Func<TModel, Set<T>>> property, Set<T> newSet, ReleaseContext dbContext) where TModel : IHasASet<T>
        {
            //Get reference to the Set property
            MemberExpression mex = (MemberExpression)property.Body;
            PropertyInfo setProperty = (PropertyInfo)mex.Member;
            Set<T> sourceSet = (Set<T>)setProperty.GetValue(model);

            if (!(newSet?.Collection?.Count > 0))
            {
                //Remove the whole set and do nothing
                if (sourceSet != null)
                {
                    sourceSet.Delete(dbContext);
                }
            }
            else //Add/Remove items
            {
                if (sourceSet == null)
                {
                    //Add all items
                    setProperty.SetValue(model, newSet);
                }
                else
                {
                    //Add/Remove items
                    sourceSet.UpdateModel(dbContext, newSet);
                    
                    /*IList<T> itemsToRemove = sourceSet.Collection.UpdateCollectionByIds(newSet.Collection, dbContext);
                    if (itemsToRemove.Count > 0) //&& DeleteItemsFromTheContext
                    {
                        DeleteItems(itemsToRemove, dbContext);
                    }*/
                }
            }
        }

        public override void UpdateModel(ReleaseContext dbContext, object sourceModel)
        {  
            IList<T> itemsToRemove = Collection.UpdateCollectionByIds(((Set<T>)sourceModel).Collection, dbContext);
            if (itemsToRemove.Count > 0)
            {
                DeleteItems(itemsToRemove, dbContext);
            }
        }

        /// <summary>
        /// Deletes the set from the database within the supplied context
        /// </summary>
        /// <param name="dbContext"></param>
        public override void Delete(ReleaseContext dbContext)
        {
            //Delete collection items from the database
            DeleteItems(Collection, dbContext);

            dbContext.Entry(this).State = System.Data.Entity.EntityState.Deleted;
        }

        /// <summary>
        /// Permanently deletes items within the collection from the database within the supplied context
        /// </summary>
        /// <param name="itemsToRemove"></param>
        /// <param name="dbContext"></param>
        private static void DeleteItems(IList<T> itemsToRemove, ReleaseContext dbContext)
        {
            if (DeleteItemsFromTheContext)
            {
                foreach (T item in itemsToRemove.ToArray())
                {
                    item.Delete(dbContext);
                }
            }
        }

        /// <summary>
        /// Tells whether to permanently delete items contained within this set from the database
        /// </summary>
        /// <returns></returns>
        private static bool DeleteItemsFromTheContext
        {
            get
            {
                return typeof(T).GetCustomAttribute(typeof(OneToOneRelationshipAttribute)) != null;
            }
        }

        public bool IsEmpty
        {
            get
            {
                return !(Collection.Count > 0); //TODO: make T impletent IKnowIfImEmpty (add this implementation to
                //Release model. 2: add call for each collectionItem.IsEmpty to this result
            }
        }
    }
}
