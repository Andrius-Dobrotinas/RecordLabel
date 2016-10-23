using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace RecordLabel.Data.ok
{
    public class ScalarPropertyUpdater : IEntityUpdater
    {
        protected readonly DbContext dbContext;

        public ScalarPropertyUpdater(DbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public TModel UpdateEntity<TModel>(TModel model) where TModel : class, IHasId
        {
            // If adding new entity
            if (model.Id == default(int))
            {
                return dbContext.Set<TModel>().Add(model);
                // TODO: if we add new entity but assign, like, existing metadata... do that else if anyway.
            }
            else
            {
                /* Beware of searching for entities that have base entities and whose Ids are bad. In such situation, if
                 * there is an other entity that derives from the base entity with that Id, Find will return it, and then
                 * will throw an invalid case exception because it will try to cast it to TModel.
                 * In other words, it won't say it could not find TModel with the supplied Id, it will find a "sibling"
                 * entity with that Id (if it exists) and try to cast it. */
                TModel originalModel = dbContext.Set<TModel>().Find(model.Id);
                if (originalModel == null)
                    throw new KeyNotFoundException($"Entity of type \"{typeof(TModel)}\" with a key value \"{model.Id}\" not found in the context");
                dbContext.Entry(originalModel).CurrentValues.SetValues(model);
                return originalModel;
            }
        }
    }
}
