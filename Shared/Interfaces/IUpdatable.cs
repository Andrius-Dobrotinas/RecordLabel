namespace RecordLabel
{
    public interface IUpdatableModel<TContext>
    {
        void UpdateModel(TContext context, object sourceModel);
    }
}
