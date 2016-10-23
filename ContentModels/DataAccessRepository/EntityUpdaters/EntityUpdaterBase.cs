using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;

namespace RecordLabel.Data.ok
{
    public abstract class EntityUpdaterBase : IRecursiveEntityUpdater
    {
        protected DbContext DbContext { get; }
        protected IEntityUpdater ScalarPropertyUpdater { get; }

        public EntityUpdaterBase(DbContext dbContext, IEntityUpdater scalarPropertyUpdater)
        {
            this.DbContext = dbContext;
            this.ScalarPropertyUpdater = scalarPropertyUpdater;
        }

        public TEntity UpdateEntity<TEntity>(TEntity model, IRecursiveEntityUpdater updater)
            where TEntity : class, IHasId
        {
            // Update scalar properties
            TEntity updatedModel = UpdateEntity<TEntity>(model);

            UpdateAllNavigationProperties(updatedModel, model, updater, updatedModel == model);

            return updatedModel;
        }

        public TEntity UpdateEntity<TEntity>(TEntity model)
            where TEntity : class, IHasId
        {
            return ScalarPropertyUpdater.UpdateEntity<TEntity>(model);
        }

        protected abstract void UpdateAllNavigationProperties<TEntity>(TEntity updatedModel, TEntity model, IRecursiveEntityUpdater entityUpdater, bool isNew)
            where TEntity : class, IHasId;
    }
}
