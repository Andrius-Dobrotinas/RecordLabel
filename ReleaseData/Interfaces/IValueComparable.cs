using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RecordLabel.Catalogue
{
    /// <summary>
    /// Denotes that a class can compare values of properties that matter (in such a comparison) and tell whether the values of two instances are equal
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueComparable<T>
    {
        /// <summary>
        /// Compares actual property values of this instance with compareTo and tells whether they are equal
        /// </summary>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        bool ValuesEqual(T compareTo);
    }
}
