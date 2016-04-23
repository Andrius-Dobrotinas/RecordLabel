using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
{
    public interface ICollectionWrapper<T> : IEnumerable<T>
    {
        T this[int index]
        {
            get; set;
        }

        int Count { get; }

        void Add(T item);
        void AddCollection(IList<T> itemCollection);
        bool Remove(T item);
    }
}
