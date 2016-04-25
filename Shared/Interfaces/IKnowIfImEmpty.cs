namespace RecordLabel
{
    /// <summary>
    /// Implementing classes can be checked whether they have any meaningful data
    /// </summary>
    public interface IKnowIfImEmpty
    {
        bool IsEmpty { get; }
    }
}
