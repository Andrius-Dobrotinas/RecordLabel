using System.Collections.Generic;

namespace RecordLabel
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
