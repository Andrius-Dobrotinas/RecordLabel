namespace RecordLabel
{
    /// <summary>
    /// Denotes that a class can compare values of properties that matter (in such a comparison) and tell whether the values of two instances are equal
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IValueComparable<in T>
    {
        /// <summary>
        /// Compares actual property values of this instance with compareTo and tells whether they are equal
        /// </summary>
        /// <param name="compareTo"></param>
        /// <returns></returns>
        bool ValuesEqual(T compareTo);
    }
}
