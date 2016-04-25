using RecordLabel.Content;

namespace RecordLabel.Web
{
    /// <summary>
    /// Indicates that a model has a reference to a database context
    /// </summary>
    public interface IHasDbContext<TContext>
    {
        TContext DbContext { get; }
    }
}
