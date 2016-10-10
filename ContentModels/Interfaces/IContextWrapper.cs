using System;
using System.Collections.Generic;
using System.Data.Entity;

namespace RecordLabel.Content
{
    public interface IContextWrapper<TContext> : IDisposable
        where TContext : DbContext
    {
        TContext Context { get; }

        void SaveChanges();

        DbSet<T> Set<T>() where T : class;

        void UpdateModel<TModel>(TModel model, TModel newState) where TModel : EntityBase;
    }
}
