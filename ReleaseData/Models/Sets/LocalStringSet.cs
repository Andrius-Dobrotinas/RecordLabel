using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.ComponentModel.DataAnnotations.Schema;

namespace RecordLabel.Content
{
    public class LocalStringSet : Set<LocalString>, IValueComparable<LocalStringSet>, IQueryable<LocalString>, IEnumerable<LocalString>
    {
        /// <summary>
        /// A string that corresponds to the current thread culture or, if not found, a default culture, or, if not found, first non-empty string
        /// </summary>
        public string Text => Collection?.FirstOrDefault(item => item.Language == RecordLabel.Localization.CurrentLanguage)?.Text ??
            Collection?.FirstOrDefault(item => item.Language == RecordLabel.Localization.DefaultLanguage)?.Text ??
            Collection.FirstOrDefault(item => !String.IsNullOrEmpty(item.Text))?.Text;
        
        public override void UpdateModel(ReleaseContext dbContext, object newModel)
        {
            //Update Collection items with values from newModel and Add new items from newModel
            //Collection items are updated by values from newModel's items that have a corresponding language
            IList<Language> newKeys = new List<Language>(); //a List containing newModel's LocalString Languages
            foreach (LocalString newItem in ((Set<LocalString>)newModel).Collection)
            {
                LocalString itemInCollection = Collection.SingleOrDefault(entity => entity.Language == newItem.Language);
                if (itemInCollection == null)
                {
                    Collection.Add(newItem);
                }
                else
                {
                    //Update existing item
                    itemInCollection.UpdateModel(dbContext, newItem);
                }
                newKeys.Add(newItem.Language);
            }

            //Remove items that are not present in the newModel
            IList<LocalString> itemsToRemove = new List<LocalString>();
            foreach (LocalString item in Collection)
            {
                if (newKeys.Contains(item.Language) == false)
                {
                    itemsToRemove.Add(item);
                }
            }
            ((List<LocalString>)Collection).RemoveAll(item => itemsToRemove.Contains(item));
        }

        public bool ValuesEqual(LocalStringSet localization)
        {
            foreach (var item in typeof(Language).GetEnumValues())
            {
                int lang = (int)item;
                LocalString source = Collection.SingleOrDefault(entry => (int)entry.Language == lang);
                LocalString compareTo = localization.Collection.SingleOrDefault(entry => (int)entry.Language == lang);

                if (ModelHelpers.CompareReferenceTypes(source, compareTo) == false)
                {
                    return false;
                }
            }
            return true;
        }

        public override void Delete(ReleaseContext dbContext)
        {
            //LocalStrings are automatically deleted when detached from the set due to EntityFramework magic
            Collection.RemoveAllItems();

            base.Delete(dbContext);
        }



        
        Expression IQueryable.Expression
        {
            get
            {
                return Collection.AsQueryable<LocalString>().Expression;
            }
        }

        Type IQueryable.ElementType
        {
            get
            {
                return Collection.AsQueryable<LocalString>().GetType();
            }
        }

        IQueryProvider IQueryable.Provider
        {
            get
            {
                return Collection.AsQueryable<LocalString>().Provider;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        IEnumerator<LocalString> IEnumerable<LocalString>.GetEnumerator()
        {
            return Collection.GetEnumerator();
        }

        /*public LocalString this[int index] {
            get {
                return Collection[index];
            }
            set {
                Collection[index] = value;
            }
        }*/
    }
}
