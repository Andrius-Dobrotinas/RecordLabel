using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Linq.Expressions;

namespace RecordLabel.Content
{
    public abstract class Set<T> : FirstBase, ISet<T>
        where T : EntityBase, IValueComparable<T>
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